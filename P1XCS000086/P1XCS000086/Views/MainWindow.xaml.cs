using P1XCS000086.Properties;

using MahApps.Metro.Controls;

using System.ComponentModel;
using System;
using System.Windows;

namespace P1XCS000086.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			// ウィンドウ復元
			RecoverWindowBounds();

			// イベントをハンドル
			Closing += new CancelEventHandler(MainWindow_Closed);
		}

		#region WindowSize Save&Load
		protected override void OnClosing(CancelEventArgs e)
		{
			// ウィンドウサイズを保存
			SaveWindowBounds();
			base.OnClosing(e);
		}
		private void MainWindow_Closed(object sender, EventArgs e)
			=> (DataContext as IDisposable)?.Dispose();

		/// <summary>
		/// ウィンドウの位置・サイズを保存する
		/// </summary>
		void SaveWindowBounds()
		{
			var settings = Settings.Default;
			// 
			settings.windowMaximized = WindowState == WindowState.Maximized;
			WindowState = WindowState.Normal;
			// ウィンドウの左上の座標を保存
			settings.windowLeft = Left;
			settings.windowTop = Top;
			// ウィンドウのサイズを保存
			settings.windowWidth = Width;
			settings.windowHeight = Height;
			settings.Save();
		}

		/// <summary>
		/// ウィンドウの位置・サイズを復元
		/// </summary>
		void RecoverWindowBounds()
		{
			var settings = Settings.Default;

			// ウィンドウ左の座標を復元
			if (settings.windowLeft >= SystemParameters.VirtualScreenLeft &&
				(settings.windowLeft + settings.windowWidth) < SystemParameters.VirtualScreenWidth)
			{ Left = settings.windowLeft; }

			// ウィンドウ上の座標を復元
			if (settings.windowTop >= SystemParameters.VirtualScreenTop &&
				(settings.windowTop + settings.windowHeight) < SystemParameters.VirtualScreenHeight)
			{ Top = settings.windowTop; }

			// ウィンドウの幅を復元
			if (settings.windowWidth > 0 &&
				settings.windowWidth <= SystemParameters.WorkArea.Width)
			{ Width = settings.windowWidth; }

			// ウィンドウの高さを復元
			if (settings.windowHeight > 0 &&
				settings.windowHeight <= SystemParameters.WorkArea.Height)
			{ Height = settings.windowHeight; }

			// 最大化
			if (settings.windowMaximized)
			{
				// ロード後に最大化
				Loaded += (o, e) => WindowState = WindowState.Maximized;
			}
		}
		#endregion
	}
}
