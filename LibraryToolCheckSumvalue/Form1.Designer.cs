namespace LibraryToolCheckSumvalue
{
    partial class Form1
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
            this.btnChecksum = new System.Windows.Forms.Button();
            this.txtChecksum = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnChecksum
            // 
            this.btnChecksum.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnChecksum.Location = new System.Drawing.Point(0, 229);
            this.btnChecksum.Name = "btnChecksum";
            this.btnChecksum.Size = new System.Drawing.Size(572, 38);
            this.btnChecksum.TabIndex = 0;
            this.btnChecksum.Text = "Get CheckSum Value";
            this.btnChecksum.UseVisualStyleBackColor = true;
            this.btnChecksum.Click += new System.EventHandler(this.btnChecksum_Click);
            // 
            // txtChecksum
            // 
            this.txtChecksum.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtChecksum.Location = new System.Drawing.Point(0, 0);
            this.txtChecksum.Multiline = true;
            this.txtChecksum.Name = "txtChecksum";
            this.txtChecksum.Size = new System.Drawing.Size(572, 138);
            this.txtChecksum.TabIndex = 1;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtResult.Location = new System.Drawing.Point(0, 138);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(572, 90);
            this.txtResult.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 267);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnChecksum);
            this.Controls.Add(this.txtChecksum);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnChecksum;
        private System.Windows.Forms.TextBox txtChecksum;
        private System.Windows.Forms.TextBox txtResult;
    }
}

