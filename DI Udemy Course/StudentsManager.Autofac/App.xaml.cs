using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using StudentsManager.DataAccess;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;
using StudentsManager.Views;

namespace StudentsManager.Autofac
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeGraph();

            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            var container = new ContainerBuilder();

            // Existe a possibilidade de registrar todas as dependencias concretas manualmente, ou deixar que o Autofac descubra elas automaticamente usando AnyConcreteTypeNotAlreadyRegisteredSource()
            //container.RegisterType<MainView>().AsSelf();
            //container.RegisterType<MainViewModel>().AsSelf();
            
            // Registra todas as dependencias concretas ainda não registradas.
            container.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource()); //per dependency scope

            // Registra o IDataProvider (implementado por StudentsXmlProvider) e inicializa ele com o "BD" xml.
            container.RegisterType<StudentsXmlProvider>()
                     .As<IDataProvider<Student>>()
                     .WithParameter(new TypedParameter(typeof(string), @"..\..\StudentsRepo.xml"));
            
            _container = container.Build();
        }

        private void ComposeGraph()
        {

            // Autofac incentiva o uso de escopo para resolver a dependencias,
            using (var scope = _container.BeginLifetimeScope())
            {
                MainView view = scope.Resolve<MainView>();
                Application.Current.MainWindow = view;
            }            
        }
    }
}
