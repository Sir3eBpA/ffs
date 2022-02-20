using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FFS.Controls
{
    /// <summary>
    /// Taken from here:
    /// https://www.pmichaels.net/2017/03/12/wpf-performance-textblock/
    /// 
    /// And slightly adapted:
    /// - Formatted Text uses the new override with PixelsPerDip
    /// - Added foreground color brush
    ///
    /// The reason for this class is to eliminate extra overhead from the text block, even tho text block is already fast by itself we still benefit from shrinking a few ms off
    /// </summary>
    public class FastTextBlock : FrameworkElement
    {
        private FormattedText _formattedText;

        static FastTextBlock()
        {
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(FastTextBlock),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure,
                    (o, e) => ((FastTextBlock)o).TextPropertyChanged((string)e.NewValue)));

        public static readonly DependencyProperty ForegroundProperty = 
            DependencyProperty.Register(
                "Foreground", 
                typeof(Brush),
                typeof(FastTextBlock),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        private void TextPropertyChanged(string text)
        {
            var typeface = new Typeface(
                new FontFamily("Times New Roman"),
                FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            _formattedText = new FormattedText(
                text, 
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                15,
                Foreground,
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_formattedText != null)
            {
                drawingContext.DrawText(_formattedText, new Point());
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //return base.MeasureOverride(constraint);

            return _formattedText != null
                ? new Size(_formattedText.Width, _formattedText.Height)
                : new Size();
        }
    }
}
