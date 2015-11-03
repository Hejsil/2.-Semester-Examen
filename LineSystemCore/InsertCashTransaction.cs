using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(DateTime date, User user, int amount)
            : base(date, user, amount)
        {
        }

        public override string ToString()
        {
            return base.ToString() + "- Type: Inserted Cash";
        }

        public override void Execute()
        {
            User.Balance += Amount;
        }
    }
}
