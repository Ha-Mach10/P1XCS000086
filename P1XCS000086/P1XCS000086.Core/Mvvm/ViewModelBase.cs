using Prism.Mvvm;
using Prism.Navigation;

using System.Reactive;
using System.Reactive.Disposables;

namespace P1XCS000086.Core.Mvvm
{
	public abstract class ViewModelBase : BindableBase, IDestructible
	{
		protected CompositeDisposable _disposables;



		protected ViewModelBase()
		{
			_disposables = new CompositeDisposable();
		}

		public virtual void Destroy()
		{
			if (_disposables is not null)
			{
				_disposables.Dispose();
			}
		}
	}
}
