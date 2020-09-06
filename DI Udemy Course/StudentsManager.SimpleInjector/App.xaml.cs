using System.Windows;
using SimpleInjector;
using StudentsManager.CSV;
using StudentsManager.DataAccess;
using StudentsManager.DataAccess.Interface;
using StudentsManager.Models;
using StudentsManager.SimpleInjector;
using StudentsManager.ViewModels;
using StudentsManager.Views;

namespace StudentsManager.Autofac
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Container _container;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeGraph();

            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            // 1. Create a new Simple Injector container
            _container = new Container();

            // 2. Configure the container (register)
            _container.Register<IDataProvider<Student>>(() => new StudentsXmlProvider(@"..\..\StudentsRepo.xml")); // registra a classe students provider inicalizando ela com o caminho do XML
            //_container.Register<IDataProvider<Student>>(() => new StudentsCsvProvider(@"..\..\StudentsRepo.csv"))

            // Só eu quiser utilizar "auto-wiring" preciso configurar isso no container
            _container.Options.ResolveUnregisteredConcreteTypes = true;
            // Desta forma não preciso registrar todos os tipos da hierarquia, ou seja, basta registrar o primeiro Type da hierarquia pois o SimpleInjector 
            // analisa o construtor da classe MainView() e registrará as dependencias recursivamente analisando os construtores, logo
            // não preciso registrar explicitamente o MainViewModel() que é uma dependencia de MainView()
            _container.Register<MainView>(); // MainView não tem uma interface, registra o próprio tipo concreto
            //_container.Register<MainViewModel>(); // não é necessário pois  _container.Options.ResolveUnregisteredConcreteTypes é true 

            // 3. Verify your configuration
            _container.Verify();
        }

        private void ComposeGraph()
        {
            Current.MainWindow = _container.GetInstance<MainView>();
        }
    }
}
