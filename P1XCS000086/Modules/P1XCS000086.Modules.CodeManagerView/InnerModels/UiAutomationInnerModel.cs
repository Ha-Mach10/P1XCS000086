using P1XCS000086.Services.Interfaces.Models.CodeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace P1XCS000086.Modules.CodeManagerView.InnerModels
{
	internal static class UiAutomationInnerModel
	{
		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		public static async Task<AutomationElement> GetMainWindowElemnt(ICodeRegisterModel model, int delayTicks)
		{
			var hWnd = await model.FindProcessMainwindowHandle(delayTicks);
			return AutomationElement.FromHandle(hWnd);
		}
		public static void CloseWindow(AutomationElement window)
		{
			if (window != null) return;

			if (window.GetSupportedPatterns().Contains(WindowPattern.Pattern))
			{
				WindowPattern windowPattern = window.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
				windowPattern.Close();
			}
		}
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent">親である<see cref="AutomationElement"/></param>
		/// <param name="elementName">目的の<see cref="AutomationElement"/>のLocalizeControlTypeの名称</param>
		/// <param name="scrollPattern">戻す</param>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public static bool TryGetScrollableListViewElement(AutomationElement parent, string elementName, out ScrollPattern scrollPattern)
		{
			// ツリーウォーカーを宣言
			TreeWalker walker = TreeWalker.ControlViewWalker;

			while (true)
			{
				// 指定のコントロールタイプが見つかるまでコンティニュー
				if (FindElementByLocalizeControlType(parent, elementName) is null) continue;

				try
				{
					// 親要素を取得する
					var controlType = walker.GetParent(FindElementByLocalizeControlType(parent, elementName).Last());

					// 取得した親要素が"ScrollPattern"を持っている場合
					if (controlType.GetSupportedPatterns().Contains(ScrollPattern.Pattern) is false) continue;

					// ScrollPatternを取得
					var scrollPatt = controlType.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;

					// 
					scrollPattern = scrollPatt;
					return scrollPatt.Current.VerticallyScrollable;
				}
				catch(InvalidOperationException ex)
				{
					// 処理が失敗した際の戻り値
					scrollPattern = null;
					return false;
				}
			}
		}
		/// <summary>
		/// スクロール
		/// </summary>
		/// <param name="scrollPattern"></param>
		public static void ScrollableElementScrolling(ScrollPattern scrollPattern)
		{
			while (true)
			{
				scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);

				if (scrollPattern.Current.VerticalScrollPercent is 100) break;
			}

			Task.Delay(2000);
		}
		public static List<(string name, string helpText)> GetListViewContents(AutomationElement element, string localizeElementType, string first = "", string second = "")
		{
			if (TryGetScrollableListViewElement(element, localizeElementType, out ScrollPattern scrollPattern))
			{
				ScrollableElementScrolling(scrollPattern);
			}

			// ScrollItemPattern scrollItemPatten = null;
			SelectionItemPattern selectionItemPattrn = null;

			bool isEnter = false;

			List<(string, string)> nameAndHelpText = new();

			foreach (var item in FindElementByLocalizeControlType(element, localizeElementType))
			{
				// 指定のパターンが含まれていない場合、処理を抜ける
				if (item.GetSupportedPatterns().Contains(SelectionItemPattern.Pattern) is false) continue;


				selectionItemPattrn = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;

				// 
				if (item.Current.Name == first) isEnter = true;
				if (item.Current.Name == second) isEnter = false;

				if (isEnter)
				{
					// 文字列に「*** is [un]pinned」が含まれている場合、foreachステートメントの最初からやりなおす
					if (Regex.IsMatch(item.Current.Name, ".+ is [un]*pinned"))
					{
						continue;
					}

					nameAndHelpText.Add((item.Current.Name, item.Current.HelpText));
				}

				// 現在のitemを選択
				selectionItemPattrn.Select();
			}

			return nameAndHelpText;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="automationId"></param>
		public static void PushButtonById(AutomationElement element, string automationId)
		{
			// ボタンコントロールの取得
			InvokePattern button = FindElementById(element, automationId).GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			button.Invoke();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="name"></param>
		public static void PushButtonByName(AutomationElement element, string name, int ticks = 1000)
		{
			// ボタンコントロールの取得
			InvokePattern button = FindElementByName(element, name).First().GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			button.Invoke();

			// 処理を待機する
			Task.Delay(ticks);
		}



		// ---------------------------------------------------------------
		// Private Methods
		// --------------------------------------------------------------- 



		/// <summary>
		/// 指定されたautomationIdに一致するAutomationElementを取得
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="automationId"></param>
		/// <returns></returns>
		private static AutomationElement FindElementById(AutomationElement rootElement, string automationId)
			=> rootElement
			.FindFirst(TreeScope.Element | TreeScope.Descendants | TreeScope.Subtree, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
		
		/// <summary>
		/// 指定された名前に一致するAutomationElementのコレクションをIEnumerableで返す
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private static IEnumerable<AutomationElement> FindElementByName(AutomationElement rootElement, string name)
			=> rootElement
			.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name))
			.Cast<AutomationElement>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="localizedControlType"></param>
		/// <returns></returns>
		private static IEnumerable<AutomationElement> FindElementByLocalizeControlType(AutomationElement rootElement, string localizedControlType)
			=>rootElement
			.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, localizedControlType))
			.Cast<AutomationElement>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		private static IEnumerable<AutomationElement> FindElementAll(AutomationElement rootElement, string a)
			=>rootElement
			.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.FrameworkIdProperty, a))
			.Cast<AutomationElement>();
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rootElement"></param>
		/// <param name="className"></param>
		/// <returns></returns>
		private static IEnumerable<AutomationElement> FindElementClassName(AutomationElement rootElement, string className)
			=> rootElement
			.FindAll(TreeScope.Element | TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, className))
			.Cast<AutomationElement>();
	}
}
