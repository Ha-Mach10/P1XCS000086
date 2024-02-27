using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;
using P1XCS000086.Services.Interfaces.Objects;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class CodeManageFieldModel : ICodeManageFieldModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private IJsonConnectionStrings _jsonConnStr;



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバー名
		/// </summary>
		public string Server {  get; private set; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User { get; private set; }

		/// <summary>
		/// データベース
		/// </summary>
		public string Database { get; private set; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; private set; }

		/// <summary>
		/// 接続状態の保持
		/// </summary>
		public bool PersistSecurityInfo { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManageFieldModel() { }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたロジックモデル群を注入
		/// </summary>
		/// <param name="jsonConnStr"></param>
		public void InjectModels(IJsonConnectionStrings jsonConnStr)
		{
			// 
			_jsonConnStr = jsonConnStr;

			// 初期化
			Initialize();
		}



		// ****************************************************************************
		// Private Methods
		// ****************************************************************************

		/// <summary>
		/// モデルの初期化
		/// </summary>
		private void Initialize()
		{
			PickConnectionString();
		}

		/// <summary>
		/// 指定の接続文字列の各パラメータをプロパティにセット
		/// </summary>
		private void PickConnectionString()
		{
			// 「manager」データベースの接続文字列を検索し、取得
			string connStr = IJsonConnectionStrings.JsonConnectionStringItems
												   .Where(x => x.Key == "manager")
												   .First()
												   .Value;

			// 取得した接続文字列を解析し、各パラメータのプロパティを取得
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connStr);
			Server = builder.Server;
			User = builder.UserID;
			Database = builder.Database;
			Password = builder.Password;
			PersistSecurityInfo = builder.PersistSecurityInfo;
		}
	}
}
