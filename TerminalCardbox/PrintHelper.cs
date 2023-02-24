using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using PavoCardSDK;

namespace printerDemo
{
    class PrintHelper
    {
        public static String printerName = "";
        public static String configFilePath = Application.StartupPath + "\\config.json";
        public static int printMultiple = 3;
        public static double cardWidth = 8.6;
        public static double cardHeight = 5.4;
        Image colorImg;
        PrintItems printItems = new PrintItems();
        private System.Drawing.Printing.PrintDocument printDocument = new PrintDocument();
        public int InitPrinter()
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (String.Compare(strPrinter, 0, "HiTi CS-2", 0, 9) == 0)
                {
                    printerName = strPrinter;
                   // labelPrinter.Text = printerName + ": 已连接";
                }
            }

            uint nBufferSize = 0;
            if (PavoApi.PAVO_EnumUSBCardPrinters(null, ref nBufferSize) == 0)
            {
                if (nBufferSize != 0)
                {
                    byte[] printerBuf = new byte[nBufferSize];

                    PavoApi.PAVO_EnumUSBCardPrinters(printerBuf, ref nBufferSize);
                }
            }

            printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            printDocument.EndPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_EndPrint);
            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            printDocument.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.printDocument_QueryPageSettings);
            
            return 0;
        }

        private void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {

        }

        private void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            timerReject.Enabled = true;
            int t = 0;
        }

        private void printDocument_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {

        }

        public int StartPrint()
        {
            if (printerName == "")
                return -1;

            PAVO_JOB_PROPERTY JobProp = new PAVO_JOB_PROPERTY();

            JobProp.dwFieldFlag = PavoApi.FF_ORIENTATION | PavoApi.FF_COPIES | PavoApi.FF_RIBBON_TYPE;
            JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
            JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_FRONT;
            JobProp.shOrientation = 1;
            JobProp.shCopies = 1;
            JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKO;

            printDocument.PrinterSettings.PrinterName = printerName;
            printDocument.DefaultPageSettings.Margins.Left = 0;
            printDocument.DefaultPageSettings.Margins.Top = 0;
            printDocument.DefaultPageSettings.Margins.Right = 0;
            printDocument.DefaultPageSettings.Margins.Bottom = 0;

            printDocument.DocumentName = "DemoCSharp Print Job";
            uint dwRet = 0;
            dwRet = PavoApi.PAVO_SetStandbyParameters(printerName, 20, 0);
            if (dwRet < 0)
            {
                return -1;
            }
            dwRet = PavoApi.PAVO_ApplyJobSetting(printDocument.PrinterSettings, ref JobProp);
            if (dwRet < 0)
            {
                return -1;
            }
            printDocument.Print();
            return 0;
        }

        public int MoveCardToEncoder()
        {
            uint dwRet = 0;
            uint dwPosition = 0;
            string msg;
            string PosString;

            dwPosition = PavoApi.MOVE_CARD_TO_IC_ENCODER;
            PosString = "Contact Encoder";

            if (dwPosition == 0)
                return -1;

            dwRet = PavoApi.PAVO_MoveCard(printerName, dwPosition);

            msg = String.Format("Move card to {0} finished.\n\ndwRet = 0x{1:X8}", PosString, dwRet);
           
            return 0;
        }

         public int MoveCardToOuter()
        {
            uint dwRet = 0;
            uint dwPosition = 0;
            string msg;
            string PosString;

            dwPosition = PavoApi.MOVE_CARD_TO_HOPPER;
            PosString = "Output Hopper";

            if (dwPosition == 0)
                return -1;

            dwRet = PavoApi.PAVO_MoveCard(printerName, dwPosition);

            msg = String.Format("Move card to {0} finished.\n\ndwRet = 0x{1:X8}", PosString, dwRet);
            return 0;
        }

         public int MoveCardToRejectbox()
         {
             uint dwRet = 0;
             uint dwPosition = 0;
             string msg;
             string PosString;

             dwPosition = PavoApi.MOVE_CARD_TO_REJECT_BOX;
             PosString = "Output Hopper";

             if (dwPosition == 0)
                 return -1;

             dwRet = PavoApi.PAVO_MoveCard(printerName, dwPosition);

             msg = String.Format("Move card to {0} finished.\n\ndwRet = 0x{1:X8}", PosString, dwRet);
             return 0;
         }

         public int CheckPrinterStatus()
         {
             uint dwStatus = 0;
             uint dwRet = 0;
             int requestCount = 10;
             //循环问打印机状态
             while( requestCount > 0)
             {
                 dwRet = PavoApi.PAVO_CheckPrinterStatus(printerName, ref dwStatus);
                 if (dwStatus != PavoApi.PAVO_DS_BUSY && dwStatus != PavoApi.PAVO_DS_PRINTING)
                 {
                     break;
                 }
                 else
                 {
                     Thread.Sleep(2000);
                 }
                 requestCount--;
             }

             Thread.Sleep(10000);
             //问卡的状态
             byte[] dataBuf = new byte[256];
             uint dwBufSize = 256;
             uint dwValue1 = 0, dwValue2 = 0;
             string temp = "";
             dwRet = PavoApi.PAVO_GetDeviceInfo(printerName, PavoApi.PAVO_DEVINFO_CARD_POSITION, dataBuf, ref dwBufSize);
             dwValue1 = BitConverter.ToUInt32(dataBuf, 0);

             switch (dwValue1)
             {
                 default:
                 case 0: temp = "Out of printer"; break;
                 case 1: temp = "Start printing position"; break;
                 case 2: temp = "Mag out position"; break;
                 case 3: temp = "Mag in position"; break;
                 case 4: temp = "Contact encoder position"; break;
                 case 5: temp = "Contactless encoder position"; break;
                 case 6: temp = "Flipper position"; break;
                 case 7: temp = "Card jam"; break;
             }

             if (dwValue1 == 8)
             {
                 MoveCardToRejectbox();
             }
            return 0;
         }
    }
}
