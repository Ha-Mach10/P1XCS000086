using Prism.Commands;
using Prism.Mvvm;
using Prism.Ioc;
using Prism.Navigation;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using MahApps.Metro;
using MahApps.Metro.Controls;

using System;
using System.Reactive.Disposables;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;

using P1XCS000086.Services.Interfaces;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Models.CodeManageRegister;
using System.Windows.Navigation;


namespace P1XCS000086.Modules.CodeManageRegister.ViewModels
{
	public class DevelopNumberRegisterViewModel : BindableBase, IDestructible
	{
		// 破棄用
		private CompositeDisposable _disposables;
		// インジェクションされたモデル用
		private IDevelopNumberRegisterModel _model;
		private IMySqlConnectionString _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;


		// 
		public ReactivePropertySlim<string> DevelopName { get; }
		public ReactivePropertySlim<string> CodeName { get; }
		public ReactivePropertySlim<string> UseApplicationManual { get; }
		public ReactivePropertySlim<List<string>> UseApplication { get; }
		public ReactivePropertySlim<List<string>> UseApplicationSub { get; }
		public ReactivePropertySlim<string> ReferenceNumber { get; }
		public ReactivePropertySlim<string> OldNumber { get; }
		public ReactivePropertySlim<string> NewNumber { get; }
		public ReactivePropertySlim<string> InheritenceNumber { get; }
		public ReactivePropertySlim<string> Explanation { get; }
		public ReactivePropertySlim<string> Summary { get; }
		// 
		public ReactivePropertySlim<string> UseAppSelectedValue { get; }
		public ReactivePropertySlim<string> UseAppSubSelectedValue { get; }
		// 
		public ReactivePropertySlim<bool> UseAppIsChecked { get; }
		// 
		public ReactivePropertySlim<Visibility> UseAppComboVisibility { get; }
		public ReactivePropertySlim<Visibility> UseAppTextVisibility { get; }
		public ReactivePropertySlim<Visibility> RegistButtonVisibility { get; }
		// 
		public ReactivePropertySlim<SnackbarMessageQueue> StateSnackBarMessageQueue { get; }


		public DevelopNumberRegisterViewModel(IDevelopNumberRegisterModel model, ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr)
		{
			// インジェクションされたモデルインターフェース
			_model = model;
			_connStr = connStr;
			_select = select;
			_insert = insert;

			_model.SetModelBuiltin(_select, _insert, _connStr);

			// 初期値設定用変数
			string initStr = string.Empty;
			var stateMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));

			// 初期値設定
			DevelopName				= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			CodeName				= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			UseApplicationManual	= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			UseApplication			= new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			UseApplicationSub		= new ReactivePropertySlim<List<string>>().AddTo(_disposables);
			ReferenceNumber			= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			OldNumber				= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			NewNumber				= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			InheritenceNumber		= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			Explanation				= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			Summary					= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			// 
			UseAppSelectedValue		= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			UseAppSubSelectedValue	= new ReactivePropertySlim<string>(initStr).AddTo(_disposables);
			// 
			UseAppIsChecked = new ReactivePropertySlim<bool>(false).AddTo(_disposables);
			// 
			UseAppComboVisibility	= new ReactivePropertySlim<Visibility>(Visibility.Visible).AddTo(_disposables);
			UseAppTextVisibility	= new ReactivePropertySlim<Visibility>(Visibility.Collapsed).AddTo(_disposables);
			RegistButtonVisibility	= new ReactivePropertySlim<Visibility>(Visibility.Collapsed).AddTo(_disposables);
			// 
			StateSnackBarMessageQueue = new ReactivePropertySlim<SnackbarMessageQueue>(stateMessageQueue).AddTo(_disposables);


