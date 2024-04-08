using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PcManage
{
    /// <summary>
    /// Interaction logic for Window_Application.xaml
    /// </summary>
    public partial class Window_Application : Window
    {
        public Window_Application()
        {
            InitializeComponent();
            Loaded += Window_Application_Loaded;
        }

        private void Window_Application_Loaded(object sender, RoutedEventArgs e)
        {            
            Db_Read_ApplicationSafe();           
            Db_Read_Persion();
        }

        string path_sql = "Data Source=192.168.2.5;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password= oneuser1!";
        string str_ApplicationSafe = "";      
        List<Helper_ApplicationInfo> listApp = new List<Helper_ApplicationInfo>();
        public void Db_Read_ApplicationSafe()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT * FROM ApplicationSafe";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null && dr[0].ToString() != "")
                                {
                                    str_ApplicationSafe += dr[0].ToString().ToUpper() + "|";
                                }
                            }
                            str_ApplicationSafe = str_ApplicationSafe.Remove(str_ApplicationSafe.Length - 1, 1);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                Application.Current.Shutdown();
            }
            
        }

        List<Helper_ApplicationInfo> listSafe2 = new List<Helper_ApplicationInfo>();
        public void Db_Read_ApplicationSafe2()
        {
            try
            {              
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT * FROM ApplicationSafe2";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null && dr[0].ToString() != "")
                                {
                                    Helper_ApplicationInfo safe2 = new Helper_ApplicationInfo();
                                    safe2.DisplayName = dr[0].ToString();
                                    safe2.Version = dr[1].ToString();
                                    safe2.Publisher = dr[2].ToString();
                                    listSafe2.Add(safe2);
                                }
                            }
                            
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                Application.Current.Shutdown();
            }

        }

        public void Db_Read_Persion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT DISTINCT(b.fullName),b.depatment,a.hdd1Serial from ApplicationInfo as a LEFT OUTER JOIN ComputerInfomation as b ON a.hdd1Serial=b.hdd1Serial order by b.depatment asc,b.fullName asc";
                    List<Helper_PcInfomation> listPer = new List<Helper_PcInfomation>();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null && dr[0].ToString() != "")
                                {
                                    Helper_PcInfomation per = new Helper_PcInfomation();
                                    per.fullName = dr[0].ToString();
                                    per.depatment = dr[1].ToString();
                                    per.hdd1Serial = dr[2].ToString();
                                    listPer.Add(per);
                                }
                            }

                        }
                    }
                    conn.Close();
                    lvPer.Items.Clear();
                    int index = 0;
                    foreach (var item in listPer)
                    {
                        item.ID = index;
                        lvPer.Items.Add(item);
                        index++;
                    }
                    numberPC.Content = listPer.Count.ToString();
                }
            }
            catch (Exception)
            {

                Application.Current.Shutdown();
            }
            
        }
       
        public void Db_Read_AllApplication(string hdd1)
        {
            try
            {
               
                string date = "";
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT TOP(1) * FROM ApplicationInfo where hdd1Serial = '" + hdd1 + "' ORDER BY insdt desc";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[2] != null && dr[2].ToString() != "")
                                {
                                    date = dr[8].ToString();
                                }
                            }
                        }
                    }
                }  
                if(date!="")
                {
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        var command = "SELECT * FROM ApplicationInfo where hdd1Serial='" + hdd1 + "' and monthUpdate ='"+date+"' ORDER BY Publisher ASC, DisplayName ASC";
                        listApp.Clear();
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (dr[2] != null && dr[2].ToString() != "")
                                    {
                                        Helper_ApplicationInfo app = new Helper_ApplicationInfo();
                                        app.pcno = dr[0].ToString();
                                        app.DisplayName = dr[2].ToString();
                                        app.Version = dr[3].ToString();
                                        app.InstalledDate = dr[4].ToString();
                                        app.Publisher = dr[5].ToString();
                                        app.UnninstallCommand = dr[6].ToString();
                                        app.ModifyPath = dr[7].ToString();
                                        app.ModifyPath = dr[8].ToString();
                                        listApp.Add(app);
                                    }
                                }

                            }
                        }
                        conn.Close();
                        int index = 0;
                        lvApplication.Items.Clear();
                        foreach (var item in listApp)
                        {
                            item.ID = index;
                            lvApplication.Items.Add(item);
                            index++;
                        }
                        lbQtyApp.Content = listApp.Count.ToString();
                    }
                }    
                
            }
            catch (Exception)
            {

                Application.Current.Shutdown();
            }
            
        }
        
        public void Db_Read_Applicaton_Risk(string search)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT DISTINCT(b.fullName),b.depatment,a.hdd1Serial from ApplicationInfo as a LEFT OUTER JOIN ComputerInfomation as b ON a.hdd1Serial=b.hdd1Serial WHERE a.DisplayName LIKE '%" + search + "%' OR a.Publisher LIKE '%" + search + "%' order by b.depatment asc,b.fullName asc";
                    List<Helper_PcInfomation> listPer = new List<Helper_PcInfomation>();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {

                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null && dr[0].ToString() != "")
                                {
                                    Helper_PcInfomation per = new Helper_PcInfomation();
                                    per.fullName = dr[0].ToString();
                                    per.depatment = dr[1].ToString();
                                    per.hdd1Serial = dr[2].ToString();
                                    listPer.Add(per);
                                }
                            }

                        }
                    }
                    conn.Close();
                    lvPer.Items.Clear();
                    int index = 0;
                    foreach (var item in listPer)
                    {
                        item.ID = index;
                        lvPer.Items.Add(item);
                        index++;
                    }
                    numberPC.Content = listPer.Count.ToString();
                }
            }
            catch (Exception)
            {
                Application.Current.Shutdown();
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string search = txtFilterInfo.Text;
            Db_Read_Applicaton_Risk(search);
        }


        string str_hdd1Serial = "";

        private void lvPer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ListView;
            var clickItem = click.SelectedItem as Helper_PcInfomation;
            if (clickItem != null && clickItem.fullName != "" && clickItem.fullName != null)
            {
                str_hdd1Serial = clickItem.hdd1Serial;
                Process_FilterData(str_hdd1Serial);
            }
            
        }


        public void Process_FilterData(string hdd1Serial)
        {
            Db_Read_AllApplication(hdd1Serial);
            lvApplication.Items.Clear();
            List<Helper_ApplicationInfo> listAppNG = new List<Helper_ApplicationInfo>();
            foreach (var item in listApp.OrderBy(X => X.Publisher))
            {
                bool match = Regex.IsMatch(item.Publisher.ToUpper(), str_ApplicationSafe);
                if (!match)
                {
                    listAppNG.Add(item);
                }
            }
            int index = 0;
            lvApplication.Items.Clear();
            foreach (var item in listAppNG)
            {
                if (item.pcno != "Safe")
                {
                    item.ID = index;
                    lvApplication.Items.Add(item);
                    index++;
                }
            }
            lbQtyApp.Content = listAppNG.Count.ToString();
        }

        private void btnFilterInfo_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            lvApplication.Items.Clear();
            foreach (var item in listApp)
            {
                item.ID = index;
                lvApplication.Items.Add(item);
                index++;
            }
            lbQtyApp.Content = listApp.Count.ToString();
        }

        private void btnFilter_OK_Click(object sender, RoutedEventArgs e)
        {
            lvApplication.Items.Clear();
            List<Helper_ApplicationInfo> listAppOK = new List<Helper_ApplicationInfo>();
            foreach (var item in listApp.OrderBy(X => X.Publisher))
            {
                bool match = Regex.IsMatch(item.Publisher.ToUpper(), str_ApplicationSafe);
                if (match)
                {
                    listAppOK.Add(item);
                }

            }
            int index = 0;
            lvApplication.Items.Clear();
            foreach (var item in listAppOK)
            {
                item.ID = index;
                lvApplication.Items.Add(item);
                index++;
            }
            lbQtyApp.Content = listAppOK.Count.ToString();
        }

        private void btnFilter_NG_Click(object sender, RoutedEventArgs e)
        {

            lvApplication.Items.Clear();
            List<Helper_ApplicationInfo> listAppNG = new List<Helper_ApplicationInfo>();
            foreach (var item in listApp.OrderBy(X => X.Publisher))
            {
                bool match = Regex.IsMatch(item.Publisher.ToUpper(), str_ApplicationSafe);
                if (!match)
                {
                    listAppNG.Add(item);
                }
            }
            int index = 0;
            lvApplication.Items.Clear();
            foreach (var item in listAppNG)
            {
                item.ID = index;
                lvApplication.Items.Add(item);
                index++;
            }
            lbQtyApp.Content = listAppNG.Count.ToString();
        }

        private void txtFilterInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Db_Read_Applicaton_Risk(txtFilterInfo.Text);
            }

        }

        private void btnFilter_NG1_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBoxResult.Yes== MessageBox.Show("Ứng dụng là Nguy Hiểm?","Thông báo",MessageBoxButton.YesNo,MessageBoxImage.Question))
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "UPDATE ApplicationInfo SET pcno = 'Null' DisplayName='" + clickData.DisplayName + "' and Version='" + clickData.Version + "' and Publisher='" + clickData.Publisher + "'";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Đã hoàn thành","Thông báo",MessageBoxButton.OK,MessageBoxImage.Information);
            }    
           
        }

        private void btnFilter_OK1_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Ứng dụng là An Toàn?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "UPDATE ApplicationInfo SET pcno = 'Safe' WHERE DisplayName='" + clickData.DisplayName + "' and Version='" + clickData.Version + "' and Publisher='" + clickData.Publisher + "'";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                MessageBox.Show("Đã hoàn thành", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Process_FilterData(str_hdd1Serial);
            }    
                
        }


        Helper_ApplicationInfo clickData = new Helper_ApplicationInfo();
        private void lvApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ListView;
            var clickItem = click.SelectedItem as Helper_ApplicationInfo;
            if(clickItem != null)
            {
                clickData = clickItem;
            }
        }

        private void MenuItem_Safe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Risk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
