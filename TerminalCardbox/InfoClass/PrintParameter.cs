using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHBSS
{
    public class PrintParameter
    {
        private static PrintParameter _printerParam = null;
        public static PrintParameter GetInstance()
        {
            if (_printerParam == null) {
                _printerParam = new PrintParameter();
            }
            return _printerParam;
        }
        private PrintParameter()
        {
        }
        public string BankNO { get; set; }
        public int nPrinter { get; set; }
        public int nCardPosition { get; set; }
        public int nCartRidge { get; set; }
    }
}
