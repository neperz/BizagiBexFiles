namespace BexFileRead
{
    partial class frmBexFileRead
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
            this.button1 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listView1 = new System.Windows.Forms.ListView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.txtGuid = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblObject = new System.Windows.Forms.Label();
            this.jsonExplorer = new System.Windows.Forms.TreeView();
            this.cmbTabelas = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load .bex";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(282, 27);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(1);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(560, 575);
            this.propertyGrid1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(19, 188);
            this.listView1.Margin = new System.Windows.Forms.Padding(1);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(251, 533);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(19, 57);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(1);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(251, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(282, 604);
            this.txtContent.Margin = new System.Windows.Forms.Padding(1);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(558, 117);
            this.txtContent.TabIndex = 6;
            // 
            // txtGuid
            // 
            this.txtGuid.Location = new System.Drawing.Point(19, 128);
            this.txtGuid.Margin = new System.Windows.Forms.Padding(1);
            this.txtGuid.Name = "txtGuid";
            this.txtGuid.Size = new System.Drawing.Size(251, 20);
            this.txtGuid.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 150);
            this.button2.Margin = new System.Windows.Forms.Padding(1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 24);
            this.button2.TabIndex = 8;
            this.button2.Text = "By Guid";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 150);
            this.button3.Margin = new System.Windows.Forms.Padding(1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 24);
            this.button3.TabIndex = 9;
            this.button3.Text = "By Parent";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(190, 150);
            this.button4.Margin = new System.Windows.Forms.Padding(1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 24);
            this.button4.TabIndex = 10;
            this.button4.Text = "Disableds";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 43);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = ".bex xml files (select Object Catalog)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Guid or Parent Guid to Find";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(22, 83);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(13, 13);
            this.linkLabel1.TabIndex = 15;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "..";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(206, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "All Type";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Location = new System.Drawing.Point(279, 9);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(38, 13);
            this.lblObject.TabIndex = 17;
            this.lblObject.Text = "Object";
            // 
            // jsonExplorer
            // 
            this.jsonExplorer.Location = new System.Drawing.Point(844, 54);
            this.jsonExplorer.Name = "jsonExplorer";
            this.jsonExplorer.Size = new System.Drawing.Size(266, 663);
            this.jsonExplorer.TabIndex = 18;
            this.jsonExplorer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.jsonExplorer_KeyUp);
            // 
            // cmbTabelas
            // 
            this.cmbTabelas.FormattingEnabled = true;
            this.cmbTabelas.Location = new System.Drawing.Point(421, 6);
            this.cmbTabelas.Name = "cmbTabelas";
            this.cmbTabelas.Size = new System.Drawing.Size(421, 21);
            this.cmbTabelas.TabIndex = 19;
            this.cmbTabelas.SelectedIndexChanged += new System.EventHandler(this.cmbTabelas_SelectedIndexChanged);
            // 
            // frmBexFileRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 796);
            this.Controls.Add(this.cmbTabelas);
            this.Controls.Add(this.jsonExplorer);
            this.Controls.Add(this.lblObject);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtGuid);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "frmBexFileRead";
            this.Text = "Bex FIle Util";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.TextBox txtGuid;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblObject;
        private System.Windows.Forms.TreeView jsonExplorer;
        private System.Windows.Forms.ComboBox cmbTabelas;
    }
}

