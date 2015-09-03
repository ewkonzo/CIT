using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MemberManagement
{
    public class Member
    {
        public string ID = "";
        public string Name = "";
        public string Address = "";
        public string CarName = "";
        public string Tel = "";
        public string License = "";
        public string DateExpire = "";
        public int NoHour = 0;
        public int NoPrice = 0;
        public int TypeID = 200; //Mac 2015/02/09
        //public Byte[] picDriver;
        //public Byte[] picLicense;

        public bool LoadData(uint intID, bool booOffline)
        {
            bool result = false;
            string sql = "SELECT * FROM member";
            sql += " WHERE cardid=" + intID.ToString();
            DataTable dt;// = db.LoadData(sql);
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                Name = dt.Rows[0].ItemArray[1].ToString();
                Address = dt.Rows[0].ItemArray[2].ToString();
                Tel = dt.Rows[0].ItemArray[3].ToString();
                License = dt.Rows[0].ItemArray[5].ToString();
                CarName = dt.Rows[0].ItemArray[6].ToString();
                NoHour = Convert.ToInt32(dt.Rows[0].ItemArray[7]);
                NoPrice = Convert.ToInt32(dt.Rows[0].ItemArray[8]);
                DateExpire = dt.Rows[0].ItemArray[10].ToString();
                try //Mac 2015/02/09
                {
                    TypeID = Convert.ToInt32(dt.Rows[0].ItemArray[14]);
                }
                catch (Exception)
                {
                    TypeID = 200;
                }
                //picDriver = (Byte[])dt.Rows[0].ItemArray[5];
                //picLicense = (Byte[])dt.Rows[0].ItemArray[6];
                result = true;
            }
            return result;
        }
        public bool LoadDataKeyLicense(string NameMem, string LicMem, bool booOffline) //Mac 2015/04/01
        {
            bool result = false;
            string sql = "SELECT * FROM member";
            //sql += " WHERE name = '" + NameMem + "' AND license = '" + LicMem + "'";
            sql += " WHERE license = '" + LicMem + "'";
            DataTable dt;// = db.LoadData(sql);
            dt = db.LoadData(sql);
            if (dt.Rows.Count > 0)
            {
                Name = dt.Rows[0].ItemArray[1].ToString();
                Address = dt.Rows[0].ItemArray[2].ToString();
                Tel = dt.Rows[0].ItemArray[3].ToString();
                License = dt.Rows[0].ItemArray[5].ToString();
                CarName = dt.Rows[0].ItemArray[6].ToString();
                NoHour = Convert.ToInt32(dt.Rows[0].ItemArray[7]);
                NoPrice = Convert.ToInt32(dt.Rows[0].ItemArray[8]);
                DateExpire = dt.Rows[0].ItemArray[10].ToString();
                try //Mac 2015/02/09
                {
                    TypeID = Convert.ToInt32(dt.Rows[0].ItemArray[14]);
                }
                catch (Exception)
                {
                    TypeID = 200;
                }
                //picDriver = (Byte[])dt.Rows[0].ItemArray[5];
                //picLicense = (Byte[])dt.Rows[0].ItemArray[6];
                result = true;
            }
            return result;
        }
        /*
             Byte[] bytImageL = (Byte[])dt.Rows[0].ItemArray[6];
                using (var stream = new MemoryStream(bytImageL))
                {
                    bmPicD = new BitmapImage();
                    bmPicD.BeginInit();
                    bmPicD.StreamSource = stream;
                    bmPicD.CacheOption = BitmapCacheOption.OnLoad;
                    bmPicD.EndInit();
                    bmPicD.Freeze();
                }
        */

    }
}
