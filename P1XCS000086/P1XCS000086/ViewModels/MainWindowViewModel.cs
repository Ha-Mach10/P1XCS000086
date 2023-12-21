using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;

using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace P1XCS000086.ViewModels
{
	public class MainWindowViewModel : BindableBase, IDestructible
	{
		private string _title = "Prism Application";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private IRegionManager _regionManager;
		private CompositeDisposable disposables = new CompositeDisposable(); 


		public ReactivePropertySlim<string> LanguageSelectedValue { get; }
		public ReactivePropertySlim<string> DevelopmentSelectedValue { get; }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="regionManager"></param>
		public MainWindowViewModel(IRegionManager regionManager)
		{
			_regionManager = regionManager;

			// Properties
			LanguageSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);
			DevelopmentSelectedValue = new ReactivePropertySlim<string>(string.Empty).AddTo(disposables);

			// Command
			LanguageTypeComboChange = new ReactiveCommand();
			LanguageTypeComboChange.Subscribe(() => OnLanguageTypeComboChange()).AddTo(disposables);
			DevelopmentTypeComboChange = new ReactiveCommand();
			DevelopmentTypeComboChange.Subscribe(() => OnDevelopmentTypeComboChange()).AddTo(disposables);
		}


		public ReactiveCommand LanguageTypeComboChange { get; }
		private void OnLanguageTypeComboChange()
		{

		}
		public ReactiveCommand DevelopmentTypeComboChange { get; }
		private void OnDevelopmentTypeComboChange()
		{

		}


		// 「IDestructible」の実装
		public void Destroy()
		{
			disposables.Dispose();
		}
	}
}
