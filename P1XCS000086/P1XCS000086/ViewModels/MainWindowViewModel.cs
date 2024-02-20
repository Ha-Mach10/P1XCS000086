using System;
using System.Data;
using System.Windows;
using System.Diagnostics;
using System.Reactive;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Xps.Serialization;
using System.Security.Cryptography.Xml;
using System.CodeDom;
using System.Reflection.Emit;

using P1XCS000086.Services;
using P1XCS000086.Services.Models;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Objects;

using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.ObjectExtensions;

using MaterialDesignThemes.Wpf;

using Org.BouncyCastle.Bcpg.OpenPgp;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Domains;
using System.Linq;
using P1XCS000086.Services.Interfaces.Models;
using P1XCS000086.Services.Interfaces.IO;
using P1XCS000086.Services.Interfaces.Objects;
using P1XCS000086.Services.Interfaces.Sql;


namespace P1XCS000086.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDestructible, INotifyPropertyChanged
	{
		private string _title = "Prism Application";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IMainWindowModel _model;
		private IJsonConnectionStrings _jsonConnStr;
		private IJsonExtention _jsonExtention;
		private IMySqlConnectionString _sqlConnStr;
		private IJsonConnectionItem _jsonConnectionItem;
		private ISqlSchemaNames _schemaNames;
		private CompositeDisposable disposables = new CompositeDisposable();



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(
			IMainWindowModel model,
			IJsonExtention jsonExtention,
			IJsonConnectionStrings jsonConnStr,
			IMySqlConnectionString sqlConnStr,
			IJsonConnectionItem jsonConnectionItem,
			ISqlSchemaNames schemaNames)
		{
			// MainWindowModelをインターフェース(IMainWindowModel)から生成
			_model = model;
			_jsonConnStr = jsonConnStr;
			_jsonExtention = jsonExtention;
			_sqlConnStr = sqlConnStr;
			_jsonConnectionItem = jsonConnectionItem;
			_schemaNames = schemaNames;

			// インジェクションされたモデルを注入
			_model.InjectModels(_jsonConnStr, _jsonExtention, _sqlConnStr, _jsonConnectionItem, _schemaNames);

			// JSONファイルから接続文字列を生成し、IJsonConnectionItemsのstaticなディクショナリへ登録
			_model.SetConnectionString();


		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************



		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
