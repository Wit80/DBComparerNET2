namespace DBComparer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsDBInfo = new System.Windows.Forms.ToolStrip();
            this.tsDB1Info = new System.Windows.Forms.ToolStripLabel();
            this.tsDB2Info = new System.Windows.Forms.ToolStripLabel();
            this.tsProcessImage1 = new System.Windows.Forms.ToolStripLabel();
            this.tsProcessImage2 = new System.Windows.Forms.ToolStripLabel();
            this.tsLabelEquals = new System.Windows.Forms.ToolStripLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.labColorDifferent = new System.Windows.Forms.Label();
            this.labColorEpson = new System.Windows.Forms.Label();
            this.labColorEqual = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpInfo = new System.Windows.Forms.TabPage();
            this.tpSQL = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tsDBInfo.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpInfo.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1193, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileMenu,
            this.openFileMenu,
            this.saveFileMenu,
            this.closeFileMenu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newFileMenu
            // 
            this.newFileMenu.Name = "newFileMenu";
            this.newFileMenu.Size = new System.Drawing.Size(103, 22);
            this.newFileMenu.Text = "New";
            this.newFileMenu.Click += new System.EventHandler(this.newFileMenu_Click);
            // 
            // openFileMenu
            // 
            this.openFileMenu.Name = "openFileMenu";
            this.openFileMenu.Size = new System.Drawing.Size(103, 22);
            this.openFileMenu.Text = "Open";
            this.openFileMenu.Click += new System.EventHandler(this.openFileMenu_Click);
            // 
            // saveFileMenu
            // 
            this.saveFileMenu.Name = "saveFileMenu";
            this.saveFileMenu.Size = new System.Drawing.Size(103, 22);
            this.saveFileMenu.Text = "Save";
            // 
            // closeFileMenu
            // 
            this.closeFileMenu.Name = "closeFileMenu";
            this.closeFileMenu.Size = new System.Drawing.Size(103, 22);
            this.closeFileMenu.Text = "Close";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.findToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsOpen});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1193, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsNew
            // 
            this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(35, 22);
            this.tsNew.Text = "New";
            this.tsNew.Click += new System.EventHandler(this.newFileMenu_Click);
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsOpen.Image")));
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(40, 22);
            this.tsOpen.Text = "Open";
            this.tsOpen.Click += new System.EventHandler(this.openFileMenu_Click);
            // 
            // tsDBInfo
            // 
            this.tsDBInfo.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tsDBInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDB1Info,
            this.tsDB2Info,
            this.tsProcessImage1,
            this.tsProcessImage2,
            this.tsLabelEquals});
            this.tsDBInfo.Location = new System.Drawing.Point(0, 49);
            this.tsDBInfo.Name = "tsDBInfo";
            this.tsDBInfo.Size = new System.Drawing.Size(1193, 25);
            this.tsDBInfo.TabIndex = 2;
            // 
            // tsDB1Info
            // 
            this.tsDB1Info.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsDB1Info.Name = "tsDB1Info";
            this.tsDB1Info.Size = new System.Drawing.Size(114, 22);
            this.tsDB1Info.Text = "toolStripLabel1";
            // 
            // tsDB2Info
            // 
            this.tsDB2Info.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsDB2Info.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsDB2Info.Name = "tsDB2Info";
            this.tsDB2Info.Size = new System.Drawing.Size(114, 22);
            this.tsDB2Info.Text = "toolStripLabel2";
            // 
            // tsProcessImage1
            // 
            this.tsProcessImage1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsProcessImage1.Image = global::DBComparer.Properties.Resources.progress;
            this.tsProcessImage1.Name = "tsProcessImage1";
            this.tsProcessImage1.Size = new System.Drawing.Size(16, 22);
            this.tsProcessImage1.Text = "toolStripLabel1";
            this.tsProcessImage1.Visible = false;
            // 
            // tsProcessImage2
            // 
            this.tsProcessImage2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsProcessImage2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsProcessImage2.Image = global::DBComparer.Properties.Resources.progress;
            this.tsProcessImage2.Name = "tsProcessImage2";
            this.tsProcessImage2.Size = new System.Drawing.Size(16, 22);
            this.tsProcessImage2.Text = "toolStripLabel1";
            this.tsProcessImage2.Visible = false;
            // 
            // tsLabelEquals
            // 
            this.tsLabelEquals.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsLabelEquals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsLabelEquals.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsLabelEquals.ForeColor = System.Drawing.Color.Brown;
            this.tsLabelEquals.Name = "tsLabelEquals";
            this.tsLabelEquals.Size = new System.Drawing.Size(22, 22);
            this.tsLabelEquals.Text = "бд";
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 74);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1193, 591);
            this.splitContainer1.SplitterDistance = 366;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(366, 524);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "data_96285.ico");
            this.imageList1.Images.SetKeyName(1, "img1.png");
            this.imageList1.Images.SetKeyName(2, "tabl1.png");
            this.imageList1.Images.SetKeyName(3, "index.png");
            this.imageList1.Images.SetKeyName(4, "column.png");
            this.imageList1.Images.SetKeyName(5, "view.png");
            this.imageList1.Images.SetKeyName(6, "constraint.png");
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.labColorDifferent);
            this.splitContainer2.Panel1.Controls.Add(this.labColorEpson);
            this.splitContainer2.Panel1.Controls.Add(this.labColorEqual);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.treeView1);
            this.splitContainer2.Size = new System.Drawing.Size(366, 591);
            this.splitContainer2.SplitterDistance = 63;
            this.splitContainer2.TabIndex = 0;
            // 
            // labColorDifferent
            // 
            this.labColorDifferent.AutoSize = true;
            this.labColorDifferent.Location = new System.Drawing.Point(12, 10);
            this.labColorDifferent.Name = "labColorDifferent";
            this.labColorDifferent.Size = new System.Drawing.Size(16, 13);
            this.labColorDifferent.TabIndex = 6;
            this.labColorDifferent.Text = "---";
            // 
            // labColorEpson
            // 
            this.labColorEpson.AutoSize = true;
            this.labColorEpson.Location = new System.Drawing.Point(12, 28);
            this.labColorEpson.Name = "labColorEpson";
            this.labColorEpson.Size = new System.Drawing.Size(16, 13);
            this.labColorEpson.TabIndex = 5;
            this.labColorEpson.Text = "---";
            // 
            // labColorEqual
            // 
            this.labColorEqual.AutoSize = true;
            this.labColorEqual.Location = new System.Drawing.Point(12, 45);
            this.labColorEqual.Name = "labColorEqual";
            this.labColorEqual.Size = new System.Drawing.Size(16, 13);
            this.labColorEqual.TabIndex = 4;
            this.labColorEqual.Text = "---";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpInfo);
            this.tabControl1.Controls.Add(this.tpSQL);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(823, 591);
            this.tabControl1.TabIndex = 0;
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.splitContainer3);
            this.tpInfo.Location = new System.Drawing.Point(4, 22);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpInfo.Size = new System.Drawing.Size(815, 565);
            this.tpInfo.TabIndex = 0;
            this.tpInfo.Text = "Информация";
            this.tpInfo.UseVisualStyleBackColor = true;
            // 
            // tpSQL
            // 
            this.tpSQL.Location = new System.Drawing.Point(4, 22);
            this.tpSQL.Name = "tpSQL";
            this.tpSQL.Padding = new System.Windows.Forms.Padding(3);
            this.tpSQL.Size = new System.Drawing.Size(815, 565);
            this.tpSQL.TabIndex = 1;
            this.tpSQL.Text = "SQL";
            this.tpSQL.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.labelInfo1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.labelInfo2);
            this.splitContainer3.Size = new System.Drawing.Size(809, 559);
            this.splitContainer3.SplitterDistance = 269;
            this.splitContainer3.TabIndex = 0;
            // 
            // labelInfo1
            // 
            this.labelInfo1.AutoSize = true;
            this.labelInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo1.Location = new System.Drawing.Point(0, 0);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(13, 13);
            this.labelInfo1.TabIndex = 0;
            this.labelInfo1.Text = "_";
            // 
            // labelInfo2
            // 
            this.labelInfo2.AutoSize = true;
            this.labelInfo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo2.Location = new System.Drawing.Point(0, 0);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(13, 13);
            this.labelInfo2.TabIndex = 0;
            this.labelInfo2.Text = "_";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 665);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tsDBInfo);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1209, 704);
            this.Name = "Form1";
            this.Text = "Сравнение двух баз данных SQL Server";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tsDBInfo.ResumeLayout(false);
            this.tsDBInfo.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpInfo.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileMenu;
        private System.Windows.Forms.ToolStripMenuItem openFileMenu;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenu;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStrip tsDBInfo;
        private System.Windows.Forms.ToolStripLabel tsDB1Info;
        private System.Windows.Forms.ToolStripLabel tsDB2Info;
        private System.Windows.Forms.ToolStripLabel tsProcessImage1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.ToolStripLabel tsProcessImage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripLabel tsLabelEquals;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label labColorDifferent;
        private System.Windows.Forms.Label labColorEpson;
        private System.Windows.Forms.Label labColorEqual;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpInfo;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabPage tpSQL;
        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.Label labelInfo2;
    }
}

