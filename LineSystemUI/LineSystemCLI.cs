using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineSystemCore;
using ExtensionMethods;

namespace LineSystemUI
{
    public class LineSystemCLI : ILineSystemUI
    {
        public LineSystem LineSystem { get; set; }
        public CommandParser Parser { get; set; }
        private bool running = true;

        public LineSystemCLI(LineSystem lineSystem)
        {
            LineSystem = lineSystem;
            Parser = new CommandParser(lineSystem, this);
        }

        public void DisplayUserNotFound(string userName)
        {
            Console.WriteLine("User [{0}] was not found.", userName);
        }

        public void DisplayProductNotFound(string productID)
        {
            Console.WriteLine("ProductID [{0}] was not found.", productID);
        }

        public void DisplayUserInfo(User user)
        {
            var transactions = LineSystem.GetTransactionList(user);
            transactions.Sort();

            Console.WriteLine(user.ToString());
            Console.WriteLine("Balance: {0}\n", user.Balance.ToKr());

            if (user.Balance < 5000)
                Console.WriteLine("Warning. Your balance is less than 50,00 kr. We recommend you refill before next purcash.\n");

            Console.WriteLine("Latest transactions:");
            for (int i = 0; i < 10 && i < transactions.Count; i++)
                Console.WriteLine(transactions[i].ToString());

        }

        public void DisplayTooManyArgumentsError(string commandName, int count)
        {
            Console.WriteLine("Wrong number of arguments. {0} takes {1} aguments.", commandName, count);
        }

        public void DisplayAdminCommmandNotFoundMessage(string commandName)
        {
            Console.WriteLine("Admin command {0} not found.", commandName);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine("[{0}] bought [{1}].", transaction.User.UserName, transaction.Product.Name);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction, int count)
        {
            Console.WriteLine("[{0}] bought {1} [{2}].", transaction.User.UserName, count, transaction.Product.Name);
        }

        public void DisplayAdminAddedCredits(InsertCashTransaction transaction)
        {
            Console.WriteLine("[{0}] was added to [{1}]'s balance.", transaction.Amount.ToKr(), transaction.User.UserName);
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine("Couldn't buy [{0}]. Balance: [{1}] Price: [{2}].", product.Name, user.Balance.ToKr(), product.Price.ToKr());
        }

        public void DisplayInsufficientCash(User user, Product product, int count)
        {
            Console.WriteLine("Couldn't buy {0} [{1}]. Balance: [{2}] Price: [{3}].", count, product.Name, user.Balance.ToKr(), (product.Price * count).ToKr());
        }

        public void DisplayGeneralError(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayGeneralMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void Close()
        {
            running = false;
        }

        public void Start()
        {
            string input = "";

            do
            {
                Console.Clear();

                foreach (var product in LineSystem.Products)
                {
                    if (product.Active)
                    {
                        Console.WriteLine("| {0,4} | {1,-35} | {2,9} |", product.ProductID, product.Name, product.Price.ToKr());
                    }
                }

                input = Console.ReadLine();
                Console.Clear();

                Parser.ParseCommand(input);

                Console.WriteLine("\nPress any key to continue.");
                Console.ReadKey();
            } while (running);
        }
    }
}
