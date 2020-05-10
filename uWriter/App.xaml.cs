using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

using uWriter.Constants;
using uWriter.Contracts.Services;
using uWriter.Core.Contracts.Services;
using uWriter.Core.Services;
using uWriter.Models;
using uWriter.Services;
using uWriter.ViewModels;
using uWriter.Views;

namespace uWriter
{
    public partial class App : PrismApplication
    {
        private string[] _startUpArgs;

        public App()
        {
        }

        protected override Window CreateShell()
            => Container.Resolve<ShellWindow>();

        protected override async void OnInitialized()
        {
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();

            var themeSelectorService = Container.Resolve<IThemeSelectorService>();
            themeSelectorService.SetTheme();

            base.OnInitialized();
            await Task.CompletedTask;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _startUpArgs = e.Args;
            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
            containerRegistry.Register<IFileService, FileService>();

            // App Services
            containerRegistry.Register<IApplicationInfoService, ApplicationInfoService>();
            containerRegistry.Register<ISystemService, SystemService>();
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            containerRegistry.Register<IThemeSelectorService, ThemeSelectorService>();

            // Views
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>(PageKeys.Settings);
            containerRegistry.RegisterForNavigation<PublishPage, PublishViewModel>(PageKeys.Publish);
            containerRegistry.RegisterForNavigation<SchedulePage, ScheduleViewModel>(PageKeys.Schedule);
            containerRegistry.RegisterForNavigation<OrganizePage, OrganizeViewModel>(PageKeys.Organize);
            containerRegistry.RegisterForNavigation<OutlinePage, OutlineViewModel>(PageKeys.Outline);
            containerRegistry.RegisterForNavigation<PlotPage, PlotViewModel>(PageKeys.Plot);
            containerRegistry.RegisterForNavigation<WritePage, WriteViewModel>(PageKeys.Write);
            containerRegistry.RegisterForNavigation<DashboardPage, DashboardViewModel>(PageKeys.Dashboard);
            containerRegistry.RegisterForNavigation<ShellWindow, ShellViewModel>();

            // Configuration
            var configuration = BuildConfiguration();
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            containerRegistry.RegisterInstance<IConfiguration>(configuration);
            containerRegistry.RegisterInstance<AppConfig>(appConfig);
        }

        private IConfiguration BuildConfiguration()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .AddCommandLine(_startUpArgs)
                .Build();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.PersistData();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            
            // e.Handled = true;
        }
    }
}
