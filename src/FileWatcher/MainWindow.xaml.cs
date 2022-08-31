using System.Windows;
using System.Windows.Controls;
using FileWatcher.Logic;
using FileWatcher.ViewModels;

namespace FileWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly System.Windows.Forms.NotifyIcon notifier;

        public MainWindow()
        {
            InitializeComponent();

            var permissionsChecker = new PermissionsChecker();
            var permissionsElevator = new PermissionsElevator();
            var filesWatcher = new FilesWatcher();

            DataContext = new MainWindowViewModel(permissionsChecker, permissionsElevator, filesWatcher);

            notifier = new System.Windows.Forms.NotifyIcon
            {
                Icon = new System.Drawing.Icon("../../src/FileWatcher/Resources/icon.ico"),
                Visible = true
            };
            notifier.MouseDown += Notifier_MouseDown;
        }

        private void Notifier_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");

                menu.IsOpen = true;
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Menu_Show(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        private void Menu_Close(object sender, RoutedEventArgs e)
        {
            notifier.MouseDown -= Notifier_MouseDown;

            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Dispose();
            }

            System.Windows.Application.Current.Shutdown();
        }
    }
}
