# Overview

`DaisyIconText` combines an icon and optional text with size-aware spacing and coloring. Use `IconSymbol` for built-in symbols or `IconData` for custom geometry. When both are set, `IconData` takes precedence.

## Examples

```xml
<controls:DaisyIconText IconSymbol="Save" Text="Save" Variant="Primary" />
<controls:DaisyIconText IconSymbol="Share" Text="Share"
                        IconPlacement="Right" Variant="Secondary" />
<controls:DaisyIconText IconSymbol="Home" Text="Home"
                        IconPlacement="Top" Variant="Accent" />
<controls:DaisyIconText IconData="M12 4v16m8-8H4" Size="Large" />
```

`IconPlacement` accepts `Left`, `Right`, `Top`, and `Bottom`. `IconSize`, `FontSizeOverride`, and `Spacing` can override the values derived from `Size`.

## Foreground Colors

Text and icons share the control's `Foreground`. Set it explicitly to override the variant color.
Primary, Secondary, and Accent use `DaisyPrimaryForegroundBrush`, `DaisySecondaryForegroundBrush`, and `DaisyAccentForegroundBrush`.
These resources normally follow the corresponding palette colors. The Black theme uses `DaisyBaseContentColor` for readable text and icons.
Foreground resources are separate from the variant fill brushes and the content brushes used on filled backgrounds.
