using DevExpress.Spreadsheet;
using DevExpress.Utils.About;
using DevExpress.XtraSpreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImportModule
{
    public class ImportClass
    {
        private SqlConnection _stringsSQL = new SqlConnection("Integrated Security=SSPI;Initial Catalog=Imports;Data Source=IP-RDS\\SQLEXPRESS");
        private Worksheet _sheetfordata = null;

        private string _typeoffile = null;
        public string Typeoffile
        {
            get { return _typeoffile; }
            set { _typeoffile = value; }
        }

        private bool _flagErrors = false;
        public bool FlagErrors
        {
            get { return _flagErrors; }
            set { _flagErrors = value; }
        }

        private string _sheet = null;
        public string Sheet
        {
            get { return _sheet; }
            set { _sheet = value; }
        }

        private bool _autoflagsheet = false;
        public bool Autoflagsheet
        {
            get { return _autoflagsheet; }
            set { _autoflagsheet = value; }
        }

        private bool _autoflagtable = false;
        public bool Autoflagtable
        {
            get { return _autoflagtable; }
            set { _autoflagtable = value; }
        }

        private int _column = 0;
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        private int _row = 0;
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        private string _table = null;
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }

        private Dictionary<string, List<string>> _names = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> Names
        {
            get { return _names; }
            set { _names = value; }
        }

        private DataTable _dt = null;
        public void ImportCommand(string nameOfConfig, SpreadsheetControl sheetControl)
        {
            try
            {
                _stringsSQL.Open();
                var sql = "CREATE PROCEDURE GetOptionsFromConfig" +
                    "\r\n\t@config VARCHAR(255)" +
                    "\r\nAS" +
                    "\r\nBEGIN" +
                    "\r\n\tDECLARE @idconfig INT;" +
                    "\r\n\tDECLARE @idfield INT;" +
                    "\r\n\tDECLARE @idtable INT;" +
                    "\r\n\tSELECT @idconfig = IDRgImpConfig" +
                    "\r\n\tFROM tRgImpConfigs" +
                    "\r\n\tWHERE ImpConfigName = @config;" +
                    "\r\n\tSELECT @idfield = IDRgImpField" +
                    "\r\n\tFROM tRgImpColumns" +
                    "\r\n\tWHERE IDRgImpConfig = @idconfig;" +
                    "\r\n\tSELECT @idtable = IDRgImpTable" +
                    "\r\n\tFROM tRgImpFields" +
                    "\r\n\tWHERE IDRgImpField = @idfield;" +
                    "\r\n\tSELECT TypeOfFile, FlagErrors" +
                    "\r\n\tFROM tRgImpConfigs" +
                    "\r\n\tWHERE ImpConfigName = @config;" +
                    "\r\n\tSELECT SheetName, ColumnIndexTable, RowIndexTable, AutoChooseSheetFlag, AutoChooseTableFlag" +
                    "\r\n\tFROM tRgImpOptionsConfig" +
                    "\r\n\tWHERE IDRgImpConfig = @idconfig;" +
                    "\r\n\tSELECT ColumnNameUser, FieldNameSQL" +
                    "\r\n\tFROM tRgImpColumns" +
                    "\r\n\tINNER JOIN tRgImpFields ON tRgImpFields.IDRgImpField = tRgImpColumns.IDRgImpField" +
                    "\r\n\tWHERE IDRgImpConfig = @idconfig;" +
                    "\r\n\tSELECT TableNameUser" +
                    "\r\n\tFROM tRgImpTables" +
                    "\r\n\tWHERE IDRgImpTable = @idtable;" +
                    "\r\nEND;";
                var command = new SqlCommand(sql, _stringsSQL);
                command.ExecuteNonQuery();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "GetOptionsFromConfig";
                command.Parameters.Add("@config", System.Data.SqlDbType.VarChar);
                command.Parameters["@config"].Value = nameOfConfig;
                SqlDataReader reader = command.ExecuteReader();
                var i = 0;
                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (i == 0)
                        {
                            _typeoffile = reader.GetString(0);
                            _flagErrors = reader.GetBoolean(1);
                        }
                        if (i == 1)
                        {
                            if (sheetControl != null)
                            {
                                _sheetfordata = sheetControl.Document.Worksheets[reader.GetString(0)];
                            }
                            else
                            {
                                _sheet = reader.GetString(0);
                            }
                            _column = reader.GetInt32(1);
                            _row = reader.GetInt32(2);
                            _autoflagsheet = reader.GetBoolean(3);
                            _autoflagtable = reader.GetBoolean(4);
                        }
                        if (i == 2)
                        {
                            if (_names.ContainsKey(reader.GetString(1)))
                            {
                                _names[reader.GetString(1)].Add(reader.GetString(0));
                            }
                            else
                            {
                                List<string> names = new List<string>();
                                names.Add(reader.GetString(0));
                                _names.Add(reader.GetString(1), names);
                            }
                        }
                        if (i == 3)
                        {
                            _table = reader.GetString(0);
                        }
                    }
                    reader.NextResult();
                    i++;
                }
                reader.Close();
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "DROP PROCEDURE GetOptionsFromConfig";
                command.ExecuteNonQuery();
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ImportData()
        {
            try
            {
                _stringsSQL.Open();
                _dt = new DataTable();
                _dt.TableName = _table;
                var sql = "SELECT c.NAME 'Column Name', t.NAME 'Data type'" +
                    "\r\nFROM sys.columns c" +
                    "\r\nINNER JOIN sys.types t ON c.user_type_id = t.user_type_id" +
                    "\r\nWHERE c.object_id = OBJECT_ID('testtable_types') AND c.NAME != 'ID'";
                SqlCommand cmd = new SqlCommand(sql, _stringsSQL);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    switch (reader.GetString(1))
                    {
                        case "int":
                            _dt.Columns.Add(reader.GetString(0), typeof(int));
                            break;
                        case "datetime":
                            _dt.Columns.Add(reader.GetString(0), typeof(string));
                            break;
                        case "varchar":
                            _dt.Columns.Add(reader.GetString(0), typeof(string));
                            break;
                        case "bigint":
                            _dt.Columns.Add(reader.GetString(0), typeof(long));
                            break;
                        case "decimal":
                            _dt.Columns.Add(reader.GetString(0), typeof(double));
                            break;
                        case "money":
                            _dt.Columns.Add(reader.GetString(0), typeof(double));
                            break;
                    }
                }
                reader.Close();
                int indexHead = 1;
                while (GetRow(_sheetfordata, _row + 1, _column, indexHead).Count > 0)
                {
                    DataRow dataRow = _dt.NewRow();
                    dataRow.ItemArray = GetRow(_sheetfordata, _row + 1, _column, indexHead).ToArray();
                    _dt.Rows.Add(dataRow);
                    _row++;
                    indexHead++;
                }
                SqlBulkCopy bulkCopy = new SqlBulkCopy(_stringsSQL);
                bulkCopy.DestinationTableName = "dbo.testtable_types";
                foreach (var name in _names)
                {
                    bulkCopy.ColumnMappings.Add(name.Key, name.Key);
                }
                bulkCopy.WriteToServer(_dt);
                _stringsSQL.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<object> GetRow(Worksheet sheet, int x, int y, int indexHead)
        {
            var row = new List<object>();
            var i = 0;
            foreach (var dbname in _names)
            {
                y = _column;
                while (IsCellValueNotEmpty(sheet.Cells[x, y]) ||
                    IsCellHasBorders(sheet.Cells[x, y]) ||
                    IsCellHasBackground(sheet.Cells[x, y]))
                {
                    if (sheet.Cells[x - indexHead, y].Value.ToString() == dbname.Value.First())
                    {
                        try
                        {
                            if (_dt.Columns[i].DataType == typeof(int))
                            {
                                row.Add(Convert.ToInt32(Decimal.Parse(sheet.Cells[x, y].Value.ToString())));
                            }
                            else if (_dt.Columns[i].DataType == typeof(string))
                            {
                                row.Add(sheet.Cells[x, y].Value.ToString());
                            }
                            else if (_dt.Columns[i].DataType == typeof(long))
                            {
                                row.Add(Convert.ToInt64(Decimal.Parse(sheet.Cells[x, y].Value.ToString())));
                            }
                            else if (_dt.Columns[i].DataType == typeof(double))
                            {
                                row.Add(Convert.ToDouble(sheet.Cells[x, y].Value.ToString().Trim().Replace(".", ",")));
                            }
                            i++;
                        }
                        catch (Exception ex)
                        {
                            var print = sheet.Cells[x, y];
                            MessageBox.Show($"{x}, {y}");
                        }
                    }
                    y++;
                }
            }
            return row;
        }

        private bool IsCellValueNotEmpty(Cell cell)
        {
            return !cell.Value.IsEmpty;
        }

        private bool IsCellHasBorders(Cell cell)
        {
            return cell.Borders.TopBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.RightBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.LeftBorder.LineStyle.ToString() != "None" ||
                   cell.Borders.BottomBorder.LineStyle.ToString() != "None";
        }

        private bool IsCellHasBackground(Cell cell)
        {
            return !cell.Fill.BackgroundColor.IsEmpty;
        }
    }
}
