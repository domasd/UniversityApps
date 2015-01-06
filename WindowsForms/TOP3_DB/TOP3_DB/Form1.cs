using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TOP3_DB.Data;
using TOP3_DB.Properties;

namespace TOP3_DB
{
    public partial class Form1 : Form
    {
        private SqlCeConnection sqlCeConnection;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            connectionString = ConfigurationManager.ConnectionStrings["TOP3_DB.Properties.Settings.Database1ConnectionString"].ConnectionString;
            sqlCeConnection = new SqlCeConnection(connectionString);
            sqlCeConnection.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT Id, Vardas, Adresas, GimimoData, Svoris, TrenerioId FROM Klientas", sqlCeConnection);
                DataSet dataSet = new DataSet();
                da.Fill(dataSet, "Klientas");
                DataTable klientasDataTable = dataSet.Tables["Klientas"];
                

                    foreach (DataRow row in klientasDataTable.Rows)
                    {
                        foreach (DataColumn dc in klientasDataTable.Columns)
                        {
                            Write(row[dc] + " ");
                        }
                        NewLine();
                    }
                  
                    

                da.Dispose();
            
        }

        private void GuiConsole_TextChanged(object sender, EventArgs e)
        {

        }

        public void WriteLine(string text)
        {
            GuiConsole.Text = GuiConsole.Text.Insert(GuiConsole.TextLength, string.Format(
                text + "{0}", Environment.NewLine));

        }
        public void Write(string text)
        {
            GuiConsole.Text = GuiConsole.Text.Insert(GuiConsole.TextLength, 
                text);

        }
        public void NewLine()
        {
            GuiConsole.Text = GuiConsole.Text.Insert(GuiConsole.TextLength, string.Format("{0}", Environment.NewLine));

        }
        public void Clear()
        {
            GuiConsole.Text = Resources.Form1_Clear_Cleared;
            NewLine();

        }

        void Destructor()
        {
            sqlCeConnection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            #region NotORMInsert


            SqlCeCommand insert = new SqlCeCommand();
            insert.Connection = sqlCeConnection;
            insert.CommandType = CommandType.Text;
            insert.CommandText = "INSERT INTO Klientas (Id, Adresas,GimimoData) VALUES (@ID,@AD,@GI)";

            insert.Parameters.Add(new SqlCeParameter("@ID", SqlDbType.NVarChar, 50, "Id"));
            insert.Parameters.Add(new SqlCeParameter("@AD", SqlDbType.NVarChar, 50, "Adresas"));
            insert.Parameters.Add(new SqlCeParameter("@GI", SqlDbType.NVarChar, 50, "GimimoData"));


            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT Id, Vardas, Adresas, GimimoData, Svoris, TrenerioId FROM Klientas", sqlCeConnection);
            da.InsertCommand = insert;

            DataSet ds = new DataSet();
            da.Fill(ds, "Klientas");

            DataTable klientasDataTable = ds.Tables["Klientas"];

            DataRow newRow = klientasDataTable.NewRow();
            newRow["Id"] = ds.Tables[0].Rows.Count + 1;
            newRow["Adresas"] = "Raudoneliu g";
            newRow["GimimoData"] = "1956-01-01";
            newRow["Vardas"] = "Jeronimas";

            klientasDataTable.Rows.Add(newRow);

            da.Update(ds.Tables[0]);
            da.Dispose();

            #endregion





            //StringBuilder sb = new StringBuilder();
            //using (var db = new TestDbEntities1())
            //{
            //    Klientas klientas1 = new Klientas
            //    {
            //        Id = db.Klientas.Count() + 1,
            //        Adresas = "Raudoneliu g",
            //        GimimoData = "1956-01-01",
            //        Vardas = "Jeronimas"
            //    };


            //    db.Klientas.Add(klientas1); // insert
            //    try
            //    {
            //        db.SaveChanges(); // Save
            //    }
            //    catch (DbUpdateException)
            //    {
            //        MessageBox.Show("Failure in database ");
            //    }
               

            //    WriteLine("New client:");
            //    WriteLine(klientas1.getInfo());

            //} 

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            #region notORM

            SqlCeConnection cn = new SqlCeConnection();
            cn.ConnectionString = connectionString;
            cn.Open();

            SqlCeCommand update = new SqlCeCommand();
            update.Connection = cn;
            update.CommandType = CommandType.Text;
            update.CommandText = "UPDATE Klientas SET Adresas = @FN WHERE Id = @Id";

            update.Parameters.Add(new SqlCeParameter("@FN", SqlDbType.NVarChar, 50, "Adresas"));
            update.Parameters.Add(new SqlCeParameter("@Id", SqlDbType.Int, 50, "Id"));


            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT Id, Vardas, Adresas, GimimoData, Svoris, TrenerioId FROM Klientas", cn);
            da.UpdateCommand = update;

            DataSet ds = new DataSet();
            da.Fill(ds, "Klientas");

            ds.Tables[0].Rows[0]["Adresas"] = "Naujininku g";

            da.Update(ds.Tables[0]);
            cn.Close();
            da.Dispose();
            #endregion

            //StringBuilder sb = new StringBuilder();
            //using (var db = new TestDbEntities1())
            //{

            //    Klientas klientas = db.Klientas.First();
            //    if (klientas.Adresas == "Gelvonu g")
            //    {
            //        klientas.Adresas = "Naujininku g";
            //    }
            //    else
            //    {
            //        klientas.Adresas = "Gelvonu g";
            //    }
            //    try
            //    {
            //        db.SaveChanges(); // Save
            //    }
            //    catch (DbUpdateException exception)
            //    {
            //        MessageBox.Show("Failure in database - {0}",exception.Message);
            //    }


            //    WriteLine("Updated first client:");
            //    WriteLine(klientas.getInfo());
            //} 
        }

