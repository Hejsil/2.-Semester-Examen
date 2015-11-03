using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    public class BuyTransaction : Transaction
    {
        public Product Product { get; private set; }

        public BuyTransaction(DateTime date, User user, Product product)
            : base(date, user, product.Price)
        {
            Product = product;
        }

        public override string ToString()
        {
            return base.ToString() + "- Type: Bought Transaction";
        }

        public override void Execute()
        {
            if (Product.Active)
                if (User.Balance >= Amount)
                    User.Balance -= Amount;
                else
                    throw new InsufficientCreditsException(User, Product);
            else
                throw new NotActiveException(Product);
        }
    }
}
