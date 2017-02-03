using Hemtenta_Alexander_Litos.webshop;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hemtenta_Alexander_Litos_Tests
{
    /*  Frågor
        Vilka metoder och properties behöver testas? 
            Metoder
                basket.AddProduct()
                basket.RemoveProduct()
                webShop.Checkout()
                mockBilling.Verify(m => m.Pay(It.IsAny<decimal>()), Times.Once);            
            Properties
                basket.TotalCost
                webShop.Billing.Balance


        Ska några exceptions kastas?
            throw new NotValidAmountOrProductException();
            throw new BillingIsNullException();


        Vilka är domänerna för IWebshop och IBasket?
            IWebShop
                objekt(Basket) - null eller objekt av typen Basket
            IBasket
                objekt(Product) - null eller objekt av typen Product
                int(Amount) -  alla heltal mellan -2147483648 och +2147483647
                decimal(TotalCost) - alla heltal större än 0
     */


    public class WebshopTests
    {
        private decimal basketTotalCost;
        private int amount;
        private int multipleAmount;

        private Mock<IBilling> mockBilling;
        private Webshop webShop;
        private Basket basket;
        private Product product;

        public WebshopTests()
        {
            basketTotalCost = 0;
            amount = 1;
            multipleAmount = 5;

            mockBilling = new Mock<IBilling>();
            basket = new Basket();
            webShop = new Webshop(basket);

            webShop.Billing = mockBilling.Object;
            product = new Product { Name = "Barbie", Price = 12 };
        }

        [Fact]
        public void Should_AddProduct_To_Basket_InvalidValues_Throws()
        {
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, int.MinValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, int.MaxValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, -1));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(null, 1));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(new Product(), int.MinValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(new Product(), int.MaxValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(new Product(), -1));
        }

        [Fact]
        public void AddProduct_ProductPriceIsNegative_Throws()
        {
            var product = new Product { Price = decimal.MinusOne };

            Assert.Throws<NotValidPriceException>(() => basket.AddProduct(product, 2));
        }

        [Fact]
        public void Should_Success_AddSingleProduct_To_Basket()
        {
            basket.AddProduct(product, amount);

            basketTotalCost = basket.TotalCost;

            Assert.Equal(product.Price * amount, basketTotalCost);
        }

        [Fact]
        public void Should_Success_AddMultipleProduct_To_Basket()
        {
            decimal sum = 0;

            basket.AddProduct(product, multipleAmount);

            for (int i = 0; i < multipleAmount; i++)
            {
                sum += product.Price;
            }

            basketTotalCost = basket.TotalCost;

            Assert.Equal(sum, basketTotalCost);
        }

        [Fact]
        public void Should_RemoveProduct_To_Basket_InvalidValues_Throws()
        {
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, int.MinValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, int.MaxValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, -1));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(null, 1));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(new Product(), int.MinValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(new Product(), int.MaxValue));
            Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(new Product(), -1));
        }

        [Fact]
        public void Should_Success_RemoveSingleProduct_From_Basket()
        {
            decimal sum = 0;

            basket.AddProduct(product, 2);

            for (int i = 0; i < 2; i++)
            {
                sum += product.Price;
            }

            basket.RemoveProduct(product, amount);

            for (int i = 0; i < amount; i++)
            {
                sum -= product.Price;
            }

            basketTotalCost = basket.TotalCost;

            Assert.Equal(sum, basketTotalCost);
        }

        [Fact]
        public void Should_Success_RemoveMultipleProduct_From_Basket()
        {
            decimal sum = 0;

            basket.AddProduct(product, multipleAmount);

            for (int i = 0; i < multipleAmount; i++)
            {
                sum += product.Price;
            }

            basket.RemoveProduct(product, 3);

            for (int i = 0; i < 3; i++)
            {
                sum -= product.Price;
            }

            basketTotalCost = basket.TotalCost;

            Assert.Equal(sum, basketTotalCost);
        }

        [Fact]
        public void Should_Success_RemoveMultipleProduct_From_Basket_ToEmptyBasket()
        {
            basket.AddProduct(product, multipleAmount);

            basket.RemoveProduct(product, multipleAmount);

            basketTotalCost = basket.TotalCost;

            Assert.Equal(0, basketTotalCost);
        }

        [Fact]
        public void Should_Checkout_InvalidValues_Throws()
        {
            Assert.Throws<BillingIsNullException>(() => webShop.Checkout(null));
        }

        [Fact]
        public void Should_Success_Checkout()
        {
            basket.AddProduct(product, multipleAmount);
            basketTotalCost = basket.TotalCost;

            mockBilling.Setup(b => b.Balance).Returns(basketTotalCost);

            webShop.Checkout(webShop.Billing);

            Assert.Equal(true, webShop.Billing.Balance == basketTotalCost);
            //mockBilling.Verify(m => m.Pay(It.IsAny<decimal>()), Times.Exactly(1));
            mockBilling.Verify((IBilling x) => x.Pay(basketTotalCost), Times.Exactly(1));

        }
    }
}
