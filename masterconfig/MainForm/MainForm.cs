﻿using masterconfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
            CreateConfigButton.Click += (sender, e) =>
            {
                MasterConfigWindow master = new MasterConfigWindow(true, null);
                master.Show();
            };

            ChangeConfigurationButton.Click += (sender, e) =>
            {
                ChooseConfig chooseConfig = new ChooseConfig();
                chooseConfig.Show();
            };

            ChooseConfigAndFileButton.Click += (sender, e) =>
            {
                ChooseForImport import = new ChooseForImport(this);
                import.Show();
                this.Enabled = false; this.Visible = false;
            };
        }
    }
}
