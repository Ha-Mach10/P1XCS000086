using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace P1XCS000086.Services.IO
{
	public static class JsonExtention
	{
		public static async void SerializeJson<T>(T obj, string path, bool append)
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

		public static T DeserializeJson<T>(string path)
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
