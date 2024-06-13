using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace masterconfig
{
    class ThirdStageOperationsClass
    {
        private SqlConnection _stringsSQL = new SqlConnection("Integrated Security=SSPI;Initial Catalog=Imports;Data Source=IP-RDS\\SQLEXPRESS");

        private DataTable _datatable = null;
        private string _excelNameTable = "";
        private bool _flagErrors = false;

        public void SetFlagErrors(bool flagErrors)
        {
            _flagErrors = flagErrors;
        }

        public bool GetFlagErrors()
        {
            return _flagErrors;
        }
        public void SetExcelNameTable(string name)
        {
            _excelNameTable = name;
        }

        public string GetExcelNameTable()
        {
            return _excelNameTable;
        }

        public void SetSource(DataTable source)
        {
            _datatable = source;
        }

        public DataTable GetSource()
        {
            return _datatable;
        }

        public List<string> ShowTables()
        {
            List<string> ListTables = new List<string>();
            try
            {
                _stringsSQL.Open();
                string sql = "SELECT TableNameUser FROM tRgImpTables";
                SqlCommand command = new SqlCommand(sql, _stringsSQL);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ListTables.Add(reader.GetString(0));
                }
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ListTables;
        }

        public List<String> ShowColumns(string NameOfTable)
        {
            List<String> ListColumns = new List<String>();
            try
            {
                _stringsSQL.Open();
                string sql = "CREATE PROCEDURE GetColumnsByKeyword" +
                    "\r\n    @keyword VARCHAR(255)" +
                    "\r\nAS" +
                    "\r\nBEGIN" +
                    "\r\n    DECLARE @id INT;" +
                    "\r\n    SELECT @id = IDRgImpTable" +
                    "\r\n    FROM tRgImpTables" +
                    "\r\n    WHERE TableNameUser = @keyword;" +
                    "\r\n    SELECT u.FieldNameSQL" +
                    "\r\n    FROM tRgImpFields u" +
                    "\r\n    WHERE u.IDRgImpTable = @id;" +
                    "\r\nEND";
                SqlCommand command = new SqlCommand(sql, _stringsSQL);
                command.ExecuteNonQuery();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "GetColumnsByKeyword";
                command.Parameters.Add("@keyword", System.Data.SqlDbType.VarChar);
                command.Parameters["@keyword"].Value = NameOfTable;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ListColumns.Add(reader.GetString(0));
                }
                reader.Close();
                command.CommandText = "DROP PROCEDURE GetColumnsByKeyword";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ListColumns;
        }
    }
}
