namespace Kompiuteriuparduotuve.UI
{
    partial class DetalusElementas
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.StaticPavadinimasLabel = new System.Windows.Forms.Label();
            this.staticaprasymaslabel = new System.Windows.Forms.Label();
            this.Aprasymas = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.staticKainalabel = new System.Windows.Forms.Label();
            this.Kaina = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.kainaLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(339, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 234);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // StaticPavadinimasLabel
            // 
            this.StaticPavadinimasLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StaticPavadinimasLabel.Location = new System.Drawing.Point(9, 23);
            this.StaticPavadinimasLabel.Name = "StaticPavadinimasLabel";
            this.StaticPavadinimasLabel.Size = new System.Drawing.Size(117, 27);
            this.StaticPavadinimasLabel.TabIndex = 1;
            this.StaticPavadinimasLabel.Text = "Pavadinimas";
            // 
            // staticaprasymaslabel
            // 
            this.staticaprasymaslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.staticaprasymaslabel.Location = new System.Drawing.Point(9, 50);
            this.staticaprasymaslabel.Name = "staticaprasymaslabel";
            this.staticaprasymaslabel.Size = new System.Drawing.Size(117, 27);
            this.staticaprasymaslabel.TabIndex = 2;
            this.staticaprasymaslabel.Text = "Aprašymas";
            this.staticaprasymaslabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // Aprasymas
            // 
            this.Aprasymas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Aprasymas.Location = new System.Drawing.Point(134, 23);
            this.Aprasymas.Name = "Aprasymas";
            this.Aprasymas.Size = new System.Drawing.Size(174, 19);
            this.Aprasymas.TabIndex = 3;
            this.Aprasymas.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(171, 124);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // staticKainalabel
            // 
            this.staticKainalabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.staticKainalabel.Location = new System.Drawing.Point(12, 174);
            this.staticKainalabel.Name = "staticKainalabel";
            this.staticKainalabel.Size = new System.Drawing.Size(105, 27);
            this.staticKainalabel.TabIndex = 5;
            this.staticKainalabel.Text = "Kaina";
            this.staticKainalabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // Kaina
            // 
            this.Kaina.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Kaina.Location = new System.Drawing.Point(143, 177);
            this.Kaina.Name = "Kaina";
            this.Kaina.Size = new System.Drawing.Size(177, 19);
            this.Kaina.TabIndex = 10;
            this.Kaina.Text = "label2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 11;
            this.button1.Text = "Pirkti";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kainaLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.StaticPavadinimasLabel);
            this.groupBox1.Controls.Add(this.Aprasymas);
            this.groupBox1.Controls.Add(this.staticaprasymaslabel);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 234);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 27);
            this.label1.TabIndex = 5;
            this.label1.Text = "Kaina";
            // 
            // kainaLabel
            // 
            this.kainaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kainaLabel.Location = new System.Drawing.Point(134, 179);
            this.kainaLabel.Name = "kainaLabel";
            this.kainaLabel.Size = new System.Drawing.Size(174, 19);
            this.kainaLabel.TabIndex = 6;
            this.kainaLabel.Text = "label1";
            // 
            // DetalusElementas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 294);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Kaina);
            this.Controls.Add(this.staticKainalabel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DetalusElementas";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.DetalusElementas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label StaticPavadinimasLabel;
        private System.Windows.Forms.Label staticaprasymaslabel;
        private System.Windows.Forms.Label Aprasymas;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label staticKainalabel;
        private System.Windows.Forms.Label Kaina;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label kainaLabel;
        private System.Windows.Forms.Label label1;
    }
}