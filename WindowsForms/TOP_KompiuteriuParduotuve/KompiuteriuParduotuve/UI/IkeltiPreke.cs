using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KompiuteriuParduotuve;
using Kompiuteriuparduotuve.Resources;

namespace Kompiuteriuparduotuve.UI
{
    public partial class IkeltiPreke : Form
    {
        public bool HasErrors { get; set; }

        public IkeltiPreke(Delegatas d)
        {
            InitializeComponent();
            d("Ikelimo formos langas atidarytas pirmąjį kartą"); //Delegatas
            this.Text = Resource.ProgramosPavadinimas;
        }

        public IkeltiPreke()
        {
            InitializeComponent();
            this.Text = Resource.ProgramosPavadinimas;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Sukurtinauja_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
            && !char.IsDigit(e.KeyChar)
            && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    if (Image.FromFile(openFileDialog1.FileName).GetType() == typeof(System.Drawing.Bitmap))
                        this.pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    else throw new Exception("Klaida įkeliant paveiksliuką. \n Bandykite dar kartą");
                }
                catch (IOException e1)
                {
                    Logeris.EntryLog(ResourceLogeris.klaida);
                    MessageBox.Show(e1.Message.ToString());
                }
                catch (Exception e2)
                {
                    Logeris.EntryLog(ResourceLogeris.klaida);
                   MessageBox.Show(e2.Message.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string errors;


            if (HasErrors)
            {
                MessageBox.Show(Resource.NeteisingiDuomenys);
            }
            else
            {
                if (textBox1.Text == ""
                    || textBox2.Text == ""
                    || textBox3.Text == ""
                    || trukmeTextBox.Text == "")
                {
                    MessageBox.Show(Resource.TustiLaukai);
                }
                else
                {
                    try
                    {
                        Preke elem = new Preke(this.textBox1.Text,
                            this.textBox2.Text,
                            new Kaina(decimal.Parse(this.textBox3.Text)), int.Parse(this.trukmeTextBox.Text),
                            this.pictureBox1.Image);
                        this.Hide();
                        VitrinosFormosInitializeris.Show();
                        VitrinosFormosInitializeris.AddRow(elem);
                        Logeris.EntryLog(ResourceLogeris.sukurtaPreke + elem.pavadinimas);
                    }
                    catch (OverflowException ex)
                    {
                        MessageBox.Show(Resource.NeteisingiDuomenys);
                    }
                    

                }


            }
        }

        private void Sukurtinauja_Validating(object sender, CancelEventArgs e)
        {
            Regex regex = new Regex(@"\d+");
            Match match;

            if (regex.Match(this.textBox1.Text).Success)
            {
                this.textBox1.BackColor = System.Drawing.Color.DarkSalmon;
                HasErrors = true;
            }
            else
            {
                HasErrors = false;
                this.textBox1.BackColor = System.Drawing.Color.White;
            }
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Default_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Kompiuteris";
            textBox2.Text = "Labai geras ir puikus";
            textBox3.Text = 1000.ToString();
            trukmeTextBox.Text = 100.ToString();
            pictureBox1.Image = Resources.Resource.NoImg;

        }


    }
}
