
namespace DemosPlus
{
    partial class MainWindow
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
            this.btnRun = new System.Windows.Forms.Button();
            this.dropDuration = new System.Windows.Forms.ComboBox();
            this.txtDuration = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dropItem = new System.Windows.Forms.ComboBox();
            this.txtTax = new System.Windows.Forms.Label();
            this.dropTax = new System.Windows.Forms.ComboBox();
            this.txtReturn = new System.Windows.Forms.Label();
            this.inputReturn = new System.Windows.Forms.TextBox();
            this.dumpView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(212, 192);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "开始计算";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.OnClickCalculate);
            // 
            // dropDuration
            // 
            this.dropDuration.FormattingEnabled = true;
            this.dropDuration.Location = new System.Drawing.Point(166, 32);
            this.dropDuration.Name = "dropDuration";
            this.dropDuration.Size = new System.Drawing.Size(121, 20);
            this.dropDuration.TabIndex = 2;
            // 
            // txtDuration
            // 
            this.txtDuration.AutoSize = true;
            this.txtDuration.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDuration.Location = new System.Drawing.Point(25, 32);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(69, 20);
            this.txtDuration.TabIndex = 3;
            this.txtDuration.Text = "计算周期";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(25, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "物品类型";
            // 
            // dropItem
            // 
            this.dropItem.FormattingEnabled = true;
            this.dropItem.Location = new System.Drawing.Point(166, 144);
            this.dropItem.Name = "dropItem";
            this.dropItem.Size = new System.Drawing.Size(121, 20);
            this.dropItem.TabIndex = 6;
            // 
            // txtTax
            // 
            this.txtTax.AutoSize = true;
            this.txtTax.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTax.Location = new System.Drawing.Point(25, 108);
            this.txtTax.Name = "txtTax";
            this.txtTax.Size = new System.Drawing.Size(69, 20);
            this.txtTax.TabIndex = 9;
            this.txtTax.Text = "选择税率";
            // 
            // dropTax
            // 
            this.dropTax.FormattingEnabled = true;
            this.dropTax.Location = new System.Drawing.Point(166, 108);
            this.dropTax.Name = "dropTax";
            this.dropTax.Size = new System.Drawing.Size(121, 20);
            this.dropTax.TabIndex = 8;
            // 
            // txtReturn
            // 
            this.txtReturn.AutoSize = true;
            this.txtReturn.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReturn.Location = new System.Drawing.Point(25, 70);
            this.txtReturn.Name = "txtReturn";
            this.txtReturn.Size = new System.Drawing.Size(84, 20);
            this.txtReturn.TabIndex = 11;
            this.txtReturn.Text = "输入返还率";
            // 
            // inputReturn
            // 
            this.inputReturn.Location = new System.Drawing.Point(166, 70);
            this.inputReturn.Name = "inputReturn";
            this.inputReturn.Size = new System.Drawing.Size(121, 21);
            this.inputReturn.TabIndex = 12;
            // 
            // dumpView
            // 
            this.dumpView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dumpView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dumpView.Location = new System.Drawing.Point(332, 32);
            this.dumpView.Name = "dumpView";
            this.dumpView.RowTemplate.Height = 23;
            this.dumpView.Size = new System.Drawing.Size(982, 600);
            this.dumpView.TabIndex = 13;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 644);
            this.Controls.Add(this.dumpView);
            this.Controls.Add(this.inputReturn);
            this.Controls.Add(this.txtReturn);
            this.Controls.Add(this.txtTax);
            this.Controls.Add(this.dropTax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dropItem);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.dropDuration);
            this.Controls.Add(this.btnRun);
            this.Name = "MainWindow";
            this.Text = "主页";
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox dropDuration;
        private System.Windows.Forms.Label txtDuration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox dropItem;
        private System.Windows.Forms.Label txtTax;
        private System.Windows.Forms.ComboBox dropTax;
        private System.Windows.Forms.Label txtReturn;
        private System.Windows.Forms.TextBox inputReturn;
        private System.Windows.Forms.DataGridView dumpView;
    }
}

