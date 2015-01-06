using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aukcionas
{
    public partial class DetalusElementas : Form
    {
        private AukcionoElementas aukcionoElementas;

        public DetalusElementas(AukcionoElementas elem)
        {
            InitializeComponent();
            this.Text = elem.pavadinimas;
            this.Aprasymas.Text = elem.pavadinimas;
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.Image = elem.nuotrauka;
            this.textBox1.Text = elem.aprasimas;
            this.KainaPrad.Text = elem.kaina.PradKaina.ToString();
            this.Kaina.Text = elem.kaina.dabartKaina.ToString();
            this.Statymas.Text = (elem.kaina.dabartKaina + 1).ToString();
            this.aukcionoElementas = elem;
        }

        private void DetalusElementas_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.aukcionoElementas.Likeslaikas <= 0)
            {
                MessageBox.Show(Resources.Resource.BaigesiLaikas);
                return;
            }
            else
            {
                this.aukcionoElementas.kaina.dabartKaina += 1;
                this.Kaina.Text = aukcionoElementas.kaina.dabartKaina.ToString();
                this.Statymas.Text = (aukcionoElementas.kaina.dabartKaina+1).ToString();
                AukcionuFormosInitializeris.Get().pakeistiKaina(aukcionoElementas.pavadinimas, aukcionoElementas.kaina.dabartKaina);
                Logeris.EntryLog(Resources.ResourceLogeris.statymas + this.aukcionoElementas.pavadinimas + " nauja kaina " + aukcionoElementas.kaina.dabartKaina);
            }
            return;

        }

        private void Aprasymas_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Sifravimas.Encrypt(aukcionoElementas.pavadinimas));
        }
    }
}
