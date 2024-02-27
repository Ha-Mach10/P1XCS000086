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

namespace P1XCS000086.Modules.CodeManageMaster.ViewModels
{
	public class CodeManageFieldViewModel : BindableBase, IDestructible
	{
		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}



		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private CompositeDisposable _disposables;
		private ICodeManageFieldModel _model;
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

		public CodeManageFieldViewModel(ICodeManageFieldModel model, IJsonConnectionStrings jsonConnStr)
		{
			// 
			_model = model;
			_jsonConnStr = jsonConnStr;

			// DIされたロジック群をモデルへ注入
			_model.InjectModels(jsonConnStr);

			Message = "View A from your Prism Module";
		}



		// ****************************************************************************
		// Reactive Command
		// ****************************************************************************

		public ReactiveCommandSlim RegistConnString { get; }
		private void OnRegistConnString()
		{

		}
		public ReactiveCommandSlim ConnectionTest { get; }
		private void OnConnectionTest()
		{

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
