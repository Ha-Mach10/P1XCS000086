using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using P1XCS000086.Services.Interfaces.IO;

namespace P1XCS000086.Services.IO
{
    public class JsonExtention : IJsonExtention
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		private static readonly string _jsonFolderPath = "settings";
		private static readonly string _jsonSqlFilePath = "settings/SqlConnectionString.json";



		// ****************************************************************************
		// Properties
		// ****************************************************************************

		/// <summary>
		/// JSONフォルダパス
		/// </summary>
		public string JsonFolderPath { get; private set; }

		/// <summary>
		/// SQL接続文字列用JSONファイルパス
		/// </summary>
		public string JsonSqlFilePath { get; private set; }



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public JsonExtention()
		{
			JsonFolderPath = _jsonFolderPath;
			JsonSqlFilePath = _jsonSqlFilePath;
		}



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// フォルダ及びファイルの存在チェック。存在しなければ生成する。
		/// </summary>
		/// <returns>ファイルが存在すれば：true、存在しなければ：false</returns>
		public bool PathCheckAndGenerate()
		{
			// 戻り値用
			bool isExists = true;

			// JSONファイルを格納するフォルダの存在をチェックし、存在しなければ生成
			if (!Directory.Exists(JsonFolderPath))
			{
				Directory.CreateDirectory(JsonFolderPath);
			}
			// JSONファイルの存在をチェックし、存在しなければ生成
			if (!File.Exists(JsonSqlFilePath))
			{
				File.Create(JsonSqlFilePath);
				isExists = false;
			}

			return isExists;
		}

		/// <summary>
		/// シリアライズ（直列化）
		/// </summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="obj">JSON化するオブジェクト</param>
		/// <param name="path">JSONファイルパス</param>
		/// <param name="append">追記するか否か</param>
		public async void SerializeJson<T>(T obj, string path, bool append)
		{
			try
			{
				using (var sw = new StreamWriter(path, append, Encoding.UTF8))
				{
					var jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented);
					await sw.WriteAsync(jsonData);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		/// <summary>
		/// デシリアライズ（直列化復号）
		/// </summary>
		/// <typeparam name="T">任意の型</typeparam>
		/// <param name="path">JSONファイルパス</param>
		/// <returns>元の型</returns>
		public T DeserializeJson<T>(string path)
		{
			try
			{
				// StreamReaderでファイルを読み込む
				using (var sr = new StreamReader(path, Encoding.UTF8))
				{
					// 
					var jsonData = sr.ReadToEnd();
					// 
					if (jsonData == string.Empty) { return default; }
					return JsonConvert.DeserializeObject<T>(jsonData);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
				return default;
			}
		}
	}
}
