using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
using ImportModule;
using masterconfig;
using System.IO;
using System.Windows.Forms;

namespace MainForm
{
    public partial class ChooseForImport : DevExpress.XtraEditors.XtraForm
    {
        private DBOperations _DB = new DBOperations();
        private SpreadsheetControl _sheet = null;
        private string _NameConfig = null;
        private string _extension = null;
        public ChooseForImport(MainForm form)
        {
            InitializeComponent();
            bool FlagClosed = true;
            foreach (string config in _DB.ShowListOfConfigs().Item1)
            {
                BoxOfConfigs.Properties.Items.Add(config);
            }
            BoxOfConfigs.Properties.Items.Add("Новая...");

            CancelButton.Click += (sender, e) =>
            {
                FlagClosed = false;
                form.Enabled = true; form.Visible = true;
                this.Close();
            };

            this.FormClosed += (sender, e) =>
            {
                if (FlagClosed)
                {
                    form.Close();
                }
            };

            BoxOfConfigs.TextChanged += (sender, e) =>
            {
                if (BoxOfConfigs.Text == "Новая...")
                {
                    FlagClosed = false;
                    form.Enabled = true; form.Visible = true;
                    MasterConfigWindow masterConfig = new MasterConfigWindow(true, null);
                    masterConfig.Show();
                    this.Close();
                }
                else
                {
                    _NameConfig = BoxOfConfigs.Text.Trim();
                }
            };

            OpenFileButton.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "C:\\";
                if (_DB.ShowListOfConfigs().Item2 == "xls")
                {
                    openFileDialog.Filter = "XLS files (*.xls)|*.xls";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PathOfFile.Text = openFileDialog.FileName;
                        _extension = Path.GetExtension(openFileDialog.FileName);
                        _sheet = new SpreadsheetControl();
                        _sheet.LoadDocument(openFileDialog.FileName, DocumentFormat.Xls);
                    }
                }
                else if (_DB.ShowListOfConfigs().Item2 == "xlsx")
                {
                    openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PathOfFile.Text = openFileDialog.FileName;
                        _extension = Path.GetExtension(openFileDialog.FileName);
                        _sheet = new SpreadsheetControl();
                        _sheet.LoadDocument(openFileDialog.FileName, DocumentFormat.Xlsx);
                    }
                }
                else
                {
                    openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PathOfFile.Text = openFileDialog.FileName;
                        _extension = Path.GetExtension(openFileDialog.FileName);
                        _sheet = new SpreadsheetControl();
                        _sheet.LoadDocument(openFileDialog.FileName, DocumentFormat.Csv);
                    }
                }
            };

            ImportButton.Click += (sender, e) =>
            {
                if (BoxOfConfigs.Text == "")
                {
                    MessageBox.Show("Конфигурация не выбрана");
                }
                else if (PathOfFile.Text == "")
                {
                    MessageBox.Show("Файл не выбран");
                }
                else if (_extension != ".xls" && _extension != ".xlsx" && _extension != ".csv")
                {
                    MessageBox.Show("Файл неправильного типа");
                }
                else
                {
                    ImportClass importClass = new ImportClass();
                    importClass.ImportCommand(_NameConfig, _sheet);
                    importClass.ImportData();
                    MessageBox.Show("Импорт завершен");
                }
            };
        }
    }
}