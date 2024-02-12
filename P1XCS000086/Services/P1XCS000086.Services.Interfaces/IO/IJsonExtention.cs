using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.IO
{
    public interface IJsonExtention
    {
		// ****************************************************************************
		// Static Readonly Fields
		// ****************************************************************************

		/// <summary>
		/// JSONフォルダパス
		/// </summary>
		public string JsonFolderPath { get; }

		/// <summary>
		/// SQL接続文字列用JSONファイルパス
		/// </summary>
		public string JsonSqlFilePath { get; }

		/// <summary>
		/// SQLデータベース名用JSONファイルパス
		/// </summary>
		public string JsonSqlDatabaseFilePath { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// フォルダ及びファイルの存在チェック。存在しなければ生成する。
		/// </summary>
		/// <returns>ファイルが存在すれば：true、存在しなければ：false</returns>
		public bool PathCheckAndGenerate();

		/// <summary>
		/// シリアライズ（直列化）
		/// </summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="obj">JSON化するオブジェクト</param>
		/// <param name="path">JSONファイルパス</param>
		/// <param name="append">追記するか否か</param>
		public void SerializeJson<T>(T obj, string path, bool append);

		/// <summary>
		/// デシリアライズ（直列化複合）
		/// </summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="path">JSONファイルパス</param>
		/// <returns>元の型</returns>
		public T DeserializeJson<T>(string path);
    }
}
