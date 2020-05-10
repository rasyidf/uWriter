using System;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using uWriter.Contracts.Services;
using uWriter.Models;

namespace uWriter.ViewModels
{ 
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly AppConfig _config;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ISystemService _systemService;
        private readonly IApplicationInfoService _applicationInfoService;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;

        public AppTheme Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { SetProperty(ref _versionDescription, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new DelegateCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new DelegateCommand(OnPrivacyStatement));

        public SettingsViewModel(AppConfig config, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
        {
            _config = config;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
            _applicationInfoService = applicationInfoService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VersionDescription = $"uWriter - {_applicationInfoService.GetVersion()}";
            Theme = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnSetTheme(string themeName)
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
            => _systemService.OpenInWebBrowser(_config.PrivacyStatement);

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;
    }
}
