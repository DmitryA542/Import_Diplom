using DevExpress.Entity.Model;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImportModule;

namespace masterconfig
{
    public partial class MasterConfigWindow : DevExpress.XtraEditors.XtraForm
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private FirstStageOperationsClass _firstStage = null;
        private SecondStageOperationsClass _secondStage = null;
        private ThirdStageOperationsClass _thirdStage = null;
        private string[] _types = new string[] { "xls", "xlsx", "csv" };
        private string _nameDocument = null;
        private Worksheet _sheet = null;
        private string _sheetnameforupdate = null;
        private (string, string) _coupleOfNameAndExtension = (null, null);
        private bool _flagDocumentLoaded = false;
        private bool _flagCell = true;
        private bool _flagSheetLoaded = false;
        private DataTable _dataTable = null;
        private ImportClass _importClass = new ImportClass();
        public MasterConfigWindow(bool CreateFlag, string nameofconfig)
        {
            if (CreateFlag)
            {
                #region InitiateAllThings
                InitializeComponent();
                WindowAllStages.TabPages[1].PageVisible = false; WindowAllStages.TabPages[2].PageVisible = false; WindowAllStages.TabPages[3].PageVisible = false;
                AutoChooseSheetFlag.Checked = true; AutoChooseTableFlag.Checked = true;
                ListOfColumnsInDB.Enabled = false; MainTableOfLinks.Enabled = false;
                MainSpreadSheet.Enabled = false; MainSpreadSheet.Visible = false;
                _firstStage = new FirstStageOperationsClass();
                _secondStage = new SecondStageOperationsClass();
                _thirdStage = new ThirdStageOperationsClass();

                _dataTable = new DataTable();
                _dataTable.Columns.Add("DBName", typeof(string));
                _dataTable.Columns.Add("ExcelName", typeof(string));
                MainTableOfLinks.DataSource = _dataTable;
                #endregion
            }
            else
            {
                InitializeComponent();
                MainSpreadSheet.Enabled = false; MainSpreadSheet.Visible = false;
                _firstStage = new FirstStageOperationsClass();
                _secondStage = new SecondStageOperationsClass();
                _thirdStage = new ThirdStageOperationsClass();
                _importClass.ImportCommand(nameofconfig, null);
                _sheetnameforupdate = _importClass.Sheet;

                _dataTable = new DataTable();
                _dataTable.Columns.Add("DBName", typeof(string));
                _dataTable.Columns.Add("ExcelName", typeof(string));
                MainTableOfLinks.DataSource = _dataTable;
                NameOfConfiguration.EditValue = nameofconfig;
                TypesOfFilesBox.EditValue = _importClass.Typeoffile;
                if (_importClass.Autoflagsheet)
                {
                    AutoChooseSheetFlag.Checked = true;
                }
                else
                {
                    AutoChooseSheetFlag.Checked = false;
                    NamesOfSheetsBox.EditValue = _importClass.Sheet;
                }
                if (_importClass.Autoflagtable)
                {
                    AutoChooseTableFlag.Checked = true;
                }
                else
                {
                    AutoChooseTableFlag.Checked = false;
                    string first = _importClass.Column.ToString();
                    int i = int.Parse(first) + 1;
                    string second = _importClass.Row.ToString();

                    string str = string.Empty;
                    while (i > 0)
                    {
                        str = ALPHABET[(i - 1) % 26] + str;
                        i /= 26;
                    }
                    ColumnIndexUpLeft.EditValue = str;
                    int second1 = Int32.Parse(second);
                    second1 = second1 + 1;
                    RowIndexUpLeft.EditValue = second1.ToString();
                }
                DBTablesBox.Text = _importClass.Table;
                foreach (var column in _thirdStage.ShowColumns(DBTablesBox.Text))
                {
                    ListOfColumnsInDB.Items.Add(column);
                }
                _dataTable.Rows.Clear();
                foreach (var name in _importClass.Names)
                {
                    foreach (var excelname in name.Value)
                    {
                        _dataTable.Rows.Add(name.Key, excelname);
                    }
                }
                if (_importClass.FlagErrors)
                {
                    FlagIngoreErrors.Checked = true;
                }
                else
                {
                    FlagIngoreErrors.Checked = false;
                }
            }

            WindowAllStages.SelectedPageChanged += (sender, e) =>
            {
                if (WindowAllStages.SelectedTabPage == WindowAllStages.TabPages[2] && !CreateFlag)
                {
                    if (MainSpreadSheet.Enabled)
                    {
                        if (AutoChooseSheetFlag.Checked)
                        {
                            foreach (var i in MainSpreadSheet.Document.Worksheets)
                            {
                                if (_secondStage.GetFlagIsTableExists(i, 0, 0).Item1)
                                {
                                    _sheet = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _sheet = MainSpreadSheet.ActiveWorksheet;
                        }
                        if (AutoChooseTableFlag.Checked)
                        {
                            _secondStage.AutoFillTable(_sheet, AutoChooseTableFlag.Checked);
                        }
                        else
                        {
                            _secondStage.AutoFillTable(_sheet, false, _sheet.Cells[ColumnIndexUpLeft.Text + RowIndexUpLeft.Text].ColumnIndex, _sheet.Cells[ColumnIndexUpLeft.Text + RowIndexUpLeft.Text].RowIndex);
                        }
                    }
                    else
                    {
                        _secondStage.SetList();
                    }
                    repositoryItemComboBox1.Items.Clear();
                    if (_secondStage.GetList() != null)
                    {
                        foreach (var header in _secondStage.GetList())
                        {
                            if (header != null)
                            {
                                repositoryItemComboBox1.Items.Add(header);
                            }
                        }
                    }
                    else
                    {
                        repositoryItemComboBox1.Items.Clear();
                    }
                    _dataTable.Rows.Add("", repositoryItemComboBox1);
                    _dataTable.Rows.RemoveAt(_dataTable.Rows.Count - 1);
                }
            };

            #region ChangePagesButtons
            ButtonToStage2.Click += (sender, e) =>
            {
                if (NameOfConfiguration.EditValue == null)
                {
                    MessageBox.Show("Название конфигурации не может быть пустым");
                }
                else if (TypesOfFilesBox.EditValue == null)
                {
                    MessageBox.Show("Расширение файла не может быть пустым");
                }
                else if (!_types.Contains(TypesOfFilesBox.EditValue.ToString()))
                {
                    MessageBox.Show("Расширение файла должно соответствовать формату: XLS, XLSX или CSV");
                }
                else
                {
                    _coupleOfNameAndExtension.Item1 = NameOfConfiguration.Text;
                    WindowAllStages.TabPages[1].PageVisible = true;
                    WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[1];
                }
            };

            ButtonToStage3.Click += (sender, e) =>
            {
                if (!AutoChooseSheetFlag.Checked && NamesOfSheetsBox.Text == "")
                {
                    MessageBox.Show("В группе 'Лист' ничего не выбрано");
                }
                else if (!AutoChooseTableFlag.Checked && (ColumnIndexUpLeft.Text == "" || RowIndexUpLeft.Text == ""))
                {
                    MessageBox.Show("В группе 'Таблица' ничего не выбрано");
                }
                else
                {
                    if (MainSpreadSheet.Enabled)
                    {
                        if (AutoChooseSheetFlag.Checked)
                        {
                            foreach (var i in MainSpreadSheet.Document.Worksheets)
                            {
                                if (_secondStage.GetFlagIsTableExists(i, 0, 0).Item1)
                                {
                                    _sheet = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            _sheet = MainSpreadSheet.ActiveWorksheet;
                        }
                        if (AutoChooseTableFlag.Checked)
                        {
                            _secondStage.AutoFillTable(_sheet, AutoChooseTableFlag.Checked);
                        }
                        else
                        {
                            _secondStage.AutoFillTable(_sheet, false, _sheet.Cells[ColumnIndexUpLeft.Text + RowIndexUpLeft.Text].ColumnIndex, _sheet.Cells[ColumnIndexUpLeft.Text + RowIndexUpLeft.Text].RowIndex);
                        }
                    }
                    else
                    {
                        _secondStage.SetList();
                    }
                    if (CreateFlag)
                    {
                        DBTablesBox.Text = "";
                        repositoryItemComboBox1.Items.Clear();
                        ListOfColumnsInDB.Items.Clear();
                        ListOfColumnsInDB.Enabled = false;
                        _dataTable.Rows.Clear();
                        MainTableOfLinks.Enabled = false;
                    }
                    else
                    {
                        if (MainSpreadSheet.Enabled)
                        {
                            repositoryItemComboBox1.Items.Clear();
                            if (_secondStage.GetList() != null)
                            {
                                foreach (var header in _secondStage.GetList())
                                {
                                    if (header != null)
                                    {
                                        repositoryItemComboBox1.Items.Add(header);
                                    }
                                }
                            }
                            else
                            {
                                repositoryItemComboBox1.Items.Clear();
                            }
                            _dataTable.Rows.Add("", repositoryItemComboBox1);
                            _dataTable.Rows.RemoveAt(_dataTable.Rows.Count - 1);
                        }
                    }
                    WindowAllStages.TabPages[2].PageVisible = true;
                    WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[2];
                }
            };

            ButonTo4Stage.Click += (sender, e) =>
            {
                WindowAllStages.TabPages[3].PageVisible = true;
                WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[3];
            };

            ButtonBackTo1.Click += (sender, e) =>
            {
                WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[0];
            };

            ButtonBackTo2.Click += (sender, e) =>
            {
                WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[1];
            };

            BackTo3Stage.Click += (sender, e) =>
            {
                WindowAllStages.SelectedTabPage = WindowAllStages.TabPages[2];
            };
            #endregion

            #region FlagsControlls
            AutoChooseSheetFlag.CheckedChanged += (sender, e) =>
            {
                if (NamesOfSheetsBox.Text != "")
                {
                    if (AutoChooseSheetFlag.Checked)
                    {
                        NamesOfSheetsBox.Text = "";
                    }
                }
            };

            AutoChooseTableFlag.CheckedChanged += (sender, e) =>
            {
                if (AutoChooseTableFlag.Checked)
                {
                    ColumnIndexUpLeft.Text = "";
                    RowIndexUpLeft.Text = "";
                    if (MainSpreadSheet.Enabled)
                    {
                        MainSpreadSheet.ActiveWorksheet.Selection = _secondStage.GetFlagIsTableExists(MainSpreadSheet.ActiveWorksheet, 0, 0).Item2;
                    }
                }
                else
                {
                    _flagCell = false;
                    MainSpreadSheet.ActiveWorksheet.Selection = MainSpreadSheet.ActiveWorksheet.Cells[20, 20];
                    _flagCell = true;
                }
            };

            FlagIngoreErrors.CheckedChanged += (sender, e) =>
            {
                if (FlagIngoreErrors.Checked)
                {
                    labelControl1.Text = "В этом режиме программа \n\rбудет игнорировать все встреченные ошибки \n\rи не записывать их базу данных.";
                }
                else
                {
                    labelControl1.Text = "В этом режиме, в случае возникновения ошибки программа сообщит, \n\rв скольких строках произошла ошибка.";
                }
            };
            #endregion

            #region SpreadsheetControllers
            NamesOfSheetsBox.TextChanged += (sender, e) =>
            {
                if (NamesOfSheetsBox.Text != "")
                {
                    AutoChooseSheetFlag.Checked = false;
                    if (_flagDocumentLoaded)
                    {
                        if (NamesOfSheetsBox.Properties.Items.Contains(NamesOfSheetsBox.Text))
                        {
                            MainSpreadSheet.Document.Worksheets.ActiveWorksheet = MainSpreadSheet.Document.Worksheets[NamesOfSheetsBox.SelectedText];
                        }
                        else
                        {
                            MessageBox.Show("Такого листа не существует");
                        }
                    }
                }
            };

            MainSpreadSheet.DocumentLoaded += (sender, e) =>
            {
                _flagSheetLoaded = false;
            };

            MainSpreadSheet.SelectionChanged += (sender, e) =>
            {
                if (_flagCell && _flagSheetLoaded)
                {
                    AutoChooseTableFlag.Checked = false;
                    var cell = MainSpreadSheet.ActiveWorksheet.Cells[MainSpreadSheet.ActiveWorksheet.Selection.LeftColumnIndex, MainSpreadSheet.ActiveWorksheet.Selection.TopRowIndex];
                    var print = cell.ToString().Substring(5, 2);
                    ColumnIndexUpLeft.EditValue = MainSpreadSheet.ActiveWorksheet.Selection.ToString().Substring(8, 1);
                    RowIndexUpLeft.EditValue = MainSpreadSheet.ActiveWorksheet.Selection.ToString().Substring(9, 1);
                }
                _flagSheetLoaded = true;
            };

            ColumnIndexUpLeft.TextChanged += (sender, e) =>
            {
                if (ColumnIndexUpLeft.Text != "" && RowIndexUpLeft.Text != "")
                {
                    AutoChooseTableFlag.Checked = false;
                }
            };

            RowIndexUpLeft.TextChanged += (sender, e) =>
            {
                if (ColumnIndexUpLeft.Text != "" && RowIndexUpLeft.Text != "")
                {
                    AutoChooseTableFlag.Checked = false;
                }
            };

            ShowCellButton.Click += (sender, e) =>
            {
                if (_flagDocumentLoaded && ColumnIndexUpLeft.Text != "" && RowIndexUpLeft.Text != "")
                {
                    if ((ColumnIndexUpLeft.Text.ToCharArray()[0] >= 'A') && (ColumnIndexUpLeft.Text.ToCharArray()[0] <= 'Z'))
                    {
                        _flagCell = false;
                        MainSpreadSheet.Selection = MainSpreadSheet.ActiveWorksheet.Cells[ColumnIndexUpLeft.Text + RowIndexUpLeft.Text];
                        _flagCell = true;
                    }
                    else
                    {
                        MessageBox.Show("Некорректный ввод");
                    }
                }
            };
            #endregion

            #region BoxOfTypesFiles
            foreach (var type in _types)
            {
                TypesOfFilesBox.Properties.Items.Add(type);
            }
            #endregion

            #region BoxDBTables
            foreach (var table in _thirdStage.ShowTables())
            {
                DBTablesBox.Properties.Items.Add(table);
            }

            DBTablesBox.TextChanged += (sender, e) =>
            {
                if (DBTablesBox.Properties.Items.Contains(DBTablesBox.Text))
                {
                    ListOfColumnsInDB.Enabled = true; MainTableOfLinks.Enabled = true;
                    ListOfColumnsInDB.Items.Clear();
                    _dataTable.Rows.Clear();
                    foreach (var column in _thirdStage.ShowColumns(DBTablesBox.Text))
                    {
                        ListOfColumnsInDB.Items.Add(column);
                    }
                    repositoryItemComboBox1.Items.Clear();
                    if (_secondStage.GetList() != null)
                    {
                        foreach (var header in _secondStage.GetList())
                        {
                            if (header != null)
                            {
                                repositoryItemComboBox1.Items.Add(header);
                            }
                        }
                    }
                    else
                    {
                        repositoryItemComboBox1.Items.Clear();
                    }
                    if (!CreateFlag && DBTablesBox.Text == _importClass.Table)
                    {
                        foreach (var name in _importClass.Names)
                        {
                            foreach (var excelname in name.Value)
                            {
                                _dataTable.Rows.Add(name.Key, excelname);
                            }
                        }
                    }
                    else
                    {
                        foreach (var column in ListOfColumnsInDB.Items)
                        {
                            _dataTable.Rows.Add(column.ToString(), repositoryItemComboBox1);
                        }
                    }
                }
            };
            #endregion

            TransferButton.Click += (sender, e) =>
            {
                repositoryItemComboBox1.Items.Clear();
                if (_secondStage.GetList() != null)
                {
                    foreach (var header in _secondStage.GetList())
                    {
                        repositoryItemComboBox1.Items.Add(header);
                    }
                }
                else
                {
                    repositoryItemComboBox1.Items.Clear();
                }
                foreach (var column in ListOfColumnsInDB.SelectedItems)
                {
                    _dataTable.Rows.Add(column.ToString(), repositoryItemComboBox1);
                }
            };

            DeleteRowButton.Click += (sender, e) =>
            {
                gridView1.DeleteSelectedRows();
            };

            PathOfFile.TextChanged += (sender, e) =>
            {
                if (PathOfFile.Text == "")
                {
                    MasterConfigWindow.ActiveForm.Size = new Size(1754, 460);
                    this.CenterToScreen();
                    MainSpreadSheet.Enabled = false; MainSpreadSheet.Visible = false;
                    NamesOfSheetsBox.Properties.Items.Clear();
                }
            };

            TypesOfFilesBox.TextChanged += (sender, e) =>
            {
                if (PathOfFile.Text != "" && _coupleOfNameAndExtension.Item2 != "." + TypesOfFilesBox.Text)
                {
                    PathOfFile.Text = "";
                }
            };

            #region OpenFileCommand
            ChooseFileButton.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx|XLS files (*.xls)|*.xls|CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _nameDocument = openFileDialog.FileName;
                    _coupleOfNameAndExtension.Item2 = Path.GetExtension(_nameDocument);
                    _flagDocumentLoaded = true;
                    PathOfFile.Text = _nameDocument;
                    foreach (string type in TypesOfFilesBox.Properties.Items)
                    {
                        if (_coupleOfNameAndExtension.Item2 == "." + type)
                        {
                            TypesOfFilesBox.EditValue = type;
                        }
                    }
                    MasterConfigWindow.ActiveForm.Size = new Size(1754, 800);
                    this.CenterToScreen();
                    MainSpreadSheet.Enabled = true; MainSpreadSheet.Visible = true;
                    if (_coupleOfNameAndExtension.Item2 == ".xls")
                    {
                        MainSpreadSheet.LoadDocument(_nameDocument, DocumentFormat.Xls);
                    }
                    else if (_coupleOfNameAndExtension.Item2 == ".xlsx")
                    {
                        MainSpreadSheet.LoadDocument(_nameDocument, DocumentFormat.Xlsx);
                    }
                    else
                    {
                        MainSpreadSheet.LoadDocument(_nameDocument, DocumentFormat.Csv);
                        AutoChooseSheetFlag.Enabled = false; NamesOfSheetsBox.Enabled = false;
                    }
                    foreach (var sheet in MainSpreadSheet.Document.Worksheets)
                    {
                        NamesOfSheetsBox.Properties.Items.Add(sheet.Name);
                    }
                }
            };
            #endregion

            SaveButton.Click += (sender, e) =>
            {
                _firstStage.SetName(NameOfConfiguration.Text);
                _firstStage.SetTypeOfFile(TypesOfFilesBox.Text);
                if (AutoChooseSheetFlag.Checked)
                {
                    _secondStage.SetAutoSheetFlag(true);
                    if (!CreateFlag)
                    {
                        _secondStage.SetSheetName(_sheetnameforupdate);
                    }
                }
                else
                {
                    _secondStage.SetAutoSheetFlag(false);
                    if (CreateFlag)
                    {
                        _secondStage.SetSheetName(_sheet.Name);
                    }
                    else
                    {
                        _secondStage.SetSheetName(NamesOfSheetsBox.Text);
                    }
                }
                if (AutoChooseTableFlag.Checked)
                {
                    _secondStage.SetAutoTableFlag(true);
                }
                else
                {
                    _secondStage.SetAutoTableFlag(false);
                    int i = 0;
                    var ce = ColumnIndexUpLeft.Text.GetEnumerator();
                    while (ce.MoveNext())
                    {
                        i = (26 * i) + ALPHABET.IndexOf(ce.Current) + 1;
                    }
                    _secondStage.SetColumnIndex(i - 1);
                    _secondStage.SetRowIndex(Int32.Parse(RowIndexUpLeft.Text) - 1);
                }
                _thirdStage.SetSource(_dataTable);
                _thirdStage.SetExcelNameTable(DBTablesBox.Text);
                if (FlagIngoreErrors.Checked)
                {
                    _thirdStage.SetFlagErrors(true);
                }
                else
                {
                    _thirdStage.SetFlagErrors(false);
                }
                SaveConfigOperationClass operation = new SaveConfigOperationClass();
                if (nameofconfig != null)
                {
                    operation.MainOperationSave(_firstStage, _secondStage, _thirdStage, CreateFlag, nameofconfig);
                }
                else
                {
                    operation.MainOperationSave(_firstStage, _secondStage, _thirdStage, CreateFlag, null);
                }
                this.Close();
            };
        }
    }
}