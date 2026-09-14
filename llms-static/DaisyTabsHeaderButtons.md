<!-- Supplementary documentation for DaisyTabsHeaderButtons -->
<!-- This content is merged into auto-generated docs by generate_docs.py -->

## Overview

DaisyTabsHeaderButtons provides optional previous and next buttons and a menu of named views.
It works in a DaisyTabs header or as a separate toolbar control.
The buttons use the Default variant, Small size, and Square shape by default.
The application handles navigation, view selection, and view management through events or commands.

## Options

| Property | Default | Behavior |
| -------- | ------- | -------- |
| ShowNavigationButtons | false | Shows previous and next buttons |
| ShowViewMenuButton | false | Shows the view menu button |
| Views | null | Supplies an `IList<DaisyTabView>` for the menu |
| ActiveViewId | null | Identifies the checked view through object equality |
| ShowManageViewsEntry | true | Adds the localized management entry after the views |
| Size | Small | Sets the size of the three buttons and icons |

Each `DaisyTabView` has a required `object Id` and a required `string Name`.
Use unique identifiers. The control reads the list and active identifier each time the menu opens.

The management entry has a separator when view entries precede it.
With no views, the menu contains only the management entry.
If the view list and management entry are absent, the menu button is disabled unless `ViewMenuOpening` has a handler.

## Events and Commands

| Event | Command | Command parameter |
| ----- | ------- | ----------------- |
| PreviousRequested | PreviousCommand | null |
| NextRequested | NextCommand | null |
| ViewSelected | ViewSelectedCommand | Selected view Id |
| ManageViewsRequested | ManageViewsCommand | null |

`ViewSelected` supplies `DaisyTabViewEventArgs` with `Id` and `Name`.
The other events use `EventArgs`. If an event handler and command are both assigned, both receive the action.
Commands use the standard Avalonia `CanExecute` behavior. Disabled commands disable their buttons or menu entries.

The control does not change `ActiveViewId` when a user selects a view.
The application sets this property after it accepts the request.
The control does not change tab visibility, persist views, or open a management dialog.

## Quick Examples

### Navigation in a tab header

```xml
<controls:DaisyTabs>
    <controls:DaisyTabs.HeaderTrailingContent>
        <controls:DaisyTabsHeaderButtons ShowNavigationButtons="True"
                                       PreviousCommand="{Binding PreviousCommand}"
                                       NextCommand="{Binding NextCommand}" />
    </controls:DaisyTabs.HeaderTrailingContent>
    <TabItem Header="Records"><TextBlock Text="Record content" /></TabItem>
</controls:DaisyTabs>
```

### View menu with MVVM

```xml
<controls:DaisyTabsHeaderButtons ShowViewMenuButton="True"
                               Views="{Binding Views}"
                               ActiveViewId="{Binding ActiveViewId}"
                               ViewSelectedCommand="{Binding SelectViewCommand}"
                               ManageViewsCommand="{Binding ManageViewsCommand}"
                               ShowManageViewsEntry="{Binding CanManageViews}" />
```

### Built-in group

```xml
<controls:DaisyTabs x:Name="RecordTabs" ShowHeaderButtons="True">
    <TabItem Header="Records"><TextBlock Text="Record content" /></TabItem>
</controls:DaisyTabs>
```

```csharp
var buttons = RecordTabs.HeaderButtons!;
buttons.ShowNavigationButtons = true;
buttons.ShowViewMenuButton = true;
buttons.Views = new ObservableCollection<DaisyTabView>
{
    new() { Id = 1, Name = "Overview" },
    new() { Id = 2, Name = "Details" },
    new() { Id = 3, Name = "Archive" }
};
buttons.ActiveViewId = 2;
buttons.ViewSelected += OnViewSelected;

void OnViewSelected(object? sender, DaisyTabViewEventArgs e)
{
    if (sender is DaisyTabsHeaderButtons source)
        source.ActiveViewId = e.Id;
}
```

### Separate toolbar control

```xml
<StackPanel Orientation="Horizontal" Spacing="8">
    <TextBlock Text="Records" VerticalAlignment="Center" />
    <controls:DaisyTabsHeaderButtons ShowNavigationButtons="True"
                                   ShowViewMenuButton="True"
                                   Views="{Binding Views}"
                                   PreviousRequested="OnPrevious"
                                   NextRequested="OnNext"
                                   ViewSelected="OnViewSelected"
                                   ManageViewsRequested="OnManageViews" />
</StackPanel>
```

## Load Views Before Opening

`ViewMenuOpening` uses `EventHandler<CancelEventArgs>` and fires before the control builds the menu entries.
The handler can load `Views`, set `ActiveViewId`, and update `ShowManageViewsEntry` after it checks permissions.
Setting `Cancel` to true prevents both the menu rebuild and popup display.

A subscribed handler keeps the menu button available even when the view list is initially empty.
If the handler leaves no entries, the control cancels the empty popup.
The event is synchronous. Load the list in the handler, or prepare data asynchronously before the user opens the menu.

```csharp
buttons.ViewMenuOpening += OnViewMenuOpening;

void OnViewMenuOpening(object? sender, CancelEventArgs e)
{
    if (!CanReadViews())
    {
        e.Cancel = true;
        return;
    }

    if (sender is DaisyTabsHeaderButtons source)
    {
        source.Views = LoadViews();
        source.ShowManageViewsEntry = CanManageViews();
    }
}
```

The application supplies `CanReadViews`, `LoadViews`, and `CanManageViews` in this example.
`CancelEventArgs` is in `System.ComponentModel`.

## Collection Updates

The menu always reads the current `Views` list when it opens.
An `ObservableCollection<DaisyTabView>` also updates button availability immediately when its contents change.
A plain list updates availability on the next layout pass or when `Views` is replaced.
Collection changes and property assignments must occur on the UI thread.

## Size and Scaling

`Size` supports ExtraSmall, Small, Medium, Large, and ExtraLarge.
Small is the default. With global sizing enabled, the group follows the current application size, including changes after attachment.
An explicit `Size` value, binding, or application style takes precedence. `FlowerySizeManager.IgnoreGlobalSize` opts out of global changes.
The group exposes `SizeProperty` for FlowerySizeManager and implements `IScalableControl` for FloweryScaleManager.
`ApplyScaleFactor` scales the font and icon dimensions. The normal button size resources set button dimensions.

## Localization and Accessibility

`Tabs_ManageViews` supplies the management entry text in all supported languages.
`Tabs_Views` supplies the view button label. Navigation buttons reuse the previous-item and next-item labels.
Each icon button has a localized tooltip and automation name.
Button labels follow culture changes, and menu labels use the culture at opening time.

---
