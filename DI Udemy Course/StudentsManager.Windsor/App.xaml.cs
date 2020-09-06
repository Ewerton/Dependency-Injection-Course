using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using StudentsManager.DataAccess;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;
using StudentsManager.ViewModels;
using StudentsManager.Views;

namespace StudentsManager.Windsor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private WindsorContainer _container;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();        
            ComposeGraph();

            Application.Current.MainWindow.Show();
        }
        private void ConfigureContainer()
        {
            _container = new WindsorContainer();
            
            // Castle.Windsor exige que você registre todos os tipo concretos manualmente, porém, é possível usar o "workaround" abaixo para registrar várias de uma vez
            _container.Register(Classes.FromAssemblyContaining<MainViewModel>()
                .Where(type=>type.IsPublic && type.IsClass)
                .WithService.DefaultInterfaces()
                .LifestyleTransient(),

                Classes.FromAssemblyContaining<MainView>()
                    .Where(type => type.IsPublic && type.IsClass)
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());

            _container.Register(Component.For<IDataProvider<Student>>()
                    .ImplementedBy<StudentsXmlProvider>()
                    .DependsOn(Dependency.OnValue<string>(@"..\..\StudentsRepo.xml"))
                    .LifestyleTransient()               
            );
        }

        private void ComposeGraph()
        {            
            Application.Current.MainWindow = _container.Resolve<MainView>();
        }
    }   
}
