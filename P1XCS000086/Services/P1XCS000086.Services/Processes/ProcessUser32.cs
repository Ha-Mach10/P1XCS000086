using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static P1XCS000086.Services.Models.CodeManager.CodeRegisterModel;

namespace P1XCS000086.Services.Processes
{
	internal static class ProcessUser32
	{
		// *********************************************************************
		// Refernces
		// *********************************************************************

		// Title : C#で他アプリケーションを操作するための基礎知識(Hatena Blog)
		// Url   : https://tech.sanwasystem.com/entry/2015/11/25/171004

		// Title : メッセージとメッセージキューについて(MSD)
		// Url   : https://learn.microsoft.com/ja-jp/windows/win32/winmsg/about-messages-and-message-queues?redirectedfrom=MSDN

		// Title : Windowsで他プロセスを操る(Qiita)
		// Url   : https://qiita.com/kumaS-kumachan/items/8ff65a7215ff21ed15f5




		// *********************************************************************
		// Constants
		// *********************************************************************

		// user32.dll 操作メッセージの定数値
		// 参考サイト：

		// マウスボタン操作のメッセージ
		public const int WM_LBUTTONDOWN = 0x201;
		public const int WM_LBUTTONUP = 0x202;
		public const int MK_LBUTTON = 0x0001;



		// *********************************************************************
		// Properties
		// *********************************************************************

		/// <summary>
		/// クラス名とウィンドウタイトルを格納するタプルリスト
		/// </summary>
		public static List<(string ClassName, string WindowTitle, int TextLen)> ProcessValues { get; private set; } = new();



		// *********************************************************************
		// Delegates
		// *********************************************************************

		/// <summary>
		/// ウィンドウ列挙のデリゲート
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <param name="lparam"></param>
		/// <returns></returns>
		public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);



		// *********************************************************************
		// user32.dll Methods
		// *********************************************************************

		/// <summary>
		/// 画面上の最上位ウィンドウを列挙するC++関数
		/// </summary>
		/// <param name="lpEnumFunc">アプリケーション定義のコールバック関数へのポインター</param>
		/// <param name="lparam">コールバック関数に渡されるアプリケーション定義の値</param>
		/// <returns>成功時、0以外の値を返す。失敗時、0を返す</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

		/// <summary>
		/// 指定したクラス名、タイトルを持つ要素のハンドルを取得
		/// </summary>
		/// <param name="lpClassName">指定するクラス名</param>
		/// <param name="lpWindowName">指定するタイトル名</param>
		/// <returns>指定した要素のハンドル。指定したものが無ければ0が戻る</returns>
		[DllImport("user32.dll")]
		public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// 指定した子要素を取得。一件ずつしか取得できないので、第二引数でどの子要素をとるか指定
		/// </summary>
		/// <param name="hWnd">親要素のウィンドウハンドル</param>
		/// <param name="hwndChildAfter">このハンドルの次の子要素を取得</param>
		/// <param name="lpszClass">クラス名を指定。nullで全て可</param>
		/// <param name="lpszWindow">タイトルを指定。nullで全て可</param>
		/// <returns>指定した子要素のハンドル</returns>
		[DllImport("user32.dll")]
		public extern static IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		/// <summary>
		/// ウィンドウのタイトルバーのテキスト、またはコントロール内のテキストを取得するC++関数
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドルを指定</param>
		/// <param name="lpString">文字列バッファへのポインタを指定し、このバッファへ取得したテキストが格納される</param>
		/// <param name="nMaxCount">lpStringバッファのサイズ（終端のNULL文字を含む）をバイト単位で指定</param>
		/// <returns>成功時、バッファにコピーされたテキストのバイト数（終端のNULL文字を含む）を返す。失敗した場合、0を返す</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		/// <summary>
		/// ウィンドウのタイトルバーのテキスト、またはコントロール内のテキストのバイト数を取得するC++関数
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドルを指定</param>
		/// <returns>失敗した場合、0を返す</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private extern static int GetWindowTextLength(IntPtr hWnd);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドルを指定</param>
		/// <param name="lpClassName">クラス名の文字列</param>
		/// <param name="nMaxCount">lpClassNameバッファのサイズ（終端のNULL文字を含む）をバイト単位で指定</param>
		/// <returns>成功時、バッファにコピーされた文字数を返す。失敗した場合、0を返す</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private extern static int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		/// <summary>
		/// 指定のウィンドウハンドルへメッセージを送信する
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="Msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

