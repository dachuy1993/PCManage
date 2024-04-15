using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Configuration;
using System.Reflection;
using System.Windows;
using System.Management;
using System.IO;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using Newtonsoft.Json;

namespace PCManageService
{
    public partial class Service1 : ServiceBase
    {
        public bool checkBiosSerial = false;
        string Check; 
        string SeqHis;
        string Ver;
        string path_sql = "Data Source=192.168.2.5,1433;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password= oneuser1!";
        string command_AllData = "SELECT * FROM ComputerInfomation order by depatment asc,idNumber asc";

        public Service1()
        {
            UpdateVer();
            InitializeComponent();
            Load_Read_UpdateNew();
            Process_CheckBiosPC();
            ProcessUpdateInfo();
        }

        protected override void OnStart(string[] args)
        {
            UpdateVer();
            Load_Read_UpdateNew();
            Process_CheckBiosPC();
            ProcessUpdateInfo();
            Utilities.WriteLogError("Start " + PCInfomation.pcInfo.hdd1Serial + " " + PCInfomation.pcInfo.upddt);
            TimeStart();
        }

        public void UpdateVer()
        {
            using (SqlConnection conn = new SqlConnection(path_sql))
            {
                var commandcheck = "select * from TblCheckVer ";
                var resultCheck = DataProvider.Instance.ExecuteScalar(commandcheck, new object[]
                {
                            
                });
                Ver = resultCheck.ToString();
            }

        }

        public void TimeStart()
        {
            using (SqlConnection conn = new SqlConnection(path_sql))
            {
                conn.Open();
                var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
                var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
                string dateInset = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
                var commandcheck = "SPGetSeqMaxPC @HddSerial ";
                var resultCheck = DataProvider.Instance.ExecuteScalar( commandcheck, new object[]
                {
                            PCInfomation.pcInfo.hdd1Serial
                });
                Check = resultCheck.ToString();

                if (int.Parse(Check) == 0)
                {
                    SeqHis = "1";
                }
                else
                {
                    SeqHis = (int.Parse(Check) + 1).ToString();
                }

                var command = "Insert TBLUpdateInfoPCTimeHis(Hdd1Serial,Seq,TimeStart,TimeStop) SELECT '" + PCInfomation.pcInfo.hdd1Serial + "','" + SeqHis.ToString() + "','" + dateInset + "',''";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            } 
        }

