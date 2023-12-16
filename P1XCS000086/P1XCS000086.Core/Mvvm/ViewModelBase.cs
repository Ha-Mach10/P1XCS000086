using Prism.Mvvm;
using Prism.Navigation;

namespace P1XCS000086.Core.Mvvm
{
	public abstract class ViewModelBase : BindableBase, IDestructible
	{
		protected ViewModelBase()
		{

		}

		public virtual void Destroy()
		{

		}
	}
}
