using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace SHBSS
{
    class SSCSQL
    {
        static string lijing = AppDomain.CurrentDomain.BaseDirectory;
        static string url = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lijing + "\\Data\\SSC.mdb;Jet OLEDB:Database Password=";
        static SSCTable ssc = SSCTable.getSSCTable();

        /// <summary>
        /// 添加数据
        //SSCTable ssc = SSCTable.getSSCTable();
        //ssc.Name = "huamginhua";
        //    ssc.IDCard = "45658986525";
        //    ssc.PayCode = "46558894856";
        //    ssc.Transtime = "2020/04/03 10:26:03";
        //    ssc.nPro = 0;
        //    bool b = SSCSQL.Insert();
        /// </summary>
        /// <returns></returns>

        static public bool Insert()
        {
            bool bolReturn = false;
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strCmd = "insert into PayRec(Name,IDCard,PayCode,Transtime,nPro,Error) values (:Name,:IDCard,:PayCode,:Transtime,:nPro,:Error)";
                OleDbCommand oleCmd = new OleDbCommand(strCmd, conn);
                if (ssc.Error == null) {
                    ssc.Error = "";
                }
                oleCmd.Parameters.AddWithValue("Name", ssc.Name);
                oleCmd.Parameters.AddWithValue("IDCard", ssc.IDCard);
                oleCmd.Parameters.AddWithValue("PayCode", ssc.PayCode);
                oleCmd.Parameters.AddWithValue("Transtime", ssc.Transtime);
                oleCmd.Parameters.AddWithValue("nPro", ssc.nPro);
                oleCmd.Parameters.AddWithValue("Error", ssc.Error);
                int a = oleCmd.ExecuteNonQuery();
                conn.Close();
                if (a > 0) {
                    Utility.WriteLog("插入数据：" + ssc.nPro.ToString());
                    bolReturn = true;
                }
                else {
                    Utility.WriteLog("缴费记录插入失败。");
                }
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("缴费记录插入：" + ex.Message);
                return bolReturn;
            }
            return bolReturn;
        }
        static public bool InsertCardsRec()
        {
            bool bolReturn = false;
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strCmd = "insert into CardsRec(Name,Sex,IDCard,SSCard,Phone,MakeTime,Error) values (:Name,Sex,:IDCard,:SSCard,:Phone,:MakeTime,:Error)";
                OleDbCommand oleCmd = new OleDbCommand(strCmd, conn);
                if (ssc.Error == null) ssc.Error = "";
                oleCmd.Parameters.AddWithValue("Name", ssc.Name);
                oleCmd.Parameters.AddWithValue("Sex", ssc.Sex);
                oleCmd.Parameters.AddWithValue("IDCard", ssc.IDCard);
                oleCmd.Parameters.AddWithValue("SSCard", ssc.SSCard);
                oleCmd.Parameters.AddWithValue("Phone", ssc.Phone);
                oleCmd.Parameters.AddWithValue("MakeTime", ssc.MakeTime);
                oleCmd.Parameters.AddWithValue("Error", ssc.Error);
                int a = oleCmd.ExecuteNonQuery();
                conn.Close();
                if (a > 0) {
                    bolReturn = true;
                }
                else {
                    Utility.WriteLog("制卡数据插入失败。");
                }
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("制卡数据插入：" + ex.Message);
                return bolReturn;
            }
            return bolReturn;
        }

        /// <summary>
        /// 返回  缴款码|交易时间|制卡进度
        /// </summary>
        /// <param name="IDCard"></param>
        /// <returns></returns>
        static public string Query()
        {
            string payRecord = "";
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select PayCode,Transtime,nPro from PayRec where IDCard=" + "'" + ssc.IDCard + "'";
                OleDbDataReader dr = cmd.ExecuteReader();
                string code = "", time = "", pro = "";
                while (dr.Read()) {
                    code = dr["PayCode"].ToString();
                    time = dr["Transtime"].ToString();
                    pro = dr["nPro"].ToString();
                }
                if (pro != "")
                    payRecord = code + "|" + time + "|" + pro;
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("查询：" + ex.Message);
                return "";
            }
            return payRecord;
        }
        //修改数据
        static public bool Update()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("Update PayRec set nPro='{0}',Error='{1}' where PayCode='{1}'", ssc.nPro, ssc.Error, ssc.PayCode);
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                int dnum = cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                if (dnum > 0) {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("更新：" + ex.Message);
                return false;
            }
        }
        //删除数据
        static public bool Delete()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = "delete from PayRec where IDCard ='" + ssc.IDCard + "'";
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                int dnum = cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                if (dnum > 0) {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("删除：" + ex.Message);
                return false;
            }
        }

        static public bool Delete10Ago()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string day10Ago = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd 23:59:59");
                string strSql = string.Format("delete from PayRec where Transtime <=#{0}#", day10Ago);
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                int dnum = cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                if (dnum > 0) {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("删除：" + ex.Message);
                return false;
            }
        }
        //取对象集合中ID最大的对象
        static public SSCTable MaxOderData(List<SSCTable> orderList)
        {
            orderList.Sort(
                delegate (SSCTable st1, SSCTable st2)
                {
                    //降序排列
                    //return st2.ID.CompareTo(st1.ID);
                    //升序版（颠倒 st1 和 st2 即可）
                    return st1.ID.CompareTo(st2.ID);
                }
            );
            return orderList.Last();
        }

        /************************************ CityCode ************************************/

        static public bool getAreaTable(string CityName, ref DataTable dt)
        {
            OleDbConnection conn = null;
            dt.Clear();
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("SELECT distinct AreaCode,AreaName FROM CityCode where CityName='{0}'", CityName);
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                OleDbDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                cmd.Dispose();
                conn.Close();
                if (dt.Rows.Count > 0) {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("getAreaTable：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据区县代码，查询镇编号和名称
        /// </summary>
        /// <param name="PostCode">区县代码</param>
        /// <param name="dt">镇编号、名称表</param>
        /// <returns></returns>
        static public bool getTownTable(string AreaCode, ref DataTable dt)
        {
            OleDbConnection conn = null;
            dt.Clear();
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("SELECT distinct TownCode,TownName FROM CityCode where AreaCode={0}", AreaCode);
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                OleDbDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                cmd.Dispose();
                conn.Close();
                if (dt.Rows.Count > 0) {
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("getTownsTable：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据镇名称，查询所有村编号和名称
        /// </summary>
        /// <param name="TownsCode">镇编号</param>
        /// <param name="dt">村编号、名称表</param>
        /// <returns></returns>
        static public bool getSqbhTable(string TownsCode, ref DataTable dt)
        {
            OleDbConnection conn = null;
            dt.Clear();
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("SELECT distinct VillageNo,VillageName FROM CityCode where TownCode={0}", TownsCode);
                OleDbCommand cmd = new OleDbCommand(strSql, conn);
                OleDbDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                cmd.Dispose();
                conn.Close();
                if (dt.Rows.Count > 0) {
                    return true;
                }
                Utility.WriteLog("getSqbhTable：查找社区编号条数为0");
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("getSqbhTable：" + ex.Message);
                return false;
            }
        }
    }

}
