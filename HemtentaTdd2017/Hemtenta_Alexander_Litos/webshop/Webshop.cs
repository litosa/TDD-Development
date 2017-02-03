using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.webshop
{
    public class Webshop : IWebshop
    {
        public IBasket Basket { get; set; }

        public IBilling Billing { get; set; }

        public Webshop(IBasket basket)
        {
            Basket = basket;
        }

        public void Checkout(IBilling billing)
        {
            if (billing == null)
            {
                throw new BillingIsNullException();
            }
            Billing = billing;

            if (Billing.Balance == Basket.TotalCost)
            {
                Billing.Pay(Billing.Balance);
            }
        }
    }
}
