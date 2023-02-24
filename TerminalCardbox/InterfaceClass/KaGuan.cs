﻿/*************************************************************************
*  @brief：卡管接口类
*  参考文件：《航天信息一体机接口文档20180813.docx》
*  对接人：李子林
***************************************************************************/
using System.Xml.Linq;
using SHBSS.SIService;
using System;
using System.Net;
using System.IO;
using System.Drawing;

namespace SHBSS
{  
    class KaGuan
    {
        static public string USER; // 用户名
        static public string PSWD; // 密码
        static public string PICUSER; // 照片用户名
        static public string PICPSWD; // 照片密码
        static public string CITYCODE; // 城市代码
        static public string AGENCY; // 经办机构
        static public string DISTRCT; // 所属区县  
        static public string POSTCODE; // 邮政编码
        static public string BANK;

        static public string jsTime;
        static public string lkdInfo;
        static public string kgPhone;
        static public string kgBankNO;
        static public string kgBankName;

        static CardServiceClient csc;  // 卡管接口类对象
        static SocialSecurityInfo ssi = SocialSecurityInfo.GetInstance();
        static public string g_strErrMsg = string.Empty;      
        public static SSCTable ssc = SSCTable.getSSCTable();

        public static void GetSIService()
        {
            // 该文件记录内容：制卡步骤号，身份证号，姓名，社保卡号
            Global gb = Global.GetInstance();
            USER = gb.KGUser;
            PSWD = gb.KGPwd;
            PICUSER = gb.KGPicUser;
            PICPSWD = gb.KGPicPwd;
            CITYCODE = gb.CITYCODE;
            AGENCY = gb.AGENCY;
            DISTRCT = gb.DISTRCT;
            POSTCODE = gb.PostCode;
            BANK = gb.BankCode;
            csc = new CardServiceClient("CardService");  // 卡管接口类对象
        }
        static public int getStatus()
        {
            try {
                string rysjRet = csc.getRysj(USER, PSWD, ssi.ID, ssi.ownerName, "");
                XElement root = XElement.Parse("<root>" + rysjRet.Trim() + "</root>");
                string err = root.Element("ERR").Value;
                if (err.Equals("OK")) {
                    return 0;
                }
                Utility.WriteLog("状态查询: " + err);
                g_strErrMsg = "用户没有社保卡，无法进行补换操作";
                return 1;
            }
            catch (WebException we) {
                g_strErrMsg = "卡管服务器超时！";
                Utility.WriteLog("卡管服务器超时：" + we.Message);
                return 2;
            }

        }

        /*
         <root>
          <ERR>OK</ERR>
          <AAB301>411300</AAB301>
          <AAZ500></AAZ500>
          <AAC002>450802********1282</AAC002>
          <AAC003>黄敏华</AAC003>
          <AAC004>2</AAC004>
          <MOBILE>137****8670</MOBILE>
          <AAE006>河南省洛阳市老城区</AAE006>
          <AAE007>471000</AAE007>
          <EMAIL></EMAIL>
          <AAB004>测试</AAB004>
          <JHRXM></JHRXM>
          <SZSQ>41030201009</SZSQ>
        </root>
         */
        static public bool GetRysj()
        {
            string rysjRet = csc.getRysj(PICUSER, PICPSWD, ssi.ID, ssi.ownerName, "");
            XElement root = XElement.Parse("<root>" + rysjRet.Trim() + "</root>");
            string err = root.Element("ERR").Value;
            if (!err.Equals("OK")) return false;

            //ssi.cityCode = root.Element("AAB301").Value;
            ssi.addr = root.Element("AAE006").Value;
            ssi.securityOldNo = root.Element("AAZ500").Value;
            kgPhone = root.Element("MOBILE").Value;
            return true;
        }
        
