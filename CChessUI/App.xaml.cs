using CChessDatabase;
using CChessUI.Tools;
using CChessUI.Views;
using CommonServiceLocator;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfTools;
using WpfTools.Dialogs;

namespace CChessUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    var bootstrapper = new Bootstrapper();
        //    bootstrapper.Run();

        //    ShutdownMode = ShutdownMode.OnMainWindowClose;
        //}

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IDialogService>(new DialogService());
            containerRegistry.RegisterInstance<ICChessDatabaseService>(new CChessDatabaseService());
            containerRegistry.RegisterSingleton<IGameController, GameController>();
            containerRegistry.Register<ITool, FenSupport>("FenSupport");
            containerRegistry.Register<ITool, OpenFromDatabaseSupport>("OpenFromDatabaseSupport");
            containerRegistry.Register<ITool, SaveToDatabaseSupport>("SaveToDatabaseSupport");
            containerRegistry.Register<ITool, PgnSupport>("PgnSupport");
        }

        protected override Window CreateShell()
        {
            return new Shell();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping(typeof(StackPanel), new StackPanelRegionAdapter(this.Container.Resolve<IRegionBehaviorFactory>()));
        }


    }
}
