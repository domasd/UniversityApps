using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KompiuteriuParduotuve;

namespace Kompiuteriuparduotuve
{
    public class Seller : Employee
    {
        private int deals;
        private int minimumDeals;
        private int minimumDeposit;

        public Seller(String name, String lname, int id, IConsumer buyer )
            : base( name,  lname,  id, buyer) {}

         
        public int Deals
        {
            get { return deals; }
            set { deals = value; }
        }

        public void sell(Buyer buyer)
        {
            foreach (Preke preke in buyer.cart)
            {
                this.Deals++;
                preke.atimtiVieneta();
                buyer.BuyItem(preke);
                
            }
        }  
    }



    public class Employee : Man
    {
          private readonly int workid; 
          private String Contract { get; set; }
          private IConsumer specialBuyer; 




        public Employee(String name, String lname, int id, IConsumer buyer) : base(name,lname)
        {
            this.workid = id;
            this.specialBuyer = buyer; // Dependency injection
        }

        public void sellToSpecialBuyer(Preke preke) 
        {
            specialBuyer.BuyItem(preke);// Dependency injection
        }

        protected override double SpendMoney(double spentMoney)
        {
            if (this.Money < spentMoney)
            {
                this.Money = 0;
                return 0;
            }

            this.Money -= spentMoney;
            return this.Money;
        }
    }

    public abstract class Man
    {
        protected readonly String Name;
        protected readonly String Lname;
        private double money;

        protected double Money
        {
            get { return money; }
            set { money = value; }
        }

        protected abstract double SpendMoney(double money);

        protected Man(String name, String lname)
        {
            this.Name = name;
            this.Lname = lname;
        }

        protected Man(String name, String lname, double money)
        {
            this.Name = name;
            this.Lname = lname;
            this.money = money;
        }


        public override string ToString()
        {
            return (Name + " " + Lname);
        }

    }


    public class Buyer : Man, IConsumer
    {
        private double discount { get; set; } // 1 - 0proc, 0,5 - 50proc ir t.t.
        public LinkedList<Preke> cart = new LinkedList<Preke>();
        private Lazy<LinkedList<Preke>> boughtCart = new Lazy<LinkedList<Preke>>();  // Tingus inicializavimas


        public Buyer(string name, string lname, double money) : base(name, lname,money) {}

        protected override double SpendMoney(double money)
        {
            this.Money -= money;// Tarkime, pirkėjas turi kreditinę kortelę ir gali pirkti į skolą
            return this.Money;
        }

        public void BuyItem(Preke preke)
        {
            this.Money -= (double)preke.kaina.dabartKaina * discount;
            boughtCart.Value.AddFirst(preke);
        }

        public void putToCart(Preke preke)
        {
            cart.AddFirst(preke);
        }


    }

    public interface IConsumer 
    {
         void BuyItem(Preke preke);
    }



}