        /*
         <root><ERR>OK</ERR>
          <AAZ500></AAZ500>
          <AAC002>450802********1282</AAC002>
          <AAC003>黄敏华</AAC003>
          <TRANSACTTYPE>5</TRANSACTTYPE>
          <AAB301>411300</AAB301>
          <ORGANID>41130027</ORGANID>
          <AAE008>95580</AAE008>
          <CITYTIME></CITYTIME>
          <SLFF></SLFF>
          <ZKZT>制卡中</ZKZT>
        </root>
        */
        static public bool GetZkjd()
        {
            // 制卡进度查询
            string rysjRet = csc.getZkjd(PICUSER, PICPSWD, ssi.ID, ssi.ownerName, "");
            XElement root = XElement.Parse("<root>" + rysjRet.Trim() + "</root>");
            string err = root.Element("ERR").Value;
            if (!err.Equals("OK")) return false;

            //ssi.bankcode = root.Element("AAE008").Value;
            kgBankNO = root.Element("AAE008").Value;
            kgBankName = getBankName(kgBankNO);
            ssi.sqlx = root.Element("TRANSACTTYPE").Value; // 申请类型
            ssi.zkzt = root.Element("ZKZT").Value; // 制卡状态          
            return true;
        }
        public static string getBankName(string bankNO)
        {
            if (bankNO == "95580") return "邮储银行";
            else if (bankNO == "96288") return "河南农信";
            else if (bankNO == "95533") return "建设银行";
            else if (bankNO == "95555") return "招商银行";
            else if (bankNO == "95558") return "中信银行";
            else if (bankNO == "95559") return "交通银行";
            else if (bankNO == "95561") return "兴业银行";
            else if (bankNO == "95566") return "中国银行";
            else if (bankNO == "95588") return "工商银行";
            else if (bankNO == "95599") return "农业银行";
            else if (bankNO == "96588") return "平顶山银行";
            else if (bankNO == "96688") return "中原银行";
            else if (bankNO == "96699") return "洛阳银行";
            else return "其他银行";
        }

        public static bool GetLkdInfo()
        {
            string SendDataXML, RecvData = "";
            SendDataXML = string.Format("<操作*>获取领卡地信息</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><城市代码>{4}</城市代码>",
                USER, PSWD, ssi.ID, ssi.ownerName, CITYCODE);
            if (!CallDsjk(SendDataXML, ref RecvData)) return false;
            XElement root = XElement.Parse("<root>" + RecvData.Trim() + "</root>");
            jsTime = root.Element("市接收时间").Value;
            lkdInfo = root.Element("领卡地信息").Value;
            return true;
        }
        static public bool GetPhoto()
        {
            string photoRet = csc.getPhoto(PICUSER, PICPSWD, ssi.ID, ssi.ownerName, "");
            XElement pRoot = XElement.Parse("<root>" + photoRet.Trim() + "</root>");
            string err = pRoot.Element("ERR").Value;
            if (!err.Equals("OK")) return false;

            string base64Photo = pRoot.Element("PHOTO").Value;
            byte[] bytes = Convert.FromBase64String(base64Photo);
            MemoryStream memStream = new MemoryStream(bytes);
            Bitmap img = new Bitmap(memStream);
            ssi.photo = img;
            return true;
        }
        public static int CheckCard()
        {
            string strSendXML = "", strRecvData = "";
            //strdata = "\查询卡状态...\n";
            strRecvData = csc.getCard(USER, PSWD, ssi.securityOldNo, ssi.ID, ssi.ownerName, CITYCODE);
            Utility.WriteLog(strRecvData);
            if (strRecvData == "OK" || strRecvData != "正式挂失") return 1;

            //strdata = "\查询交费登记状态...\n";
            strSendXML = string.Format("<操作*>查询交费登记状态</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*>",
               USER, PSWD, ssi.ID, ssi.ownerName);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 2;
            return 0;
        }

