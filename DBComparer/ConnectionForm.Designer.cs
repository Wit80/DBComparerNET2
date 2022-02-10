namespace DBComparer
{
    partial class ConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cbDatabase2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPassword2 = new System.Windows.Forms.TextBox();
            this.lPassword2 = new System.Windows.Forms.Label();
            this.tbLoginName2 = new System.Windows.Forms.TextBox();
            this.lLoginName2 = new System.Windows.Forms.Label();
            this.cbAutent2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbServer2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbDatabase1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbPassword1 = new System.Windows.Forms.TextBox();
            this.lPassword1 = new System.Windows.Forms.Label();
            this.tbLoginName1 = new System.Windows.Forms.TextBox();
            this.lLoginName1 = new System.Windows.Forms.Label();
            this.cbAutent1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbServer1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.butCompare = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(987, 503);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pictureBox2);
            this.groupBox3.Controls.Add(this.cbDatabase2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbPassword2);
            this.groupBox3.Controls.Add(this.lPassword2);
            this.groupBox3.Controls.Add(this.tbLoginName2);
            this.groupBox3.Controls.Add(this.lLoginName2);
            this.groupBox3.Controls.Add(this.cbAutent2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.tbServer2);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(673, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 186);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source2";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DBComparer.Properties.Resources.progress;
            this.pictureBox2.Location = new System.Drawing.Point(46, 147);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 33);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Visible = false;
            // 
            // cbDatabase2
            // 
            this.cbDatabase2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabase2.FormattingEnabled = true;
            this.cbDatabase2.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cbDatabase2.Location = new System.Drawing.Point(98, 127);
            this.cbDatabase2.Name = "cbDatabase2";
            this.cbDatabase2.Size = new System.Drawing.Size(185, 21);
            this.cbDatabase2.TabIndex = 9;
            this.cbDatabase2.DropDown += new System.EventHandler(this.cbDatabase2_DropDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Database";
            // 
            // tbPassword2
            // 
            this.tbPassword2.Location = new System.Drawing.Point(98, 101);
            this.tbPassword2.Name = "tbPassword2";
            this.tbPassword2.Size = new System.Drawing.Size(185, 20);
            this.tbPassword2.TabIndex = 7;
            this.tbPassword2.Visible = false;
            this.tbPassword2.TextChanged += new System.EventHandler(this.tbPassword2_TextChanged);
            // 
            // lPassword2
            // 
            this.lPassword2.AutoSize = true;
            this.lPassword2.Location = new System.Drawing.Point(6, 104);
            this.lPassword2.Name = "lPassword2";
            this.lPassword2.Size = new System.Drawing.Size(53, 13);
            this.lPassword2.TabIndex = 6;
            this.lPassword2.Text = "Password";
            this.lPassword2.Visible = false;
            // 
            // tbLoginName2
            // 
            this.tbLoginName2.Location = new System.Drawing.Point(98, 77);
            this.tbLoginName2.Name = "tbLoginName2";
            this.tbLoginName2.Size = new System.Drawing.Size(185, 20);
            this.tbLoginName2.TabIndex = 5;
            this.tbLoginName2.Visible = false;
            this.tbLoginName2.TextChanged += new System.EventHandler(this.tbLoginName2_TextChanged);
            // 
            // lLoginName2
            // 
            this.lLoginName2.AutoSize = true;
            this.lLoginName2.Location = new System.Drawing.Point(6, 80);
            this.lLoginName2.Name = "lLoginName2";
            this.lLoginName2.Size = new System.Drawing.Size(62, 13);
            this.lLoginName2.TabIndex = 4;
            this.lLoginName2.Text = "Login name";
            this.lLoginName2.Visible = false;
            // 
            // cbAutent2
            // 
            this.cbAutent2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutent2.FormattingEnabled = true;
            this.cbAutent2.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cbAutent2.Location = new System.Drawing.Point(98, 50);
            this.cbAutent2.Name = "cbAutent2";
            this.cbAutent2.Size = new System.Drawing.Size(185, 21);
            this.cbAutent2.TabIndex = 3;
            this.cbAutent2.SelectedIndexChanged += new System.EventHandler(this.cbAutent2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Authentication";
            // 
            // tbServer2
            // 
            this.tbServer2.Location = new System.Drawing.Point(98, 23);
            this.tbServer2.Name = "tbServer2";
            this.tbServer2.Size = new System.Drawing.Size(185, 20);
            this.tbServer2.TabIndex = 1;
            this.tbServer2.TextChanged += new System.EventHandler(this.tbServer2_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Server";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.cbDatabase1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbPassword1);
            this.groupBox2.Controls.Add(this.lPassword1);
            this.groupBox2.Controls.Add(this.tbLoginName1);
            this.groupBox2.Controls.Add(this.lLoginName1);
            this.groupBox2.Controls.Add(this.cbAutent1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tbServer1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 186);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Source1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DBComparer.Properties.Resources.progress;
            this.pictureBox1.Location = new System.Drawing.Point(46, 147);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 33);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // cbDatabase1
            // 
            this.cbDatabase1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabase1.FormattingEnabled = true;
            this.cbDatabase1.Location = new System.Drawing.Point(98, 127);
            this.cbDatabase1.Name = "cbDatabase1";
            this.cbDatabase1.Size = new System.Drawing.Size(185, 21);
            this.cbDatabase1.TabIndex = 9;
            this.cbDatabase1.DropDown += new System.EventHandler(this.cbDatabase1_DropDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Database";
            // 
            // tbPassword1
            // 
            this.tbPassword1.Location = new System.Drawing.Point(98, 101);
            this.tbPassword1.Name = "tbPassword1";
            this.tbPassword1.PasswordChar = '*';
            this.tbPassword1.Size = new System.Drawing.Size(185, 20);
            this.tbPassword1.TabIndex = 7;
            this.tbPassword1.Visible = false;
            this.tbPassword1.TextChanged += new System.EventHandler(this.tbPassword1_TextChanged);
            // 
            // lPassword1
            // 
            this.lPassword1.AutoSize = true;
            this.lPassword1.Location = new System.Drawing.Point(6, 104);
            this.lPassword1.Name = "lPassword1";
            this.lPassword1.Size = new System.Drawing.Size(53, 13);
            this.lPassword1.TabIndex = 6;
            this.lPassword1.Text = "Password";
            this.lPassword1.Visible = false;
            // 
            // tbLoginName1
            // 
            this.tbLoginName1.Location = new System.Drawing.Point(98, 77);
            this.tbLoginName1.Name = "tbLoginName1";
            this.tbLoginName1.Size = new System.Drawing.Size(185, 20);
            this.tbLoginName1.TabIndex = 5;
            this.tbLoginName1.Visible = false;
            this.tbLoginName1.TextChanged += new System.EventHandler(this.tbLoginName1_TextChanged);
            // 
            // lLoginName1
            // 
            this.lLoginName1.AutoSize = true;
            this.lLoginName1.Location = new System.Drawing.Point(6, 80);
            this.lLoginName1.Name = "lLoginName1";
            this.lLoginName1.Size = new System.Drawing.Size(62, 13);
            this.lLoginName1.TabIndex = 4;
            this.lLoginName1.Text = "Login name";
            this.lLoginName1.Visible = false;
            // 
            // cbAutent1
            // 
            this.cbAutent1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutent1.FormattingEnabled = true;
            this.cbAutent1.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cbAutent1.Location = new System.Drawing.Point(98, 50);
            this.cbAutent1.Name = "cbAutent1";
            this.cbAutent1.Size = new System.Drawing.Size(185, 21);
            this.cbAutent1.TabIndex = 3;
            this.cbAutent1.SelectedIndexChanged += new System.EventHandler(this.cbAutent1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Authentication";
            // 
            // tbServer1
            // 
            this.tbServer1.Location = new System.Drawing.Point(98, 23);
            this.tbServer1.Name = "tbServer1";
            this.tbServer1.Size = new System.Drawing.Size(185, 20);
            this.tbServer1.TabIndex = 1;
            this.tbServer1.TextChanged += new System.EventHandler(this.tbServer1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // butCompare
            // 
            this.butCompare.Location = new System.Drawing.Point(270, 229);
            this.butCompare.Name = "butCompare";
            this.butCompare.Size = new System.Drawing.Size(109, 23);
            this.butCompare.TabIndex = 1;
            this.butCompare.Text = "Сравнить";
            this.butCompare.UseVisualStyleBackColor = true;
            this.butCompare.Click += new System.EventHandler(this.butCompare_Click);
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 566);
            this.Controls.Add(this.butCompare);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(688, 313);
            this.Name = "ConnectionForm";
            this.Text = "Новая схема сравнения БД";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbAutent1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbServer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPassword1;
        private System.Windows.Forms.Label lPassword1;
        private System.Windows.Forms.TextBox tbLoginName1;
        private System.Windows.Forms.Label lLoginName1;
        private System.Windows.Forms.ComboBox cbDatabase1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbDatabase2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPassword2;
        private System.Windows.Forms.Label lPassword2;
        private System.Windows.Forms.TextBox tbLoginName2;
        private System.Windows.Forms.Label lLoginName2;
        private System.Windows.Forms.ComboBox cbAutent2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbServer2;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button butCompare;
    }
}