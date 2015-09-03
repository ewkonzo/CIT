using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace MemberManagement
{
    public static class db
    {
        private static MySqlConnection mscon;

        //string csql = "SELECT SCHEMA_NAME, DEFAULT_CHARACTER_SET_NAME, DEFAULT_COLLATION_NAME FROM information_schema.SCHEMATA WHERE SCHEMA_NAME='carparkboffline'";

        public static DataTable LoadData(string sql)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            try
            {
                Open();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, mscon);
                da.Fill(ds, "Table");
                dt = ds.Tables["Table"];
                mscon.Close();
            }
            catch (Exception)
            {
            }
            return dt;
        }

        public static bool Connect(string strIP,string databaseName)
        {
            string strConnMySQL = "Database="+databaseName+";Data Source=" + strIP + ";User Id=cit;Password=db13apr;Charset=utf8;";
            bool booConnect = false;
            try
            {
                mscon = new MySqlConnection(strConnMySQL);
                mscon.Open();
                mscon.Close();
                booConnect = true;
                //IPServerMain = strIP;
            }
            catch (Exception)
            {
            }
            return booConnect;
        }

        private static void Open()
        {
            if (mscon.State == ConnectionState.Open)
                mscon.Close();
            mscon.Open();
        }

        public static string SaveData(string sql)
        {
            string result = "";
            MySqlCommand cmd;
            try
            {
                Open();
                cmd = new MySqlCommand(sql, mscon);
                cmd.ExecuteNonQuery();
                mscon.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
        /*
        public static string SaveData(string sql, Byte[] btyImageL, Byte[] btyImageD)
        {
            string result = "";
            MySqlCommand cmd;
            try
            {
                Open();
                cmd = new MySqlCommand(sql, mscon);
                cmd.Parameters.AddWithValue("@FileD", btyImageD);
                cmd.Parameters.AddWithValue("@FileL", btyImageL);
                cmd.ExecuteNonQuery();
                mscon.Close();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
     */
    }
}