        public static string getAgentCode(string cardagent)
        {
            switch (cardagent)
            {
                case "8651": return "1";
                case "8648": return "3";
                case "8645": return "5";
                case "8653": return "6";
                case "8684": return "7";
                case "8649": return "8";
                case "8688": return "9";
                default:
                    return "1";
            }
        }
        // 补换社保卡  修改人员照片
        static public bool xgryzp()
        {
            string strXML = "", retXML = "";
            //string base64Pic = Base64Pic.ImageToBase64(ssi.picpath);
            string base64Pic = Convert.ToBase64String(File.ReadAllBytes(ssi.photopath));
            Utility.WriteLog("申请社保卡...");
            strXML = string.Format("<操作*>申请社保卡</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><经办机构*>{3}</经办机构*><服务银行*>{4}</服务银行*><国家地区*>CHN</国家地区*><证件类型*>1</证件类型*><证件号码*>{5}</证件号码*><证件有效期*>{6}</证件有效期*><姓名*>{7}</姓名*><性别*></性别*><民族*></民族*><出生日期*></出生日期*><人员状态></人员状态><户口性质></户口性质><户口地址></户口地址><移动电话>{8}</移动电话><固定电话*></固定电话*><通讯地址></通讯地址><邮政编码></邮政编码><电子邮箱></电子邮箱><所在社区(村)编号></所在社区(村)编号><单位编号></单位编号><单位名称></单位名称><监护人姓名></监护人姓名><监护人身份证号></监护人身份证号><监护人联系电话></监护人联系电话><办卡类型*>5</办卡类型*><补换原因>51</补换原因><社保卡号>{9}</社保卡号><相片*></相片*><指纹所属手指编码></指纹所属手指编码><指纹1></指纹1><指纹2></指纹2><指纹3></指纹3><指纹4></指纹4><个人编号></个人编号><社区（村）名称></社区（村）名称><单位部门></单位部门><学校所在系></学校所在系><是否大学></是否大学><是否邮寄></是否邮寄><收卡地址></收卡地址><收卡联系人></收卡联系人><收卡联系人电话></收卡联系人电话>",
                USER, PSWD, CITYCODE, AGENCY, BANK, ssi.ID, ssi.idvalid, ssi.ownerName, ssi.phone, ssi.securityOldNo);
            if (!CallDsjk(strXML, ref retXML)) return false;
            //Utility.WriteLog(strXML);
            Utility.WriteLog("相片修改...");
            strXML = string.Format("<操作*>相片修改</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*><相片*>{4}</相片*>",
                USER, PSWD, ssi.ID, ssi.ownerName, base64Pic);
            return CallDsjk(strXML, ref retXML);
        }
        static public bool sqsbk()
        {
            //if (ssi.bankcode.Length == 5) {
            //    BANK = ssi.bankcode;
            //}
            //else {
            //    BANK = ssi.bankcode.Substring(0, 5);
            //}
            string base64Pic = Convert.ToBase64String(File.ReadAllBytes(ssi.photopath));
            string strXML = "";
            // 成年人 
            if (ssi.isAdult) {
                // 监护人姓名、监护人身份证号、监护人联系电话  为空
                //ssi.guardianName = "";
                //ssi.guardianID = "";
                //ssi.guardianPhone = "";
                // 职工
                if (ssi.rylb == "1") {
                    // 社区村编号、社区村名称、是否大学生、班级、学校所在系 为空
                    ssi.sqbh = "";
                    ssi.sqcmc = "";
                    //ssi.strIsCollege = "";
                    //ssi.className = "";
                    //ssi.universitySeries = "";
                }
                // 学生
                else if (ssi.rylb == "4") {
                    // 单位编号、单位名称、社区村编号、社区村名称 为空
                    ssi.dwbh = "";
                    //ssi.cbdw = ""; 单位编号为学校名称
                    ssi.sqbh = "";
                    ssi.sqcmc = "";
                    // 中学生  学校所在系 为空
                    //if (ssi.strIsCollege == "0") {
                    //    ssi.universitySeries = "";
                    //}
                }
                // 居民
                else {
                    // 单位编号、单位名称、是否大学生、班级、学校所在系  为空
                    ssi.dwbh = "";
                    ssi.cbdw = "";
                    //ssi.strIsCollege = "";
                    //ssi.className = "";
                    //ssi.universitySeries = "";
                    //ssi.sqcmc = ssi.sqbh;
                }
            }
            // 未成年人
            else {
                //  单位编号、单位名称  为空
                //ssi.dwbh = "";
                //ssi.cbdw = "";
                // 中小学生
                if (ssi.rylb == "4") {
                    //  社区村编号、社区村名称 为空
                    ssi.sqbh = "";
                    ssi.sqcmc = "";
                    //if (ssi.strIsCollege == "0") {
                    //    ssi.universitySeries = "";
                    //}
                }
                // 居民
                else if (ssi.rylb == "2" || ssi.rylb == "3") {
                    // 班级  为空
                    ssi.cbdw = "";
                    ssi.dwbh = "";
                    //ssi.className = "";
                    //ssi.sqcmc = ssi.sqbh;
                }
                else {
                    g_strErrMsg = "未成年人的人员类别不能选城镇职工";
                    return false;
                }
            }
            Utility.WriteLog("申请社保卡...");
            strXML = string.Format("<操作*>申请社保卡</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><经办机构*>{3}</经办机构*><人员类别*>{4}</人员类别*><服务银行*>{5}</服务银行*><国家地区*>CHN</国家地区*><证件类型*>1</证件类型*><证件号码*>{6}</证件号码*><证件有效期*>{7}</证件有效期*><姓名*>{8}</姓名*><性别*>{9}</性别*><民族*>{10}</民族*><出生日期*>{11}</出生日期*><人员状态>{12}</人员状态><户口性质>{13}</户口性质><户口地址>{14}</户口地址><移动电话>{15}</移动电话><固定电话*></固定电话*><通讯地址>{16}</通讯地址><邮政编码>{17}</邮政编码><电子邮箱></电子邮箱><所在社区(村)编号>{18}</所在社区(村)编号><单位编号>{19}</单位编号><单位名称>{20}</单位名称><监护人姓名>{21}</监护人姓名><监护人身份证号>{22}</监护人身份证号><监护人联系电话>{23}</监护人联系电话><办卡类型*>1</办卡类型*><补换原因></补换原因><社保卡号></社保卡号><相片*>{24}</相片*><指纹所属手指编码></指纹所属手指编码><指纹1></指纹1><指纹2></指纹2><指纹3></指纹3><指纹4></指纹4><个人编号></个人编号><社区(村)名称>{25}</社区（村）名称><单位部门></单位部门><学校所在系>{26}</学校所在系><班级名称>{27}</班级名称><是否大学>{28}</是否大学><是否邮寄></是否邮寄><收卡地址></收卡地址><收卡联系人></收卡联系人><收卡联系人电话></收卡联系人电话><物流公司></物流公司><数据来源*></数据来源*><数据来源类型*></数据来源类型*>",
                USER, PSWD, CITYCODE, AGENCY, ssi.rylb, BANK, ssi.ID, ssi.idvalid, ssi.ownerName, ssi.sex, ssi.nation, ssi.ID.Substring(6, 8), ssi.ryzt,
                ssi.hkxz, ssi.addr, ssi.phone, ssi.addr, POSTCODE, ssi.sqbh, ssi.dwbh, ssi.cbdw, "", "", "", base64Pic, ssi.sqcmc,
                "", "", "");

            string retXML = "";
            return CallDsjk(strXML, ref retXML);
        }

