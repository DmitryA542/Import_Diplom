using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using masterconfig;

namespace MainForm
{
    public partial class ChooseConfig : DevExpress.XtraEditors.XtraForm
    {
        public ChooseConfig()
        {
            InitializeComponent();
            var dt = new DataTable();
            DBOperations dB = new DBOperations();
            dt.Columns.Add("Configs", typeof(string));
            foreach (var row in dB.ShowListOfConfigs().Item1)
            {
                dt.Rows.Add(row);
            }
            gridControl1.DataSource = dt;

            ChooseButton.Click += (sender, e) =>
            {
                if (gridView1.SelectedRowsCount == 0)
                {
                    MessageBox.Show("Выберите конфигурацию");
                }
                else
                {
                    MasterConfigWindow masterConfigWindow = new MasterConfigWindow(false, dt.Rows[gridView1.GetSelectedRows()[0]].ItemArray[0].ToString());
                    masterConfigWindow.Show();
                    this.Close();
                }
            };

            gridView1.DoubleClick += (sender, e) =>
            {
                if (gridView1.SelectedRowsCount > 0)
                {
                    MasterConfigWindow masterConfigWindow = new MasterConfigWindow(false, dt.Rows[gridView1.GetSelectedRows()[0]].ItemArray[0].ToString());
                    masterConfigWindow.Show();
                    this.Close();
                }
            };

            DeleteButton.Click += (sender, e) =>
            {
                AcceptDeleteDialog dlg = new AcceptDeleteDialog(dt.Rows[gridView1.GetSelectedRows()[0]].ItemArray[0].ToString(), this);
                dlg.ShowDialog();
            };
        }
    }
}