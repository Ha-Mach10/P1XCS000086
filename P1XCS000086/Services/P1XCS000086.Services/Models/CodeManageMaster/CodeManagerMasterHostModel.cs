using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P1XCS000086.Services.Interfaces.Models.CodeManageMaster;

namespace P1XCS000086.Services.Models.CodeManageMaster
{
	public class CodeManagerMasterHostModel : ICodeManagerMasterHostModel
	{
		// ****************************************************************************
		// Fields
		// ****************************************************************************

		public IIntegrMasterModel _integrModel;



		// ****************************************************************************
		// Constructor
		// ****************************************************************************

		public CodeManagerMasterHostModel() { }




		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// DIされたモデルを注入
		/// </summary>
		/// <param name="integrModel">統合モデル</param>
		public void InjectModels(IIntegrMasterModel integrModel)
		{
			_integrModel = integrModel;
		}
	}
}