		/// <summary>
		/// 指定したハンドルが存在するか
		/// </summary>
		/// <param name="hWnd">存在確認の対象となるハンドル</param>
		/// <returns>存在する場合 true.存在しない場合 false</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool IsWindow(IntPtr hWnd);

		/// <summary>
		/// 指定したウィンドウの指定した属性を調べる
		/// </summary>
		/// <param name="hWnd">ウィンドウハンドル</param>
		/// <param name="nIndex">属性指定</param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);



		// *********************************************************************
		// Public Methods
		// *********************************************************************

		/// <summary>
		/// クラス名とそのウィンドウタイトルのタプルリストを取得
		/// </summary>
		/// <returns></returns>
		public static List<(string ClassName, string WindowTitle, int TextLen)> GetEnumWindowAndClassNames()
		{
			// ウィンドウ列挙関数を実行（処理内容はコールバックメソッドを参照）
			EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

			// 
			return ProcessValues;
		}
		public static int GetWindowTitleLen(string processName, string windowTitle)
		{
			GetEnumWindowAndClassNames();

			if (ProcessValues.Count > 0)
			{
				int textLen = ProcessValues.Where(x => x.ClassName == processName && x.WindowTitle == windowTitle).First().TextLen;
				return textLen;
			}

			return 0;
		}
		public static List<Window> GetAllChildWindows(Window parent, List<Window> dest)
		{
			dest.Add(parent);
			EnumChildWindows(parent.hWnd).ToList().ForEach(x => GetAllChildWindows(x, dest));
			
			return dest;
		}
		public static IEnumerable<Window> EnumChildWindows(IntPtr hParentWindow)
		{
			// null
			IntPtr hWnd = IntPtr.Zero;
			while ((hWnd = FindWindowEx(hParentWindow, hWnd, null, null)) != IntPtr.Zero)
			{
				yield return GetWindow(hWnd);
			}
		}
		public static Window GetWindow(IntPtr hWnd)
		{
			int textLen = GetWindowTextLength(hWnd);
			string windowText = null;
            if (0 < textLen)
            {
				StringBuilder windowTextBuffer = new(textLen + 1);
				GetWindowText(hWnd, windowTextBuffer, windowTextBuffer.Capacity);
				windowText = windowTextBuffer.ToString();
            }

			// 
			StringBuilder classNameBuffer = new(256);
			GetClassName(hWnd, classNameBuffer, classNameBuffer.Capacity);

			// Window構造体を返す
			return new Window()
			{
				hWnd = hWnd,
				Title = windowText,
				ClassName = classNameBuffer.ToString(),
			};
        }



		// *********************************************************************
		// Private Methods
		// *********************************************************************

		/// <summary>
		/// EnumWindowsのコールバック関数
		/// </summary>
		/// <param name="hWnd">トップレベルウィンドウのハンドル</param>
		/// <param name="lparam">EnumWindowsまたはEnumDesktopWindowsで指定されたアプリケーション定義の値</param>
		/// <returns>列挙の続行：true、列挙の停止：false</returns>
		private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
		{
			// リストのクリア
			ProcessValues.Clear();


			// ウィンドウのタイトルの長さを取得
			int textLen = GetWindowTextLength(hWnd);
			if (0 < textLen)
			{
				// ウィンドウのタイトルを取得
				StringBuilder tsb = new StringBuilder(textLen + 1);
				GetWindowText(hWnd, tsb, tsb.Capacity);

				// ウィンドウのクラス名を取得
				StringBuilder csb = new StringBuilder(256);
				GetClassName(hWnd, csb, csb.Capacity);

				ProcessValues.Add((tsb.ToString(), csb.ToString(), textLen));
			}

			return true;
		}
	}
}
