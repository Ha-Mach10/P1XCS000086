using P1XCS000086.Services.Interfaces.Sql;

using Reactive.Bindings;

using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageRegister
{
    public interface IDevelopNumberRegisterModel
    {
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// ViewModelへ注入されたインターフェースをセット
		/// </summary>
		/// <param name="select"></param>
		/// <param name="insert"></param>
		/// <param name="connStr"></param>
		public void SetModelBuiltin(ISqlSelect select, ISqlInsert insert, IMySqlConnectionString connStr);

		/// <summary>
		/// 開発番号を返す
		/// </summary>
		/// <returns>開発番号</returns>
		public string GetDevelopmentNumber();

		/// <summary>
		/// ReactivePropertySlim<T>から値を取得する
		/// </summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="sourcePropertiy">値を取得するReactivePropertySlim<T></param>
		/// <param name="substutituteValue">ReactivePropertySlimがnullの場合に代わりとなる値</param>
		/// <returns>Tの値</returns>
		public T GetValue<T>(ReactivePropertySlim<T> sourcePropertiy, T substutituteValue);

		/// <summary>
		/// 使用用途の取得
		/// </summary>
		/// <param name="useAppSelectedValue">自動使用用途</param>
		/// <param name="useAppSubSelectedValue">自動副使用用途</param>
		/// <param name="useApplicationManual">手動使用用途</param>
		/// <returns>使用用途</returns>
		public string GetUseApplication
			(ReactivePropertySlim<string> useAppSelectedValue, ReactivePropertySlim<string> useAppSubSelectedValue, ReactivePropertySlim<string> useApplicationManual);

		/// <summary>
		/// 開発番号をデータベースに登録
		/// </summary>
		/// <param name="developNumber">開発番号</param>
		/// <param name="developName">開発名称</param>
		/// <param name="codeName">コードネーム</param>
		/// <param name="useApplication">使用用途</param>
		/// <param name="referenceNumber">参照番号</param>
		/// <param name="oldNumber">旧番号</param>
		/// <param name="newNumber">新番号</param>
		/// <param name="inheritenceNumber">継承番号</param>
		/// <param name="explanation">説明</param>
		/// <param name="summary">摘要</param>
		public string RegistValues
            (string developNumber, string developName, string codeName, string useApplication, string referenceNumber, string oldNumber, string newNumber, string inheritenceNumber, string explanation, string summary);

		/// <summary>
		/// 日本語名で与えられた開発種別・言語種別をマスタからコードで取得
		/// </summary>
		/// <param name="developType">開発種別</param>
		/// <param name="languageType">言語種別</param>
		/// <returns>
		/// 開発番号用の値
		/// </returns>
		public string CodeNumberClassification(string developType, string languageType);
	}
}
