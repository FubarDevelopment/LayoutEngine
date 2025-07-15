using System.Windows;
using System.Windows.Controls;

using FubarDev.LayoutEngine;
using FubarDev.LayoutEngine.Elements;

using static FubarDev.LayoutEngine.WpfBuilderMethods;
using static FubarDev.LayoutEngine.AttachedProperties.AttachedSize;

using LE = FubarDev.LayoutEngine;

namespace TestLayoutEngineWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILayoutRoot _layoutRoot;

        public MainWindow()
        {
            InitializeComponent();
            _layoutRoot = CreateFillingLayout();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // DumpInfo();
        }

        private void ScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // DumpInfo();
        }

        /*
        private void DumpInfo()
        {
            Debug.WriteLine("-----------------------------------");
            Debug.WriteLine($"Scrollable Size: {scrollViewer.ScrollableWidth}/{scrollViewer.ScrollableHeight}");
            Debug.WriteLine($"Extent Size: {scrollViewer.ExtentWidth}/{scrollViewer.ExtentHeight}");
            Debug.WriteLine($"Viewport: {scrollViewer.ViewportWidth}/{scrollViewer.ViewportHeight}");
            Debug.WriteLine($"Size: {scrollViewer.ActualWidth}/{scrollViewer.ActualHeight}");
            Debug.WriteLine($"Content Offset: {scrollViewer.ContentHorizontalOffset}/{scrollViewer.ContentVerticalOffset}");
            Debug.WriteLine($"Offset: {scrollViewer.HorizontalOffset}/{scrollViewer.VerticalOffset}");
            Debug.WriteLine("===================================");
            _layoutRoot.DumpLayout();
        }
        */

        private ILayoutRoot CreateCenteredLayout()
            => (CreateRoot(this, Orientation.Horizontal).Name("root")
                << (Pane(Orientation.Vertical).Name("nonDashboardArea").Width(Factor(1))
                    << (Pane().Name("headerArea").HorizontalStackLayout(LE.VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Item().Name("spacerTop").Height(Factor(1))
                    << (Pane().Name("mainArea").HorizontalStackLayout(LE.VerticalAlignment.Center)
                        << Item().Name("spacerLeft").Width(Factor(1))
                        << Item(pnlMenu)
                        << Item(pnlDialogView)
                        << Item().Name("spacerRight").Width(Factor(1)))
                    << Item().Name("spacerBottom").Height(Factor(1)))
                << Item(pnlDashboard))
                .Build();

        private ILayoutRoot CreateFillingLayout()
            => (CreateRoot(this, Orientation.Horizontal).Name("root")
                << (Pane(Orientation.Vertical).Name("nonDashboardArea").Width(Factor(1))
                    << (Pane().Name("headerArea").HorizontalStackLayout(LE.VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Item().Name("spacerTop").Height(Fixed(100))
                    << (Pane().Name("mainArea").HorizontalStackLayout(LE.VerticalAlignment.Fill).Height(Factor(1))
                        << Item(pnlMenu)
                        << Item(pnlDialogView).Width(Factor(1))))
                << Item(pnlDashboard))
                .Build();

        private ILayoutRoot CreateOverlapLayout()
            => (CreateRoot(this, Orientation.Horizontal)
                << (Pane(Orientation.Vertical).Width(Factor(1))
                    << (Pane().HorizontalStackLayout(LE.VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Pane().Height(Factor(1)).Identifier("a"))
                << Item(pnlDashboard))
                .AddOverlap(
                    "a",
                    Pane(Orientation.Vertical)
                    << Item().Height(Factor(1))
                    << (Pane().HorizontalStackLayout(LE.VerticalAlignment.Center)
                        << Item().Width(Factor(1))
                        << Item(pnlMenu)
                        << Item(pnlDialogView)
                        << Item().Width(Factor(1)))
                    << Item().Height(Factor(1)))
                .Build();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var minSize = _layoutRoot.ApplyMinimumSize();
            var borderSize = _layoutRoot.Size - _layoutRoot.ClientSize;
            var minimumSize = minSize + borderSize;
            MinWidth = minimumSize.Width;
            MinHeight = minimumSize.Height;
            _layoutRoot.Layout();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _layoutRoot.Layout();
        }
    }
}
