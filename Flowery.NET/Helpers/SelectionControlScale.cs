using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Flowery.Services;

namespace Flowery.Controls;

/// <summary>Combines the current font-size resource and inherited scale without local FontSize writes.</summary>
internal sealed class SelectionControlScale : IMultiValueConverter
{
    public static readonly AttachedProperty<double> BaseFontSizeProperty =
        AvaloniaProperty.RegisterAttached<Control, Control, double>("BaseFontSize", 14d);

    public static SelectionControlScale Instance { get; } = new();
    public static IValueConverter Indicator { get; } = new IndicatorScaleConverter();

    public static double GetBaseFontSize(Control control) => control.GetValue(BaseFontSizeProperty);
    public static void SetBaseFontSize(Control control, double value) => control.SetValue(BaseFontSizeProperty, value);

    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) =>
        values is [double fontSize, double factor]
            ? Math.Max(1d, fontSize * FloweryScaleManager.SanitizeScaleFactor(factor))
            : AvaloniaProperty.UnsetValue;

    private sealed class IndicatorScaleConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var factor = FloweryScaleManager.SanitizeScaleFactor(value is double scale ? scale : 1d);
            return new ScaleTransform { ScaleX = factor, ScaleY = factor };
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
