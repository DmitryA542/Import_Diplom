namespace MainForm
{
    partial class AcceptDeleteDialog
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.YesButton = new DevExpress.XtraEditors.SimpleButton();
            this.NoButton = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(272, 44);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Вы уверены, что хотите удалить эту \r\nконфигурацию?";
            // 
            // YesButton
            // 
            this.YesButton.Location = new System.Drawing.Point(43, 78);
            this.YesButton.Name = "YesButton";
            this.YesButton.Size = new System.Drawing.Size(94, 29);
            this.YesButton.TabIndex = 1;
            this.YesButton.Text = "Да";
            // 
            // NoButton
            // 
            this.NoButton.Location = new System.Drawing.Point(161, 78);
            this.NoButton.Name = "NoButton";
            this.NoButton.Size = new System.Drawing.Size(94, 29);
            this.NoButton.TabIndex = 2;
            this.NoButton.Text = "Нет";
            // 
            // AcceptDeleteDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 119);
            this.Controls.Add(this.NoButton);
            this.Controls.Add(this.YesButton);
            this.Controls.Add(this.labelControl1);
            this.IconOptions.ShowIcon = false;
            this.LookAndFeel.SkinName = "Office 2013";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "AcceptDeleteDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Удаление";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton YesButton;
        private DevExpress.XtraEditors.SimpleButton NoButton;
    }
}