        // 传银行代码
        public static int QryCardInfo(ref string Batch1)
        {
            /*if(ssi.bankcode.Length == 5) {
                BANK = ssi.bankcode;
            }else {
                BANK = ssi.bankcode.Substring(0, 5);
            }*/
            
            string strSendXML = "", strRecvData = "";
            string cardagentcode = getAgentCode(ssi.cardagent);
            switch (ssc.nPro) {
                case 0:
                    goto zero;
                case 1:
                    goto one;
                case 2:
                    goto two;
                case 3:
                case 4:
                    goto three;
                case 5:
                    goto five;
                default:
                    break;
            }
            zero: 
            Utility.WriteLog("申请补换社保卡...");
            strSendXML = string.Format("<操作*>申请社保卡</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><经办机构*>{3}</经办机构*><服务银行*>{13}</服务银行*><国家地区*>CHN</国家地区*><证件类型*>1</证件类型*><证件号码*>{4}</证件号码*><证件有效期*>{5}</证件有效期*><姓名*>{6}</姓名*><性别*>{7}</性别*><民族*>{8}</民族*><出生日期*>{9}</出生日期*><人员状态>1</人员状态><户口性质>11</户口性质><户口地址>{10}</户口地址><移动电话>{11}</移动电话><固定电话*></固定电话*><通讯地址></通讯地址><邮政编码></邮政编码><电子邮箱></电子邮箱><所在社区(村)编号></所在社区(村)编号><单位编号></单位编号><单位名称></单位名称><监护人姓名></监护人姓名><监护人身份证号></监护人身份证号><监护人联系电话></监护人联系电话><办卡类型*>5</办卡类型*><补换原因>51</补换原因><社保卡号>{12}</社保卡号><相片*></相片*><指纹所属手指编码></指纹所属手指编码><指纹1></指纹1><指纹2></指纹2><指纹3></指纹3><指纹4></指纹4><个人编号></个人编号><社区（村）名称></社区（村）名称><单位部门></单位部门><学校所在系></学校所在系><是否大学></是否大学><是否邮寄></是否邮寄><收卡地址></收卡地址><收卡联系人></收卡联系人><收卡联系人电话></收卡联系人电话>",
                USER, PSWD, CITYCODE, AGENCY, ssi.ID, ssi.idvalid, ssi.ownerName, ssi.sex,
                ssi.nation, ssi.birthday, ssi.addr,ssi.phone,ssi.securityOldNo, BANK);
            if (!CallDsjk(strSendXML, ref strRecvData)) return -1;

            one:
            Utility.WriteLog("即制卡标注...");
            strSendXML = string.Format("<操作*>即制卡标注</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><社会保障号码*>{3}</社会保障号码*><姓名*>{4}</姓名*>",
                USER, PSWD, CITYCODE, ssi.ID, ssi.ownerName);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 1;
            two:
            //strdata = "\n即制卡人员...\n";  申请类型区分
            Utility.WriteLog("即制卡人员...");
            strSendXML = string.Format("<操作*>即制卡人员</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><所属区县>{3}</所属区县><经办机构>{4}</经办机构><服务银行>{5}</服务银行><申请类型>{6}</申请类型><社会保障号码>{7}</社会保障号码>",
                USER, PSWD, CITYCODE, DISTRCT, AGENCY, BANK, ssi.ID, ssi.sqlx);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 2;

            three:
            //strdata = "\n\r获取批次号...\n";
            Utility.WriteLog("获取批次号...");
            strSendXML = string.Format("<操作*>获取批次号</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><所属区县*>{2}</所属区县*>",
                USER, PSWD, DISTRCT);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 3;
            //pch = strRecvData;
            XElement root = XElement.Parse("<root>" + strRecvData.Trim() + "</root>");
            string pchret = root.Element("批次号").Value;
            
            //strdata = "\n\r即制卡批次...\n";
            Utility.WriteLog("即制卡批次...");
            strSendXML = string.Format("<操作*>即制卡批次</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><城市代码*>{2}</城市代码*><所属区县*>{3}</所属区县*><服务银行*>{7}</服务银行*><卡商*>{8}</卡商*><批次号*>{4}</批次号*><人数*>1</人数*>\r\n" +"序号,社会保障号码,姓名\r\n" +"1,{5},{6}", 
            USER, PSWD, CITYCODE, DISTRCT, pchret, ssi.ID, ssi.ownerName, BANK, cardagentcode);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 4;

            five:
            //strdata = "\n\r查询批次...\n";
            Utility.WriteLog("查询批次...");
            strSendXML = string.Format("<操作*>查询批次号</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><社会保障号码*>{2}</社会保障号码*><姓名*>{3}</姓名*>",
                USER, PSWD, ssi.ID, ssi.ownerName);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 5;

            XElement qryRoot = XElement.Parse("<root>" + strRecvData.Trim() + "</root>");

            string newSecurityNo = qryRoot.Element("AAZ500").Value;
            string newBatch = qryRoot.Element("batchno").Value;

            //strdata = "\n\r即制卡数据...\n";
            Utility.WriteLog("即制卡数据...");
            string cardData = "";
            strSendXML = string.Format("<操作*>即制卡数据</操作*><用户名*>{0}</用户名*><密码*>{1}</密码*><批次号*>{2}</批次号*><社保卡号*>{3}</社保卡号*><社会保障号码*>{4}</社会保障号码*>",
                USER, PSWD, newBatch, newSecurityNo, ssi.ID);
            if (!CallDsjk(strSendXML, ref strRecvData)) return 5;

            //cardData = "<ERR>00</ERR><社保卡号>X12399063</社保卡号><国籍>CHN</国籍><证件类型>1</证件类型><社会保障号码>652927198601039823</社会保障号码><证件有效期>20301212</证件有效期><姓名>伏念之</姓名><性别>1</性别><民族>01</民族><出生日期>19770818</出生日期><人员状态>1</人员状态><户口性质>11</户口性质><户口地址>户口地址1</户口地址><移动电话>12345678901</移动电话><固定电话>固定电话1</固定电话><通讯地址>通讯地址1</通讯地址><邮政编码>123451</邮政编码><电子邮箱>1@12333.cn</电子邮箱><所在社区(村)>41990000001</所在社区(村)><单位编号>单位编号1</单位编号><单位名称>单位名称1</单位名称><监护人证号>监护人证号1</监护人证号><监护人姓名>监护人姓名1</监护人姓名><监护人电话>监护人电话1</监护人电话><经办机构>41990000</经办机构><人员类别>1</人员类别><申请类型>5</申请类型><补换原因>51</补换原因><制卡日期>20180929</制卡日期><有效期至>20280929</有效期至><个人账户></个人账户><银行卡号></银行卡号><相片>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</相片>";

            cardData = "<root>" + strRecvData.Trim() + "</root>";

            cardData = cardData.Replace("(", string.Empty);
            cardData = cardData.Replace(")", string.Empty);
            XElement qryData = XElement.Parse(cardData);
            Batch1 = newBatch;
            //pch1 = pch;
            return ProcesData(cardData, newBatch);
        }
        

