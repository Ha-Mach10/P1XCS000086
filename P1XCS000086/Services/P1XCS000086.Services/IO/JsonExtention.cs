using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using P1XCS000086.Services.Interfaces;

namespace P1XCS000086.Services.IO
{
	public class JsonExtention : IJsonExtention
	{
		// Static readonly fields
		public static readonly string JsonFolderPath = "settings";
		public static readonly string JsonFilePath = "settings/SqlConnectionString.json";


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
			if (!File.Exists(JsonFilePath))
			{
				File.Create(JsonFilePath);
				isExists = false;
			}

			return isExists;
		}

		/// <summary>
		/// シリアライズ（直列化）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="path"></param>
		/// <param name="append"></param>
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
		/// デシリアライズ（直列化複合）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns></returns>
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
