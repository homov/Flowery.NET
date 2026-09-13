using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Flowery.Controls;

/// <summary>
/// Visual group separator used by theme dropdown item templates. It is not a selectable item.
/// </summary>
internal sealed class ThemeDropdownSeparator : Border
{
    public static readonly StyledProperty<bool> ShowSeparatorProperty =
        AvaloniaProperty.Register<ThemeDropdownSeparator, bool>(nameof(ShowSeparator));

    public bool ShowSeparator
    {
        get => GetValue(ShowSeparatorProperty);
        set => SetValue(ShowSeparatorProperty, value);
    }

    static ThemeDropdownSeparator()
    {
        IsVisibleProperty.OverrideDefaultValue<ThemeDropdownSeparator>(false);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateVisibility();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ShowSeparatorProperty)
            UpdateVisibility();
    }

    private void UpdateVisibility() =>
        SetCurrentValue(IsVisibleProperty, ShowSeparator && this.FindAncestorOfType<ComboBoxItem>() != null);

    protected override void OnPointerPressed(PointerPressedEventArgs e) => e.Handled = true;

    protected override void OnPointerReleased(PointerReleasedEventArgs e) => e.Handled = true;

}
