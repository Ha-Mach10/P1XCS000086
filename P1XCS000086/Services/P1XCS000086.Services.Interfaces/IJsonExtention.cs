using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces
{
	public interface IJsonExtention
	{
		public bool PathCheckAndGenerate();
		public void SerializeJson<T>(T obj, string path, bool append);
		public T DeserializeJson<T>(string path);
	}
}
