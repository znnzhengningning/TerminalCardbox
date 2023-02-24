using System;
using System.Data;
using System.Data.OleDb;

namespace SHBSS
{
    public class MDATA
    {
        private static MDATA _mdata = null;
        private MDATA()    //私有构造函数，保证唯一性
        {
        }
        public static MDATA getInstance()
        {
            if (_mdata == null) {
                _mdata = new MDATA();
            }
            return _mdata;
        }
        public void Init()
        {
            BankNO = "FFFFFF FFFFFF FFFFFF";
            BankCode = "FFFFF";
            nCardPosition = -1;
            nCartRidge = -1;
            AddTime = "1970-01-01 00:00:00";
        }
        public string CardId { get; set; }
        public string BankNO { get; set; }
        public string BankCode { get; set; }
        public int nCardPosition { get; set; }
        public int nCartRidge { get; set; }
        public string AddTime { get; set; }
        public int nType { get; set; }

    }
    
    public class MDATASQL
    {
        static string lujing = AppDomain.CurrentDomain.BaseDirectory;
        static string url = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lujing + "\\Data\\MDATA.mdb;Jet OLEDB:Database Password=";
        static MDATA mdata = MDATA.getInstance();
        static public bool AddCardAllowance()
        {
            bool bolReturn = false;
            string strCmd = "";
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                strCmd = "insert into CardAllowance(BankNO,BankCode,nCardPosition,nCartRidge,AddTime) values (:BankNO,:BankCode,:nCardPosition,:nCartRidge,:AddTime)";
                OleDbCommand oleCmd = new OleDbCommand(strCmd, conn);

                oleCmd.Parameters.AddWithValue("BankNO", mdata.BankNO);
                oleCmd.Parameters.AddWithValue("BankCode", mdata.BankCode);
                oleCmd.Parameters.AddWithValue("nCardPosition", mdata.nCardPosition);
                oleCmd.Parameters.AddWithValue("nCartRidge", mdata.nCartRidge);
                oleCmd.Parameters.AddWithValue("AddTime", mdata.AddTime);
                int a = oleCmd.ExecuteNonQuery();
                conn.Close();
                bolReturn = true;
                return bolReturn;
            }
            catch (Exception ex) {
                Utility.WriteLog("MDATASQL Exception - AddCardAllowance：" + ex.Message);
                if (conn != null)
                    conn.Close();
                return bolReturn;
            }
        }

