namespace MainForm
{
    partial class ChooseForImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BoxOfConfigs = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.PathOfFile = new DevExpress.XtraEditors.TextEdit();
            this.OpenFileButton = new DevExpress.XtraEditors.SimpleButton();
            this.CancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.ImportButton = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.BoxOfConfigs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOfFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BoxOfConfigs
            // 
            this.BoxOfConfigs.Location = new System.Drawing.Point(135, 28);
            this.BoxOfConfigs.Name = "BoxOfConfigs";
            this.BoxOfConfigs.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BoxOfConfigs.Properties.Appearance.Options.UseFont = true;
            this.BoxOfConfigs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BoxOfConfigs.Size = new System.Drawing.Size(173, 28);
            this.BoxOfConfigs.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(117, 22);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Конфигурация:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(12, 118);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(46, 22);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Файл:";
            // 
            // PathOfFile
            // 
            this.PathOfFile.Location = new System.Drawing.Point(64, 115);
            this.PathOfFile.Name = "PathOfFile";
            this.PathOfFile.Properties.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PathOfFile.Properties.Appearance.Options.UseFont = true;
            this.PathOfFile.Size = new System.Drawing.Size(270, 28);
            this.PathOfFile.TabIndex = 4;
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OpenFileButton.Appearance.Options.UseFont = true;
            this.OpenFileButton.Location = new System.Drawing.Point(340, 115);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(114, 28);
            this.OpenFileButton.TabIndex = 5;
            this.OpenFileButton.Text = "Обзор...";
            // 
            // CancelButton
            // 
            this.CancelButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CancelButton.Appearance.Options.UseFont = true;
            this.CancelButton.Location = new System.Drawing.Point(240, 220);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 29);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Отмена";
            // 
            // ImportButton
            // 
            this.ImportButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ImportButton.Appearance.Options.UseFont = true;
            this.ImportButton.Location = new System.Drawing.Point(340, 220);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(132, 29);
            this.ImportButton.TabIndex = 7;
            this.ImportButton.Text = "Импорт";
            // 
            // ChooseForImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.PathOfFile);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BoxOfConfigs);
            this.IconOptions.ShowIcon = false;
            this.LookAndFeel.SkinName = "Office 2013";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "ChooseForImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Импорт файла";
            ((System.ComponentModel.ISupportInitialize)(this.BoxOfConfigs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOfFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit BoxOfConfigs;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit PathOfFile;
        private DevExpress.XtraEditors.SimpleButton OpenFileButton;
        private DevExpress.XtraEditors.SimpleButton CancelButton;
        private DevExpress.XtraEditors.SimpleButton ImportButton;
    }
}