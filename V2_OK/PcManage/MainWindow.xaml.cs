using DataHelper;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;

namespace PcManage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string Ver = "V4.0";      
        string path_sql = "Data Source=192.168.2.5,1433;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password= oneuser1!";
        List<Helper_DataButton> listButtonTop = new List<Helper_DataButton>();
        List<Helper_PcProcessData> listFileUploadServer = new List<Helper_PcProcessData>();
        Helper_AccessManger access_db = new Helper_AccessManger();
        DispatcherTimer dt = new DispatcherTimer();
        Helper_PcProcessData file_Exe_Create = new Helper_PcProcessData();
        Helper_PcInfomation pcClickItem = new Helper_PcInfomation();
        string command_AllData = "SELECT * FROM ComputerInfomation order by depatment asc,idNumber asc";    
        string str_FilterInfo = "";
        string dateExeFile = "";
        public bool checkBiosSerial = false;
        public bool checkUpdateVersion = false;
        List<Helper_PcProcessData> listGetFile = new List<Helper_PcProcessData>();
        int step_get = 0;
        List<Helper_PcProcessData> listFolder = new List<Helper_PcProcessData>();
        int min = 0;
        int sec = 0;
       
        public MainWindow()
        {
            InitializeComponent();
            lbVersion.Content = "Version: "+ Ver;
            Random rnd1 = new Random();
            Random rnd2 = new Random();
            min = rnd1.Next(1, 10);
            sec = rnd2.Next(1, 50);
            CreatAllButtonEdit();          
            Load_Read_AllInfoPC();
            Process_CheckBiosPC();
            //OnStartUp();



        } 
      
        public void Load_Read_AllInfoPC()
        {
            Db_Read_Employ();
            Db_Read_Depatment();
            Db_Read_TableSql(command_AllData);
            MainBoard_Info();
            Bios_Info();
            HDD_Info();
            IPAddress_Info();            
        }      

        public void Load_Read_UpdateNew()
        {
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
            var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
            string dateInset = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
            IPAddress_Info();
            HDD_Info();
            Ram_Info();
            CPU_Info();
            MainBoard_Info();
            Bios_Info();
            HardDiskInfo();
            OSInfo();
            PCInfomation.pcInfo.cmpcode = "02";
            PCInfomation.pcInfo.bizdiv = "300";
            PCInfomation.pcInfo.version = Ver;
            PCInfomation.pcInfo.monthUpdate = DateTime.Now.ToString("yyyy-MM");
            PCInfomation.pcInfo.insdt = dateInset;
        }
              
       
        public void CreatAllButtonEdit()
        {
            lvButtonTop.Items.Clear();
            listButtonTop.Clear();
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 1,
                ContentButton = "Add",
                ImageSource = "Image/Edit/add.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 2,
                ContentButton = "Del",
                ImageSource = "Image/Edit/delete.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 3,
                ContentButton = "Edit",
                ImageSource = "Image/Edit/edit.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 4,
                ContentButton = "Save",
                ImageSource = "Image/Edit/save.png",
                BackGroundColor = PinValue.OFF
            });
            listButtonTop.Add(new Helper_DataButton
            {
                ID = 5,
                ContentButton = "Print",
                ImageSource = "Image/Edit/printer.png",
                BackGroundColor = PinValue.OFF
            });

            foreach (var button in listButtonTop)
            {
                lvButtonTop.Items.Add(button);
            }

        }
               
        private void ButtonTop_Click(object sender, RoutedEventArgs e)
        {
            var click = sender as Button;
            var clickItem = click.DataContext as Helper_DataButton;
            if (clickItem != null)
            {
                switch (clickItem.ContentButton)
                {
                    case "Add":
                        {
                            ProcessButtonEdit_Add();
                            break;
                        }
                    case "Del":
                        {
                            ProcessButtonEdit_Del();
                            break;
                        }
                    case "Edit":
                        {
                            ProcessButtonEdit_Edit();
                            break;
                        }
                    case "Save":
                        {
                            ProcessButtonEdit_Save();
                            break;
                        }
                }
                foreach (var button in listButtonTop)
                {
                    button.BackGroundColor = PinValue.OFF;
                    if (button.ContentButton == clickItem.ContentButton)
                    {
                        button.BackGroundColor = PinValue.ON;
                    }
                }

            }
        }
        [STAThread]
        public static void OnStartUp()
        {
            


            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue("PcManage", "C:\\PcManage");
        }
            
        public void ProcessButtonEdit_Add()
        {
            txtFilterInfo.Text = "";
            rbPcNV.IsChecked = true;
            txtIdNumber.Text = "";
            txtIdNumber2.Text = "";
            txbFullName.Text = "";
            txbFullName2.Text = "";
            txtMonitorNo.Text = "";
            txtMonitorNo2.Text = "";
            txtMonitorNo3.Text = "";           
            txtMonitorModel.Text = "";
            txtMonitorModel2.Text = "";
            txtMonitorModel3.Text = "";
        }

        public void ProcessButtonEdit_Del()
        {           

            try
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Bạn muốn xóa thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();                       
                        var command1 = "DELETE ComputerInfomation where hdd1Serial='" + pcClickItem.hdd1Serial + "'";
                        var command2 = "DELETE PcRun where pcno='" + pcClickItem.pcno + "'";
                        using (SqlCommand cmd = new SqlCommand(command1, conn))
                        {
                            cmd.ExecuteNonQuery();                            
                        }
                        using (SqlCommand cmd = new SqlCommand(command2, conn))
                        {
                            cmd.ExecuteNonQuery();                            
                        }
                        conn.Close();
                    }
                    Db_Read_TableSql(command_AllData);
                    MessageBox.Show("Xóa dữ liệu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProcessButtonEdit_Del", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void ProcessButtonEdit_Edit()
        {
            try
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Bạn muốn sửa lại thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        string id1 = txtIdNumber.Text.Trim();
                        string name1 = txbFullName.Text.Trim();
                        string dep1 = cbb_Depatment.SelectedItem.ToString();
                        string id2 = txtIdNumber2.Text.Trim();
                        string name2 = txbFullName2.Text.Trim();
                        string dep2 = cbb_Depatment2.SelectedItem.ToString();
                        var command = "UPDATE ComputerInfomation SET idNumber =N'" + id1 + "',fullName=N'" + name1 + "',depatment=N'" + dep1 + "',idNumber2 =N'" + id2 + "',fullName2=N'" + name2 + "',depatment2=N'" + dep2 + "',ipAddress =N'" + txtFilterInfo.Text.Trim()+ "',monitorNo =N'" + txtMonitorNo.Text.ToUpper() + "',monitorModel =N'" + txtMonitorModel.Text.ToUpper() + "',monitorNo2 =N'" + txtMonitorNo2.Text.ToUpper() + "',monitorModel2 =N'" + txtMonitorModel2.Text.ToUpper() + "',monitorNo3 =N'" + txtMonitorNo3.Text.ToUpper() + "',monitorModel3 =N'" + txtMonitorModel3.Text.ToUpper() + "' where hdd1Serial='" + pcClickItem.hdd1Serial + "'";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    Db_Read_TableSql(command_AllData);
                    MessageBox.Show("Sửa thông tin thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProcessButtonEdit_Edit", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void ProcessUpdateInfo()
        {
            try
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Bạn muốn cập nhật thông tin?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();

                        var command1 = "Insert  ComputerInfomation_Backup(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                        "SELECT cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monthUpdate,imsempcode,insdt,N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "' FROM ComputerInfomation where hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "' ";
                        using (SqlCommand cmd = new SqlCommand(command1, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        //var command = "UPDATE ComputerInfomation SET idNumber =N'" + id1 + "',fullName=N'" + name1 + "',depatment=N'" + dep1 + "',idNumber2 =N'" + id2 + "',fullName2=N'" + name2 + "',depatment2=N'" + dep2 + "',ipAddress =N'" + txtFilterInfo.Text.Trim() + "',monitorNo =N'" + txtMonitorNo.Text.ToUpper() + "',monitorModel =N'" + txtMonitorModel.Text.ToUpper() + "',monitorNo2 =N'" + txtMonitorNo2.Text.ToUpper() + "',monitorModel2 =N'" + txtMonitorModel2.Text.ToUpper() + "',monitorNo3 =N'" + txtMonitorNo3.Text.ToUpper() + "',monitorModel3 =N'" + txtMonitorModel3.Text.ToUpper() + "' where hdd1Serial='" + pcClickItem.hdd1Serial + "'";
                        var command = "UPDATE ComputerInfomation SET biosSerial =N'" + PCInfomation.pcInfo.biosSerial + "',mainSerial=N'" + PCInfomation.pcInfo.mainSerial + "',pcName=N'" + PCInfomation.pcInfo.pcName + "',ipAddress =N'" + PCInfomation.pcInfo.ipAddress + "',macAddress =N'" + PCInfomation.pcInfo.macAddress + "',hdd1Model =N'" + PCInfomation.pcInfo.hdd1Model + "',hdd1Serial =N'" + PCInfomation.pcInfo.hdd1Serial + "',hdd2Model =N'" + PCInfomation.pcInfo.hdd2Model + "',hdd2Serial =N'" + PCInfomation.pcInfo.hdd2Serial + "',ramTotal =N'" + PCInfomation.pcInfo.ramTotal + "',ramWorking = N'" + PCInfomation.pcInfo.ramWorking + "',cpuSerial = N'" + PCInfomation.pcInfo.cpuSerial + "',run =N'" + PCInfomation.pcInfo.run + "',diskTotal_C =N'" + PCInfomation.pcInfo.diskTotal_C + "',diskTotal_D =N'" + PCInfomation.pcInfo.diskTotal_D + "',diskTotal_E = N'" + PCInfomation.pcInfo.diskTotal_E + "',diskFree_C = N'" + PCInfomation.pcInfo.diskFree_C + "',diskFree_D = N'" + PCInfomation.pcInfo.diskFree_D + "',diskFree_E =N'" + PCInfomation.pcInfo.diskFree_E + "',version =N'" + PCInfomation.pcInfo.version + "',monthUpdate =N'" + PCInfomation.pcInfo.monthUpdate + "',imsempcode =N'" + PCInfomation.pcInfo.imsempcode + "',insdt =N'" + PCInfomation.pcInfo.insdt + "',updempcode = N'" + PCInfomation.pcInfo.updempcode + "',upddt = N'" + PCInfomation.pcInfo.upddt + "' where hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "'";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                            
                        }
                        conn.Close();
                    }
                    Db_Read_TableSql(command_AllData);
                    MessageBox.Show("Sửa thông tin thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Infomation error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ProcessButtonEdit_Save()
        {
            try
            {
                Db_Read_BiosSerial();
                if (checkBiosSerial == false)
                {
                    Process_AddNewPC();
                    if (PCInfomation.pcInfo.idNumber != "" && PCInfomation.pcInfo.fullName != "" && PCInfomation.pcInfo.depatment != "")
                    {
                        Db_Insert_NewPc();
                        btnCheckPC.Background = Brushes.LightGreen;
                    }
                    else
                    {
                        MessageBox.Show("Nhập thiếu thông tin :\r\r Mã,Tên hoặc Bộ phận", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("PC đã có trong dữ liệu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Kiểm tra lại Internet", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
               
        public void IPAddress_Info()
        {
            try
            {
               
                string localIP;
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                    PCInfomation.pcInfo.ipAddress = localIP;
                }

               
                var macAddr =
                        (
                            from nic in NetworkInterface.GetAllNetworkInterfaces()
                            where nic.OperationalStatus == OperationalStatus.Up
                            select nic.GetPhysicalAddress().ToString()
                        ).FirstOrDefault();
                PCInfomation.pcInfo.macAddress = macAddr;

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/IPAddress_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public static void HDD_Info()
        {
            try
            {
               
                ArrayList hdCollection = new ArrayList();
                ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");  
                int index1 = 0, index2 = 0;
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    index1++;
                    if (index1 == 1)
                    {
                        PCInfomation.pcInfo.hdd1Model = wmi_HD["Model"].ToString().TrimStart();
                    }
                    if (index1 == 2)
                    {
                        PCInfomation.pcInfo.hdd2Model = wmi_HD["Model"].ToString().TrimStart();
                    }
                }               
               
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    index2++;
                    if (index2 == 1)
                    {
                        PCInfomation.pcInfo.hdd1Serial = wmi_HD["SerialNumber"].ToString().TrimStart();
                    }
                    if (index2 == 2)
                    {
                        PCInfomation.pcInfo.hdd2Serial = wmi_HD["SerialNumber"].ToString().TrimStart();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/HDD_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void Ram_Info()
        {
            try
            {
               
                Process proc = Process.GetCurrentProcess();               
                PCInfomation.pcInfo.ramWorking = (proc.WorkingSet64 / 1024 / 1000).ToString();

                string Query = "SELECT Capacity FROM Win32_PhysicalMemory";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Query);
                UInt64 Capacity = 0;
                foreach (ManagementObject ram in searcher.Get())
                {                  
                    Capacity += Convert.ToUInt64(ram.Properties["Capacity"].Value);
                }
                PCInfomation.pcInfo.ramTotal = (Capacity / 1024 / 1024 / 1000).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/Ram_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void CPU_Info()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {                    
                    PCInfomation.pcInfo.cpuSerial = queryObj["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/CPU_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        public void MainBoard_Info()
        {
            try
            {

                ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("SELECT Product, SerialNumber FROM Win32_BaseBoard");

                ManagementObjectCollection information1 = searcher.Get();
                foreach (ManagementObject obj in information1)
                {
                    foreach (PropertyData data in obj.Properties)
                    {
                        PCInfomation.pcInfo.mainSerial = data.Value.ToString();
                    }
                    //Console.WriteLine("{0} = {1}", data.Name, data.Value);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/MainBoard_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void Bios_Info()
        {
            try
            {
                Console.WriteLine("BIOS :");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(" Select * From Win32_BIOS ");
                foreach (ManagementObject getserial in searcher.Get())
                {
                    //Console.WriteLine(getserial["SerialNumber"].ToString());
                    PCInfomation.pcInfo.biosSerial = getserial["SerialNumber"].ToString();
                }
                if(PCInfomation.pcInfo.biosSerial=="" || PCInfomation.pcInfo.biosSerial==null)
                {
                    PCInfomation.pcInfo.biosSerial = PCInfomation.pcInfo.mainSerial;
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/MainBoard_Info", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void HardDiskInfo()
        {
            try
            {
                int index = 0;
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        index++;
                        double totalsize = drive.TotalSize / 1024 / 1024 / 1000;
                        double freesize = drive.TotalFreeSpace / 1024 / 1024 / 1000;
                        double usesize = totalsize - freesize;
                        if (index == 1)
                        {
                            PCInfomation.pcInfo.diskTotal_C = totalsize.ToString();
                            PCInfomation.pcInfo.diskFree_C = freesize.ToString();
                        }
                        if (index == 2)
                        {
                            PCInfomation.pcInfo.diskTotal_D = totalsize.ToString();
                            PCInfomation.pcInfo.diskFree_D = freesize.ToString();
                        }
                        if (index == 3)
                        {
                            PCInfomation.pcInfo.diskTotal_E = totalsize.ToString();
                            PCInfomation.pcInfo.diskFree_E  = freesize.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/HardDiskInfo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void OSInfo()
        {
            try
            {
                PCInfomation.pcInfo.pcName = Environment.MachineName;
                PCInfomation.pcInfo.pcName = Environment.MachineName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PcManage/OSInfo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }       
               
        public void Process_AddNewPC()
        {
            var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
            var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
            string dateInset = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
            IPAddress_Info();
            HDD_Info();
            Ram_Info();
            CPU_Info();
            MainBoard_Info();
            Bios_Info();                
            HardDiskInfo();
            OSInfo();
            PCInfomation.pcInfo.cmpcode = "02";
            PCInfomation.pcInfo.bizdiv = "300";
            PCInfomation.pcInfo.pcno = Db_Read_PcNoMax();
            PCInfomation.pcInfo.fullName = txbFullName.Text;
            PCInfomation.pcInfo.idNumber = txtIdNumber.Text.ToUpper();
            PCInfomation.pcInfo.depatment = cbb_Depatment.SelectedItem.ToString();
            PCInfomation.pcInfo.monitorNo = txtMonitorNo.Text.ToUpper();
            PCInfomation.pcInfo.monitorModel = txtMonitorModel.Text.ToUpper();
            PCInfomation.pcInfo.fullName2 = txbFullName2.Text;
            PCInfomation.pcInfo.idNumber2 = txtIdNumber2.Text.ToUpper();
            if(txbFullName2.Text!="")
            {
                PCInfomation.pcInfo.depatment2 = cbb_Depatment2.SelectedItem.ToString();
            }
            else
            {
                PCInfomation.pcInfo.depatment2 = "";
            }
            PCInfomation.pcInfo.monitorNo2 = txtMonitorNo2.Text.ToUpper();
            PCInfomation.pcInfo.monitorModel2 = txtMonitorModel2.Text.ToUpper();
            PCInfomation.pcInfo.monitorNo3 = txtMonitorNo3.Text.ToUpper();
            PCInfomation.pcInfo.monitorModel3 = txtMonitorModel3.Text.ToUpper();
            PCInfomation.pcInfo.version = Ver;
            PCInfomation.pcInfo.monthUpdate = DateTime.Now.ToString("yyyy-MM");           
            PCInfomation.pcInfo.insdt = dateInset;
        }
               
        public void Db_Read_TableSql(string command)
        {
            try
            {
                lvPcManage.Items.Clear();
                PCInfomation.listPC.Clear();
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Helper_PcInfomation pc_info = new Helper_PcInfomation();
                                if (dr[0] != null)
                                {
                                    pc_info.cmpcode = dr[0].ToString();
                                    pc_info.bizdiv = dr[1].ToString();
                                    pc_info.pcno = dr[2].ToString();
                                    pc_info.biosSerial = dr[3].ToString();
                                    pc_info.mainSerial = dr[4].ToString();
                                    pc_info.pcName = dr[5].ToString();
                                    pc_info.ipAddress = dr[6].ToString();
                                    pc_info.macAddress = dr[7].ToString();
                                    pc_info.hdd1Model = dr[8].ToString();
                                    pc_info.hdd1Serial = dr[9].ToString();
                                    pc_info.hdd2Model = dr[10].ToString();
                                    pc_info.hdd2Serial = dr[11].ToString();
                                    pc_info.ramTotal = dr[12].ToString();
                                    pc_info.ramWorking = dr[13].ToString();
                                    pc_info.cpuSerial = dr[14].ToString();
                                    pc_info.idNumber = dr[15].ToString();
                                    pc_info.fullName = dr[16].ToString();
                                    pc_info.depatment = dr[17].ToString();                                   
                                    pc_info.diskTotal_C = dr[18].ToString();
                                    pc_info.diskTotal_D = dr[19].ToString();
                                    pc_info.diskTotal_E = dr[20].ToString();
                                    pc_info.diskFree_C = dr[21].ToString();
                                    pc_info.diskFree_D = dr[22].ToString();
                                    pc_info.diskFree_E = dr[23].ToString();
                                    pc_info.monitorNo = dr[25].ToString();
                                    pc_info.monitorModel = dr[26].ToString();
                                    pc_info.version = dr[27].ToString();
                                    pc_info.monitorNo2 = dr[28].ToString();
                                    pc_info.monitorModel2 = dr[29].ToString();
                                    pc_info.monitorNo3 = dr[30].ToString();
                                    pc_info.monitorModel3 = dr[31].ToString();
                                    pc_info.idNumber2 = dr[32].ToString();
                                    pc_info.fullName2 = dr[33].ToString();
                                    if(pc_info.fullName2!="")
                                    {
                                        pc_info.depatment2 = dr[34].ToString();
                                    }    
                                    else
                                    {
                                        pc_info.depatment2 = "";
                                    }    
                                    pc_info.monthUpdate = dr[35].ToString();
                                    pc_info.imsempcode = dr[36].ToString();
                                    pc_info.insdt = dr[37].ToString();
                                    pc_info.updempcode = dr[38].ToString();
                                    pc_info.upddt = dr[39].ToString();
                                }
                                PCInfomation.listPC.Add(pc_info);
                            }
                        }
                    }

                    conn.Close();

                }
                int indexPC = 0;
                foreach (var item in PCInfomation.listPC)
                {                    
                    item.ID = indexPC;
                    lvPcManage.Items.Add(item);
                    indexPC++;
                }
                numberPC.Content = indexPC.ToString();
                
            }
            catch (Exception ex)
            {
                
            }
        }
              
        public void Db_Read_Employ()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from pf_employee", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null && dr[0].ToString().ToUpper() == txtIdNumber.Text.ToUpper())
                                {
                                    txbFullName.Text = dr[1].ToString();
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
               
        public void Db_Read_Depatment()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    List<string> listDep = new List<string>();
                    using (SqlCommand cmd = new SqlCommand("select * from pf_department", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    listDep.Add(dr[1].ToString());
                                }
                            }
                        }
                    }
                    cbb_Depatment.ItemsSource = listDep;
                    cbb_Depatment2.ItemsSource = listDep;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }       

        public void Db_Read_BiosSerial()
        {
            try
            {
               
                HDD_Info();
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from ComputerInfomation", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    if (PCInfomation.pcInfo.hdd1Serial==dr[9].ToString())
                                    {
                                        checkBiosSerial = true;
                                    }
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
               
        public string Db_Read_PcNoMax()
        {
            try
            {
                string idNumberMax = "";
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT MAX(pcno) FROM ComputerInfomation", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    idNumberMax = dr[0].ToString();
                                }
                            }
                        }
                    }
                    conn.Close();
                }
                if (idNumberMax == "")
                {
                    idNumberMax = "PC-0001";
                }
                else
                {
                    int number = int.Parse(idNumberMax.Substring(3, 4));
                    idNumberMax = "PC-" + (number + 1).ToString("0000");
                }

                return idNumberMax;
            }
            catch (Exception)
            {
                MessageBox.Show("Kiểm tra lại Internet", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
               
        public void Db_Insert_NewPc()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "Insert  ComputerInfomation(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monitorNo2,monitorModel2,monitorNo3,monitorModel3,idNumber2,fullName2,depatment2,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                        "Values(N'" + PCInfomation.pcInfo.cmpcode + "',N'" + PCInfomation.pcInfo.bizdiv + "',N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.hdd1Model + "',N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + PCInfomation.pcInfo.hdd2Model + "',N'" + PCInfomation.pcInfo.hdd2Serial + "',N'" + PCInfomation.pcInfo.ramTotal + "',N'" + PCInfomation.pcInfo.ramWorking + "',N'" + PCInfomation.pcInfo.cpuSerial + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "',N'" + PCInfomation.pcInfo.diskTotal_C + "',N'" + PCInfomation.pcInfo.diskTotal_D + "',N'" + PCInfomation.pcInfo.diskTotal_E + "',N'" + PCInfomation.pcInfo.diskFree_C + "',N'" + PCInfomation.pcInfo.diskFree_D + "',N'" + PCInfomation.pcInfo.diskFree_E + "',N'" + PCInfomation.pcInfo.monitorNo + "',N'" + PCInfomation.pcInfo.monitorModel + "',N'" + PCInfomation.pcInfo.version + "',N'" + PCInfomation.pcInfo.monitorNo2 + "',N'" + PCInfomation.pcInfo.monitorModel2 + "',N'" + PCInfomation.pcInfo.monitorNo3 + "',N'" + PCInfomation.pcInfo.monitorModel3 + "',N'" + PCInfomation.pcInfo.idNumber2 + "',N'" + PCInfomation.pcInfo.fullName2 + "',N'" + PCInfomation.pcInfo.depatment2 + "',N'" + PCInfomation.pcInfo.monthUpdate + "',N'" + PCInfomation.pcInfo.imsempcode + "',N'" + PCInfomation.pcInfo.insdt + "',N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "')";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();                       
                    }
                    var command1 = "Insert  ComputerInfomation_Backup(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monitorNo2,monitorModel2,monitorNo3,monitorModel3,idNumber2,fullName2,depatment2,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                        "Values(N'" + PCInfomation.pcInfo.cmpcode + "',N'" + PCInfomation.pcInfo.bizdiv + "',N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.hdd1Model + "',N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + PCInfomation.pcInfo.hdd2Model + "',N'" + PCInfomation.pcInfo.hdd2Serial + "',N'" + PCInfomation.pcInfo.ramTotal + "',N'" + PCInfomation.pcInfo.ramWorking + "',N'" + PCInfomation.pcInfo.cpuSerial + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "',N'" + PCInfomation.pcInfo.diskTotal_C + "',N'" + PCInfomation.pcInfo.diskTotal_D + "',N'" + PCInfomation.pcInfo.diskTotal_E + "',N'" + PCInfomation.pcInfo.diskFree_C + "',N'" + PCInfomation.pcInfo.diskFree_D + "',N'" + PCInfomation.pcInfo.diskFree_E + "',N'" + PCInfomation.pcInfo.monitorNo + "',N'" + PCInfomation.pcInfo.monitorModel + "',N'" + PCInfomation.pcInfo.version + "',N'" + PCInfomation.pcInfo.monitorNo2 + "',N'" + PCInfomation.pcInfo.monitorModel2 + "',N'" + PCInfomation.pcInfo.monitorNo3 + "',N'" + PCInfomation.pcInfo.monitorModel3 + "',N'" + PCInfomation.pcInfo.idNumber2 + "',N'" + PCInfomation.pcInfo.fullName2 + "',N'" + PCInfomation.pcInfo.depatment2 + "',N'" + PCInfomation.pcInfo.monthUpdate + "',N'" + PCInfomation.pcInfo.imsempcode + "',N'" + PCInfomation.pcInfo.insdt + "',N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "')";
                    using (SqlCommand cmd = new SqlCommand(command1, conn))
                    {
                        cmd.ExecuteNonQuery();                       
                    }
                    conn.Close();
                }
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "Insert  PcRun(pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,idNumber,fullName,depatment,run) " +
                        "Values(N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "')";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                Db_Read_TableSql(command_AllData);
                MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Kiểm tra lại Internet", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
               
        int checkUpdate = 1;
        public void Db_MonthUpdate()
        {
            try
            {   
               
                if(checkUpdate==1)
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    string month = DateTime.Now.ToString("yyyy-MM");
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM PcInfo_MonthUpdate WHERE hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "'  order by insdt desc", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[35].ToString() == month)
                                {
                                    checkUpdate++;
                                }                                    
                            }
                            if(checkUpdate==1)
                            {
                                checkUpdate = 0;
                            }    
                        }
                    }                  
                    conn.Close();
                }
                if (checkUpdate == 0)
                {
                    Process_AddNewPC();
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        var command1 = "Insert  PcInfo_MonthUpdate(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monitorNo2,monitorModel2,monitorNo3,monitorModel3,idNumber2,fullName2,depatment2,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                        "Values(N'" + PCInfomation.pcInfo.cmpcode + "',N'" + PCInfomation.pcInfo.bizdiv + "',N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.hdd1Model + "',N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + PCInfomation.pcInfo.hdd2Model + "',N'" + PCInfomation.pcInfo.hdd2Serial + "',N'" + PCInfomation.pcInfo.ramTotal + "',N'" + PCInfomation.pcInfo.ramWorking + "',N'" + PCInfomation.pcInfo.cpuSerial + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "',N'" + PCInfomation.pcInfo.diskTotal_C + "',N'" + PCInfomation.pcInfo.diskTotal_D + "',N'" + PCInfomation.pcInfo.diskTotal_E + "',N'" + PCInfomation.pcInfo.diskFree_C + "',N'" + PCInfomation.pcInfo.diskFree_D + "',N'" + PCInfomation.pcInfo.diskFree_E + "',N'" + PCInfomation.pcInfo.monitorNo + "',N'" + PCInfomation.pcInfo.monitorModel + "',N'" + PCInfomation.pcInfo.version + "',N'" + PCInfomation.pcInfo.monitorNo2 + "',N'" + PCInfomation.pcInfo.monitorModel2 + "',N'" + PCInfomation.pcInfo.monitorNo3 + "',N'" + PCInfomation.pcInfo.monitorModel3 + "',N'" + PCInfomation.pcInfo.idNumber2 + "',N'" + PCInfomation.pcInfo.fullName2 + "',N'" + PCInfomation.pcInfo.depatment2 + "',N'" + PCInfomation.pcInfo.monthUpdate + "',N'" + PCInfomation.pcInfo.imsempcode + "',N'" + PCInfomation.pcInfo.insdt + "',N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "')";
                        using (SqlCommand cmd = new SqlCommand(command1, conn))
                        {
                            cmd.ExecuteNonQuery();
                            conn.Close();                           
                        }
                    }
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        var command = "UPDATE ComputerInfomation SET ipAddress = N'"+ PCInfomation.pcInfo.ipAddress+ "' WHERE hdd1Serial = '" + PCInfomation.pcInfo.hdd1Serial + "'";
                        using (SqlCommand cmd = new SqlCommand(command, conn))
                        {
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    checkUpdate = 2;
                }
            }
            catch (Exception ex)
            {
                
            }
        }           

        List<Helper_ApplicationInfo> List_AllApplication = new List<Helper_ApplicationInfo>();
        public void GetAll_Application()
        {
            try
            {
                string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                        {
                            if (subkey.GetValue("DisplayName") != null)
                            {
                                List_AllApplication.Add(new Helper_ApplicationInfo
                                {
                                    DisplayName = (string)subkey.GetValue("DisplayName"),
                                    Version = (string)subkey.GetValue("DisplayVersion"),
                                    InstalledDate = (string)subkey.GetValue("InstallDate"),
                                    Publisher = (string)subkey.GetValue("Publisher"),
                                    UnninstallCommand = (string)subkey.GetValue("UninstallString"),
                                    ModifyPath = (string)subkey.GetValue("ModifyPath")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }   
        }
        int checkApplication = 1;
        public void Db_InsertApplication()
        {
            try
            {                
                if (checkApplication == 1)
                {
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        string month = DateTime.Now.ToString("yyyy-MM");
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM ApplicationInfo WHERE hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "'  order by insdt desc", conn))
                        {
                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (dr[8].ToString() == month)
                                    {
                                        checkApplication++;
                                    }
                                }
                                if (checkApplication == 1)
                                {
                                    checkApplication = 0;
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                if (checkApplication == 0)
                {
                    GetAll_Application();
                    var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
                    var settingMonth = new JsonSerializerSettings { DateFormatString = "yyyy-MM" };
                    var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
                    var jsonDateMonth = JsonConvert.SerializeObject(DateTime.Now, settingMonth);
                    string dateInset = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
                    string month = jsonDateInput.Substring(1, jsonDateMonth.Length - 2);
                    using (SqlConnection conn = new SqlConnection(path_sql))
                    {
                        conn.Open();
                        foreach (var item in List_AllApplication)
                        {
                            var command = "INSERT ApplicationInfo(hdd1Serial, DisplayName, Version, InstalledDate, Publisher, UnninstallCommand, ModifyPath, monthUpdate, insdt) " +
                                "VALUES(N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + item.DisplayName + "',N'" + item.Version + "',N'" + item.InstalledDate + "',N'" + item.Publisher + "',N'" + item.UnninstallCommand + "',N'" + item.ModifyPath + "',N'" + month + "',N'" + dateInset + "')";
                            using (SqlCommand cmd = new SqlCommand(command, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        conn.Close();
                    }
                }                    
            }
            catch (Exception)
            {
                
            }            
        }
     
        public void Db_DeleteDataTable(string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "Delete " + tableName + "";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        step_get = 2;
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
                
            }

        }

        bool check_pcProcessBios = false;
        bool check_PcUpdatef = false;
        public string Db_Read_Action()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from PcProcess", conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (dr[0] != null)
                                {
                                    if (dr[15].ToString() == PCInfomation.pcInfo.hdd1Serial)
                                    {
                                        check_pcProcessBios = true;
                                    }
                                    PCInfomation.pcData.biosSerial = dr[0].ToString();
                                    PCInfomation.pcData.upFile = dr[1].ToString();
                                    PCInfomation.pcData.dowFile = dr[2].ToString();
                                    PCInfomation.pcData.listFolder = dr[3].ToString();
                                    PCInfomation.pcData.listFile = dr[4].ToString();
                                    PCInfomation.pcData.sourcePath = dr[5].ToString();
                                    PCInfomation.pcData.destPath = dr[6].ToString();
                                    PCInfomation.pcData.fodlerPath = dr[7].ToString();
                                    PCInfomation.pcData.filePath = dr[8].ToString();
                                    PCInfomation.pcData.fileName = dr[9].ToString();
                                    PCInfomation.pcData.fileType = dr[10].ToString();
                                    PCInfomation.pcData.fileSize = dr[11].ToString();
                                    PCInfomation.pcData.run = dr[12].ToString();
                                    PCInfomation.pcData.etc1 = dr[13].ToString();
                                    PCInfomation.pcData.etc2 = dr[14].ToString();
                                    PCInfomation.pcData.hdd1Serial = dr[15].ToString();
                                    PCInfomation.pcData.etc4 = dr[16].ToString();
                                    PCInfomation.pcData.etc5 = dr[17].ToString();
                                    PCInfomation.pcData.etc6 = dr[18].ToString();
                                    PCInfomation.pcData.etc7 = dr[19].ToString();
                                    PCInfomation.pcData.etc8 = dr[20].ToString();
                                    PCInfomation.pcData.etc9 = dr[21].ToString();
                                    PCInfomation.pcData.etc10 = dr[22].ToString();

                                }
                            }
                        }
                    }                   
                    conn.Close();
                }
                return PCInfomation.pcData.run;
            }
            catch (Exception)
            {               
                return "";
            }
        }

        public void Process_CheckBiosPC()
        {
            checkBiosSerial = false;
            Db_Read_BiosSerial();
            if (checkBiosSerial == false)
            {
                btnCheckPC.Background = Brushes.Red;
                txtIdNumber.Focus();
                  
            }
            else
            {
                btnCheckPC.Background = Brushes.LightGreen;
            }

        }
        
        public void Process_Filter_Info(string filter)
        {
            try
            {
                var command = "";
                string str_Filter = txtFilterInfo.Text.Trim();
                switch (filter)
                {
                    case "Tất cả":
                        {
                            command = "SELECT * FROM ComputerInfomation order by depatment asc";
                            break;
                        }
                    case "PC Name":
                        {
                            command = "SELECT * FROM ComputerInfomation  where pcName LIKE '%" + str_Filter + "%' order by idNumber asc";
                            break;
                        }
                    case "Bô phận":
                        {
                            command = "SELECT * FROM ComputerInfomation where depatment LIKE '%" + str_Filter + "%' order by idNumber asc";
                            break;
                        }
                    case "Mã nhân viên":
                        {
                            command = "SELECT * FROM ComputerInfomation where idNumber LIKE '%" + str_Filter + "%' order by depatment asc";
                            break;
                        }
                    case "Địa chỉ IP":
                        {
                            command = "SELECT * FROM ComputerInfomation where ipAddress LIKE '%" + str_Filter + "%' order by depatment asc";
                            break;
                        }
                    case "CPU":
                        {
                            command = "SELECT * FROM ComputerInfomation where cpuSerial LIKE '%" + str_Filter + "%' order by cpuSerial asc";
                            break;
                        }
                    case "Memory":
                        {
                            command = "SELECT * FROM ComputerInfomation where ramTotal LIKE '%" + str_Filter + "%' order by ramTotal asc";
                            break;
                        }
                    case "Version":
                        {
                            command = "SELECT * FROM ComputerInfomation where version LIKE '%" + str_Filter + "%' order by depatment asc,version asc";
                            break;
                        }
                        //case "Disk":
                        //    {
                        //        command = "SELECT * FROM ComputerInfomation order by disk"+ str_Filter + " asc";
                        //        break;
                        //    }

                }
                Db_Read_TableSql(command);
            }
            catch (Exception)
            {
                
            }
        }

        private void rbPcNV_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void rbPcPOP_Checked(object sender, RoutedEventArgs e)
        {
            txbFullName.Text = "POP";            
        }

        private void cbbFilterInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ComboBox;
            var clickItem = click.SelectedItem as ComboBoxItem;
            if (click.SelectedIndex > 0)
            {
                str_FilterInfo = clickItem.Content.ToString();
            }
        }

        private void btnFilterInfo_Click(object sender, RoutedEventArgs e)
        {
            if (str_FilterInfo != "")
                Process_Filter_Info(str_FilterInfo);
        }

        private void txtIdNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && rbPcNV.IsChecked == true)
            {
                Db_Read_Employ();
            }
        }

        private void btnCheckPC_Click(object sender, RoutedEventArgs e)
        {
            Load_Read_UpdateNew();
            Process_CheckBiosPC();
            ProcessUpdateInfo();
            Db_Read_TableSql(command_AllData);
        }

        private void lvPcManage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var click = sender as ListView;
            var clickItem = click.SelectedItem as Helper_PcInfomation;
            if (clickItem != null)
            {
                pcClickItem = clickItem;
                txtIdNumber.Text = pcClickItem.idNumber;
                txbFullName.Text = pcClickItem.fullName;
                txtFilterInfo.Text = pcClickItem.ipAddress;
                txtMonitorNo.Text = pcClickItem.monitorNo;
                txtMonitorModel.Text = pcClickItem.monitorModel;
                txtIdNumber2.Text = pcClickItem.idNumber2;
                txbFullName2.Text = pcClickItem.fullName2;
                txtMonitorNo2.Text = pcClickItem.monitorNo2;
                txtMonitorNo3.Text = pcClickItem.monitorNo3;
                txtMonitorModel2.Text = pcClickItem.monitorModel2;
                txtMonitorModel3.Text = pcClickItem.monitorModel3;
                switch (clickItem.depatment)
                {
                    case "BP IT":
                        {
                            cbb_Depatment.SelectedIndex = 0;
                            break;
                        }
                    case "BP HR":
                        {
                            cbb_Depatment.SelectedIndex = 1;
                            break;
                        }
                    case "BP GA":
                        {
                            cbb_Depatment.SelectedIndex = 2;
                            break;
                        }
                    case "BP ACC":
                        {
                            cbb_Depatment.SelectedIndex = 3;
                            break;
                        }
                    case "BP PUR":
                        {
                            cbb_Depatment.SelectedIndex = 4;
                            break;
                        }
                    case "BP MAR":
                        {
                            cbb_Depatment.SelectedIndex = 5;
                            break;
                        }
                    case "BP PRO":
                        {
                            cbb_Depatment.SelectedIndex = 6;
                            break;
                        }
                    case "BP QC":
                        {
                            cbb_Depatment.SelectedIndex = 7;
                            break;
                        }
                    case "BP Sabari":
                        {
                            cbb_Depatment.SelectedIndex = 8;
                            break;
                        }
                    case "BP Cushion":
                        {
                            cbb_Depatment.SelectedIndex = 9;
                            break;
                        }
                    case "BP HiCup":
                        {
                            cbb_Depatment.SelectedIndex = 10;
                            break;
                        }
                    case "QL Korea":
                        {
                            cbb_Depatment.SelectedIndex = 11;
                            break;
                        }
                    default:
                        {
                            cbb_Depatment.SelectedIndex = 12;
                            break;
                        }
                }                    
            }
        }

        private void btnListIp_Click(object sender, RoutedEventArgs e)
        {
            Window_ListIP listIP = new Window_ListIP();
            listIP.Show();            
        }

        private void Window_Closed(object sender, EventArgs e)
        {        
           
        }

        private void btnApps_Click(object sender, RoutedEventArgs e)
        {
            Window_Application application = new Window_Application();
            application.Show();
        }

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
       
        private void MenuItem_Safe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Risk_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnLockUsb_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }

    public static class PCInfomation
    {
        public static List<Helper_PcInfomation> listPC = new List<Helper_PcInfomation>();
        public static Helper_PcInfomation pcInfo = new Helper_PcInfomation();
        public static Helper_PcProcessData pcData = new Helper_PcProcessData();
    }

    
}
