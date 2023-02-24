using System;
using System.Windows.Forms;
using SHBSS.SIService;
using System.Xml.Linq;
using System.IO;
using CommenLib.MP3;
using System.Text;
using CommenLib.HttpUtils;
using System.Runtime.InteropServices;
using System.Threading;
using CommenLib.Sec;

namespace SHBSS
{
    public partial class FrmSetting : Form
    {
        public FrmSetting()
        {
            InitializeComponent();
            Solid510.Init();
        }

        SocialSecurityInfo ssi;
        CardServiceClient _csc;

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            ssi = SocialSecurityInfo.GetInstance();
            _csc = new SIService.CardServiceClient("CardService");
            KaGuan.GetSIService();
        }

        private void btn_GetCardInfo_Click(object sender, EventArgs e)
        {
            MP3Player.Play(FrmMain.g_soundDir + "1IDCard.mp3");
            if (!SiCard.ReadIDCard()) {
                lblError.Text = SiCard.error;
                return;
            }
            lblError.Text = SiCard.error;
            //////////////////////////////////////////////////////////
            //通过姓名和身份证号取社保卡信息
            ////////////////////////////////////////////////////////////

            ////通过社保Webservice服务获取社保卡信息
            //lblError.Text = "查询社保信息...";
            //string rysjRet = "";
            //rysjRet = _csc.getRysj(KaGuan.USER, KaGuan.PSWD, ICReader.userID, ICReader.userName, "");

            //XElement root = XElement.Parse("<root>" + rysjRet.Trim() + "</root>");

            //string err = root.Element("ERR").Value;
            //if (!err.Equals("OK"))
            //{
            //    Utility.WriteLog(ICReader.userID + "," + ICReader.userName);
            //    Utility.WriteLog(rysjRet);
            //    lblError.Text = "\r\n查不到您的社保信息，请到服务台咨询。";
            //    return;
            //}

            //string Addr = root.Element("AAE006").Value;
            //string socialInsCard = root.Element("AAZ500").Value;

            //lblError.Text = "地址：" + Addr;
            //lblError.Text += "社保卡号：" + socialInsCard;

            //if (socialInsCard == "") { MessageBox.Show("查不到您的社保卡号，请到服务台咨询"); return; }

            //ssi.ownerName = ICReader.userName;
            //ssi.ID = ICReader.userID;
            //ssi.addr = Addr;
            //ssi.securityOldNo = socialInsCard;
            //ssi.idvalid = ICReader.validTime;

            //txtidcode.Text = ssi.ID;
            //txtName.Text = ssi.ownerName;
        }

        private void btn_lost_Click(object sender, EventArgs e)
        {
            //后台操作挂失
            lblError.Text += "\r\n挂失...\n";
            lblError.Text += _csc.allDsjk(string.Format("<操作*>正式挂失</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障卡卡号*>{3}</社会保障卡卡号*><社会保障号码*>{4}</社会保障号码*><姓名*>{5}</姓名*><开户银行></开户银行><银行卡号></银行卡号>",
                KaGuan.USER,
                KaGuan.PSWD,
                KaGuan.CITYCODE,
                ssi.securityOldNo,
                ssi.ID,
                ssi.ownerName));

            // 申请补换社保卡
            //string strRecvData = "";
            //string strSendXML = string.Format("<操作*>申请社保卡</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><经办机构*>{3}</经办机构*><服务银行*>{13}</服务银行*><国家地区*>CHN</国家地区*><证件类型*>1</证件类型*><证件号码*>{4}</证件号码*><证件有效期*>{5}</证件有效期*><姓名*>{6}</姓名*><性别*>{7}</性别*><民族*>{8}</民族*><出生日期*>{9}</出生日期*><人员状态>1</人员状态><户口性质>11</户口性质><户口地址>{10}</户口地址><移动电话>{11}</移动电话><固定电话*></固定电话*><通讯地址></通讯地址><邮政编码></邮政编码><电子邮箱></电子邮箱><所在社区(村)编号></所在社区(村)编号><单位编号></单位编号><单位名称></单位名称><监护人姓名></监护人姓名><监护人身份证号></监护人身份证号><监护人联系电话></监护人联系电话><办卡类型*>5</办卡类型*><补换原因>51</补换原因><社保卡号>{12}</社保卡号><相片*></相片*><指纹所属手指编码></指纹所属手指编码><指纹1></指纹1><指纹2></指纹2><指纹3></指纹3><指纹4></指纹4><个人编号></个人编号><社区（村）名称></社区（村）名称><单位部门></单位部门><学校所在系></学校所在系><是否大学></是否大学><是否邮寄></是否邮寄><收卡地址></收卡地址><收卡联系人></收卡联系人><收卡联系人电话></收卡联系人电话>",
            //     KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, KaGuan.AGENCY, ssi.ID, ssi.Valid, ssi.ownerName, ssi.sex,
            //     ssi.nation, ssi.birthday, ssi.Addr, ssi.phone, ssi.securityNo, KaGuan.BANK);
            // KaGuan.CallDsjk(strSendXML, ref strRecvData);
            // lblError.Text += strRecvData;
        }

