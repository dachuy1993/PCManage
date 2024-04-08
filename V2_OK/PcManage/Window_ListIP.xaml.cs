using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace PcManage
{
    /// <summary>
    /// Interaction logic for Window_ListIP.xaml
    /// </summary>
    public partial class Window_ListIP : Window
    {
      
        public Window_ListIP()
        {
            InitializeComponent();          
        }


        private void lvListIP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        string path_sql = "Data Source=192.168.2.5;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password= oneuser1!";

        List<Helper_PcInfomation> listIPAddres = new List<Helper_PcInfomation>();
        public void Db_Read_TableSql(string ipAddress)
        {
            try
            {
                listIPAddres.Clear();
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    var command = "SELECT * from  ComputerInfomation";
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Helper_PcInfomation pc_info = new Helper_PcInfomation();
                                if (dr[0] != null)
                                {
                                    string ip = dr[6].ToString();                                   
                                    int index = ip.LastIndexOf(".");
                                    string ipTemp = ip.Substring(8, index - 8);
                                    if(ipTemp == ipAddress)
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
                                        pc_info.run = dr[18].ToString();
                                        pc_info.diskTotal_C = dr[19].ToString();
                                        pc_info.diskTotal_D = dr[20].ToString();
                                        pc_info.diskTotal_E = dr[21].ToString();
                                        pc_info.diskFree_C = dr[22].ToString();
                                        pc_info.diskFree_D = dr[23].ToString();
                                        pc_info.diskFree_E = dr[24].ToString();
                                        pc_info.monitorNo = dr[25].ToString();
                                        pc_info.monitorModel = dr[26].ToString();
                                        pc_info.version = dr[27].ToString();
                                        pc_info.monitorNo2 = dr[28].ToString();
                                        pc_info.monitorModel2 = dr[29].ToString();
                                        pc_info.monitorNo3 = dr[30].ToString();
                                        pc_info.monitorModel3 = dr[31].ToString();
                                        pc_info.idNumber2 = dr[32].ToString();
                                        pc_info.fullName2 = dr[33].ToString();
                                        pc_info.depatment2 = dr[34].ToString();
                                        pc_info.monthUpdate = dr[35].ToString();
                                        pc_info.imsempcode = dr[36].ToString();
                                        pc_info.insdt = dr[37].ToString();
                                        pc_info.updempcode = dr[38].ToString();
                                        pc_info.upddt = dr[39].ToString();
                                        listIPAddres.Add(pc_info);
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
                Application.Current.Shutdown();
            }
        }

        public void Process_IP(string ip)
        {
            Db_Read_TableSql(ip);
            List<Helper_PcInfomation> listTemp = new List<Helper_PcInfomation>();
            List<Helper_PcInfomation> listIPFull = new List<Helper_PcInfomation>();
            listTemp.Clear();
            listIPFull.Clear();
            int qtyIP = 0;
            for (int i = 0; i <= 255; i++)
            {
                Helper_PcInfomation pc = new Helper_PcInfomation();
                pc.ipAddress = "192.168." + ip + "." + i.ToString();
                pc.ID = i;
               
                foreach (var item in listIPAddres)
                {
                    int indexC = item.ipAddress.LastIndexOf(".") + 1;
                    int leng = item.ipAddress.Length;
                    int ipItem = int.Parse(item.ipAddress.Substring(indexC, leng - indexC));
                    item.ipAddress = "192.168." + ip + "." + ipItem.ToString();
                    if (item.ipAddress == pc.ipAddress)
                    {
                        pc = item;
                        qtyIP++;
                    }    
                }               
                listTemp.Add(pc);
            }
           
            lvListIP.Items.Clear();
            int indexColor = 0;
            foreach (var item in listTemp)
            {
                indexColor++;
                item.ID = indexColor;
                lvListIP.Items.Add(item);
            }
            lbQtyIP.Content = qtyIP.ToString() + " PC";
        }

        private void btnFilterInfo_Click(object sender, RoutedEventArgs e)
        {
            string ip = txtFilterInfo.Text;
            Process_IP(ip);
        }

        private void txtFilterInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string ip = txtFilterInfo.Text;
                Process_IP(ip);
            }    
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
        }        
    }
}
