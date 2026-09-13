using Avalonia;
using Avalonia.Controls;

namespace Flowery.Controls;

/// <summary>Supplies a global default without replacing styles, bindings, or local values.</summary>
internal sealed class DaisyGlobalSizeSubscription
{
    private readonly Control _owner;
    private readonly StyledProperty<DaisySize> _sizeProperty;
    private bool _attached;
    private bool _updating;
    private bool _providedDefault;

    public DaisyGlobalSizeSubscription(Control owner, StyledProperty<DaisySize> sizeProperty)
    {
        _owner = owner;
        _sizeProperty = sizeProperty;
        owner.AttachedToVisualTree += OnAttached;
        owner.DetachedFromVisualTree += OnDetached;
        owner.PropertyChanged += OnPropertyChanged;
    }

    private void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _attached = true;
        FlowerySizeManager.SizeChanged += OnSizeChanged;
        ApplySize();
    }

    private void OnDetached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _attached = false;
        FlowerySizeManager.SizeChanged -= OnSizeChanged;
        ClearDefault();
    }

    private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (_attached && !_updating && e.Property == _sizeProperty)
            ApplySize();
    }

    private void OnSizeChanged(object? sender, DaisySize size) => ApplySize();

    private void ApplySize()
    {
        if (_updating || _owner.GetBaseValue(_sizeProperty).HasValue || _owner.IsAnimating(_sizeProperty))
            return;
        _updating = true;
        try
        {
            if (FlowerySizeManager.EnableGlobalAutoSize && FlowerySizeManager.UseGlobalSizeByDefault &&
                !FlowerySizeManager.ShouldIgnoreGlobalSize(_owner))
            {
                // With no base value, SetCurrentValue retains default priority.
                _providedDefault = true;
                _owner.SetCurrentValue(_sizeProperty, FlowerySizeManager.CurrentSize);
            }
            else
                ClearDefault();
        }
        finally
        {
            _updating = false;
        }
    }

    private void ClearDefault()
    {
        if (_providedDefault && !_owner.GetBaseValue(_sizeProperty).HasValue && !_owner.IsAnimating(_sizeProperty))
            _owner.ClearValue(_sizeProperty);
        _providedDefault = false;
    }
}
