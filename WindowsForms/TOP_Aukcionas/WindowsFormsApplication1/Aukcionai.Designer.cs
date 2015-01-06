namespace Aukcionas
{
    partial class Aukcionai
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.pavadinimas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kaina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.likeslaikas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pavadinimas,
            this.kaina,
            this.likeslaikas});
            this.dataGridView1.Location = new System.Drawing.Point(6, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(548, 388);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(479, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Kurti naują";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pavadinimas
            // 
            this.pavadinimas.HeaderText = "Pavadinimas";
            this.pavadinimas.Name = "pavadinimas";
            this.pavadinimas.ReadOnly = true;
            this.pavadinimas.Width = 300;
            // 
            // kaina
            // 
            this.kaina.HeaderText = "Dabartinė kaina";
            this.kaina.Name = "kaina";
            this.kaina.ReadOnly = true;
            // 
            // likeslaikas
            // 
            this.likeslaikas.HeaderText = "Likęs Laikas";
            this.likeslaikas.Name = "likeslaikas";
            this.likeslaikas.ReadOnly = true;
            // 
            // Aukcionai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 433);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Aukcionai";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Aukcionai_FormClosing);
            this.Load += new System.EventHandler(this.Aukcionai_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pavadinimas;
        private System.Windows.Forms.DataGridViewTextBoxColumn kaina;
        private System.Windows.Forms.DataGridViewTextBoxColumn likeslaikas;
    }
}