using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System.Reactive;
using System.Reactive.Disposables;

namespace P1XCS000086.Core.Mvvm
{
	public abstract class ViewModelBase : BindableBase, IDestructible, IRegionMemberLifetime
	{
		protected CompositeDisposable _disposables;

		public bool KeepAlive { get; private set; } = false;

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
