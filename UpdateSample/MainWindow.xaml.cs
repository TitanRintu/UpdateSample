using Squirrel;

using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            var taskResult =Task.Run(() => CheckForUpdateAsync());
            taskResult.Wait();  
            VersionNumberTextBlock.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            
        }

        private async Task CheckForUpdateAsync()
        {
            using (var manager = new UpdateManager(@"C:\Satya\UpdateSample", "Update Sample"))
            {
                var updateResult = await manager.CheckForUpdate();
                if (updateResult.ReleasesToApply.Count > 0)
                {
                    var result = MessageBox.Show("Update available, Do you want to update ?", 
                                                              "Update Sample", 
                                                              MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        await manager.UpdateApp();
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }

                }

            }
        }
    }
}
