using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aukcionas.Resources;

namespace Aukcionas
{
    public partial class Aukcionai : Form
    {
        private const int ONEMSTOMIN = 60000;
        private const int ONEMSTOSEC = 1000;
        private List<AukcionoElementas> elementai = new List<AukcionoElementas>();
        Timer timer = new Timer
        {
            Interval = ONEMSTOSEC,// keiciasi kas sekunde
            Enabled = true,
        };

        public Aukcionai()
        {
            InitializeComponent();
            this.Text = Resources.Resource.ProgramosPavadinimas;
            timer.Tick += Tick;
            timer.Start();

        }

        public void addRowToGrid(AukcionoElementas elem)
        {
            int likusiosMinutes = elem.Likeslaikas / 60;
            int likusiosSekundes = elem.Likeslaikas % 60;
            string laikas = likusiosMinutes.ToString() + ":" + likusiosSekundes.ToString();

            this.dataGridView1.Rows.Insert(dataGridView1.RowCount, elem.pavadinimas, elem.kaina.dabartKaina, laikas);
        }

        public void addRowToList(AukcionoElementas elem)
        {
            elementai.Add(elem);

        }

        public void pakeistiKaina(string pavadinimas, decimal kaina)
        {
            DataGridViewRow row = (from DataGridViewRow eilute in this.dataGridView1.Rows
                       where ((DataGridViewRow)eilute).Cells[0].Value == pavadinimas
                       select eilute).Single();
            row.Cells[1].Value = kaina;

        }

        private void Tick(object sender, EventArgs e)
        {
            foreach (AukcionoElementas item in this.elementai)
            {
                item.atimtiSekunde();

                var row = (from DataGridViewRow eilute in this.dataGridView1.Rows
                           where ((DataGridViewRow)eilute).Cells[0].Value == item.pavadinimas
                           select eilute).Single();

                int likusiosMinutes = item.Likeslaikas / 60;
                int likusiosSekundes = item.Likeslaikas % 60;
                string laikas = likusiosMinutes.ToString() + ":" + likusiosSekundes.ToString();

                row.Cells[2].Value = item.Likeslaikas <= 0 ? Resource.BaigesiLaikas : laikas;

            }
            //this.dataGridView1.Update();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sukurtinauja sukurt = new Sukurtinauja();
            sukurt.Show();

        }

        private void Aukcionai_Load(object sender, EventArgs e)
        {

        }

        private void Aukcionai_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logeris.EntryLog(Resources.ResourceLogeris.pabaiga);
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridView dgv = sender as DataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {
                AukcionoElementas elementas = (from AukcionoElementas item in this.elementai
                                      where item.pavadinimas == dgv.CurrentRow.Cells[0].Value
                           select item).Single();

                DetalusElementas detalus = new DetalusElementas(elementas);
                detalus.Show();
            }

        }
    }
}
