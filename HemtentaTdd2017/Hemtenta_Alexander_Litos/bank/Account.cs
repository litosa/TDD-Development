using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alexander_Litos.bank
{
    public class Account : IAccount
    {
        public double Amount { get; set; }


        public void Deposit(double amount)
        {
            if (double.IsInfinity(amount) || double.IsNaN(amount) || amount <= 0)
            {
                throw new IllegalAmountException();
            }
            Amount += amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            if (destination == null || double.IsInfinity(amount) || double.IsNaN(amount) || amount <= 0)
            {
                throw new OperationNotPermittedException();
            }
            if (Amount < amount)
            {
                throw new InsufficientFundsException();
            }
            Amount -= amount;
            destination.Deposit(amount);
        }

        public void Withdraw(double amount)
        {
            if (double.IsInfinity(amount) || double.IsNaN(amount) || amount <= 0)
            {
                throw new IllegalAmountException();
            }
            if (Amount < amount)
            {
                throw new InsufficientFundsException();
            }
            Amount -= amount;
        }
    }
}
