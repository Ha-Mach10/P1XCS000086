using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces.Models.CodeManager
{
    public interface IVSCreateDialogModel
    {
		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		/// <summary>
		/// Visal Studio 2022を起動する
		/// </summary>
		public void AwakeVS2022();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Task<IntPtr> FindProcessMainwindowHandle(int delayTicks);
	}
}
