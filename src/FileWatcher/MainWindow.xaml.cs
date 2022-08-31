using FileWatcher.Logic;
using FileWatcher.ViewModels;

namespace FileWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            var permissionsChecker = new PermissionsChecker();
            var permissionsElevator = new PermissionsElevator();
            var filesWatcher = new FilesWatcher();

            DataContext = new MainWindowViewModel(permissionsChecker, permissionsElevator, filesWatcher);

            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Dispose();
            }
        }
    }
}
