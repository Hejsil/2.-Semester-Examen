using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

namespace LineSystemCore
{
    public abstract class Transaction : IComparable<Transaction>
    {
        private static int _idGetter = 0;
        private static int IDGetter
        {
            get
            {
                _idGetter++;
                return _idGetter;
            }
        }

        public int TransactionID { get; private set; }
        private User _user;
        public User User 
        {
            get
            {
                return _user;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("User");

                _user = value;
            }
        }
        public DateTime Date { get; private set; }
        public int Amount { get; private set; }

        public Transaction(DateTime date, User user, int amount)
        {
            TransactionID = IDGetter;
            User = user;
            Date = date;
            Amount = amount;
        }

        public override string ToString()
        {
            return TransactionID + " - " + User.UserName + " - " + Amount.ToKr() + " - " + Date.ToString("dd:MM:yy HH:mm ");
        }

        public abstract void Execute();

        public int CompareTo(Transaction other)
        {
            return other.TransactionID - TransactionID;
        }
    }
}
