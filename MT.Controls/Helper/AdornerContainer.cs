using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MT.Controls
{

    public class AdornerContainer : AdornerDecorator
    {

    }
    public class AdornerMask : Adorner
    {
        private UIElement _child;
        public AdornerMask(UIElement adornedElement) : base(adornedElement)
        {
        }

        public UIElement Child
        {
            get => _child;
            set
            {
                if (value == null)
                {
                    RemoveVisualChild(_child);
                    // ReSharper disable once ExpressionIsAlwaysNull
                    _child = value;
                    return;
                }
                AddVisualChild(value);
                _child = value;
            }
        }
        protected override int VisualChildrenCount => _child != null ? 1 : 0;

        // 设置遮罩大小为容器大小
        protected override Size ArrangeOverride(Size finalSize)
        {
            _child?.Arrange(new Rect(finalSize));
            //Debug.WriteLine("ArrangeOverride" + finalSize.ToString());
            return finalSize;
        }

        //protected override Size MeasureOverride(Size constraint) {
        //    Debug.WriteLine("MeasureOverride" + constraint.ToString());
        //    return base.MeasureOverride(constraint);
        //}

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0 && _child != null) return _child;
            return base.GetVisualChild(index);
        }
    }
}