        private void button3_Click(object sender, EventArgs e) // Delete
        {
            // Solution with not ORM tools

            #region notORMDelete

            SqlCeConnection cn = new SqlCeConnection();
            cn.ConnectionString = connectionString;
            cn.Open();

            SqlCeCommand delete = new SqlCeCommand();
            delete.Connection = cn;
            delete.CommandType = CommandType.Text;
            delete.CommandText = "DELETE FROM Klientas WHERE Id = @Id";

            delete.Parameters.Add(new SqlCeParameter("@Id", SqlDbType.Int, 50, "Id"));

            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT Id, Vardas, Adresas, GimimoData, Svoris, TrenerioId FROM Klientas", cn);
            da.DeleteCommand = delete;

            DataSet ds = new DataSet();
            da.Fill(ds, "Klientas");



            ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1].Delete();

            da.Update(ds.Tables[0]);
            cn.Close();
            da.Dispose();
            #endregion


            
            //using (var db = new TestDbEntities1())
            //{
            //    StringBuilder sb = new StringBuilder();

            //    Klientas klientas = (from klientai in db.Klientas
            //                            select klientai).ToList().Last(); // I just intented to use linq here

            //    db.Klientas.Remove(klientas);
                
            //    try
            //    {
            //        db.SaveChanges(); // Save
            //    }
            //    catch (DbUpdateException exception)
            //    {
            //        MessageBox.Show("Failure in database - {0}",exception.Message);
            //    }


            //    WriteLine("Deleted last client:");
            //    WriteLine(klientas.getInfo());
            //} 
        }

        private void button6_Click(object sender, EventArgs e) // Customers and their orders
        {
            using (var db = new TestDbEntities1())
            {
                DateTime fromDatetime = new DateTime(2014,6,1); // Date to show orders from

                WriteLine(string.Format("Showing orders from {0}, till now",fromDatetime.ToShortDateString()));

                // Linq solution
                var custWithOrders = from cust in db.Klientas.ToList()  // .ToList() helps use anonymous type in the select clause
                    join order in db.Uzsakymai on cust.Id equals order.KlientoId
                    join subscript in db.Abonementas on order.AbonementoId equals subscript.Id
                    where order.PirkimoData >= fromDatetime
                    orderby cust.Vardas,order.PirkimoData
                    select new {Name = string.Format("Name - {0}, ",cust.Vardas), OrderInfo = string.Format("Order date - {0}, Subscription name - {1}, Price - {2}",
                        order.PirkimoData,subscript.Pavadinimas,subscript.Kaina)};

                foreach (var custWithOrder in custWithOrders)
                {
                    WriteLine(custWithOrder.Name + " " + custWithOrder.OrderInfo);
                }

                //Solution with entity framework 
                //foreach (var cust in db.Klientas)
                //{
                //    foreach (var order in cust.Uzsakymai)
                //    {
                //        WriteLine(string.Format("Subscription name - {0}, Description - {1}",order.Abonementas.Pavadinimas,order.Abonementas.Aprasymas));
                //    }
                    
                //}
               
            }
        }

        private void button7_Click(object sender, EventArgs e) // Customers by their name
        {
            using (var db = new TestDbEntities1())
            {
                

                var group = from client in db.Klientas.ToArray() //ToArray, coz Otherwise could not do the foreach iterate in a proper way
                    group client by client.Vardas[0]
                    into clientg
                    select new {grouping = clientg, Key = clientg.Key, sum = clientg.Sum(x=> x.Uzsakymai.Sum(o=> o.Abonementas.Kaina))}; // Aggregate function


                foreach (var custGroup in group)
                {
                    WriteLine(string.Format("Group {0}, Which spent {1}:",custGroup.Key,custGroup.sum));
                    foreach (var cust in custGroup.grouping)
                    {
                        WriteLine(cust.Vardas + " ");
                    }
                }



                var group2 = db.Klientas.GroupBy(x => x.Vardas[0]);

            }
           
        }

        private void generateOrdersFor2FirstCustomers()
        {
            using (var db = new TestDbEntities1())
            {
                Random rnd = new Random();

                //rnd.Next(db.Abonementas.Count()); // Abonementas Id
                //rnd.Next(2); //Klientas id

                DateTime now = DateTime.Now;
                DateTime from = new DateTime(2013,01,1);
                DateTime to = new DateTime(now.Year,now.Month,1);

              
                for (DateTime d = from; d < to; d = d.AddMonths(1))
                {
                   // WriteLine("adding - " + d.ToShortDateString());

                    db.Uzsakymai.Add(new Uzsakymai()
                    {
                        AbonementoId = rnd.Next(1,db.Abonementas.Count()+1),
                        KlientoId = rnd.Next(1,3),
                        PirkimoData = d
                    });
                }



                db.SaveChanges();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
           

            StringBuilder sb = new StringBuilder();
            using (var db = new TestDbEntities1())
            {
                if (!db.Uzsakymai.ToList().Any<Uzsakymai>())
                {

                    generateOrdersFor2FirstCustomers(); //Used this to generate some rows in Uzsakymai table

                }
                foreach (var order in db.Uzsakymai.Take(10)) // take
                {
                    WriteLine(sb.Append(order.KlientoId).Append(" ").Append(order.AbonementoId).Append(" ").Append(order.PirkimoData).ToString());
                    sb.Clear();
                }
            }
        }




    }
}