        public void TimeStop()
        {
            using (SqlConnection conn = new SqlConnection(path_sql))
            {
                conn.Open();
                var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
                var jsonDateInput = JsonConvert.SerializeObject(DateTime.Now, settings);
                string dateInset = jsonDateInput.Substring(1, jsonDateInput.Length - 2);
                var commandcheck = "SPGetSeqMaxPC @HddSerial ";
                var resultCheck = DataProvider.Instance.ExecuteScalar(commandcheck, new object[]
                {
                            PCInfomation.pcInfo.hdd1Serial
                });
                Check = resultCheck.ToString();

                var command = "update TBLUpdateInfoPCTimeHis set TimeStop = '" + dateInset + "' where Hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "' and Seq = '" + Check + "'";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
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
            PCInfomation.pcInfo.monthUpdate = DateTime.Now.ToString("yyyy-MM");
            PCInfomation.pcInfo.insdt = dateInset;
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
                if (PCInfomation.pcInfo.biosSerial == "" || PCInfomation.pcInfo.biosSerial == null)
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
                            PCInfomation.pcInfo.diskFree_E = freesize.ToString();
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

        public void ProcessUpdateInfo()
        {
            //try
            //{

            using (SqlConnection conn = new SqlConnection(path_sql))
            {
                conn.Open();

                var commandcheck = "Select count(*) From ComputerInfomation where hdd1Serial= @P1 ";
                var resultCheck = DataProvider.Instance.ExecuteScalar(commandcheck, new object[]
                {
                            PCInfomation.pcInfo.hdd1Serial
                });

                Check = resultCheck.ToString();

                if (int.Parse(Check) == 0)
                {
                    Db_Insert_NewPc();
                }
                else
                {
                    var command1 = "Insert ComputerInfomation_Backup(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                    "SELECT cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monthUpdate,imsempcode,insdt,N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "' FROM ComputerInfomation where hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "' ";
                    using (SqlCommand cmd = new SqlCommand(command1, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    //var command = "UPDATE ComputerInfomation SET idNumber =N'" + id1 + "',fullName=N'" + name1 + "',depatment=N'" + dep1 + "',idNumber2 =N'" + id2 + "',fullName2=N'" + name2 + "',depatment2=N'" + dep2 + "',ipAddress =N'" + txtFilterInfo.Text.Trim() + "',monitorNo =N'" + txtMonitorNo.Text.ToUpper() + "',monitorModel =N'" + txtMonitorModel.Text.ToUpper() + "',monitorNo2 =N'" + txtMonitorNo2.Text.ToUpper() + "',monitorModel2 =N'" + txtMonitorModel2.Text.ToUpper() + "',monitorNo3 =N'" + txtMonitorNo3.Text.ToUpper() + "',monitorModel3 =N'" + txtMonitorModel3.Text.ToUpper() + "' where hdd1Serial='" + pcClickItem.hdd1Serial + "'";
                    var command = "UPDATE ComputerInfomation SET biosSerial =N'" + PCInfomation.pcInfo.biosSerial + "',mainSerial=N'" + PCInfomation.pcInfo.mainSerial + "',pcName=N'" + PCInfomation.pcInfo.pcName + "',ipAddress =N'" + PCInfomation.pcInfo.ipAddress + "',macAddress =N'" + PCInfomation.pcInfo.macAddress + "',hdd1Model =N'" + PCInfomation.pcInfo.hdd1Model + "',hdd1Serial =N'" + PCInfomation.pcInfo.hdd1Serial + "',hdd2Model =N'" + PCInfomation.pcInfo.hdd2Model + "',hdd2Serial =N'" + PCInfomation.pcInfo.hdd2Serial + "',ramTotal =N'" + PCInfomation.pcInfo.ramTotal + "',ramWorking = N'" + PCInfomation.pcInfo.ramWorking + "',cpuSerial = N'" + PCInfomation.pcInfo.cpuSerial + "',run =N'" + PCInfomation.pcInfo.run + "',diskTotal_C =N'" + PCInfomation.pcInfo.diskTotal_C + "',diskTotal_D =N'" + PCInfomation.pcInfo.diskTotal_D + "',diskTotal_E = N'" + PCInfomation.pcInfo.diskTotal_E + "',diskFree_C = N'" + PCInfomation.pcInfo.diskFree_C + "',diskFree_D = N'" + PCInfomation.pcInfo.diskFree_D + "',diskFree_E =N'" + PCInfomation.pcInfo.diskFree_E + "',monthUpdate =N'" + PCInfomation.pcInfo.monthUpdate + "',imsempcode =N'" + PCInfomation.pcInfo.imsempcode + "',insdt =N'" + PCInfomation.pcInfo.insdt + "',updempcode = N'" + PCInfomation.pcInfo.updempcode + "',upddt = N'" + PCInfomation.pcInfo.upddt + "',version = N'" + Ver +  "' where hdd1Serial='" + PCInfomation.pcInfo.hdd1Serial + "'";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();

                    }
                }
                conn.Close();
            }
            Db_Read_TableSql(command_AllData);

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Update Infomation error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        public void Db_Insert_NewPc()
        {
            //try
            //{
            using (SqlConnection conn = new SqlConnection(path_sql))
            {
                conn.Open();
                var command = "Insert  ComputerInfomation(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monitorNo2,monitorModel2,monitorNo3,monitorModel3,idNumber2,fullName2,depatment2,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                    "Values(N'" + PCInfomation.pcInfo.cmpcode + "',N'" + PCInfomation.pcInfo.bizdiv + "',N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.hdd1Model + "',N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + PCInfomation.pcInfo.hdd2Model + "',N'" + PCInfomation.pcInfo.hdd2Serial + "',N'" + PCInfomation.pcInfo.ramTotal + "',N'" + PCInfomation.pcInfo.ramWorking + "',N'" + PCInfomation.pcInfo.cpuSerial + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "',N'" + PCInfomation.pcInfo.diskTotal_C + "',N'" + PCInfomation.pcInfo.diskTotal_D + "',N'" + PCInfomation.pcInfo.diskTotal_E + "',N'" + PCInfomation.pcInfo.diskFree_C + "',N'" + PCInfomation.pcInfo.diskFree_D + "',N'" + PCInfomation.pcInfo.diskFree_E + "',N'" + PCInfomation.pcInfo.monitorNo + "',N'" + PCInfomation.pcInfo.monitorModel + "',N'" + Ver + "',N'" + PCInfomation.pcInfo.monitorNo2 + "',N'" + PCInfomation.pcInfo.monitorModel2 + "',N'" + PCInfomation.pcInfo.monitorNo3 + "',N'" + PCInfomation.pcInfo.monitorModel3 + "',N'" + PCInfomation.pcInfo.idNumber2 + "',N'" + PCInfomation.pcInfo.fullName2 + "',N'" + PCInfomation.pcInfo.depatment2 + "',N'" + PCInfomation.pcInfo.monthUpdate + "',N'" + PCInfomation.pcInfo.imsempcode + "',N'" + PCInfomation.pcInfo.insdt + "',N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "')";
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                var command1 = "Insert  ComputerInfomation_Backup(cmpcode,bizdiv,pcno,biosSerial,mainSerial,pcName,ipAddress,macAddress,hdd1Model,hdd1Serial,hdd2Model,hdd2Serial,ramTotal,ramWorking,cpuSerial,idNumber,fullName,depatment,run,diskTotal_C,diskTotal_D,diskTotal_E,diskFree_C,diskFree_D,diskFree_E,monitorNo,monitorModel,version,monitorNo2,monitorModel2,monitorNo3,monitorModel3,idNumber2,fullName2,depatment2,monthUpdate,imsempcode,insdt,updempcode,upddt) " +
                    "Values(N'" + PCInfomation.pcInfo.cmpcode + "',N'" + PCInfomation.pcInfo.bizdiv + "',N'" + PCInfomation.pcInfo.pcno + "',N'" + PCInfomation.pcInfo.biosSerial + "',N'" + PCInfomation.pcInfo.mainSerial + "',N'" + PCInfomation.pcInfo.pcName + "',N'" + PCInfomation.pcInfo.ipAddress + "',N'" + PCInfomation.pcInfo.macAddress + "',N'" + PCInfomation.pcInfo.hdd1Model + "',N'" + PCInfomation.pcInfo.hdd1Serial + "',N'" + PCInfomation.pcInfo.hdd2Model + "',N'" + PCInfomation.pcInfo.hdd2Serial + "',N'" + PCInfomation.pcInfo.ramTotal + "',N'" + PCInfomation.pcInfo.ramWorking + "',N'" + PCInfomation.pcInfo.cpuSerial + "',N'" + PCInfomation.pcInfo.idNumber + "',N'" + PCInfomation.pcInfo.fullName + "',N'" + PCInfomation.pcInfo.depatment + "',N'" + PCInfomation.pcInfo.run + "',N'" + PCInfomation.pcInfo.diskTotal_C + "',N'" + PCInfomation.pcInfo.diskTotal_D + "',N'" + PCInfomation.pcInfo.diskTotal_E + "',N'" + PCInfomation.pcInfo.diskFree_C + "',N'" + PCInfomation.pcInfo.diskFree_D + "',N'" + PCInfomation.pcInfo.diskFree_E + "',N'" + PCInfomation.pcInfo.monitorNo + "',N'" + PCInfomation.pcInfo.monitorModel + "',N'" + Ver + "',N'" + PCInfomation.pcInfo.monitorNo2 + "',N'" + PCInfomation.pcInfo.monitorModel2 + "',N'" + PCInfomation.pcInfo.monitorNo3 + "',N'" + PCInfomation.pcInfo.monitorModel3 + "',N'" + PCInfomation.pcInfo.idNumber2 + "',N'" + PCInfomation.pcInfo.fullName2 + "',N'" + PCInfomation.pcInfo.depatment2 + "',N'" + PCInfomation.pcInfo.monthUpdate + "',N'" + PCInfomation.pcInfo.imsempcode + "',N'" + PCInfomation.pcInfo.insdt + "',N'" + PCInfomation.pcInfo.updempcode + "',N'" + PCInfomation.pcInfo.upddt + "')";
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
            // Db_Read_TableSql(command_AllData);
            //MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Kiểm tra lại Internet", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
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


        public void Process_CheckBiosPC()
        {
            checkBiosSerial = false;
            Db_Read_BiosSerial();


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
                                    if (PCInfomation.pcInfo.hdd1Serial == dr[9].ToString())
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

        public void Db_Read_TableSql(string command)
        {
            try
            {
                //lvPcManage.Items.Clear();
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
                                    pc_info.version = Ver;
                                    pc_info.monitorNo2 = dr[28].ToString();
                                    pc_info.monitorModel2 = dr[29].ToString();
                                    pc_info.monitorNo3 = dr[30].ToString();
                                    pc_info.monitorModel3 = dr[31].ToString();
                                    pc_info.idNumber2 = dr[32].ToString();
                                    pc_info.fullName2 = dr[33].ToString();
                                    if (pc_info.fullName2 != "")
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
                    //lvPcManage.Items.Add(item);
                    indexPC++;
                }
                //numberPC.Content = indexPC.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        

        protected override void OnStop()
        {
            Utilities.WriteLogError("Stop " + PCInfomation.pcInfo.hdd1Serial + " " + PCInfomation.pcInfo.upddt);
            TimeStop();
        }


        public static class PCInfomation
        {
            public static List<Helper_PcInfomation> listPC = new List<Helper_PcInfomation>();
            public static Helper_PcInfomation pcInfo = new Helper_PcInfomation();
            public static Helper_PcProcessData pcData = new Helper_PcProcessData();
        }
    }
}
