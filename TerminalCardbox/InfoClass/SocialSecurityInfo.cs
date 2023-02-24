using System;
using System.Drawing;
using System.Reflection;

namespace SHBSS
{
    public class SocialSecurityInfo
    {
        private static SocialSecurityInfo _ssi = null;
        public static SocialSecurityInfo GetInstance()
        {
            if (_ssi == null)
            {
                _ssi = new SocialSecurityInfo();
            }
            return _ssi;
        }
        private SocialSecurityInfo()
        {
        }
        public bool isAdult { get; set; }         // 是否成年人
        public string ID { get; set; }            // 身份证号
        public string ownerName { get; set; }     // 姓名
        public string sex { get; set; }           // 性别
        public string nation { get; set; }        // 民族
        public string idvalid { get; set; }       // 身份证有效期
        public string securityOldNo { get; set; } // 旧社保卡号
        public string securityNo { get; set; }    // 新社保卡号
        public string makeDate                    // 制卡日期
        {
            get { return DateTime.Now.ToString("yyyyMMdd"); }
        }
        
        public string sqlx { get; set; }          // 申请类型
        public string zkzt { get; set; }          // 制卡进度
        public string valid { get; set; }         // 社保卡有效期
        public string birthday { get; set; }      // 出生日期
        public string phone { get; set; }         // 手机号
        public Image photo { get; set; }          // 照片
        public string photopath { get; set; }     // 照片路径
        public bool b_replacephoto { get; set; } // 补换卡是否换照片
        public string sbkstatus { get; set; }     // 卡状态
        public string addr { get; set; }          // 地址
        public string bankcode { get; set; }      // 银行代码

        public string hkxz { get; set; }      // 户口性质
        public string ryzt { get; set; }      // 人员状态
        public string rylb { get; set; }      // 人员类别
        public string dwbh { get; set; }      // 单位编号
        public string cbdw { get; set; }      // 参保单位
        public string sqbh { get; set; }      // 社区编号
        public string sqcmc { get; set; }      // 社区村名称

        public string cardverson { get; set; }    // 卡版本
        public string cardIdentify { get; set; }  // 卡识别码
        public string cardReset { get; set; }    // 卡复位码
        public string CA { get; set; }           // 签名公钥
        public string cardagent { get; set; }    // 卡商

        public string mdbQryResult { get; set; } // 数据库查找结果

        public void Init()
        {
            ID = "";
            ownerName = "";
            sex = "";
            nation = "";
            idvalid = "";
            securityOldNo = "";
            securityNo = "";
            //makeDate = "";

            sqlx = "";
            zkzt = "";
            valid = "";
            birthday = "";
            phone = "";
            photo = null;
            //photopath = "";
            b_replacephoto = false;
            sbkstatus = "";
            addr = "";
            bankcode = "";

            sqbh = "";
            sqcmc = "";

            cardverson = "";
            cardIdentify = "";
            cardReset = "";
            CA = "";
            cardagent = "";
        }
    }
}
