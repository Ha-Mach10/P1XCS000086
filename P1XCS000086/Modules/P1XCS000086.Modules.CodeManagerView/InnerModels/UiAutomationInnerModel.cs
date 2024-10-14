using P1XCS000086.Services.Interfaces.Models.CodeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace P1XCS000086.Modules.CodeManagerView.InnerModels
{
	internal static class UiAutomationInnerModel
	{
		// ---------------------------------------------------------------
		// Public Methods
		// --------------------------------------------------------------- 

		public static AutomationElement GetMainWindowElemnt(ICodeRegisterModel model, int delayTicks)
		{
			var hWnd = model.FindProcessMainwindowHandle(5000);
			return AutomationElement.FromHandle(hWnd.Result);
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
		/// <param name="parent"></param>
		/// <param name="elementName"></param>
		/// <param name="isScrollable"></param>
		/// <returns></returns>
		public static bool TryGetScrollableListViewElement(AutomationElement parent, string elementName, out ScrollPattern scrollPattern)
		{
			// ツリーウォーカーを宣言
			TreeWalker walker = TreeWalker.ControlViewWalker;

			// 
			IEnumerable<AutomationElement> children = FindElementByLocalizeControlType(parent, elementName);
			// 親要素を取得する
			var controlType = walker.GetParent(children.Last());

			// 取得した親要素が"ScrollPattern"を持っている場合
			if (controlType.GetSupportedPatterns().Contains(ScrollItemPattern.Pattern))
			{
				// ScrollPatternを取得
				var scrollPatt = controlType.GetCurrentPattern(ScrollItemPattern.Pattern) as ScrollPattern;
				scrollPattern = scrollPatt;
				return scrollPatt.Current.VerticallyScrollable;
			}

			scrollPattern = null;
			return false;
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
		}
		public static List<(string, string)> GetListViewContents(AutomationElement element, string localizeElementType, string a, string b)
		{
			if (TryGetScrollableListViewElement(element, localizeElementType, out ScrollPattern scrollPattern))
			{
				ScrollableElementScrolling(scrollPattern);
			}

			foreach (var item in FindElementByLocalizeControlType(element, localizeElementType))
			{
				if (item.GetSupportedPatterns().Contains(SynchronizedInputPattern.Pattern) &&
					item.GetSupportedPatterns().Contains(SelectionItemPattern.Pattern))
				{
					return null;
				}

				var synchronizedInputPattrn = item.GetCurrentPattern(SynchronizedInputPattern.Pattern) as SynchronizedInputPattern;
				var selectionItemPattrn = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
			}
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
		public static void PushButtonByName(AutomationElement element, string name)
		{
			// ボタンコントロールの取得
			InvokePattern button = FindElementByName(element, name).First().GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
			button.Invoke();
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
