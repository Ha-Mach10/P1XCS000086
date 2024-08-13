using Microsoft.Xaml.Behaviors;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace P1XCS000086.Core.Behaviors
{
    public class GridBehavior/* : Behavior<Grid>*/
    {
        private int _columnsCount = 0;

        /// <summary>
        /// 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ColumnsCountProperty =
            DependencyProperty.RegisterAttached(
                "ColumnsCount",
                typeof(int),
                typeof(GridBehavior),
                new PropertyMetadata(1, OnColumnsCount)
            );
        /// <summary>
        /// ゲッター
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int GetColumnsCount(DependencyObject target)
            => (int)target.GetValue(ColumnsCountProperty);

        /// <summary>
        /// セッター
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetColumnsCount(DependencyObject target, int value)
            => target.SetValue(ColumnsCountProperty, value);
        /// <summary>
        /// コールバックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnColumnsCount(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as Grid;
            if (element is null)
            {
                return;
            }

            // 更新された値を取得する
            var newValue = (int)e.NewValue;
            // element.ColumnDefinitions.Count() 
        }


        /*
        protected override void OnAttached()
        {
            base.OnAttached();

            // イベントの追加
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            // イベントの削除
            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                _columnsCount = grid.ColumnDefinitions.Count();
                SetColumnsCount(grid, _columnsCount);
            }
        }
        */
    }
}
