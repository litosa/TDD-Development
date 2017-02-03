using Hemtenta_Alexander_Litos.bank;
using Moq;
using System;
using Xunit;

namespace Hemtenta_Alexander_Litos_Tests
{
    public class BankTests
    {
        private Account account;
        private double amount;

        public BankTests()
        {
            account = new Account();
            amount = 22.5;
        }

        [Fact]
        public void Should_Deposit_InvalidValues_Throws()
        {
            Assert.Throws<IllegalAmountException>(() => account.Deposit(double.NaN));
            Assert.Throws<IllegalAmountException>(() => account.Deposit(double.PositiveInfinity));
            Assert.Throws<IllegalAmountException>(() => account.Deposit(double.NegativeInfinity));
            Assert.Throws<IllegalAmountException>(() => account.Deposit(double.MinValue));
            Assert.Throws<IllegalAmountException>(() => account.Deposit(-000.1));
            Assert.Throws<IllegalAmountException>(() => account.Deposit(0));
        }

        [Fact]
        public void Should_Success_Deposit()
        {
            account.Deposit(amount);
            
            Assert.Equal(amount, account.Amount);
        }

        [Fact]
        public void Should_Withdraw_InvalidValues_Throws()
        {
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(double.NaN));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(double.PositiveInfinity));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(double.NegativeInfinity));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(double.MinValue));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(-000.1));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(0));
        }

        [Fact]
        public void Should_Fail_Withdraw_InsufficientFunds()
        {
            account.Deposit(amount);

            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(30));            
        }

        [Fact]
        public void Should_Success_Withdraw()
        {
            double accountAmount = 0;

            account.Deposit(amount);

            account.Withdraw(18.6);
            accountAmount = account.Amount;
            Assert.True(Math.Round(accountAmount, 1) == 3.9);

            account.Withdraw(2.3);
            accountAmount = account.Amount;
            Assert.True(Math.Round(accountAmount, 1) == 1.6);
        }

        [Fact]
        public void Should_TransferFounds_InvalidValues_Throws()
        {
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, double.NaN));
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, double.PositiveInfinity));
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, double.NegativeInfinity));
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, double.MinValue));
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, -000.1));
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, 0));

            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, 2));
        }

        [Fact]
        public void Should_Fail_Transfer_InsufficientFunds()
        {
            account.Deposit(amount);

            Assert.Throws<InsufficientFundsException>(() => account.TransferFunds(new Account(), 30));
        }

        [Fact]
        public void Should_Success_Transfer()
        {
            double accountAmount = 0;

            account.Deposit(amount);

            account.TransferFunds(new Account(), 18.6);
            accountAmount = account.Amount;
            Assert.True(Math.Round(accountAmount, 1) == 3.9);

            account.TransferFunds(new Account(), 2.3);
            accountAmount = account.Amount;
            Assert.True(Math.Round(accountAmount, 1) == 1.6);
        }
    }
}