        static public bool CallDsjk(string SendDataXML, ref string RecvData)
        {
            try {
                RecvData = string.Empty;
                RecvData = csc.allDsjk(SendDataXML);

                int nPosBegin = RecvData.IndexOf("<ERR>");
                int nPosEnd = RecvData.IndexOf("</ERR>");
                if (nPosEnd < 0 || nPosBegin < 0) {
                    if (RecvData == "00" || RecvData == "OK" || RecvData == "01") {
                        return true;
                    }
                    else if (RecvData == "未做交费登记或已撤销交费登记") return false;
                    else {
                        g_strErrMsg = RecvData;
                    }
                    Utility.WriteLog("SendData: " + SendDataXML);
                    Utility.WriteLog("RecvData: " + RecvData);
                    return false;
                }
                g_strErrMsg = RecvData.Substring(nPosBegin + 5, nPosEnd - nPosBegin - 5); // 确保错误解释由接口返回提供
                if (g_strErrMsg == "00" || g_strErrMsg == "02银行卡号为空" || g_strErrMsg == "01" || g_strErrMsg == "OK") {
                    return true;
                }
                else {
                    if (g_strErrMsg.Substring(0, 2) == "02") return true;
                    if(SendDataXML.IndexOf("申请社保卡") < 0){
                        Utility.WriteLog("SendData: " + SendDataXML);
                    }
                    Utility.WriteLog("RecvData: " + RecvData);
                    return false;
                }
            }
            catch (System.Net.WebException wex) {
                g_strErrMsg = "卡管服务器超时！";
                Utility.WriteLog("KaGuan Exception：" + wex.Message);
                return false;
            }
            catch (Exception ex) {
                g_strErrMsg = ex.Message;
                Utility.WriteLog("KaGuan Exception - CallDsjk: " + ex.Message);
                return false;
            }
        }

