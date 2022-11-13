namespace MA_Crawler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textB_link = new System.Windows.Forms.TextBox();
            this.textB_search = new System.Windows.Forms.TextBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.treeV_crawl = new System.Windows.Forms.TreeView();
            this.dataGV_crawl = new System.Windows.Forms.DataGridView();
            this.col_Document = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_clear = new System.Windows.Forms.Button();
            this.label_foundCnt = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV_crawl)).BeginInit();
            this.SuspendLayout();
            // 
            // textB_link
            // 
            this.textB_link.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textB_link.Location = new System.Drawing.Point(12, 59);
            this.textB_link.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textB_link.Name = "textB_link";
            this.textB_link.ReadOnly = true;
            this.textB_link.Size = new System.Drawing.Size(504, 22);
            this.textB_link.TabIndex = 0;
            this.textB_link.TabStop = false;
            this.textB_link.Text = "http://crawlertest.cs.tu-varna.bg";
            // 
            // textB_search
            // 
            this.textB_search.Location = new System.Drawing.Point(555, 59);
            this.textB_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textB_search.Name = "textB_search";
            this.textB_search.ShortcutsEnabled = false;
            this.textB_search.Size = new System.Drawing.Size(436, 22);
            this.textB_search.TabIndex = 1;
            this.textB_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textB_search_KeyPress);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(12, 12);
            this.btn_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(104, 41);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(555, 12);
            this.btn_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(104, 41);
            this.btn_search.TabIndex = 3;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // treeV_crawl
            // 
            this.treeV_crawl.Location = new System.Drawing.Point(12, 87);
            this.treeV_crawl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeV_crawl.Name = "treeV_crawl";
            this.treeV_crawl.Size = new System.Drawing.Size(504, 400);
            this.treeV_crawl.TabIndex = 4;
            // 
            // dataGV_crawl
            // 
            this.dataGV_crawl.AllowUserToAddRows = false;
            this.dataGV_crawl.AllowUserToDeleteRows = false;
            this.dataGV_crawl.AllowUserToResizeColumns = false;
            this.dataGV_crawl.AllowUserToResizeRows = false;
            this.dataGV_crawl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGV_crawl.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGV_crawl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGV_crawl.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_Document});
            this.dataGV_crawl.Location = new System.Drawing.Point(555, 87);
            this.dataGV_crawl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGV_crawl.MultiSelect = false;
            this.dataGV_crawl.Name = "dataGV_crawl";
            this.dataGV_crawl.ReadOnly = true;
            this.dataGV_crawl.RowHeadersVisible = false;
            this.dataGV_crawl.RowHeadersWidth = 51;
            this.dataGV_crawl.RowTemplate.Height = 24;
            this.dataGV_crawl.Size = new System.Drawing.Size(583, 400);
            this.dataGV_crawl.TabIndex = 5;
            // 
            // col_Document
            // 
            this.col_Document.HeaderText = "Document";
            this.col_Document.MinimumWidth = 6;
            this.col_Document.Name = "col_Document";
            this.col_Document.ReadOnly = true;
            this.col_Document.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1143, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 242);
            this.label1.TabIndex = 6;
            this.label1.Text = "Legend:\r\n\r\nC - count\r\n\r\nW - weight\r\n\r\nP - position";
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(1033, 14);
            this.btn_clear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(104, 41);
            this.btn_clear.TabIndex = 7;
            this.btn_clear.Text = "Clear List";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // label_foundCnt
            // 
            this.label_foundCnt.Location = new System.Drawing.Point(997, 59);
            this.label_foundCnt.Name = "label_foundCnt";
            this.label_foundCnt.Size = new System.Drawing.Size(141, 22);
            this.label_foundCnt.TabIndex = 8;
            this.label_foundCnt.Text = "Found 0/0";
            this.label_foundCnt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1293, 498);
            this.Controls.Add(this.label_foundCnt);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGV_crawl);
            this.Controls.Add(this.treeV_crawl);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.textB_search);
            this.Controls.Add(this.textB_link);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web Crawler";
            ((System.ComponentModel.ISupportInitialize)(this.dataGV_crawl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textB_link;
        private System.Windows.Forms.TextBox textB_search;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TreeView treeV_crawl;
        private System.Windows.Forms.DataGridView dataGV_crawl;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Document;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Label label_foundCnt;
    }
}

