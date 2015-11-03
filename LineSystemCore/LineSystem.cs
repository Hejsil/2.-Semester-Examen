using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace LineSystemCore
{
    enum Data
    {
        ID,
        Name,
        Price,
        Active,
        DeactivateDate
    }

    public class LineSystem
    {
        public List<User> Users { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        private StreamWriter log;

        public LineSystem() : this ("products.csv") {}
        public LineSystem(string path)
        {
            Users = new List<User>();
            Products = new List<Product>();
            Transactions = new List<Transaction>();

            //LineSystem keeps a log running until the LineSystem is disposed.
            log = new StreamWriter("log.txt", true);
            log.AutoFlush = true;
            log.WriteLine();
            log.WriteLine("Program opened: " + DateTime.Now.ToString());

            if (path != null)
                ImportProducts(path);
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            if (user != null && product != null)
                return new BuyTransaction(DateTime.Now, user, product);
            else
                return null;
        }

        public InsertCashTransaction AddCreditsToAccount(User user, int amount)
        {
            if (user != null)
                return new InsertCashTransaction(DateTime.Now, user, amount);
            else
                return null;
        }

        public bool ExecuteTransaction(Transaction transaction)
        {
            //Execute transaction if either:
            //1. It's a InsertCashTransaction.
            //2. It's a BuyTransaction and the product is active, and the user has the balance to buy the product.
            if (transaction != null 
                && ((transaction is BuyTransaction && (transaction.User.Balance >= transaction.Amount && (transaction as BuyTransaction).Product.Active)) 
                || transaction is InsertCashTransaction))
            {
                transaction.Execute();
                log.WriteLine(transaction.ToString());
                Transactions.Add(transaction);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Product GetProduct(int id)
        {
            return Products.Find(product => product.ProductID == id);
        }

        public User GetUser(string userName)
        {
            return Users.Find(user => user.UserName == userName);
        }

        public List<Transaction> GetTransactionList(User user)
        {
            if (user != null)
                return Transactions.FindAll(transaction => transaction.User.Equals(user));
            else
                return new List<Transaction>();
        }

        public List<Product> GetActiveProducts()
        {
            return Products.FindAll(product => product.Active);
        }

        private void ImportProducts(string path)
        {
            var daDK = new CultureInfo("da-DK");
            var reader = new StreamReader(path, Encoding.GetEncoding("iso-8859-1"));
            var line = reader.ReadLine();

            while ((line = reader.ReadLine()) != null)
            {
                var data = line.Split(';');
                var id = int.Parse(data[(int)Data.ID]);
                var name = Regex.Replace(data[(int)Data.Name].Trim('"'), "<.*?>", "");
                var price = int.Parse(data[(int)Data.Price]);
                var active = int.Parse(data[(int)Data.Active]) == 1;
                DateTime? seasonEndDate = null;
                DateTime tempDate;

                if (DateTime.TryParseExact(data[(int)Data.DeactivateDate].Trim('"'), "yyyy-MM-dd HH:mm:ss", daDK, DateTimeStyles.None, out tempDate))
                {
                    seasonEndDate = tempDate;
                }

                Products.Add(new SeasonalProduct(name, price, active, null, seasonEndDate, id));
            }

            reader.Close();
        }
    }
}
