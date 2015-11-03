using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using LineSystemCore;
using ExtensionMethods;

namespace LineSystemUI
{
    enum ProductBoolField
    {
        Active, 
        CanBeBoughtOnCredit,
    }

    public class CommandParser
    {
        public LineSystem LineSystem { get; set; }
        public ILineSystemUI UI { get; set; }
        private Dictionary<string, Action<string[]>> adminCommands = new Dictionary<string, Action<string[]>>();

        public CommandParser(LineSystem lineSystem, ILineSystemUI ui)
        {
            LineSystem = lineSystem;
            UI = ui;

            adminCommands.Add(":close", args => UI.Close());
            adminCommands.Add(":activate", args =>
            {
                ChangeProductBoolField(":activate", args, ProductBoolField.Active, true);
            });
            adminCommands.Add(":deactivate", args =>
            {
                ChangeProductBoolField(":deactivate", args, ProductBoolField.Active, false);
            });
            adminCommands.Add(":crediton", args =>
            {
                ChangeProductBoolField(":crediton", args, ProductBoolField.CanBeBoughtOnCredit, true);
            });
            adminCommands.Add(":creditoff", args =>
            {
                ChangeProductBoolField(":creditoff", args, ProductBoolField.CanBeBoughtOnCredit, false);
            });
            adminCommands.Add(":addcredits", args =>
            {
                InsertCashTransaction transaction;
                User user;
                int amount;

                if (IsValidArgs(":addcredits", args, 2))
                {
                    user = LineSystem.GetUser(args[0]);

                    if (user != null)
                    {
                        if (args[1].TryToCredit(out amount))
                        {
                            transaction = LineSystem.AddCreditsToAccount(user, amount);
                            UI.DisplayAdminAddedCredits(transaction);
                            LineSystem.ExecuteTransaction(transaction);
                        }
                        else
                        {
                            UI.DisplayGeneralError(args[1] + " is not a invalid cash amount");
                        }
                    }
                    else
                    {
                        UI.DisplayUserNotFound(args[0]);
                    }
                }
            });
        }

        //Changes a field of a product depending on the parameters of the method
        private void ChangeProductBoolField(string commandName, string[] args, ProductBoolField field, bool newValue)
        {
            Product product;
            int id;

            if (IsValidArgs(commandName, args, 1))
            {
                if (int.TryParse(args[0], out id))
                {
                    product = LineSystem.GetProduct(id);

                    if (product != null)
                    {
                        switch (field)
                        {
                            case ProductBoolField.Active:
                                product.Active = newValue;
                                break;
                            case ProductBoolField.CanBeBoughtOnCredit:
                                product.CanBeBoughtOnCredit = newValue;
                                break;
                            default:
                                break;
                        }

                        UI.DisplayGeneralMessage(product.Name + "'s " + field.ToString() + " field was set to " + newValue.ToString());
                    }
                    else
                    {
                        UI.DisplayProductNotFound(args[0]);
                    }
                }
                else
                {
                    UI.DisplayGeneralError(args[0] + " is not a invalid productID");
                }
            }
        }

        private bool IsValidArgs(string commandName, string[] args, int count)
        {
            if (args == null || args.Length != count)
            {
                UI.DisplayTooManyArgumentsError(commandName, count);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ParseCommand(string input)
        {
            //Looking at the first character in string to varify wether or not it's a ':'
            var regex = new Regex("^[:]");
            var inputArray = input.Split(' ');
            var inputCount = inputArray.Count();

            if (inputArray[0].Count() != 0 && regex.IsMatch(inputArray[0]))
            {
                AdminCommand(inputArray, inputCount);
            }
            else if (inputCount > 3)
            {
                UI.DisplayTooManyArgumentsError("Buying a product doesn't", inputCount);
            }
            else
            {
                GetUser(inputArray, inputCount);
            }
        }

        private void AdminCommand(string[] inputArray, int inputCount)
        {
            string[] args = inputArray.Skip(1).ToArray();
            Action<string[]> adminMethod;

            if (adminCommands.TryGetValue(inputArray[0], out adminMethod))
            {
                adminMethod(args);
            }
            else
            {
                UI.DisplayAdminCommmandNotFoundMessage(inputArray[0]);
            }
        }

        private void GetUser(string[] inputArray, int inputCount)
        {
            User user = LineSystem.GetUser(inputArray[0]);

            if (user != null)
            {
                if (inputCount == 1)
                    UI.DisplayUserInfo(user);
                else if (inputCount == 2)
                    QuickBuy(inputArray, inputCount, user);
                else
                    MultiBuy(inputArray, inputCount, user);
            }
            else
            {
                UI.DisplayUserNotFound(inputArray[0]);
            }
        }

        private void QuickBuy(string[] inputArray, int inputCount, User user)
        {
            Product product;
            BuyTransaction transaction;
            int id;

            if (int.TryParse(inputArray[1], out id))
            {
                product = LineSystem.GetProduct(id);

                if (product != null && product.Active)
                {
                    transaction = LineSystem.BuyProduct(user, product);

                    if (LineSystem.ExecuteTransaction(transaction))
                    {
                        UI.DisplayUserBuysProduct(transaction);
                    }
                    else
                    {
                        UI.DisplayInsufficientCash(user, product);
                    }
                }
                else
                {
                    UI.DisplayProductNotFound(inputArray[1]);
                }
            }
            else
            {
                UI.DisplayGeneralError(inputArray[1] + " is not a invalid productID");
            }
        }

        private void MultiBuy(string[] inputArray, int inputCount, User user)
        {
            List<BuyTransaction> transactions = new List<BuyTransaction>();
            Product product;
            int amount, id, price;

            if (int.TryParse(inputArray[1], out amount) && amount > 0)
            {
                if (int.TryParse(inputArray[2], out id))
                {
                    product = LineSystem.GetProduct(id);

                    if (product != null && product.Active)
                    {
                        for (int i = 0; i < amount; i++)
                            transactions.Add(LineSystem.BuyProduct(user, product));

                        price = transactions[0].Amount * amount;


                        if (price <= user.Balance)
                        {
                            foreach (var transaction in transactions)
                            {
                                LineSystem.ExecuteTransaction(transaction);
                            }

                            UI.DisplayUserBuysProduct(transactions[0], amount);
                        }
                        else
                        {
                            UI.DisplayInsufficientCash(user, product, amount);
                        }
                    }
                    else
                    {
                        UI.DisplayProductNotFound(inputArray[2]);
                    }
                }
                else
                {
                    UI.DisplayGeneralError(inputArray[2] + " is not a valid productID");
                }
            }
            else
            {
                UI.DisplayGeneralError(inputArray[1] + " is not a valid amount");
            }
        }
    }
}
