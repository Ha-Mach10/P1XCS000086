using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Media.Animation;

namespace P1XCS000086.Modules.CodeManagerView.InnerModels
{
	internal static class UiAutomationInnerModel
	{
		/// <summary>
		/// ウィンドウの表示形式を変更する。
		/// </summary>
		/// <param name="window">ウィンドウを表す<see cref="AutomationElement"/></param>
		/// <param name="state">ウィンドウの状態を管理する<see cref="WindowVisualState"/>.\nNormal：通常のサイズ, Maximized：最大化, Minimized：最小化</param>
		/// <returns><see cref="bool"/>で値を返す.成功：true, 失敗：false</returns>
		public static bool MainWindowChangeScreen(AutomationElement window, WindowVisualState state)
		{
			// パターンを設定
			var pattern = WindowPattern.Pattern;

			// 渡されたAutomationElementにWindowPatternが含まれていない場合、この処理を抜ける
			if (window.GetSupportedPatterns().Contains(pattern) is false) return false;

			// ウィンドウのパターンを取得
			WindowPattern windowPattern = window.GetCurrentPattern(pattern) as WindowPattern;
			// ウィンドウの表示形式を変更
			windowPattern.SetWindowVisualState(state);

			return true;
		}
		public static AutomationElement GetListViewElement()
		{

		}



	}
}
