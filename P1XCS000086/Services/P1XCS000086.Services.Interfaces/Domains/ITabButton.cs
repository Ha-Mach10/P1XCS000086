using Reactive.Bindings;
using Reactive;

namespace P1XCS000086.Services.Interfaces.Domains
{
	public interface ITabButton
	{
		// ****************************************************************************
		// Properties
		// ****************************************************************************

		public string Header { get; }
		public string RegionName { get; }
		public string ViewName { get; }



		// ****************************************************************************
		// Public Methods
		// ****************************************************************************

		/// <summary>
		/// 親のインターフェースプロパティをコピー
		/// </summary>
		/// <param name="tabButton"></param>
		public void CopyParent(ITabButton tabButton);



		// ****************************************************************************
		// Reactive Commands
		// ****************************************************************************
		
		public ReactiveCommandSlim Close { get; }
	}
}
