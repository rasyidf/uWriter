using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using MahApps.Metro.Controls;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using uWriter.Constants;
using uWriter.Properties;

namespace uWriter.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
        private HamburgerMenuItem _selectedMenuItem;
        private HamburgerMenuItem _selectedOptionsMenuItem;
        private DelegateCommand _goBackCommand;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;
        private ICommand _menuItemInvokedCommand;
        private ICommand _optionsMenuItemInvokedCommand;
        private bool _IsPaneOpen;
        private ICommand _hamburgerOpenCommand;

        public bool IsPaneOpen
        {
            get { return _IsPaneOpen; }
            set { SetProperty(ref _IsPaneOpen, value); }
        }

        public HamburgerMenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set { SetProperty(ref _selectedMenuItem, value); }
        }

        public HamburgerMenuItem SelectedOptionsMenuItem
        {
            get { return _selectedOptionsMenuItem; }
            set { SetProperty(ref _selectedOptionsMenuItem, value); }
        }

        public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
            new HamburgerMenuGlyphItem() { Label = Resources.ShellDashboardPage, Glyph = "\uF246", Tag = PageKeys.Dashboard },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellWritePage, Glyph = "\uE929", Tag = PageKeys.Write },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellPlotPage, Glyph = "\uE7B8", Tag = PageKeys.Plot },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellOutlinePage, Glyph = "\uE70B", Tag = PageKeys.Outline },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellOrganizePage, Glyph = "\uF168", Tag = PageKeys.Organize },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellSchedulePage, Glyph = "\uE787", Tag = PageKeys.Schedule }
        };

        public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
        {
            new HamburgerMenuGlyphItem() { Label = Resources.ShellPublishPage, Glyph = "\uE724", Tag = PageKeys.Publish },
            new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", Tag = PageKeys.Settings }
        };

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));
        public ICommand HamburgerOpenCommand => _hamburgerOpenCommand ?? (_hamburgerOpenCommand = new DelegateCommand(OnHamburgerOpen));

        private void OnHamburgerOpen()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        public ICommand LoadedCommand => _loadedCommand ??= new DelegateCommand(OnLoaded);

        public ICommand UnloadedCommand => _unloadedCommand ??= new DelegateCommand(OnUnloaded);

        public ICommand MenuItemInvokedCommand => _menuItemInvokedCommand ?? (_menuItemInvokedCommand = new DelegateCommand(OnMenuItemInvoked));

        public ICommand OptionsMenuItemInvokedCommand => _optionsMenuItemInvokedCommand ?? (_optionsMenuItemInvokedCommand = new DelegateCommand(OnOptionsMenuItemInvoked));

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _navigationService.Navigated += OnNavigated;
            SelectedMenuItem = MenuItems.First();
        }

        private void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
            _regionManager.Regions.Remove(Regions.Main);
        }

        private bool CanGoBack()
            => _navigationService != null && _navigationService.Journal.CanGoBack;

        private void OnGoBack()
            => _navigationService.Journal.GoBack();

        private void OnMenuItemInvoked()
            => RequestNavigate(SelectedMenuItem.Tag?.ToString());

        private void OnOptionsMenuItemInvoked()
            => RequestNavigate(SelectedOptionsMenuItem.Tag?.ToString());

        private void RequestNavigate(string target)
        {
            if (_navigationService.CanNavigate(target))
            {
                _navigationService.RequestNavigate(target);
            }
        }

        private void OnNavigated(object sender, RegionNavigationEventArgs e)
        {
            var item = MenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => e.Uri.ToString() == i.Tag?.ToString());
            if (item != null)
            {
                SelectedMenuItem = item;
            }
            else
            {
                SelectedOptionsMenuItem = OptionMenuItems
                        .OfType<HamburgerMenuItem>()
                        .FirstOrDefault(i => e.Uri.ToString() == i.Tag?.ToString());
            }

            IsPaneOpen = false;
            GoBackCommand.RaiseCanExecuteChanged();
        }
    }
}
