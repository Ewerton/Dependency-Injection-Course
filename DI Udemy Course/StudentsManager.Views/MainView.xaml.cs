using System.Windows;
using StudentsManager.ViewModels;

namespace StudentsManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void ExitFromApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }        
    }
}
