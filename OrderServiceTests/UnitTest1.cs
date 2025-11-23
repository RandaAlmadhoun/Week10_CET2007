using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Week10_CET2007;
namespace OrderServiceTests
{
    [TestClass]
    public class OrderServiceIntegrationTests
    {
        [TestMethod]
        public void PlaceOrder_ReservesStock_And_ChargesPayment()
        {
            // Arrange
            var inventory = new InventoryService();
            var payment = new FakePaymentService();
            var service = new OrderService(inventory, payment);

            // Act
            service.PlaceOrder("P001", 2, "4111111111111111", 50);
            //service.PlaceOrder("INVALID", 1, "1111-2222", 50m);

            // Assert
            Assert.IsTrue(payment.WasCharged);
            Assert.AreEqual(100, payment.LastAmount);
        }
        // if the quantity check in placeorder is commented the following method will fail (TDD )

        [TestMethod]
        public void PlaceOrder_Throws_When_Quantity_Is_Zero()
        {
            var inventory = new FakeInventoryService();
            var payment = new FakePaymentService();
            var service = new OrderService(inventory, payment);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                service.PlaceOrder("P001", 0, "4111", 50));
        }
    }
}
