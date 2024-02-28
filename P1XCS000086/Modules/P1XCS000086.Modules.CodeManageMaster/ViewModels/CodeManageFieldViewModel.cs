using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using MaterialDesignThemes.Wpf;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Sql;
using P1XCS000086.Services.Interfaces.IO;

namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class CodeManageFieldViewModel : BindableBase, IDestructible
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposables;
		private ICodeManageFieldModel _model;
		private ISqlSelect _select;
		private ISqlShowTables _showTables;
		private ISqlConnectionTest _connectionTest;
		private IJsonExtention _jsonExtention;
		private IJsonConnectionItem _jsonConnItem;
		private IJsonConnectionStrings _jsonConnStr;



		// ****************************************************************************
		// Property
		// ****************************************************************************

		public ReactivePropertySlim<string> Server { get; }
		public ReactivePropertySlim<string> User { get; }
		public ReactivePropertySlim<string> Database { get; }
		public ReactivePropertySlim<string> Password { get; }
		public ReactivePropertySlim<bool> PersistSecurityInfo { get; }
		public ReactivePropertySlim<SnackbarMessageQueue> SnackBarMessageQueue { get; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManageFieldViewModel
			(ICodeManageFieldModel model, ISqlSelect select, ISqlShowTables showTables, ISqlConnectionTest connectionTest, IJsonExtention jsonExtention, IJsonConnectionItem jsonConnItem, IJsonConnectionStrings jsonConnStr)
		{
			// 
			_model = model;
			_select = select;
			_showTables = showTables;
			_connectionTest = connectionTest;
			_jsonExtention = jsonExtention;
			_jsonConnItem = jsonConnItem;
			_jsonConnStr = jsonConnStr;

			// DIされたロジック群をモデルへ注入
			_model.InjectModels(_select, _showTables, _connectionTest, _jsonExtention, _jsonConnItem, _jsonConnStr);

			// Properties Setting
			Server				= new ReactivePropertySlim<string>(_model.Server).AddTo(_disposables);
			User				= new ReactivePropertySlim<string>(_model.User).AddTo(_disposables);
			Database			= new ReactivePropertySlim<string>(_model.Database).AddTo(_disposables);
			Password			= new ReactivePropertySlim<string>(_model.Password).AddTo(_disposables);
			PersistSecurityInfo = new ReactivePropertySlim<bool>(_model.PersistSecurityInfo).AddTo(_disposables);

			// Commands
			RegistConnString = new ReactiveCommandSlim();
			RegistConnString.Subscribe(() => OnRegistConnString()).AddTo(_disposables);
			ConnectionTest = new ReactiveCommandSlim();
			ConnectionTest.Subscribe(() => OnConnectionTest()).AddTo(_disposables);
		}



		// ****************************************************************************
		// Reactive Command
		// ****************************************************************************

		public ReactiveCommandSlim RegistConnString { get; }
		
		/// <summary>
		/// 接続文字列の登録
		/// </summary>
		private void OnRegistConnString()
		{
			_model.RegistConnectionString(Server.Value, User.Value, Database.Value, Password.Value, PersistSecurityInfo.Value);
		}


		public ReactiveCommandSlim ConnectionTest { get; }
		
		/// <summary>
		/// 接続テスト
		/// </summary>
		private void OnConnectionTest()
		{
			// 接続テスト
			_model.TestDatabaseConnection();
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