        static public bool AddUserRecord(string CardId, int nType)
        {
            bool bolReturn = false;
            string strCmd = "";
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                strCmd = "insert into UserRecord(CardId,BankNO,BankCode,AddTime,nType) values (:CardId,:BankNO,:BankCode,:AddTime,:nType)";
                OleDbCommand oleCmd = new OleDbCommand(strCmd, conn);

                oleCmd.Parameters.AddWithValue("CardId", mdata.CardId);
                oleCmd.Parameters.AddWithValue("BankNO", mdata.BankNO);
                oleCmd.Parameters.AddWithValue("BankCode", mdata.BankCode);
                oleCmd.Parameters.AddWithValue("AddTime", mdata.AddTime);
                oleCmd.Parameters.AddWithValue("nType", mdata.nType);
                int a = oleCmd.ExecuteNonQuery();
                conn.Close();
                bolReturn = true;
                return bolReturn;
            }
            catch (Exception ex) {
                Utility.WriteLog("MDATASQL Exception - AddUserRecord：" + ex.Message);
                if (conn != null)
                    conn.Close();
                return bolReturn;
            }
        }

        /// <summary>
        /// 获取所有银行编码
        /// </summary>
        ///  <param name="banks">new 一个字符串数组</param>
        /// <returns>返回不同的银行编码数量</returns>
        static public int getAllBank(ref DataTable dtBanks)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT distinct BankCode  FROM CardAllowance";
                OleDbDataReader dr = cmd.ExecuteReader();
                dtBanks.Load(dr); //dtBanks = dt.DefaultView.ToTable(true, "BankCode");
                dr.Close();
                cmd.Dispose();
                conn.Close();
                int lenth = dtBanks.Rows.Count;
                return lenth;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getAllBank：" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 通过银行编码获取第一条卡片所在位置的数据
        /// </summary>
        /// <param name="Bank"></param>
        /// <returns></returns>
        static public bool getCardPosition(string BankCode, ref int[] nPos, ref string BankNo)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select top 1 ID, nCardPosition,nCartRidge,BankNO from CardAllowance where BankCode ='" + BankCode + "'" + "order by ID asc";
                OleDbDataReader dr = cmd.ExecuteReader();
                // reader[0]; 速度中等  reader["字段名"]; 速度最慢  reader.GetValue(0); 速度最快
                bool bret;
                if (dr.Read()) {
                    bret = true;
                    nPos[0] = int.Parse(dr.GetValue(1).ToString());
                    nPos[1] = int.Parse(dr.GetValue(2).ToString());
                    BankNo = dr.GetValue(3).ToString();
                }
                else {
                    bret = false;
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
                return bret;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getCardPosition：" + ex.Message);
                return false ;
            }
        }

        /// <summary>
        /// 通过两个位置参数 返回银行卡号
        /// </summary>
        /// <param name="nCardPosition"></param>
        /// <param name="nCartRidge"></param>
        /// <returns></returns>
        string getBankNo(int nCardPosition, int nCartRidge)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("select BankNO from CardAllowance where nCardPosition ={0} and nCartRidge ={1}", nCardPosition, nCartRidge);
                OleDbDataReader dr = cmd.ExecuteReader();
                // reader[0]; 速度中等  reader["字段名"]; 速度最慢  reader.GetValue(0); 速度最快
                string bankno = "";
                while (dr.Read()) {
                    bankno = dr.GetValue(0).ToString();
                    break;
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
                return bankno;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getBankNo：" + ex.Message);
                return "";
            }
        }

        static public bool getEachBankSum(ref DataTable dtBankSum)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT BankCode, count(ID) as countAll FROM CardAllowance group by BankCode  order by BankCode asc";
                OleDbDataReader dr = cmd.ExecuteReader();
                dtBankSum.Load(dr);
                dr.Close();
                cmd.Dispose();
                conn.Close();
                if (dtBankSum.Rows.Count > 0) return true;
                return false;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getEachBankSum：" + ex.Message);
                return false;
            }
        }

        static public bool DeletePreCard(int nCardPosition, int nCartRidge)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("delete from CardAllowance where nCardPosition ={0} and nCartRidge = {1}", nCardPosition, nCartRidge);
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
                Utility.WriteLog("MDATASQL Exception - DeletePreCard：" + ex.Message);
                return false;
            }
        }

        static public bool DeletePreCardAll()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = "delete from CardAllowance";
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
                Utility.WriteLog("MDATASQL Exception - DeletePreCardAll：" + ex.Message);
                return false;
            }
        }

        /////////////////////////////////////////////////////////////////////

        public static bool AddSiCard()
        {
            bool bolReturn = false;
            string strCmd = "";
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                strCmd = "insert into SiCard(CardId,nCardPosition,nCartRidge,AddTime) values (:CardId,:nCardPosition,:nCartRidge,:AddTime)";
                OleDbCommand oleCmd = new OleDbCommand(strCmd, conn);

                oleCmd.Parameters.AddWithValue("CardId", mdata.CardId);
                oleCmd.Parameters.AddWithValue("nCardPosition", mdata.nCardPosition);
                oleCmd.Parameters.AddWithValue("nCartRidge", mdata.nCartRidge);
                oleCmd.Parameters.AddWithValue("AddTime", mdata.AddTime);
                int a = oleCmd.ExecuteNonQuery();
                conn.Close();
                bolReturn = true;
            }
            catch (Exception ex) {
                Utility.WriteLog("MDATASQL Exception - AddSiCard：" + ex.Message);
                if (conn != null)
                    conn.Close();
                return bolReturn;
            }
            return bolReturn;
        }

        static public int SelectSiCardAll()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                //打开数据
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT count(1) AS count FROM SiCard";
                OleDbDataReader dr = cmd.ExecuteReader();
                return int.Parse(dr.GetValue(0).ToString());
            }
            catch (Exception ex) {
                Utility.WriteLog("MDATASQL Exception - SelectSiCardAll：" + ex.Message);
                if (conn != null)
                    conn.Close();
                return 0;
            }
        }
        static public bool DeleteSiCard(int nCardPosition, int nCartRidge)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = string.Format("delete from SiCard where nCardPosition ={0} and nCartRidge = {1}", nCardPosition, nCartRidge);
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
                Utility.WriteLog("MDATASQL Exception - DeleteSiCard：" + ex.Message);
                return false;
            }
        }

        static public bool DeleteSiCardAll()
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                string strSql = "delete from SiCard";
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
                Utility.WriteLog("MDATASQL Exception - DeleteSiCardAll：" + ex.Message);
                return false;
            }
        }

        static public bool getSiCardPosition(ref int[] nPos)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select top 1 ID, nCardPosition,nCartRidge from SiCard order by ID asc";
                OleDbDataReader dr = cmd.ExecuteReader();
                // reader[0]; 速度中等  reader["字段名"]; 速度最慢  reader.GetValue(0); 速度最快
                bool bret;
                if (dr.Read()) {
                    bret = true;
                    nPos[0] = int.Parse(dr.GetValue(1).ToString());
                    nPos[1] = int.Parse(dr.GetValue(2).ToString());
                }
                else {
                    bret = false;
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
                return bret;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getSiCardPosition：" + ex.Message);
                return false;
            }
        }

        static public bool getSiCardPosition2(string CardId, ref int[] nPos)
        {
            OleDbConnection conn = null;
            try {
                conn = new OleDbConnection(url);
                if (conn.State == ConnectionState.Open) {
                    conn.Close();
                }
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select nCardPosition,nCartRidge from SiCard where CardId = '" + CardId + "'";
                OleDbDataReader dr = cmd.ExecuteReader();
                // reader[0]; 速度中等  reader["字段名"]; 速度最慢  reader.GetValue(0); 速度最快
                bool bret;
                if (dr.Read()) {
                    bret = true;
                    nPos[0] = int.Parse(dr.GetValue(0).ToString());
                    nPos[1] = int.Parse(dr.GetValue(1).ToString());
                }
                else {
                    bret = false;
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
                return bret;
            }
            catch (Exception ex) {
                if (conn != null)
                    conn.Close();
                Utility.WriteLog("MDATASQL Exception - getSiCardPosition2：" + ex.Message);
                return false;
            }
        }

        static public string getBankCode(string BankNO)
        {
            string taitou = BankNO.Substring(0, 6);
            switch (taitou) {
                case "621756":
                    return "95566";
                case "623059": {
                        string quxian = BankNO.Substring(6, 4);
                        if (quxian == "4561")
                            return "962884561";
                        else if (quxian == "4562")
                            return "962884562";
                        else if (quxian == "4563")
                            return "962884563";
                        else if (quxian == "4570")
                            return "962884570";
                        else if (quxian == "4568")
                            return "962884568";
                        else if (quxian == "4569")
                            return "962884569";
                        else if (quxian == "4571")
                            return "962884571";
                        else
                            return "96288";
                    }
                case "621797":
                    return "95580";
                case "623660":
                    return "96688";
                case "621467":
                    return "95533";
                case "621483":
                    return "95555";
                case "621770":
                    return "95558";
                case "622262":
                    return "95559";
                case "621721":
                    return "95588";
                case "622823":
                    return "95599";
                case "623152":
                    return "96588";
                case "623118":
                    return "96699";
                case "sitest":
                    return "99999";
                default:
                    return "00000";
            }
        }

        static public string getBankName(string bankcode)
        {
            switch (bankcode) {
                case "95580":
                    return "邮储银行";
                case "96288":
                    return "河南农信";
                case "962884561":
                    return "浉河农信银行";
                case "962884562":
                    return "平桥农信银行";
                case "962884563":
                    return "罗山农信银行";
                case "962884570":
                    return "新县农信银行";
                case "962884568":
                    return "淮滨农信银行";
                case "962884569":
                    return "商城农信银行";
                case "962884571":
                    return "明港农信银行";
                case "95533":
                    return "建设银行";
                case "95555":
                    return "招商银行";
                case "95558":
                    return "中信银行";
                case "95559":
                    return "交通银行";
                case "95561":
                    return "兴业银行";
                case "95566":
                    return "中国银行";
                case "95588":
                    return "工商银行";
                case "95599":
                    return "农业银行";
                case "96588":
                    return "平顶山银行";
                case "96688":
                    return "中原银行";
                case "96699":
                    return "洛阳银行";
                case "99999":
                    return "测试银行";
                default:
                    return "未知银行";
            }
        }
    }
}
