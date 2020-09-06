using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using StudentsManager.CSV;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;
using StudentsManager.Views;

namespace StudentsManager.Unity
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityContainer _container;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeGraph();

            Current.MainWindow.Show();
        }

        private void ComposeGraph()
        {
            // MainView depende de MainViewModel, o Unit "vê" isso e tenta automaticamente registrar o MainViewModel
            // Ou seja, não é necessário registrar todos os tipos concretos pois o Unity registra-os, recursivamente. 
            // Isso é uma peculiaridade do Unity, outros DI, geralmente não fazem isso.
            // De qualeur forma, se vc quise registrar tudo manualmente, para deixar explícito, não tem problema.
            Current.MainWindow = _container.Resolve<MainView>(); 
        }

        private void ConfigureContainer()
        {
            _container = new UnityContainer();
            _container.RegisterType<StudentsCsvProvider>(new InjectionConstructor(@"..\..\StudentsRepo.csv"));
            _container.RegisterType<IDataProvider<Student>, StudentsCsvProvider>(
                new ContainerControlledLifetimeManager() //singleton - one instance shared by all objects that depend on IDataProvider<Student>
            );

            /*
            _container.RegisterType<IDataProvider<Student>, StudentsCsvProvider>(
                new TransientLifetimeManager() //instance per request
                new PerThreadLifetimeManager() //single instance per thread
            );
            */
        }
    }
}
