using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KompiuteriuParduotuve;
using Kompiuteriuparduotuve.Resources;



namespace Kompiuteriuparduotuve.UI
{
    public delegate void ProgramClosing(object sender, EventArgs e); //event

    public partial class Shop : Form
    {
        private List<Preke> elementai = new List<Preke>();
        private IConsumer consumer;
        private Seller seller;

        public event ProgramClosing closed;

        public Shop()
        {
            InitializeComponent();
            this.Text = Resource.ProgramosPavadinimas;

            consumer = new Buyer("Florijonas", "Piniguocius", 600000.5);

            seller = new Seller("Zacharijus", "Nebidonas", 1, consumer);

            this.closed += new ProgramClosing(methodOnClose);



        }

        private void methodOnClose(object sender, EventArgs e)
        {
            Logeris.EntryLog(ResourceLogeris.pabaiga);
        }



        protected virtual void Onclose(EventArgs e)
        {
            closed(this, e);
        }

        public void addRowToGrid(Preke elem)
        {
            this.dataGridView1.Rows.Insert(dataGridView1.RowCount, elem.pavadinimas, elem.kaina.dabartKaina, elem.Vienetai);
        }

        public void addRowToList(Preke elem)
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



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IkeltiPreke sukurt = new IkeltiPreke();
            sukurt.Show();

        }


        private void Vitrina_FormClosing(object sender, FormClosingEventArgs e)
        {
            Onclose(EventArgs.Empty);
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // events
        {

            DataGridView dgv = sender as DataGridView;
            if (dgv == null)
                return;
            if (dgv.CurrentRow.Selected)
            {

                Preke elementas = (from Preke item in this.elementai
                                   where item.pavadinimas == dgv.CurrentRow.Cells[0].Value
                                   select item).FirstOrDefault();

                DetalusElementas detalus = new DetalusElementas(elementas, consumer, seller);
                detalus.Show();
            }

        }

        private void Vitrina_Load(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void ikelt_Click(object sender, EventArgs e)
        {
            IProgress<int> progress = new Progress<int>(percent =>
            {
                if (percent > 100)
                {
                    //Slowly progressbar workaround
                    progressBar1.Value = 100;
                    progressBar1.Value = 99;
                    progressBar1.Value = 100;
                }
                else
                {
                    //Slowly progressbar workaround
                    progressBar1.Value = percent;
                    if (percent != 0) progressBar1.Value = percent - 1;
                    progressBar1.Value = percent;
                }

            });

            List<Preke> listas = await Task.Run(() => duomenys(progress)); //lambda & async/await



            foreach (var preke in listas)
            {
                VitrinosFormosInitializeris.AddRow(preke);
            }


        }

        private List<Preke> duomenys(IProgress<int> progress)
        {
            Random rnd = new Random();
            int random100 = rnd.Next(1, 100);

            Preke[] preke = new Preke[10]
            {
                new Preke("Kompiuteris", "Geras",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Pele", "Gera",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Trupinys is programuotojo klaviaturos", "Skanus",new Kaina(rnd.Next(100,600)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Plansete", "Gera",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Acer Aspire", "Geras",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Lenovo T500", "Geras",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Lenovo T16", "Geras",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("HP ProBook", "Geras",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Gaz 130", "Dar važiuoja!",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
                new Preke("Samsung Tab", "Nauja",new Kaina(rnd.Next(100,1000)),rnd.Next(1,15),Resources.Resource.NoImg),
            };

            var listas = new List<Preke>();
            for (int i = 1; i <= random100; i++)
            {
                listas.Add(preke[rnd.Next(0, 9)]);
                var theProgr = (int)(((float)i / (float)random100) * (float)100) + 5;

                progress.Report(theProgr);

                //Immitating hard work!
                Thread.Sleep(50);


            }

            #region testGenerics

            Preke preke1 = listas[1];
            Preke preke2 = listas[2];


            Preke.Swap(ref preke1, ref preke2); //Generics

            listas[1] = preke1;
            listas[2] = preke2;
            #endregion


            progress.Report(0);
            return listas;

        }

        private void filtr_Click(object sender, EventArgs e)
        {
            int filtrSk = 500;
            Func<Preke, bool> filtras = preke => preke.kaina.dabartKaina < filtrSk; // lambda

            this.dataGridView1.Rows.Clear();



            foreach (var elem in elementai.Filter(filtras))
            {
                addRowToGrid(elem);
            }



        }






    }
}
