using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineSystemCore;

namespace LineSystemUI
{
    public interface ILineSystemUI
    {
        void DisplayUserNotFound(string userName);
        void DisplayProductNotFound(string productID);
        void DisplayUserInfo(User user);
        void DisplayTooManyArgumentsError(string commandName, int count);
        void DisplayAdminCommmandNotFoundMessage(string commandName);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(BuyTransaction transaction, int count);
        void DisplayAdminAddedCredits(InsertCashTransaction transaction);
        void DisplayInsufficientCash(User user, Product product);
        void DisplayInsufficientCash(User user, Product product, int count);
        void DisplayGeneralError(string message);
        void DisplayGeneralMessage(string message);
        void Close();
    }
}
