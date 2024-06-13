using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masterconfig
{
    class FirstStageOperationsClass
    {
        private string _NameOfConfig = "";
        private string _TypeOfFile = "";

        public void SetName(string nameOfConfig)
        {
            _NameOfConfig = nameOfConfig;
        }

        public string GetName()
        {
            return _NameOfConfig;
        }

        public void SetTypeOfFile(string typeOfFile)
        {
            _TypeOfFile = typeOfFile;
        }

        public string GetTypeOfFile()
        {
            return _TypeOfFile;
        }
    }
}
