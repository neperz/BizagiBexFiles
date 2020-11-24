
namespace BexFileRead.Util
{
    partial class frmProtect
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
            this.txtIn = new System.Windows.Forms.TextBox();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtIn
            // 
            this.txtIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIn.Location = new System.Drawing.Point(12, 21);
            this.txtIn.Multiline = true;
            this.txtIn.Name = "txtIn";
            this.txtIn.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtIn.Size = new System.Drawing.Size(669, 105);
            this.txtIn.TabIndex = 0;
            // 
            // txtOut
            // 
            this.txtOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOut.Location = new System.Drawing.Point(12, 182);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOut.Size = new System.Drawing.Size(669, 105);
            this.txtOut.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 136);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cript";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(361, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 34);
            this.button2.TabIndex = 3;
            this.button2.Text = "Decript";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmProtect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 306);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.txtIn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmProtect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Protect info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIn;
        private System.Windows.Forms.TextBox txtOut;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}