using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Flowery.Controls;
using Xunit;

namespace Flowery.NET.Tests
{
    public class DaisyTabsTests
    {
        [AvaloniaFact]
        public void NoneVariant_SelectedHeader_FitsSemiBoldTitle()
        {
            var selected = new TabItem { Header = "Einstellungen" };
            var tabs = new DaisyTabs { Variant = DaisyTabVariant.None };
            tabs.Items.Add(selected);
            tabs.Items.Add(new TabItem { Header = "Andere" });
            tabs.SelectedItem = selected;

            var window = new Window
            {
                Width = 600,
                Height = 200,
                Content = tabs
            };
            window.Show();
            Dispatcher.UIThread.RunJobs();
            window.UpdateLayout();

            var headerText = selected.GetVisualDescendants()
                .OfType<TextBlock>()
                .First();

            Assert.True(
                headerText.DesiredSize.Width <= headerText.Bounds.Width + 0.5,
                $"Selected header clips: desired {headerText.DesiredSize.Width}, arranged {headerText.Bounds.Width}.");
            Assert.Equal(FontWeight.SemiBold, selected.FontWeight);
        }
    }
}
