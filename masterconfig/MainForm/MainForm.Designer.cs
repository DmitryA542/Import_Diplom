namespace MainForm
{
    partial class MainForm
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
            this.CreateConfigButton = new DevExpress.XtraEditors.SimpleButton();
            this.ChooseConfigAndFileButton = new DevExpress.XtraEditors.SimpleButton();
            this.ChangeConfigurationButton = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // CreateConfigButton
            // 
            this.CreateConfigButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreateConfigButton.Appearance.Options.UseFont = true;
            this.CreateConfigButton.Location = new System.Drawing.Point(140, 55);
            this.CreateConfigButton.Name = "CreateConfigButton";
            this.CreateConfigButton.Size = new System.Drawing.Size(200, 29);
            this.CreateConfigButton.TabIndex = 0;
            this.CreateConfigButton.Text = "Создать конфигурацию";
            // 
            // ChooseConfigAndFileButton
            // 
            this.ChooseConfigAndFileButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChooseConfigAndFileButton.Appearance.Options.UseFont = true;
            this.ChooseConfigAndFileButton.Location = new System.Drawing.Point(140, 156);
            this.ChooseConfigAndFileButton.Name = "ChooseConfigAndFileButton";
            this.ChooseConfigAndFileButton.Size = new System.Drawing.Size(200, 29);
            this.ChooseConfigAndFileButton.TabIndex = 1;
            this.ChooseConfigAndFileButton.Text = "Импортировать файл";
            // 
            // ChangeConfigurationButton
            // 
            this.ChangeConfigurationButton.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChangeConfigurationButton.Appearance.Options.UseFont = true;
            this.ChangeConfigurationButton.Location = new System.Drawing.Point(140, 90);
            this.ChangeConfigurationButton.Name = "ChangeConfigurationButton";
            this.ChangeConfigurationButton.Size = new System.Drawing.Size(200, 29);
            this.ChangeConfigurationButton.TabIndex = 2;
            this.ChangeConfigurationButton.Text = "Изменить конфигурацию";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.ChangeConfigurationButton);
            this.Controls.Add(this.ChooseConfigAndFileButton);
            this.Controls.Add(this.CreateConfigButton);
            this.IconOptions.ShowIcon = false;
            this.LookAndFeel.SkinName = "Office 2013";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главное меню";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton CreateConfigButton;
        private DevExpress.XtraEditors.SimpleButton ChooseConfigAndFileButton;
        private DevExpress.XtraEditors.SimpleButton ChangeConfigurationButton;
    }
}

