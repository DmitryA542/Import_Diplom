using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    class DBOperations
    {
        private SqlConnection _stringsSQL = new SqlConnection("Integrated Security=SSPI;Initial Catalog=Imports;Data Source=IP-RDS\\SQLEXPRESS");

        public (List<string>, string) ShowListOfConfigs()
        {
            List<string> configs = new List<string>();
            string extension = "";
            try
            {
                _stringsSQL.Open();
                string sql = "SELECT ImpConfigName, TypeOfFile FROM tRgImpConfigs";
                SqlCommand command = new SqlCommand(sql, _stringsSQL);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    configs.Add(reader.GetString(0));
                    extension = reader.GetString(1);
                }
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return (configs, extension);
        }

        public void DeleteConfig(string nameofconfig)
        {
            try
            {
                _stringsSQL.Open();
                var sql = "CREATE PROCEDURE DeleteConfig" +
                    "\r\n\t@configname VARCHAR(255)" +
                    "\r\nAS" +
                    "\r\nBEGIN" +
                    "\r\n\tDECLARE @idconfig INT;" +
                    "\r\n\tSELECT @idconfig = IDRgImpConfig" +
                    "\r\n\tFROM tRgImpConfigs" +
                    "\r\n\tWHERE ImpConfigName = @configname;" +
                    "\r\n\tDELETE FROM tRgImpConfigs WHERE IDRgImpConfig = @idconfig;" +
                    "\r\n\tDELETE FROM tRgImpOptionsConfig WHERE IDRgImpConfig = @idconfig;" +
                    "\r\n\tDELETE FROM tRgImpColumns WHERE IDRgImpConfig = @idconfig;" +
                    "\r\nEND;";
                SqlCommand command = new SqlCommand(sql, _stringsSQL);
                command.ExecuteNonQuery();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "DeleteConfig";
                command.Parameters.Add("@configname", System.Data.SqlDbType.VarChar);
                command.Parameters["@configname"].Value = nameofconfig;
                command.ExecuteNonQuery();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "DROP PROCEDURE DeleteConfig";
                command.ExecuteNonQuery();
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
