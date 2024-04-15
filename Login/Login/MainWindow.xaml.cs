using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string pathFileIni;
        public static string pathFolderIni;     
        string pathSql = "";
        string ip = "";
        string IPUser = "";
        string RemUser = "";
        string RemPass = "";
        string Rem = "";
         string verSQL, bufferExe, fileType, libraryName;
        //public static bool SaveLimitValue = false;
        string verFramework = "";
        string db_user = "sa";
        string db_pass = "oneuser1!";
        string nameApplication = "PC_Manage";
        string nameFolderIni = "PcManage";
        string nameFolderExe   = @"C:\TaixinProgram\PcManage\PcManage\";
        bool CheckRememberLogin = false;
        bool CheckShowPass = false;
        bool checkLogin = false;
        
        public static string UserLogin;
        public static string PassLogin;
        public MainWindow()
        {
            InitializeComponent();
            
            pb_Pass.Visibility = Visibility.Visible;
            txtPass.Visibility = Visibility.Hidden;
            btnShowPass.Visibility = Visibility.Hidden;
            btnHidenPass.Visibility = Visibility.Visible;           
            pathSql = @"Data Source= 192.168.2.5;Initial Catalog=" + nameApplication + ";Persist Security Info=True;User ID=" + db_user + ";Password=" + db_pass + "";
            IPUser = GetIPAddress();
            Process_RememberUser();
        }

        public void Process_RememberUser()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(pathSql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select top(1)* from TblUserIPLogin where IPLogin = '" + IPUser + "'ORDER BY ID DESC", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    RemUser = dr[1].ToString();
                                    RemPass = dr[2].ToString();
                                    Rem = dr[3].ToString();
                                }
                            }
                        }
                    }
                    conn.Close();
                }

                if(Rem == "True")
                {
                    txt_User.Text = RemUser;
                    pb_Pass.Password = RemPass;
                    txt_User.IsEnabled = false;
                    pb_Pass.IsEnabled = false;
                    ckbRemember.IsChecked = true;

                }
                else
                {
                    txt_User.Text = "";
                    pb_Pass.Password = "";
                    txt_User.IsEnabled = true;
                    pb_Pass.IsEnabled = true;
                    ckbRemember.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Process_RememberUser" + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void Db_Read_Version()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(pathSql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select top(1)* from FileUpdate order by ID desc", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    libraryName = dr[6].ToString();
                                    fileType = dr[2].ToString();
                                    verSQL = dr[4].ToString();
                                    bufferExe = dr[5].ToString();
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadVersion_SQLserver" + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       
        public void Db_Read_Employee()
        {
            try
            {
                checkLogin = false;
                List<string> list = new List<string>();
                using (SqlConnection conn = new SqlConnection(pathSql))
                {
                    conn.Open();
                    {
                        var command = "SELECT emp_code,password FROM pf_employee";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    list.Add(dr[0].ToString());
                                    if (dr[0] != null)
                                    {
                                        if (txt_User.Text.ToUpper() == dr[0].ToString().Trim().ToUpper() && (txtPass.Text.ToUpper() == dr[1].ToString().Trim().ToUpper() || pb_Pass.Password.ToUpper() == dr[1].ToString().Trim().ToUpper()))
                                        {
                                            checkLogin = true;
                                        }
                                    }
                                }

                            }
                        }
                       
                    }                   
                    conn.Close();
                }
                if(checkLogin == false)
                {
                    MessageBox.Show("Bạn đã nhập sai User hoặc Password, mời bạn nhập lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtPass.Text = "";
                    txt_User.Text = "";
                    pb_Pass.Password = "";
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadVersion_SQLserver" + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
       
        public void Process_UpdateExeFile()
        {
            Db_Read_Version();
            string fileExePCManage = nameFolderExe + nameFolderIni + ".exe";
            
            byte[] buffer = File.ReadAllBytes(fileExePCManage);
            string base64Encoded = Convert.ToBase64String(buffer);
            if(base64Encoded != bufferExe)
            {
                
                byte[] buferNew = System.Convert.FromBase64String(bufferExe);
                string fileExeNew = System.Text.ASCIIEncoding.ASCII.GetString(buferNew);
                //string fileExeNew = 
                File.WriteAllBytes(fileExePCManage, buferNew);
                Process.Start(fileExePCManage);
            }
            else
            {
                Process.Start(fileExePCManage);
            }    
            

            //Process.Start(fileExeOperasystem);
            this.Close();
        }

        

       
        public void User_Login()
        {
            try
            {               
                Db_Read_Employee();
                if (checkLogin == true)
                {
                    
                    SaveUserIPLogin();
                    //MainWindow Main = new MainWindow();
                    //UserLogin = txt_User.Text.ToUpper();
                    //PassLogin = pb_Pass.Password.ToUpper();
                    Process_UpdateExeFile();
                    //Main.Show();
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckLoginInput : " + ex.Message, "Login/MainWindow", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveUserIPLogin()
        {
            using (SqlConnection conn = new SqlConnection(pathSql))
            {
                conn.Open();
                var command = "Insert TblUserIPLogin(UserLogin,Pass,Remember,IPLogin,DateLogin) values('" + txt_User.Text.ToUpper() + "','" + pb_Pass.Password.ToUpper() + "','" + ckbRemember.IsChecked + "','" + IPUser + "', GETDATE())";
                using(SqlCommand cmd = new SqlCommand(command,conn))
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }    
                conn.Close();
            }    
        }

        private string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            User_Login();
        }
        private void Pb_Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User_Login();
            }
        }

        private void Txt_User_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Txt_User_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pb_Pass.Focus();
            }
        }

        private void TxtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User_Login();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

       

        private void CkbRemember_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckRememberLogin = false;        
            txtPass.Text = "";
            txt_User.Text = "";
            pb_Pass.Password = "";
            ckbRemember.IsChecked = false;
            txt_User.IsEnabled = true;
            txtPass.IsEnabled = true;
            pb_Pass.IsEnabled = true;
            btnHidenPass.IsEnabled = true;
        }

        private void CkbRemember_Checked(object sender, RoutedEventArgs e)
        {
            CheckRememberLogin = true;
            ckbRemember.IsChecked = true;
            txt_User.IsEnabled = false;
            txtPass.IsEnabled = false;
            pb_Pass.IsEnabled = false;
        }

        private void Btn_ShowPass_Click(object sender, RoutedEventArgs e)
        {
            pb_Pass.Password = txtPass.Text;
            pb_Pass.Visibility = Visibility.Visible;
            txtPass.Visibility = Visibility.Hidden;
            btnShowPass.Visibility = Visibility.Hidden;
            btnHidenPass.Visibility = Visibility.Visible;
            CheckShowPass = false;
        }

        private void Btn_HidenPass_Click(object sender, RoutedEventArgs e)
        {
            txtPass.Text = pb_Pass.Password;
            pb_Pass.Visibility = Visibility.Hidden;
            txtPass.Visibility = Visibility.Visible;
            btnShowPass.Visibility = Visibility.Visible;
            btnHidenPass.Visibility = Visibility.Hidden;
            CheckShowPass = true;
        }

        public static bool IsUserAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
   
   
}
