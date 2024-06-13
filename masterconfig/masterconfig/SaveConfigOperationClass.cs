using DevExpress.Utils.About;
using DevExpress.XtraSpreadsheet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace masterconfig
{
    class SaveConfigOperationClass
    {
        private SqlConnection _stringsSQL = new SqlConnection("Integrated Security=SSPI;Initial Catalog=Imports;Data Source=IP-RDS\\SQLEXPRESS");
        private FirstStageOperationsClass _firstStage = null;
        private SecondStageOperationsClass _secondStage = null;
        private ThirdStageOperationsClass _thirdStage = null;

        public void MainOperationSave(FirstStageOperationsClass firstStage, 
            SecondStageOperationsClass secondStage, 
            ThirdStageOperationsClass thirdStage,
            bool createFlag, string nameofconfig)
        {
            _firstStage = firstStage;
            _secondStage = secondStage;
            _thirdStage = thirdStage;
            try
            {
                _stringsSQL.Open();
                var sql = "";
                SqlCommand command = null;
                if (createFlag)
                {
                    sql = "INSERT INTO tRgImpConfigs (ImpConfigName, TypeOfFile, FlagErrors) VALUES(@configname, @typeoffile, @flagerrors)";
                    command = new SqlCommand(sql, _stringsSQL);
                    command.Parameters.Add("@configname", System.Data.SqlDbType.VarChar);
                    command.Parameters["@configname"].Value = _firstStage.GetName();
                    command.Parameters.Add("@typeoffile", System.Data.SqlDbType.VarChar);
                    command.Parameters["@typeoffile"].Value = _firstStage.GetTypeOfFile();
                    if (_thirdStage.GetFlagErrors())
                    {
                        command.Parameters.Add("@flagerrors", System.Data.SqlDbType.Bit);
                        command.Parameters["@flagerrors"].Value = 1;
                    }
                    else
                    {
                        command.Parameters.Add("@flagerrors", System.Data.SqlDbType.Bit);
                        command.Parameters["@flagerrors"].Value = 0;
                    }
                    command.ExecuteNonQuery();
                }
                sql = "CREATE PROCEDURE NastroikiConfig" +
                    "\r\n\t@keyword VARCHAR(255)," +
                    "\r\n\t@autosheet BIT," +
                    "\r\n\t@sheetname VARCHAR(255)," +
                    "\r\n\t@autotable BIT," +
                    "\r\n\t@column INT," +
                    "\r\n\t@row INT" +
                    "\r\nAS" +
                    "\r\nBEGIN" +
                    "\r\n    DECLARE @id INT;" +
                    "\r\n    SELECT @id = IDRgImpConfig" +
                    "\r\n    FROM tRgImpConfigs" +
                    "\r\n    WHERE ImpConfigName = @keyword;" +
                    (createFlag ? "\r\n    INSERT INTO tRgImpOptionsConfig (AutoChooseSheetFlag, SheetName, AutoChooseTableFlag, ColumnIndexTable, RowIndexTable, IDRgImpConfig)" +
                "\r\n    VALUES(@autosheet, @sheetname, @autotable, @column, @row, @id)" : "\r\n    UPDATE tRgImpOptionsConfig SET AutoChooseSheetFlag = @autosheet, SheetName = @sheetname, AutoChooseTableFlag = @autotable, ColumnIndexTable = @column, RowIndexTable = @row WHERE IDRgImpConfig = @id;" +
                "\r\n   DELETE FROM tRgImpColumns WHERE IDRgImpConfig = @id") +
                    "\r\nEND";
                command = new SqlCommand(sql, _stringsSQL);
                command.ExecuteNonQuery();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "NastroikiConfig";
                if (nameofconfig == null)
                {
                    command.Parameters.Add("@keyword", System.Data.SqlDbType.VarChar);
                    command.Parameters["@keyword"].Value = _firstStage.GetName();
                }
                else
                {
                    command.Parameters.Add("@keyword", System.Data.SqlDbType.VarChar);
                    command.Parameters["@keyword"].Value = nameofconfig;
                }

                if (_secondStage.GetAutoSheetFlag())
                {
                    command.Parameters.Add("@autosheet", System.Data.SqlDbType.Bit);
                    command.Parameters["@autosheet"].Value = 1;
                }
                else
                {
                    command.Parameters.Add("@autosheet", System.Data.SqlDbType.Bit);
                    command.Parameters["@autosheet"].Value = 0;
                }

                command.Parameters.Add("@sheetname", System.Data.SqlDbType.VarChar);
                command.Parameters["@sheetname"].Value = _secondStage.GetSheetName();

                if (_secondStage.GetAutoTableFlag())
                {
                    command.Parameters.Add("@autotable", System.Data.SqlDbType.Bit);
                    command.Parameters["@autotable"].Value = 1;
                }
                else
                {
                    command.Parameters.Add("@autotable", System.Data.SqlDbType.Bit);
                    command.Parameters["@autotable"].Value = 0;
                }

                command.Parameters.Add("@column", System.Data.SqlDbType.Int);
                command.Parameters["@column"].Value = _secondStage.GetColumnIndex();

                command.Parameters.Add("@row", System.Data.SqlDbType.Int);
                command.Parameters["@row"].Value = _secondStage.GetRowIndex();
                command.ExecuteNonQuery();
                command.CommandText = "DROP PROCEDURE NastroikiConfig";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                foreach (DataRow row in _thirdStage.GetSource().Rows)
                {
                    var DbName = row.ItemArray[0].ToString().Trim();
                    var ExcelName = row.ItemArray[1];
                    sql = "CREATE PROCEDURE InsertColumns" +
                    "\r\n    @dbname VARCHAR(255)," +
                    "\r\n    @configname VARCHAR(255)," +
                    "\r\n    @Name VARCHAR(255)," +
                    "\r\n    @excelname VARCHAR(255)" +
                    "\r\nAS" +
                    "\r\nBEGIN" +
                    "\r\n    DECLARE @idfield INT;" +
                    "\r\n    DECLARE @idtable INT;" +
                    "\r\n    DECLARE @idConfig INT;" +
                    "\r\n    SELECT @idConfig = IDRgImpConfig" +
                    "\r\n    FROM tRgImpConfigs" +
                    "\r\n    WHERE ImpConfigName = @configname;" +
                    "\r\n    SELECT @idtable = IDRgImpTable" +
                    "\r\n    FROM tRgImpTables" +
                    "\r\n    WHERE TableNameUser = @excelname;" +
                    "\r\n    SELECT @idfield = IDRgImpField" +
                    "\r\n    FROM tRgImpFields" +
                    "\r\n    WHERE FieldNameSQL = @dbname AND IDRgImpTable = @idtable;" +
                    "\r\n    INSERT INTO tRgImpColumns (ColumnNameUser, IDRgImpField, IDRgImpConfig)" +
                    "\r\n    VALUES(@Name, @idfield, @IdConfig)" +
                    "\r\nEND";
                    command = new SqlCommand(sql, _stringsSQL);
                    command.ExecuteNonQuery();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "InsertColumns";
                    command.Parameters.Add("@dbname", System.Data.SqlDbType.VarChar);
                    command.Parameters["@dbname"].Value = DbName;

                    command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                    command.Parameters["@Name"].Value = ExcelName;

                    if (nameofconfig == null)
                    {
                        command.Parameters.Add("@configname", System.Data.SqlDbType.VarChar);
                        command.Parameters["@configname"].Value = _firstStage.GetName();
                    }
                    else
                    {
                        command.Parameters.Add("@configname", System.Data.SqlDbType.VarChar);
                        command.Parameters["@configname"].Value = nameofconfig;
                    }

                    command.Parameters.Add("@excelname", System.Data.SqlDbType.VarChar);
                    command.Parameters["@excelname"].Value = _thirdStage.GetExcelNameTable();
                    command.ExecuteNonQuery();
                    command.CommandText = "DROP PROCEDURE InsertColumns";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                if (!createFlag)
                {
                    sql = "DECLARE @id INT;" +
                        "\r\nSELECT @id = IDRgImpConfig" +
                        "\r\nFROM tRgImpConfigs" +
                        "\r\nWHERE ImpConfigName = @configname;" +
                        "\r\nUPDATE tRgImpConfigs SET ImpConfigName = @newconfigname, TypeOfFile = @typeoffile, FlagErrors = @flagerrors WHERE IDRgImpConfig = @id;";
                    command = new SqlCommand(sql, _stringsSQL);
                    command.Parameters.Add("@configname", System.Data.SqlDbType.VarChar);
                    command.Parameters["@configname"].Value = nameofconfig;
                    command.Parameters.Add("@newconfigname", SqlDbType.VarChar);
                    command.Parameters["@newconfigname"].Value = _firstStage.GetName();
                    command.Parameters.Add("@typeoffile", System.Data.SqlDbType.VarChar);
                    command.Parameters["@typeoffile"].Value = _firstStage.GetTypeOfFile();
                    if (_thirdStage.GetFlagErrors())
                    {
                        command.Parameters.Add("@flagerrors", System.Data.SqlDbType.Bit);
                        command.Parameters["@flagerrors"].Value = 1;
                    }
                    else
                    {
                        command.Parameters.Add("@flagerrors", System.Data.SqlDbType.Bit);
                        command.Parameters["@flagerrors"].Value = 0;
                    }
                    command.ExecuteNonQuery();
                }
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
