
namespace SHBSS
{
    class SSCTable
    {
        private static SSCTable _SSCTable = null;
        private SSCTable()    //私有构造函数，保证唯一性
        {           
        }
        public void Init()
        {
            Name = "FFFF";
            IDCard = "000000000000000000";
            Error = "FFFF";
            PayCode = "FFFF";
            Transtime = "1970-01-01 00:00:00";
            nPro = -1;
            Sex = "0";
            SSCard = "000000000";
            Phone = "00000000000";
            MakeTime = "1970-01-01 00:00:00";
        }
        public static SSCTable getSSCTable()
        {
            if (_SSCTable == null) {
                _SSCTable = new SSCTable();
            }
            return _SSCTable;
        }
        // 两张表公共数据
        public long ID { get; set; }//id号
        public string Name { get; set; }//姓名
        public string IDCard { get; set; }//身份证号
        public string Error { get; set; }//错误原因

        // PayRec
        public string PayCode { get; set; }//缴款码
        public string Transtime { get; set; }//交易时间
        public int nPro { get; set; }//进度 

        // CardsRec
        public string Sex { get; set; }//性别
        public string SSCard { get; set; }//社保卡号
        public string Phone { get; set; }//手机号
        public string MakeTime { get; set; }//制卡时间
    }
    
}
