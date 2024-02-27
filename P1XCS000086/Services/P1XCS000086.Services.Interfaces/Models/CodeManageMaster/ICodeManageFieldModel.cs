using P1XCS000086.Services.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface ICodeManageFieldModel
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// サーバー名
		/// </summary>
		public string Server { get; }

		/// <summary>
		/// ユーザー名
		/// </summary>
		public string User { get; }

		/// <summary>
		/// データベース
		/// </summary>
		public string Database { get; }

		/// <summary>
		/// パスワード
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// 接続状態の保持
		/// </summary>
		public bool PersistSecurityInfo { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたロジックモデル群を注入
		/// </summary>
		/// <param name="jsonConnStr"></param>
		public void InjectModels(IJsonConnectionStrings jsonConnStr);
	}
}
