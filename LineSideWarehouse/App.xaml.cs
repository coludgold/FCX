using System.Windows;
using LineSideWarehouse.ViewModels;
using LineSideWarehouse.Services;

namespace LineSideWarehouse
{
    /// <summary>
    /// App.xaml.cs - 应用程序入口
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 初始化依赖注入并启动主窗口
            var warehouseService = new InMemoryWarehouseService();
            var viewModel = new MainViewModel(warehouseService);
            
            var mainWindow = new Views.MainWindow
            {
                DataContext = viewModel
            };
            mainWindow.Show();
        }
    }
}
