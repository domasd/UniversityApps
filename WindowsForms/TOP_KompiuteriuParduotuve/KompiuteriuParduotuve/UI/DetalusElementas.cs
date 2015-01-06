using System;
using System.Windows.Forms;
using KompiuteriuParduotuve;

namespace Kompiuteriuparduotuve.UI
{
    public partial class DetalusElementas : Form
    {
        private Preke prekesEgz;
        private IConsumer buyer;
        private Seller seller;

        public DetalusElementas(Preke elem, IConsumer buyer, Seller seller)
        {
            InitializeComponent();
            this.Text = elem.pavadinimas;
            this.Aprasymas.Text = elem.pavadinimas;
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.Image = elem.nuotrauka;
            this.textBox1.Text = elem.aprasymas;
            this.kainaLabel.Text = elem.kaina.dabartKaina.ToString();
            this.prekesEgz = elem;
            this.seller = seller;
            this.buyer = buyer;
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
            buyer.BuyItem(prekesEgz);
            seller.sell((Buyer)buyer);
        }

      

       
    }
}
