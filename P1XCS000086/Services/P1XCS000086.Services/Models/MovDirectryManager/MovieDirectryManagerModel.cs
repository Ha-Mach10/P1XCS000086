using P1XCS000086.Services.Interfaces.Models.MovDirectryManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace P1XCS000086.Services.Models.MovDirectryManager
{
	public class MovieDirectryManagerModel : IMovieDirectryManagerModel
	{
		// ---------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------- 




		// ---------------------------------------------------------------
		// Properties
		// --------------------------------------------------------------- 

		public List<string> WorkSpaceDirectries { get; private set; } = new();



		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public MovieDirectryManagerModel()
		{

		}



		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		public void SetNeedInitializeProperties(string workSpaceDirectory)
		{
			SetWorkSpaceDirectries(workSpaceDirectory);
		}
		public IEnumerable<string> GetMovieDirectoryFiles(string movieFileDirectory)
		{
			// var a = Directory.GetFiles(movieFileDirectory).Select(x => Path.GetFileName(x)).ToList();
			return Directory.GetFiles(movieFileDirectory).Select(x => Path.GetFileName(x));
		}



		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 

		private void SetWorkSpaceDirectries(string workSpaceDirectory)
		{
			// 存在していない場合は処理を抜ける
			if (Directory.Exists(workSpaceDirectory) is false) return;
			// 
			if (WorkSpaceDirectries is null && WorkSpaceDirectries.Count == 0) return;

			// プロパティへ列挙されたディレクトリを格納
			WorkSpaceDirectries.AddRange(Directory.GetDirectories(workSpaceDirectory).Select(x => Path.GetFileName(x)));
		}
	}
}
