using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MT.Controls
{
    public enum Placement
    {
        Left,
        Top,
        Right,
        Bottom,
    }
    [TemplatePart(Name = PopName, Type = typeof(Popup))]
    [TemplatePart(Name = CloseButton, Type = typeof(Button))]
    public class ContentFlyout : ContentControl
    {
        private const string PopName = "PART_Popup";
        private const string CloseButton = "PART_CloseButton";
        private Popup _popup;
        private Button _button;
        public Point TargetPoint { get; set; }
        public Panel Container { get; set; }
        public Placement Placement
        {
            get { return (Placement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        public static readonly DependencyProperty PlacementProperty =
           DependencyProperty.Register(nameof(Placement), typeof(Placement), typeof(ContentFlyout), new PropertyMetadata(Placement.Bottom));
        public ContentFlyout()
        {
            CommandBindings.Add(new CommandBinding(ControlCommands.Toggle, (s, e) => Toggle()));
        }
        public override void OnApplyTemplate()
        {
            _popup = GetTemplateChild(PopName) as Popup;
            if (_popup != null)
            {
                _popup.CustomPopupPlacementCallback = SetPlacement;              
            }
            _button = GetTemplateChild(CloseButton) as Button;
            _button.Command = ControlCommands.Toggle;
        }

        private CustomPopupPlacement[] SetPlacement(Size popupSize, Size targetSize, Point offset)
        {
            var p = new Point();
            p.X = GetPlacementX(popupSize, targetSize);
            p.Y = GetPlacementY(popupSize, targetSize);
            CustomPopupPlacement placement = new CustomPopupPlacement(p, PopupPrimaryAxis.None);
            return new[] { placement };
        }

        private double GetPlacementX(Size popupSize, Size targetSize)
        {
            if (Placement == Placement.Left)
            {
                if (TargetPoint.X > popupSize.Width)
                    return -popupSize.Width;
                else
                    return targetSize.Width;
            }
            else if (Placement == Placement.Right)
            {
                if (TargetPoint.X + targetSize.Width + popupSize.Width < Container.RenderSize.Width)
                    return targetSize.Width;
                else
                    return -popupSize.Width;
            }
            else
            {
                var x = (targetSize.Width - popupSize.Width) / 2;
                // 超出左边缘
                if (TargetPoint.X < Math.Abs(x))
                    x = 0;
                // 超出右边缘
                if (Container.RenderSize.Width - TargetPoint.X - targetSize.Width < Math.Abs(x))
                    x = x * 2;
                return x;
            }
        }

        private double GetPlacementY(Size popupSize, Size targetSize)
        {
            if (Placement == Placement.Top)
            {
                if (TargetPoint.Y > popupSize.Height)
                    return -popupSize.Height;
                else
                    return targetSize.Height;
            }
            else if (Placement == Placement.Bottom)
            {
                if (TargetPoint.Y + targetSize.Height + popupSize.Height < Container.RenderSize.Height)
                    return targetSize.Height;
                else
                    return -popupSize.Height;
            }
            else
            {
                var y = (targetSize.Height - popupSize.Height) / 2;
                // 超出上边缘
                if (TargetPoint.Y < Math.Abs(y))
                    y = 0;
                // 超出下边缘
                if (Container.RenderSize.Height - TargetPoint.Y - targetSize.Height < Math.Abs(y))
                    y = y * 2;
                return y;
            }
        }

        public void SetTarget(UIElement target)
        {
            if (_popup == null)
            {
                return;
            }
            _popup.PlacementTarget = target;
        }

        public void Toggle()
        {
            if (_popup == null)
            {
                return;
            }
            _popup.IsOpen = !_popup.IsOpen;
        }
    }
}
