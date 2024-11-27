using P1XCS000086.Services.Interfaces.Models.MovDirectryManager;
using System;
using System.Collections.Generic;
using System.IO;
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

		public List<string> WorkSpaceDirectries { get; private set; }



		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public MovieDirectryManagerModel()
		{

		}



		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		public void SetNeedInitializeProperties(string workSpaceDirectry)
		{
			SetWorkSpaceDirectries(workSpaceDirectry);
		}



		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 

		private void SetWorkSpaceDirectries(string workSpaceDirectry)
		{
			// 存在していない場合は処理を抜ける
			if (Directory.Exists(workSpaceDirectry) is false) return;

			// プロパティへ列挙されたディレクトリを格納
			WorkSpaceDirectries.AddRange(Directory.GetDirectories(workSpaceDirectry));
		}
	}
}
