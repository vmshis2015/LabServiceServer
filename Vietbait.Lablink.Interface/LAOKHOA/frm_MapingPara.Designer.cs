namespace Vietbait.Lablink.Interface.LAOKHOA
{
    partial class frm_MapingPara
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_MapingPara));
            Janus.Windows.GridEX.GridEXLayout grdLIS_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem2 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem3 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.GridEX.GridEXLayout grdHIS_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.cmdSave = new Janus.Windows.EditControls.UIButton();
            this.cmdExit = new Janus.Windows.EditControls.UIButton();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox4 = new Janus.Windows.EditControls.UIGroupBox();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.grdLIS = new Janus.Windows.GridEX.GridEX();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTrangThai = new Janus.Windows.EditControls.UIComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAlias_Name = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cboThietBi = new Janus.Windows.EditControls.UIComboBox();
            this.cmdGetData = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiTabPage3 = new Janus.Windows.UI.Tab.UITabPage();
            this.grdHIS = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).BeginInit();
            this.uiGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLIS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHIS)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.cmdSave);
            this.uiGroupBox2.Controls.Add(this.cmdExit);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox2.Image = ((System.Drawing.Image)(resources.GetObject("uiGroupBox2.Image")));
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 576);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(907, 50);
            this.uiGroupBox2.TabIndex = 2;
            this.uiGroupBox2.Text = "&Chức năng";
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Image = ((System.Drawing.Image)(resources.GetObject("cmdSave.Image")));
            this.cmdSave.Location = new System.Drawing.Point(365, 18);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(125, 26);
            this.cmdSave.TabIndex = 1;
            this.cmdSave.Text = "&Lưu lại(Ctrl+S)";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExit.Image = ((System.Drawing.Image)(resources.GetObject("cmdExit.Image")));
            this.cmdExit.Location = new System.Drawing.Point(496, 18);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(125, 26);
            this.cmdExit.TabIndex = 0;
            this.cmdExit.Text = "&Thoát(Esc)";
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // uiTab1
            // 
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.Location = new System.Drawing.Point(0, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(907, 576);
            this.uiTab1.TabIndex = 3;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2,
            this.uiTabPage3});
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.VS2005;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.uiGroupBox4);
            this.uiTabPage1.Controls.Add(this.uiGroupBox3);
            this.uiTabPage1.Controls.Add(this.uiGroupBox1);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(905, 554);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "Thông số map chi tiết";
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.grdHIS);
            this.uiGroupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiGroupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox4.Location = new System.Drawing.Point(419, 93);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Size = new System.Drawing.Size(486, 461);
            this.uiGroupBox4.TabIndex = 7;
            this.uiGroupBox4.Text = "&Danh sách thông số HIS";
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.grdLIS);
            this.uiGroupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.uiGroupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox3.Location = new System.Drawing.Point(0, 93);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(419, 461);
            this.uiGroupBox3.TabIndex = 6;
            this.uiGroupBox3.Text = "&Danh sách thông số LIS";
            // 
            // grdLIS
            // 
            grdLIS_DesignTimeLayout.LayoutString = resources.GetString("grdLIS_DesignTimeLayout.LayoutString");
            this.grdLIS.DesignTimeLayout = grdLIS_DesignTimeLayout;
            this.grdLIS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLIS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdLIS.GroupByBoxVisible = false;
            this.grdLIS.Location = new System.Drawing.Point(3, 17);
            this.grdLIS.Name = "grdLIS";
            this.grdLIS.Size = new System.Drawing.Size(413, 441);
            this.grdLIS.TabIndex = 0;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.cboTrangThai);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.txtAlias_Name);
            this.uiGroupBox1.Controls.Add(this.cboThietBi);
            this.uiGroupBox1.Controls.Add(this.cmdGetData);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(905, 93);
            this.uiGroupBox1.TabIndex = 5;
            this.uiGroupBox1.Text = "&Thông tin tìm kiếm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(351, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Trạng thái";
            // 
            // cboTrangThai
            // 
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "Tất cả";
            uiComboBoxItem1.Value = -1;
            uiComboBoxItem2.FormatStyle.Alpha = 0;
            uiComboBoxItem2.IsSeparator = false;
            uiComboBoxItem2.Text = "Đã Map";
            uiComboBoxItem2.Value = 1;
            uiComboBoxItem3.FormatStyle.Alpha = 0;
            uiComboBoxItem3.IsSeparator = false;
            uiComboBoxItem3.Text = "Chưa map";
            uiComboBoxItem3.Value = 0;
            this.cboTrangThai.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1,
            uiComboBoxItem2,
            uiComboBoxItem3});
            this.cboTrangThai.Location = new System.Drawing.Point(432, 26);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(158, 21);
            this.cboTrangThai.TabIndex = 8;
            this.cboTrangThai.Text = "trạng thái";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Alias_Name";
            // 
            // txtAlias_Name
            // 
            this.txtAlias_Name.Location = new System.Drawing.Point(102, 51);
            this.txtAlias_Name.Name = "txtAlias_Name";
            this.txtAlias_Name.Size = new System.Drawing.Size(215, 21);
            this.txtAlias_Name.TabIndex = 6;
            // 
            // cboThietBi
            // 
            this.cboThietBi.Location = new System.Drawing.Point(102, 24);
            this.cboThietBi.Name = "cboThietBi";
            this.cboThietBi.Size = new System.Drawing.Size(215, 21);
            this.cboThietBi.TabIndex = 5;
            this.cboThietBi.Text = "Thiết bị";
            // 
            // cmdGetData
            // 
            this.cmdGetData.Image = ((System.Drawing.Image)(resources.GetObject("cmdGetData.Image")));
            this.cmdGetData.Location = new System.Drawing.Point(650, 20);
            this.cmdGetData.Name = "cmdGetData";
            this.cmdGetData.Size = new System.Drawing.Size(143, 48);
            this.cmdGetData.TabIndex = 4;
            this.cmdGetData.Text = "&Lấy thông tin (F3)";
            this.cmdGetData.Click += new System.EventHandler(this.cmdGetData_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Thiết bị";
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(819, 463);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "Thông số map loại xét nghiệm";
            // 
            // uiTabPage3
            // 
            this.uiTabPage3.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage3.Name = "uiTabPage3";
            this.uiTabPage3.Size = new System.Drawing.Size(819, 463);
            this.uiTabPage3.TabStop = true;
            this.uiTabPage3.Text = "Thông số map đối tượng";
            // 
            // grdHIS
            // 
            this.grdHIS.ColumnAutoResize = true;
            grdHIS_DesignTimeLayout.LayoutString = resources.GetString("grdHIS_DesignTimeLayout.LayoutString");
            this.grdHIS.DesignTimeLayout = grdHIS_DesignTimeLayout;
            this.grdHIS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdHIS.GroupByBoxVisible = false;
            this.grdHIS.Location = new System.Drawing.Point(3, 17);
            this.grdHIS.Name = "grdHIS";
            this.grdHIS.Size = new System.Drawing.Size(480, 441);
            this.grdHIS.TabIndex = 1;
            this.grdHIS.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdLIS_FormattingRow);
            // 
            // frm_MapingPara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 626);
            this.Controls.Add(this.uiTab1);
            this.Controls.Add(this.uiGroupBox2);
            this.Name = "frm_MapingPara";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin map thông số 2 hệ thống HIS-LIS";
            this.Load += new System.EventHandler(this.frm_MapingPara_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_MapingPara_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).EndInit();
            this.uiGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdLIS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHIS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private Janus.Windows.EditControls.UIButton cmdSave;
        private Janus.Windows.EditControls.UIButton cmdExit;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox4;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private Janus.Windows.GridEX.GridEX grdLIS;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.EditControls.UIComboBox cboTrangThai;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.EditBox txtAlias_Name;
        private Janus.Windows.EditControls.UIComboBox cboThietBi;
        private Janus.Windows.EditControls.UIButton cmdGetData;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage3;
        private Janus.Windows.GridEX.GridEX grdHIS;
    }
}