        public static int ProcesData(string data, string Batch)
        {
            try {
                XElement qryData = XElement.Parse(data.Trim());
                ssi.securityNo = qryData.Element("社保卡号").Value;
                //ssi.nation = qryData.Element("国籍").Value;
                //ssi.certType = qryData.Element("证件类型").Value;
                ssi.ID = qryData.Element("社会保障号码").Value;
                ssi.ownerName = qryData.Element("姓名").Value;
                string sex = qryData.Element("性别").Value;
                if (sex == "男") ssi.sex = "1";
                else if (sex == "女") ssi.sex = "2";
                else ssi.sex = sex;
                ssi.nation = qryData.Element("民族").Value;
                ssi.birthday = qryData.Element("出生日期").Value;
                //ssi.makeDate = qryData.Element("制卡日期").Value;
                ssi.valid = qryData.Element("有效期至").Value;
                //nci.UnitCode = qryData.Element("单位编号").Value;
                //nci.UnitName = qryData.Element("单位名称").Value;

                string spt = qryData.Element("相片").Value;
                byte[] bytes = Convert.FromBase64String(spt);
                MemoryStream memStream = new MemoryStream(bytes);
                Bitmap img = new Bitmap(memStream);
                ssi.photo = img;
                return 0;
            }
            catch {
                Utility.WriteLog("KaGuan Exception - ProcesData：" + data);
                return 5;
            }
        }
    }
}
