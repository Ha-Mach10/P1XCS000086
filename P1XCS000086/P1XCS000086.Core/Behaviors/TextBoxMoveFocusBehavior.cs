using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace P1XCS000086.Core.Behaviors
{
	public class TextBoxMoveFocusBehavior : Behavior<TextBox>
	{
		private static int _textBoxTabIndex;



		/// <summary>
		/// イベント登録
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			// 
			AssociatedObject.Loaded += OnLoaded;
			// TextBoxのキーダウンイベントにOnKeyDownを追加
			AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
			// TextBoxのフォーカス取得イベントにOnTextBoxGotFocusを追加
			AssociatedObject.GotFocus += OnTextBoxGotFocus;
		}
		/// <summary>
		/// イベント解除
		/// </summary>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			// 
			AssociatedObject.Loaded -= OnLoaded;
			// TextBoxのキーダウンイベントからOnPreviewKeyDownを削除
			AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
			// TextBoxのフォーカス取得イベントからOnTextBoxGotFocusを削除
			AssociatedObject.GotFocus -= OnTextBoxGotFocus;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (sender is TextBox textBox)
			{
				if (textBox.TabIndex == _textBoxTabIndex)
				{
					textBox.Focus();
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (sender is TextBox textBox)
			{
				// Focus処理用のリクエストを宣言
				TraversalRequest request = null;

				switch (e.Key)
				{
					// Enterキー押下時
					case Key.Enter:
						// Shiftキーが押されているかを取得
						bool isPressShift = Keyboard.Modifiers.Equals(ModifierKeys.Shift);

						// トラバーサル要求の取得（ナビゲーション方向を取得：次のコントロール）
						request = new TraversalRequest(isPressShift ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
						break;

					default:
						break;
				}

				// リクエストを処理する
				if (request is not null)
				{
					textBox.MoveFocus(request);
				}
			}
		}
		private void OnTextBoxGotFocus(object sender, RoutedEventArgs args)
		{
			// senderがTextBox以外の場合
			if (sender is TextBox textBox)
			{
				// コントロールを取得
				_textBoxTabIndex = textBox.TabIndex;

				// テキストを全選択
				textBox.SelectAll();
			}
		}
	}
}
