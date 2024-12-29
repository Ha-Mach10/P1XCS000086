using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.MovDirectryManager
{
	public interface IMovieDirectryManagerModel
	{
		// ---------------------------------------------------------------
		// Properties
		// --------------------------------------------------------------- 

		public List<string> WorkSpaceDirectries { get; }



		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		public void SetNeedInitializeProperties(string workSpaceDirectory);
		public IEnumerable<string> GetMovieDirectoryFiles(string movieFileDirectory);
	}
}
