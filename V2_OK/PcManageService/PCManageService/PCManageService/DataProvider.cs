using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCManageService
{
    public class DataProvider
    {
        private const string CONNECTION_STRING = "Data Source=192.168.2.5;Initial Catalog=PC_Manage;Persist Security Info=True;User ID=sa;Password=oneuser1!";

        private static DataProvider instance;

        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }
            private set => instance = value;
        }
        /// <summary>
        /// SQL Server Execute Query
        /// </summary>
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 3600;
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }
        public DataTable ExecuteSP(string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                string[] listpara = query.Split(' ');
                SqlCommand cmd = new SqlCommand(listpara[0], con);
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameter != null)
                {
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            return dt;
        }

        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int a = 0;
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 3600;
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                a = cmd.ExecuteNonQuery();
                con.Close();
            }
            return a;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object a = 0;
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                a = cmd.ExecuteScalar();
                con.Close();
            }
            return a;
        }

        public List<string> GetList(string query, object[] parameter = null)
        {
            List<string> list = new List<string>();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (parameter != null)
                {
                    string[] listpara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listpara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.Add(new SqlParameter(item, SqlDbType.NVarChar)).Value = parameter[i];
                            i++;
                        }
                    }
                }
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    int i = 0;
                    list.Add(dr.GetString(0));
                    i += 1;
                }
                con.Close();
            }
            return list;
        }
    }
}
