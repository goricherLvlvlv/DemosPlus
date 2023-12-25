
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
            this.resourceTab = new System.Windows.Forms.RadioButton();
            this.costTab = new System.Windows.Forms.RadioButton();
            this.profitTab = new System.Windows.Forms.RadioButton();
            this.artifactTab = new System.Windows.Forms.RadioButton();
            this.gearTab = new System.Windows.Forms.RadioButton();
            this.txtSaleMode = new System.Windows.Forms.Label();
            this.dropSaleMode = new System.Windows.Forms.ComboBox();
            this.txtItemName = new System.Windows.Forms.Label();
            this.westBtn = new System.Windows.Forms.RadioButton();
            this.eastBtn = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(188, 292);
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
            this.dropDuration.Location = new System.Drawing.Point(142, 126);
            this.dropDuration.Name = "dropDuration";
            this.dropDuration.Size = new System.Drawing.Size(121, 20);
            this.dropDuration.TabIndex = 2;
            // 
            // txtDuration
            // 
            this.txtDuration.AutoSize = true;
            this.txtDuration.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtDuration.Location = new System.Drawing.Point(34, 126);
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(69, 20);
            this.txtDuration.TabIndex = 3;
            this.txtDuration.Text = "计算周期";
            this.txtDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(34, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "物品类型";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dropItem
            // 
            this.dropItem.FormattingEnabled = true;
            this.dropItem.Location = new System.Drawing.Point(142, 156);
            this.dropItem.Name = "dropItem";
            this.dropItem.Size = new System.Drawing.Size(121, 20);
            this.dropItem.TabIndex = 6;
            this.dropItem.SelectedIndexChanged += new System.EventHandler(this.OnSelectItem);
            // 
            // txtTax
            // 
            this.txtTax.AutoSize = true;
            this.txtTax.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTax.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtTax.Location = new System.Drawing.Point(34, 186);
            this.txtTax.Name = "txtTax";
            this.txtTax.Size = new System.Drawing.Size(69, 20);
            this.txtTax.TabIndex = 9;
            this.txtTax.Text = "选择税率";
            this.txtTax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dropTax
            // 
            this.dropTax.FormattingEnabled = true;
            this.dropTax.Location = new System.Drawing.Point(142, 186);
            this.dropTax.Name = "dropTax";
            this.dropTax.Size = new System.Drawing.Size(121, 20);
            this.dropTax.TabIndex = 8;
            // 
            // txtReturn
            // 
            this.txtReturn.AutoSize = true;
            this.txtReturn.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtReturn.Location = new System.Drawing.Point(34, 246);
            this.txtReturn.Name = "txtReturn";
            this.txtReturn.Size = new System.Drawing.Size(84, 20);
            this.txtReturn.TabIndex = 11;
            this.txtReturn.Text = "输入返还率";
            this.txtReturn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inputReturn
            // 
            this.inputReturn.Location = new System.Drawing.Point(142, 246);
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
            // resourceTab
            // 
            this.resourceTab.AutoSize = true;
            this.resourceTab.Location = new System.Drawing.Point(38, 68);
            this.resourceTab.Name = "resourceTab";
            this.resourceTab.Size = new System.Drawing.Size(71, 16);
            this.resourceTab.TabIndex = 15;
            this.resourceTab.TabStop = true;
            this.resourceTab.Text = "材料价格";
            this.resourceTab.UseVisualStyleBackColor = true;
            this.resourceTab.CheckedChanged += new System.EventHandler(this.OnClickResourceTab);
            // 
            // costTab
            // 
            this.costTab.AutoSize = true;
            this.costTab.Location = new System.Drawing.Point(38, 90);
            this.costTab.Name = "costTab";
            this.costTab.Size = new System.Drawing.Size(71, 16);
            this.costTab.TabIndex = 16;
            this.costTab.TabStop = true;
            this.costTab.Text = "成本计算";
            this.costTab.UseVisualStyleBackColor = true;
            this.costTab.CheckedChanged += new System.EventHandler(this.OnClickCostTab);
            // 
            // profitTab
            // 
            this.profitTab.AutoSize = true;
            this.profitTab.Location = new System.Drawing.Point(115, 90);
            this.profitTab.Name = "profitTab";
            this.profitTab.Size = new System.Drawing.Size(71, 16);
            this.profitTab.TabIndex = 17;
            this.profitTab.TabStop = true;
            this.profitTab.Text = "利润计算";
            this.profitTab.UseVisualStyleBackColor = true;
            this.profitTab.CheckedChanged += new System.EventHandler(this.OnClickProfitTab);
            // 
            // artifactTab
            // 
            this.artifactTab.AutoSize = true;
            this.artifactTab.Location = new System.Drawing.Point(115, 68);
            this.artifactTab.Name = "artifactTab";
            this.artifactTab.Size = new System.Drawing.Size(71, 16);
            this.artifactTab.TabIndex = 18;
            this.artifactTab.TabStop = true;
            this.artifactTab.Text = "神器价格";
            this.artifactTab.UseVisualStyleBackColor = true;
            this.artifactTab.CheckedChanged += new System.EventHandler(this.OnClickArtifactTab);
            // 
            // gearTab
            // 
            this.gearTab.AutoSize = true;
            this.gearTab.Location = new System.Drawing.Point(192, 68);
            this.gearTab.Name = "gearTab";
            this.gearTab.Size = new System.Drawing.Size(71, 16);
            this.gearTab.TabIndex = 19;
            this.gearTab.TabStop = true;
            this.gearTab.Text = "装备价格";
            this.gearTab.UseVisualStyleBackColor = true;
            this.gearTab.CheckedChanged += new System.EventHandler(this.OnClickGearTab);
            // 
            // txtSaleMode
            // 
            this.txtSaleMode.AutoSize = true;
            this.txtSaleMode.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSaleMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtSaleMode.Location = new System.Drawing.Point(34, 216);
            this.txtSaleMode.Name = "txtSaleMode";
            this.txtSaleMode.Size = new System.Drawing.Size(69, 20);
            this.txtSaleMode.TabIndex = 21;
            this.txtSaleMode.Text = "出售方案";
            this.txtSaleMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dropSaleMode
            // 
            this.dropSaleMode.FormattingEnabled = true;
            this.dropSaleMode.Location = new System.Drawing.Point(142, 216);
            this.dropSaleMode.Name = "dropSaleMode";
            this.dropSaleMode.Size = new System.Drawing.Size(121, 20);
            this.dropSaleMode.TabIndex = 20;
            // 
            // txtItemName
            // 
            this.txtItemName.AutoSize = true;
            this.txtItemName.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtItemName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtItemName.Location = new System.Drawing.Point(12, 615);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(0, 20);
            this.txtItemName.TabIndex = 22;
            this.txtItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // westBtn
            // 
            this.westBtn.Location = new System.Drawing.Point(0, 3);
            this.westBtn.Name = "westBtn";
            this.westBtn.Size = new System.Drawing.Size(95, 16);
            this.westBtn.TabIndex = 0;
            this.westBtn.TabStop = true;
            this.westBtn.Text = "国际服";
            this.westBtn.UseVisualStyleBackColor = true;
            // 
            // eastBtn
            // 
            this.eastBtn.Location = new System.Drawing.Point(101, 3);
            this.eastBtn.Name = "eastBtn";
            this.eastBtn.Size = new System.Drawing.Size(95, 16);
            this.eastBtn.TabIndex = 0;
            this.eastBtn.TabStop = true;
            this.eastBtn.Text = "亚服";
            this.eastBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.eastBtn);
            this.panel1.Controls.Add(this.westBtn);
            this.panel1.Location = new System.Drawing.Point(38, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 21);
            this.panel1.TabIndex = 24;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 644);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.txtSaleMode);
            this.Controls.Add(this.dropSaleMode);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.dropDuration);
            this.Controls.Add(this.gearTab);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.artifactTab);
            this.Controls.Add(this.dropItem);
            this.Controls.Add(this.profitTab);
            this.Controls.Add(this.txtTax);
            this.Controls.Add(this.costTab);
            this.Controls.Add(this.dropTax);
            this.Controls.Add(this.resourceTab);
            this.Controls.Add(this.txtReturn);
            this.Controls.Add(this.inputReturn);
            this.Controls.Add(this.dumpView);
            this.Controls.Add(this.btnRun);
            this.Name = "MainWindow";
            this.Text = "主页";
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).EndInit();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.RadioButton resourceTab;
        private System.Windows.Forms.RadioButton costTab;
        private System.Windows.Forms.RadioButton profitTab;
        private System.Windows.Forms.RadioButton artifactTab;
        private System.Windows.Forms.RadioButton gearTab;
        private System.Windows.Forms.Label txtSaleMode;
        private System.Windows.Forms.ComboBox dropSaleMode;
        private System.Windows.Forms.Label txtItemName;
        private System.Windows.Forms.RadioButton westBtn;
        private System.Windows.Forms.RadioButton eastBtn;
        private System.Windows.Forms.Panel panel1;
    }
}

