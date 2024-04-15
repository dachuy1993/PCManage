using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace PcManageServiceUpdate
{
    public partial class Service1 : ServiceBase
    {
        string path_sql = "Data Source=192.168.2.5,1433;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password= oneuser1!";
        string verSQL, bufferExe, fileType, libraryName;
        string nameFolderIni = "PCManageService";
        string nameFolderExe = @"C:\TaixinProgram\PcManage\Service\";


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            RunCmdStopService();
            Process_UpdateExeFile();
            RunCmdStartService();
        }

        protected override void OnStop()
        {
           
        }


        public void RunCmdStopService()
        {
            

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Verb = "runas";
            
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("net stop PCManageService");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        public void RunCmdStartService()
        {


            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Verb = "runas";

            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("net start PCManageService");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }



        public void Db_Read_Version()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(path_sql))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("select top(1)* from FileUpdateService order by ID desc", conn))
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

        public void Process_UpdateExeFile()
        {
            Db_Read_Version();
            string fileExeOperasystem = nameFolderExe + nameFolderIni + ".exe";

            byte[] buffer = File.ReadAllBytes(fileExeOperasystem);
            string base64Encoded = Convert.ToBase64String(buffer);
           // SecureString secureStringPass = new NetworkCredential("", PassLogin).SecurePassword;
            if (base64Encoded != bufferExe)
            {

                byte[] buferNew = System.Convert.FromBase64String(bufferExe);
                string fileExeNew = System.Text.ASCIIEncoding.ASCII.GetString(buferNew);
                //string fileExeNew = 
                File.WriteAllBytes(fileExeOperasystem, buferNew);
               // Process.Start(fileExeOperasystem, UserLogin, secureStringPass, "Abc");
            }
            else
            {
                //Process.Start(fileExeOperasystem, UserLogin, secureStringPass, "Abc");
            }


            //Process.Start(fileExeOperasystem);
            //this.Close();
        }
    }
}
