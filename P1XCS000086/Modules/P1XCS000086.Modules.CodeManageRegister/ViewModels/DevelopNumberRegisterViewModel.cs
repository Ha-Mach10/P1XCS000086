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
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Models.CodeManageRegister;
using System.Windows.Navigation;
using P1XCS000086.Services.Interfaces.Models.CodeManageRegister;


namespace P1XCS000086.Modules.CodeManageRegister.ViewModels
{
    public class DevelopNumberRegisterViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		// 破棄用
		private CompositeDisposable _disposables;
		// インジェクションされたモデル用
		private IDevelopNumberRegisterModel _model;
		private IMySqlConnectionString _connStr;
		private ISqlSelect _select;
		private ISqlInsert _insert;



		// ****************************************************************************
		// Reactive Properties
		// ****************************************************************************

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



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public DevelopNumberRegisterViewModel(IDevelopNumberRegisterModel model, ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr)
		{
			// インジェクションされたモデルインターフェース
			_model = model;
			_connStr = connStr;
			_select = select;
			_insert = insert;


			// インジェクションされたインターフェースをモデルにセット
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



		// ****************************************************************************
		// Command Event
		// ****************************************************************************

		public ReactiveCommandSlim CheckedChangedToOff { get; }
		/// <summary>
		/// 使用用途欄のトグルスイッチOFF時の処理
		/// 「自動」ComboBox入力時
		/// </summary>
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
		/// <summary>
		/// 使用用途欄のトグルスイッチON時の処理
		/// 「手動」TextBox入力時
		/// </summary>
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
		/// <summary>
		/// 開発番号登録処理
		/// </summary>
		private void OnRegistCodeNumber()
		{
			// 開発番号
			string developNumber = _model.GetDevelopmentNumber();
			// 開発名称・コードネーム
			string developName = _model.GetValue(DevelopName, string.Empty);
			string codeName = _model.GetValue(CodeName, string.Empty);
			// 使用用途
			string useApplication = _model.GetUseApplication(UseAppSelectedValue, UseAppSubSelectedValue, UseApplicationManual);
			// 参考・旧・新・継承番号
			string referenceNumber = _model.GetValue(ReferenceNumber, string.Empty);
			string oldNumber = _model.GetValue(OldNumber, string.Empty);
			string newNumber = _model.GetValue(NewNumber, string.Empty);
			string inheritenceNumber = _model.GetValue(InheritenceNumber, string.Empty);
			// 説明・摘要
			string explanation = _model.GetValue(Explanation, string.Empty);
			string summary = _model.GetValue(Summary, string.Empty);

			string resultMessege = _model.RegistValues(
				developNumber,
				developName,
				codeName,
				useApplication,
				referenceNumber,
				oldNumber,
				newNumber,
				inheritenceNumber,
				explanation,
				summary);

			// スナックバーを生成（ディレイ時間は２秒）
			SnackbarMessageQueue  snackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));
			snackbarMessageQueue.Enqueue(resultMessege);
			StateSnackBarMessageQueue.Value = snackbarMessageQueue;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		// 破棄
		public void Destroy()
		{
			_disposables.Dispose();
		}
	}
}