       private void btn_payment_Click(object sender, EventArgs e)
        {
            string strlog = string.Format("手动给{0}做交费登记", ssi.ID);
            Utility.WriteLog(strlog);
            string RecvData = "";
            string SendDataXML = string.Format("<操作*>交费登记</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><城市代码>{4}</城市代码*>",
                                KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
            //KaGuan.SIService();
            if (!KaGuan.CallDsjk(SendDataXML, ref RecvData)) {
                string retmsg = "0," + ssi.ID + "," + ssi.ownerName + ",";
                string payfilename = ssi.ID + "payret.txt";
                //Utility.WritePayRet(retmsg, payfilename);
                //lblError.Text = "交费登记失败：" + KaGuan.g_strErrMsg;
            }
            lblError.Text = "交费登记成功";
        }
        
       
       private void btnExit_Click(object sender, EventArgs e)
        {
            //SiupoPrinter.ClosePrinter();
            Environment.Exit(0);
        }

        private void btnSiupoPrinterer_Click(object sender, EventArgs e)
        {
            try
            {
                Msprint.PrintTicket("黄敏华", "450802199104181282", "dfds454872dfds9892");
                //lblError.Text = Txp532Printer.GetInstance().strErr;
                //U10.ContrlLight(U10.U10_LIGHT_3, U10.U10_CTRL_ON);
                //Thread.Sleep(1500);
                //U10.ContrlLight(U10.U10_LIGHT_3, U10.U10_CTRL_OFF);
            }
            //try {
            //    SiupoPrinter.PrintTicket("黄敏华", "450802199104181282", "dfds454872dfds9892");
            //}
            catch (Exception ex) {
                lblError.Text = ex.Message;
            }
        }       

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strSendXML = "", strRecvData = "";
            ssi.ID = "450802199104181282";
            ssi.idvalid = "";
            ssi.ownerName = "黄敏华";
            ssi.sex = "";
            ssi.nation = "";
            ssi.birthday = "";
            ssi.addr = "";
            ssi.phone = "13714168670";
            //ssi.securityOldNo = "";
            ssi.bankcode = "95588";
            /**************************  领卡启用  **************************/
            //strSendXML = string.Format("<操作*>领卡启用</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障卡卡号*>{3}</社会保障卡卡号*><社会保障号码*>{4}</社会保障号码*><姓名*>{5}</姓名*>",
            //   KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, ssi.securityOldNo, ssi.ID, ssi.ownerName);
            //if (!KaGuan.CallDsjk(SendDataXML, ref strRecvData)) { lblError.Text = KaGuan.g_strErrMsg; return; }
            //lblError.Text = "领卡启用成功";

            /**************************  申请补换卡  **************************/
            strSendXML = string.Format("<操作*>申请社保卡</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><经办机构*>{3}</经办机构*><服务银行*>{13}</服务银行*><国家地区*>CHN</国家地区*><证件类型*>1</证件类型*><证件号码*>{4}</证件号码*><证件有效期*>{5}</证件有效期*><姓名*>{6}</姓名*><性别*>{7}</性别*><民族*>{8}</民族*><出生日期*>{9}</出生日期*><人员状态></人员状态><户口性质></户口性质><户口地址>{10}</户口地址><移动电话>{11}</移动电话><固定电话*></固定电话*><通讯地址></通讯地址><邮政编码></邮政编码><电子邮箱></电子邮箱><所在社区(村)编号></所在社区(村)编号><单位编号></单位编号><单位名称></单位名称><监护人姓名></监护人姓名><监护人身份证号></监护人身份证号><监护人联系电话></监护人联系电话><办卡类型*>5</办卡类型*><补换原因>51</补换原因><社保卡号>{12}</社保卡号><相片*></相片*><指纹所属手指编码></指纹所属手指编码><指纹1></指纹1><指纹2></指纹2><指纹3></指纹3><指纹4></指纹4><个人编号></个人编号><社区（村）名称></社区（村）名称><单位部门></单位部门><学校所在系></学校所在系><是否大学></是否大学><是否邮寄></是否邮寄><收卡地址></收卡地址><收卡联系人></收卡联系人><收卡联系人电话></收卡联系人电话>",
                KaGuan.USER, KaGuan.PSWD, KaGuan.CITYCODE, KaGuan.AGENCY, ssi.ID, ssi.idvalid, ssi.ownerName, ssi.sex,
                ssi.nation, ssi.birthday, ssi.addr, ssi.phone, ssi.securityOldNo, ssi.bankcode);
            if (!KaGuan.CallDsjk(strSendXML, ref strRecvData)) { lblError.Text = KaGuan.g_strErrMsg; return; }
            lblError.Text = "申请补换社保卡成功";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 卡状态查询
            //CardServiceClient csc = new CardServiceClient();
            //string strRecvData = csc.getCard(KaGuan.USER, KaGuan.PSWD, ssi.securityNo, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
            //lblError.Text = strRecvData;

            string postMsg = "data=eyJwYXlDb2RlIjoiNDEwMDAwMTkwMDAwMDEwMTIxMHYiLCJ0cmFuc3RpbWUiOiIyMDE5LTAyLTI4IDEzOjIwOjQ5IiwicmVnaW9uQ29kZSI6IjQxMDAwMCIsInN0YXRzIjoiMiIsInBheUNoYW5uZWwiOiJhbGlwYXkiLCJhY3RQYXllckFjY3QiOiIyMDg4MTAyMTc3MzEyNjg0IiwiY2ZtRGF0ZSI6IjIwMTkwMjI4IiwiY21mVGltZSI6IjEzOjE1OjQ4IiwicmVjQWNjdCI6IjQxOTkwMTAxMDE1MDAwOTUwMSIsInNlcmlhbE5vIjoiMDAwMDEwMTIxMHY1MTMzMDkzNzc0ODM0MjMzMzU4Iiwibm90ZTEiOiIiLCJub3RlMiI6IiIsImFtdCI6MjUsImFjdFBheWVyTmFtZSI6Ium7hOaVj-WNjiIsImFjdFBheWVyVGVsIjoiIiwiYWN0UGF5ZXJDYXJkIjoiIn0&src=FSPayPlatform&noise=1551331691823&sign=kAVygWXYJu33irGmBLoryu1khgfcma2XoxoLqtbPCven4Cbg86jw6e04vuQp6CEYmz14-v743tthzK7yzs85qA";
            RestClient client = new RestClient();
            try
            {
                //client.EndPoint = @"http://10.120.1.152:80/api/values/MessagePut";
                client.EndPoint = @"http://10.120.1.152:80/api/values/Post";
                client.ContentType = "application/x-www-form-urlencoded";
                client.Method = HttpVerb.POST;
                client.PostData = postMsg;
                string json = client.MakeRequest();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                return;
            }

            client.EndPoint = @"http://10.120.1.152:80/api/values/MessagePut";
            client.Method = HttpVerb.POST;
            string payCode = "";
            string stats = "";
            string serialNo = "";
            try
            {
                while (true) {
                    string res = client.MakeRequest();
                    BeginInvoke((MethodInvoker)delegate ()
                    {
                        lblError.Text = res;
                    });
                    if (res != "无数据")
                    {
                        dynamic json = Newtonsoft.Json.Linq.JToken.Parse(res) as dynamic;
                        stats = json.stats;
                        payCode = json.payCode;
                        serialNo = json.serialNo;
                        if (stats == "2")
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    lblError.Text = ex.Message;
                });
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string RecvData = "";
            string SendDataXML = string.Format("<操作*>撤销交费登记</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><城市代码>{4}</城市代码*>",
                                KaGuan.USER, KaGuan.PSWD, ssi.ID, ssi.ownerName, KaGuan.CITYCODE);
            KaGuan.CallDsjk(SendDataXML, ref RecvData);
            lblError.Text = KaGuan.g_strErrMsg;
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            /***************************************************************
             * 六卡盒模组
            ****************************************************************/

            ////byte[] byteCmd1 = System.Text.Encoding.Default.GetBytes("vD");
            //int ret = -1;
            //F5SAN f5san = F5SAN.GetInstance();

            //// 建立连接
            //if ((ret = f5san.F5Connect()) != 0) { MessageBox.Show(f5san.strError); return; }

            //// 卡箱4发卡到小车
            //byte[] byteCmd = Encoding.Default.GetBytes("vC4");
            //if ((ret = f5san.F5Excute(byteCmd)) != 0) {
            //    // 复位弹卡
            //    byte[] bCDT = { 0x30, 0x30, 0x33, 0x32, 0x34, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
            //    MessageBox.Show(f5san.strError);
            //    return;
            //}

            //// 小车前持卡
            //byteCmd = new byte[] { 0x76, 0x44, 0x3d };
            //if (( ret = f5san.F5Excute(byteCmd) ) != 0) { MessageBox.Show(f5san.strError); return; }

            //Thread.Sleep(2000);
            //// 从小车到废卡盒
            //DevMng.OutFromCar();

            //return;
            ////// 小车到通道持卡位
            ////byte[] byteCmd1 = new byte[] { 0x76, 0x44, 0x40 };
            ////if ((ret = f5san.F5Excute(byteCmd1)) != 0) { MessageBox.Show(f5san.strError); return; }

            ////检查小车是否有卡
            //byteCmd = Encoding.Default.GetBytes("v1");
            //if (( ret = f5san.F5Excute(byteCmd) ) != 0) { MessageBox.Show(f5san.strError); return; }

            ////小车到打印机位置
            //byteCmd = Encoding.Default.GetBytes("vD4");
            //if (( ret = f5san.F5Excute(byteCmd) ) != 0) { MessageBox.Show(f5san.strError); return; }

            //// 小车后出卡
            //DevMng.gs_bFlag = true;
            //Thread th = new Thread(new ThreadStart(DevMng.CarOutBack));
            //th.Start();

            //int iRet = -1;
            //string error = "";
            //if (DevMng.CheckPrinter(0, ref error) != 0) {
            //    Utility.WriteLog(DevMng.strError);
            //    lblError.Text = error;
            //    return;
            //}

            //iRet = Solid510.Solid510_CardIn();
            ////iRet = Solid510.Solid510_FeedContact();
            //if (iRet != 0) {
            //    lblError.Text = "进卡到打印头失败！";
            //    return;
            //}
            //Thread.Sleep(500);

            ////iRet = Solid510.Solid510_ContactBack2Header();
            ////if (iRet != 0)
            ////{
            ////    lblError.Text = "触头离开失败！";
            ////    return;
            ////}

            //iRet = PrintCard();
            //if (iRet != 0) {
            //    lblError.Text = "打印失败！";
            //    return;
            //}

            //// 打印机到小车
            //DevMng.gs_bFlag = false;
            //Thread th1 = new Thread(new ThreadStart(DevMng.CarOutBack));
            //th1.Start();

            //iRet = Solid510.Solid510_Out2Front();
            //if (iRet != 0) {
            //    lblError.Text = "打印机出卡失败！";
            //    return;
            //}


            //// 小车到通道持卡位
            //byteCmd = new byte[] { 0x76, 0x44, 0x40 };
            //if (( ret = f5san.F5Excute(byteCmd) ) != 0) { MessageBox.Show(f5san.strError); return; }


            /***************************************************************
             * Solid510打印机
             * 
            ****************************************************************/
            PrintCard();
        }
        
        private int PrintCard()
        {
            //StringBuilder m_name = new StringBuilder("黄敏华");
            //StringBuilder m_socialNum = new StringBuilder("450802*********1282");
            //StringBuilder m_socialCardNum = new StringBuilder("A01234567");
            //StringBuilder m_date = new StringBuilder("20200611");
            //StringBuilder picpath = new StringBuilder(System.Environment.CurrentDirectory + "\\pic\\test.jpg");
            string m_name = "黄敏华";
            string m_socialNum = "450802*********1282";
            string m_socialCardNum = "A01234567";
            string m_date = "20200611";
            string picpath = System.Environment.CurrentDirectory + "\\pic\\test.jpg";

            int iRet = -1;
            iRet = Solid510.Solid510_SBSOpen();
            iRet = Solid510.Solid510_SBSPrintStart(false);
            if (iRet != 0) { return iRet; }
            
            string fonttype = "黑体";
            int nFontSize = 10;
            iRet = Solid510.Solid510_SBSPrintText(m_name, fonttype, nFontSize, 426, 171);
            if (iRet != 0) { return iRet; }
            iRet = Solid510.Solid510_SBSPrintText(m_socialNum, fonttype, nFontSize, 569, 228);
            iRet = Solid510.Solid510_SBSPrintText(m_socialCardNum, fonttype, nFontSize, 569, 282);
            iRet = Solid510.Solid510_SBSPrintText(m_date, fonttype, nFontSize, 496, 336);

            iRet = Solid510.Solid510_SBSPrintPicture(picpath, 47, 130, 236, 295);
            //if (iRet != 0) { return iRet; }
            Solid510.Solid510_SBSPrintEnd();
            return 0;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //// 身份证号,缴款码,交易时间
            //string strlog = string.Format("手动查询{0}的缴费结果", ssi.ID);
            //Utility.WriteLog(strlog);
            //string qryResult = SSCSQL.Query();
            //if (qryResult != "")
            //{
            //    string[] result = qryResult.Split('|');
            //    textIDBox.Text = ssi.ID;
            //    PaycodeBox.Text = result[0];
            //    Pay pay = new Pay();
            //    pay.Init(ssi.ID, ssi.phone, ssi.ownerName);
            //    pay.payCode = result[0];
            //    pay.transtime = result[1];
            //    int nRet = pay.QryPayResult();
            //    if (nRet == 2)
            //    {
            //        lblError.Text = pay.queryJson;
            //        Utility.WriteLog(pay.queryJson);
            //    }
            //    else if (nRet == 3 || nRet == 4 || nRet == 0)
            //    {
            //        lblError.Text = pay.queryData;
            //        Utility.WriteLog(pay.queryData);
            //    }
            //    else
            //    {
            //        lblError.Text = "服务器已断开！";
            //    }
            //}
            //else
            //{
            //    lblError.Text = "无缴费操作记录。";
            //    return;
            //}
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StringBuilder pOutputInfo = new StringBuilder(600);
            StringBuilder pErrMsg = new StringBuilder(600);
            int ret1 = SiCard.Print_ReadCard(SiCard.PORT_FG, SiCard.TP_CONTACT, pOutputInfo, pErrMsg);
            if(ret1 != 0) {
                MessageBox.Show(pErrMsg.ToString());
                return;
            }
            string[] infos = pOutputInfo.ToString().Split('|');
            string pWrinteInfo = "";
            if (infos[8] == "2.00")
            {
                pWrinteInfo = string.Format("410000199901013332|李四|1|01|19990101|20190101|20290101|N12345678|{0}|{1}|2.00|CA|",
                    infos[7], infos[3]);
            }
            else
            {
                pWrinteInfo = string.Format("410000199901013332|李四|1|01|19990101|20190101|20290101|N12345678|{0}|{1}|3.00|{2}|",
                    infos[7], infos[3], infos[10]);
            }
            ret1 = SiCard.Print_WriteCard(SiCard.PORT_FG, SiCard.TP_CONTACT, pWrinteInfo, pErrMsg);
            if (ret1 == 0)
            {
                MessageBox.Show("写卡成功。");
            }
            else
            {
                MessageBox.Show("写卡失败：" + pErrMsg.ToString());
            }
            //ICReader.SetVPID(Global.GetInstance().PrtVPIDArray[0], Global.GetInstance().PrtVPIDArray[1]);
            //byte[] byteBuffer = new byte[50];
            //int len = 50;
            //int nRt = ICReader.GetPsamNo(byteBuffer, ref len);
            //if(nRt != 0) {
            //    MessageBox.Show("读取PSAM卡号失败！");
            //}
            //string psamno = Encoding.Default.GetString(byteBuffer).TrimEnd('\0');
            //lblError.Text = psamno;
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            tboxQrcode.Focus();
            tboxQrcode.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //int nRet = -1;
            //int nWhich = int.Parse(tboxNLight.Text);
            //if (button8.Text == "指示灯开") {
            //    nRet = U10.ContrlLight(nWhich, 1);
            //    if(nRet > 0) button8.Text = "指示灯关";
            //}
            //else {
            //    nRet = U10.ContrlLight(nWhich, 0);
            //    if (nRet > 0) button8.Text = "指示灯开";
            //}
            
        }
    }
}
