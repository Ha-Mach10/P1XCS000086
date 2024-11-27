using LibVLCSharp.WPF;
using LibVLCSharp.Shared;
using P1XCS000086.Core.Mvvm;
using P1XCS000086.Services.Interfaces.Models.MovDirectryManager;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;



namespace P1XCS000086.Modules.MovDirectryManager.ViewModels
{
	public class MovieDirectryManagerViewModel : RegionViewModelBase
	{
		// ---------------------------------------------------------------
		// ReadOnly Fields
		// --------------------------------------------------------------- 

		private readonly string s_wsDirName = @"E:\00_other_videos\e";



		// ---------------------------------------------------------------
		// Fields
		// --------------------------------------------------------------- 

		private IRegionManager _regionManager;
		private IMovieDirectryManagerModel _model;

		private string _mediaPlaingFilePath; 



		// ---------------------------------------------------------------
		// Reactive Properties
		// --------------------------------------------------------------- 

		public ReactivePropertySlim<string> WSDirectryName { get; }
		public ReactivePropertySlim<bool> IsReadOnlyWSDirectry { get; }

		public ReactiveCollection<string> WorkSpaceDirectries { get; }
		public ReactivePropertySlim<string> SelectedWSDirectry { get; }

		public ReactiveCollection<string> SourceMovieFiles { get; }

		// Vlc Media
		public ReactivePropertySlim<VideoView> VlcControl { get; }


		// ---------------------------------------------------------------
		// Constructor
		// --------------------------------------------------------------- 

		public MovieDirectryManagerViewModel(IRegionManager regionManager, IMovieDirectryManagerModel model)
			:base(regionManager)
		{
			_regionManager = regionManager;
			_model = model;

			// Keep Alive
			KeepAlive = false;


			// Properties Initialize
			WSDirectryName = new ReactivePropertySlim<string>(string.Empty);
			IsReadOnlyWSDirectry = new ReactivePropertySlim<bool>(false);

			WorkSpaceDirectries = new ReactiveCollection<string>().AddTo(_disposables);
			SelectedWSDirectry = new ReactivePropertySlim<string>(string.Empty);

			SourceMovieFiles = new ReactiveCollection<string>().AddTo(_disposables);

			// VlcMediaPlayer Setting
			LibVLCSharp.Shared.Core.Initialize();
			VlcControl = new ReactivePropertySlim<VideoView>();
			VlcControl.Value.MediaPlayer = new MediaPlayer(new LibVLC()).AddTo(_disposables);


			// Properties Setting
			WSDirectryName.Value = s_wsDirName;


			// Commands
			IsReadOnlyWSDirectryChange = new ReactiveCommandSlim();
			IsReadOnlyWSDirectryChange.Subscribe(OnIsReadOnlyWSDirectryChange).AddTo(_disposables);
			DirSelectionChanged = new ReactiveCommandSlim<string>();
			DirSelectionChanged.Subscribe(OnDirSelectionChanged).AddTo(_disposables);


			// Awake Collection Initialize Method
			CollectionsInitialize();
		}



		// ---------------------------------------------------------------
		// Commands
		// --------------------------------------------------------------- 

		public ReactiveCommandSlim IsReadOnlyWSDirectryChange { get; }
		private void OnIsReadOnlyWSDirectryChange()
		{
			IsReadOnlyWSDirectry.Value = !IsReadOnlyWSDirectry.Value;
		}
		public ReactiveCommandSlim<string> DirSelectionChanged { get; }
		private void OnDirSelectionChanged(string param)
		{
			_mediaPlaingFilePath = param;
		}



		// ---------------------------------------------------------------
		// Override
		// --------------------------------------------------------------- 

		public override void OnNavigatedFrom(NavigationContext navigationContext)
		{
			base.OnNavigatedFrom(navigationContext);
		}



		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 

		private void CollectionsInitialize()
		{
			// モデルがnullの場合、処理を抜ける
			if (_model is null) return;

			// モデルのプロパティ初期値設定を行う
			_model.SetNeedInitializeProperties(s_wsDirName);

			WorkSpaceDirectries.AddRangeOnScheduler(_model.WorkSpaceDirectries);
		}

		private string GetFullPath(string DirFileName)
			=> @$"{s_wsDirName}\{DirFileName}";
	}
}
