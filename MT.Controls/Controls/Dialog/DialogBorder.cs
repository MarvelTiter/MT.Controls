using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MT.Controls
{
    [TemplatePart(Name = ConfirmButton, Type = typeof(Button))]
    [TemplatePart(Name = CloseButton, Type = typeof(Button))]
    public class DialogBorder : ContentControl
    {
        private const string ConfirmButton = "PART_ConfirmButton";
        private const string CloseButton = "PART_CloseButton";
        public override void OnApplyTemplate()
        {
            var confirmBtn = GetTemplateChild(ConfirmButton) as Button;
            var closeBtn = GetTemplateChild(CloseButton) as Button;
            if (confirmBtn == null || closeBtn == null)
            {
                throw new Exception($"missing confirm button name:[ {ConfirmButton} ] or close button name:[ {CloseButton} ]");
            }
            else
            {
                confirmBtn.Command = ControlCommands.Save;
                closeBtn.Command = ControlCommands.Close;
            }            
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DialogBorder), new PropertyMetadata(""));
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(DialogBorder), new PropertyMetadata(null, OnChanged));


        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Footer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(object), typeof(DialogBorder), new PropertyMetadata(null, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var border = d as DialogBorder;
            Debug.WriteLine(border.Title);
        }
    }
}
