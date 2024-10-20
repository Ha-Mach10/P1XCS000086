using P1XCS000086.Services.Interfaces.Models.CodeManager;
using P1XCS000086.Services.IO;
using P1XCS000086.Services.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P1XCS000086.Services.Models.CodeManager
{
    public class VSCreateDialogModel : IVSCreateDialogModel
    {
		// ---------------------------------------------------------------
		// Constant Fields
		// ---------------------------------------------------------------

		private const string VS2022SoftwarePath = @"C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe";



		// ---------------------------------------------------------------
		// Constructors
		// ---------------------------------------------------------------





		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		/// <summary>
		/// Visal Studio 2022を起動する
		/// </summary>
		public void AwakeVS2022() => Process.Start(VS2022SoftwarePath);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<IntPtr> FindProcessMainwindowHandle(int delayTicks)
		{
			// Visual Studio 2022を起動する(最新のバージョンを起動)
			AwakeVS2022();

			// 起動後待機（ミリ秒）
			await Task.Delay(delayTicks);

			// Visual Studio 2022のプロセス名とウィンドウタイトルを定数で宣言
			const string ProcessName = "devenv";
			const string WindowTitle = "Microsoft Visual Studio";

			List<Window> allElements = new();


			IEnumerable<Window> windows = Process.GetProcessesByName(ProcessName)
				.Select(x => x.MainWindowHandle)
				.Select(x => ProcessUser32.GetWindow(x));

			foreach (Window window in windows)
			{
				// 指定のクラス名、タイトルでない場合はコンティニュー
				if (window.ClassName != ProcessName && window.Title != WindowTitle) { continue; }

				while (true)
				{
					// 指定したウィンドウのハンドルが存在するか
					if (ProcessUser32.IsWindow(window.hWnd))
					{
						// 要素を全て取得し、処理を抜ける
						allElements = ProcessUser32.GetAllChildWindows(window, allElements);
						break;
					}
				}
			}

			return allElements.Select(x => x.hWnd).Last();
		}
	}
}
