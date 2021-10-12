using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MT.Controls.Attachs
{
    public class TextBoxAttach
    {

        public static bool GetAutoHeight(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoHeightProperty);
        }

        public static void SetAutoHeight(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoHeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoHeightProperty =
            DependencyProperty.RegisterAttached("AutoHeight", typeof(bool), typeof(TextBoxAttach), new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox textBox))
            {
                return;
            }
            if (textBox != null)
            {
                textBox.Loaded += TextBox_Loaded;
                textBox.Unloaded += TextBox_Unloaded;
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private static void TextBox_Unloaded(object sender, RoutedEventArgs e)
        {
            var c = sender as TextBox;
            c.Unloaded -= TextBox_Unloaded;
            c.Loaded -= TextBox_Loaded;
            c.TextChanged -= TextBox_TextChanged;
        }

        private static void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var c = sender as TextBox;
            ReSize(c);
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var c = sender as TextBox;
            foreach (var item in e.Changes)
            {

            }
            if (c.IsLoaded)
                ReSize(c);
        }
        private static void ReSize(TextBox textBox)
        {
            if (!isNewLine(textBox, out var lines)) return;
            var lineHeight = (double)textBox.GetValue(TextBlock.LineHeightProperty);
            double height = lineHeight * lines;
            textBox.Height = height;

            bool isNewLine(TextBox tb, out int line)
            {
                line = tb.LineCount;
                if (tb.Tag == null)
                {
                    tb.Tag = line;
                }
                else
                {
                    var current = (int)tb.Tag;
                    if (current == line) return false;
                    else tb.Tag = line;
                }
                return true;
            }
        }
    }
}
