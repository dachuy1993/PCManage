using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace PcManageServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Kiểm tra xem ứng dụng đã được đăng ký chạy ngầm chưa
            if (!IsApplicationRegisteredToRunOnStartup())
            {
                // Nếu chưa, thêm ứng dụng vào danh sách chạy ngầm
                RegisterApplicationToRunOnStartup();
            }

            // Hiển thị cửa sổ chính của ứng dụng
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private bool IsApplicationRegisteredToRunOnStartup()
        {
            // Kiểm tra xem ứng dụng đã được đăng ký chạy ngầm chưa
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                return (key.GetValue("StartupApp") != null);
            }
        }

        private void RegisterApplicationToRunOnStartup()
        {
            // Đăng ký ứng dụng chạy ngầm
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("StartupApp", Assembly.GetExecutingAssembly().Location);
            }
        }
    }

}
