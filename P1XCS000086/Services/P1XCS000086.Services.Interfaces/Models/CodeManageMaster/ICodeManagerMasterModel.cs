using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Models.CodeManageMaster
{
	public interface ICodeManagerMasterModel
	{
		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたモデルを注入
		/// </summary>
		/// <param name="integrModel">統合モデル</param>
		public void InjectModels(IIntegrMasterModel integrModel);
	}
}