			// イベントの初期化・購読
			CheckedChangedToOff = new ReactiveCommandSlim();
			CheckedChangedToOff.Subscribe(() => OnCheckedChangedToOff()).AddTo(_disposables);
			CheckedChangedToOn = new ReactiveCommandSlim();
			CheckedChangedToOn.Subscribe(() => OnCheckedChangedToOn()).AddTo(_disposables);
			RegistCodeNumber = new ReactiveCommandSlim();
			RegistCodeNumber.Subscribe(() => OnRegistCodeNumber()).AddTo(_disposables);
		}


		// 
		public ReactiveCommandSlim CheckedChangedToOff { get; }
		private void OnCheckedChangedToOff()
		{
			// false のとき
			if (!UseAppIsChecked.Value)
			{
				// ComboBox の Visibility を Collapsed へ変更
				UseAppComboVisibility.Value = Visibility.Collapsed;
				// TextBox の Visibility を Visible へ変更
				UseAppTextVisibility.Value = Visibility.Visible;
			}
		}
		public ReactiveCommandSlim CheckedChangedToOn { get; }
		private void OnCheckedChangedToOn()
		{
			// true のとき
			if (UseAppIsChecked.Value)
			{
				// ComboBox の Visibility を Visible へ変更
				UseAppComboVisibility.Value = Visibility.Visible;
				// TextBox の Visibility を Collapsed へ変更
				UseAppTextVisibility.Value = Visibility.Collapsed;
			}
		}
		public ReactiveCommandSlim RegistCodeNumber { get; }
		private void OnRegistCodeNumber()
		{
			///--------------
			// 開発番号の開発記号生成
			string devTypeValue = IntegrRegisterModel.DevTypeValue;
			string langTypeValue = IntegrRegisterModel.LangTypeValue;
            if (devTypeValue == "" && langTypeValue == "") { return; }
            string codeNumberClassificationString = _model.CodeNumberClassification(devTypeValue, langTypeValue);

			// 開発番号の追番生成
			int recordCount = IntegrRegisterModel.RecordCount;
			string codeNumber = $"{codeNumberClassificationString}{(recordCount + 1).ToString("000000")}";
			///--------------


			// 開発番号・コードネーム
			string developName	= string.Empty;
			string codeName		= string.Empty;
			if (DevelopName is not null) { developName = DevelopName.Value; }
			if (CodeName is not null) { developName = CodeName.Value; }

			// 使用用途
			string useApplication		= string.Empty;
			string useApplicationSub	= string.Empty;
			if (UseAppSelectedValue is not null || UseAppSubSelectedValue is not null)
			{
				useApplication = _model.GetUseApplicationValue(UseAppSelectedValue.Value);
				useApplicationSub = _model.GetUseApplicationValue(UseAppSubSelectedValue.Value);

				useApplication = $"{useApplication}_{useApplicationSub}";
			}
			else if (UseApplicationManual is not null)
			{
				useApplication = UseApplicationManual.Value;
			}

			// 参考・旧・新・継承番号
			string referenceNumber		= string.Empty;
			string oldNumber			= string.Empty;
			string newNumber			= string.Empty;
			string inheritenceNumber	= string.Empty;
			if (ReferenceNumber is not null) { referenceNumber = ReferenceNumber.Value; }
			if (OldNumber is not null) { oldNumber = OldNumber.Value; }
			if (NewNumber is not null) { newNumber = NewNumber.Value; }
			if (InheritenceNumber is not null) { inheritenceNumber = InheritenceNumber.Value; }

			// 説明・摘要
			string explanation	= string.Empty;
			string summary		= string.Empty;
			if (Explanation is not null) { explanation = Explanation.Value; }
			if (Summary is not null) { summary = Summary.Value; }

			List<string> values = new List<string>()
			{
				codeNumberClassificationString,
				developName,
				codeName,
				DateTime.Now.ToString(),
				useApplication,
				referenceNumber, oldNumber, newNumber, inheritenceNumber,
				explanation, summary
			};

			// 
			if (!_mainWindowModel.RegistExecute(values))
			{
				string message = $"{_mainWindowModel.ResultMessage}\n{_mainWindowModel.ExceptionMessage}";
				SnackBarMessageQueue.Enqueue(message);
			}

			int a = 0;
		}


		// 破棄
		public void Destroy()
		{
			_disposables?.Dispose();
		}
	}
}
