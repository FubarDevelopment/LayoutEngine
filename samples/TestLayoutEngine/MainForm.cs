using System.Drawing;
using System.Windows.Forms;

using FubarDev.LayoutEngine;
using FubarDev.LayoutEngine.Elements;

using static FubarDev.LayoutEngine.BuilderMethods;
using static FubarDev.LayoutEngine.AttachedProperties.AttachedSize;

namespace TestLayoutEngine
{
    public partial class MainForm : Form
    {
        private readonly ILayoutRoot _layoutRoot;

        public MainForm()
        {
            InitializeComponent();
            _layoutRoot = CreateFillingLayoutWithHwnd();
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            _layoutRoot.Layout();
        }

        private ILayoutRoot CreateCenteredLayout()
            => (CreateRoot(this, Orientation.Horizontal).Name("root")
                << (Pane(Orientation.Vertical).Name("nonDashboardArea").Width(Factor(1))
                    << (Pane().Name("headerArea").HorizontalStackLayout(VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Item().Name("spacerTop").Height(Factor(1))
                    << (Pane().Name("mainArea").HorizontalStackLayout(VerticalAlignment.Center)
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
                    << (Pane().Name("headerArea").HorizontalStackLayout(VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Item().Name("spacerTop").Height(Fixed(100))
                    << (Pane().Name("mainArea").HorizontalStackLayout(VerticalAlignment.Fill).Height(Factor(1))
                        << Item(pnlMenu)
                        << Item(pnlDialogView).Width(Factor(1))))
                << Item(pnlDashboard))
                .Build();

        private ILayoutRoot CreateFillingLayoutWithHwnd()
            => (CreateRoot((IWin32Window)this, Orientation.Horizontal).Name("root")
                << (Pane(Orientation.Vertical).Name("nonDashboardArea").Width(Factor(1))
                    << (Pane().Name("headerArea").HorizontalStackLayout(VerticalAlignment.Top)
                        << Item((IWin32Window)pnlLogo).Name("pnlLogo").Margin(new Margin(3, 4))
                        << Item((IWin32Window)pnlModuleSelector).Name("pnlModuleSelector").Margin(new Margin(3, 4)).Width(Factor(1)))
                    << Item().Name("spacerTop").Height(Fixed(100))
                    << (Pane().Name("mainArea").HorizontalStackLayout(VerticalAlignment.Fill).Height(Factor(1))
                        << Item((IWin32Window)pnlMenu).Name("pnlMenu").Margin(new Margin(3, 4)).MinimumSize(new Size(200, 400))
                        << Item((IWin32Window)pnlDialogView).Name("pnlDialogView").Margin(new Margin(3, 4)).MinimumSize(new Size(400, 400)).Width(Factor(1))))
                << Item((IWin32Window)pnlDashboard).Name("pnlDashboard").Margin(new Margin(3, 4)))
                .Build();

        private ILayoutRoot CreateOverlapLayout()
            => (CreateRoot(this, Orientation.Horizontal)
                << (Pane(Orientation.Vertical).Width(Factor(1))
                    << (Pane().HorizontalStackLayout(VerticalAlignment.Top)
                        << Item(pnlLogo)
                        << Item(pnlModuleSelector).Width(Factor(1)))
                    << Pane().Height(Factor(1)).Identifier("a"))
                << Item(pnlDashboard))
                .AddOverlap(
                    "a",
                    Pane(Orientation.Vertical)
                    << Item().Height(Factor(1))
                    << (Pane().HorizontalStackLayout(VerticalAlignment.Center)
                        << Item().Width(Factor(1))
                        << Item(pnlMenu)
                        << Item(pnlDialogView)
                        << Item().Width(Factor(1)))
                    << Item().Height(Factor(1)))
                .Build();

        private void Form1_VisibleChanged(object sender, System.EventArgs e)
        {
            var minSize = _layoutRoot.ApplyMinimumSize();
            var borderSize = Size - ClientSize;
            MinimumSize = minSize + borderSize;
        }

        private void btnDump_Click(object sender, System.EventArgs e)
        {
            _layoutRoot.DumpLayout();
        }
    }
}
