using MahApps.Metro.Controls;

using Prism.Regions;

using uWriter.Constants;

namespace uWriter.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(hamburgerMenuContentControl, Regions.Main);
            RegionManager.SetRegionManager(hamburgerMenuContentControl, regionManager);
        }

        private void Hamburger_Click(object sender, System.Windows.RoutedEventArgs e)
        { 
            Navs.IsPaneOpen = !Navs.IsPaneOpen;
        }
    }
}
