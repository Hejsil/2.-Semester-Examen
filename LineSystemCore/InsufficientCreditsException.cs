using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineSystemCore
{
    [Serializable()]
    public class InsufficientCreditsException : Exception
    {
        public User User { get; private set; }
        public Product Product { get; private set; }

        public InsufficientCreditsException(User user, Product product)
            : base(user.ToString() + " tried to buy " + product.ToString() + " but had insufficient credits") 
        {
            User = user;
            Product = product;
        }

        protected InsufficientCreditsException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
