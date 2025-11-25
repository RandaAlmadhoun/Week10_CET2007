using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week10_CET2007
{
    public interface IInventoryService
    {
        bool HasStock(string productId, int quantity);
        void ReserveStock(string productId, int quantity);
    }

    public interface IPaymentService
    {
        void Charge(string cardNumber, double amount);
    }

    public class OrderService
    {
       private  IInventoryService _inventory;
       private  IPaymentService _payment;

       public OrderService(IInventoryService inventory, IPaymentService payment)
       {
            _inventory = inventory;
           _payment = payment;
       }

       public void PlaceOrder(string productId, int quantity, string cardNumber, double unitPrice)
       {
            //comment this part when testing TDD to make it fail 
            //if (quantity <= 0)
            //    throw new ArgumentOutOfRangeException(nameof(quantity));

            if (!_inventory.HasStock(productId, quantity))
                throw new InvalidOperationException("Not enough stock.");

            _inventory.ReserveStock(productId, quantity);

            var total = quantity * unitPrice;
            _payment.Charge(cardNumber, total);
        }
    }


    public class InventoryService : IInventoryService
    {
        private readonly Dictionary<string, int> _stock = new Dictionary<string, int>
        {
            { "P001", 10 },
            { "P002", 5 }
        };

        public bool HasStock(string productId, int quantity)
            => _stock.TryGetValue(productId, out var current) && current >= quantity;

        public void ReserveStock(string productId, int quantity)
        {
            if (!HasStock(productId, quantity))
                throw new InvalidOperationException("Not enough stock to reserve.");

            _stock[productId] -= quantity;
        }
    }

    public class SimplePaymentService : IPaymentService
    {
        public bool LastChargeSucceeded { get; private set; }
        public double LastAmount { get; private set; }

        public void Charge(string cardNumber, double amount)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentException("Invalid card number.", nameof(cardNumber));

            // In a real system this would call an external payment gateway.
            // Here we just record the call so it’s easy to assert against in tests.
            LastChargeSucceeded = true;
            LastAmount = amount;
        }
    }

    public class FakePaymentService : IPaymentService
    {
        public bool WasCharged { get; private set; }
        public double LastAmount { get; private set; }

        public void Charge(string cardNumber, double amount)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentException("Invalid card number.");

            WasCharged = true;
            LastAmount = amount;
        }
    }
    public class FakeInventoryService : IInventoryService
    {
        private Dictionary<string, int> _stock = new Dictionary<string, int> { { "P001", 10 } };

        public bool HasStock(string id, int qty) =>
            _stock.ContainsKey(id) && _stock[id] >= qty;

        public void ReserveStock(string id, int qty) =>
            _stock[id] -= qty;
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            // you can add the scenario or order
            // of instructions that you like 
        }
    }
}
