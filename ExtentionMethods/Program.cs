using System;
using System.Collections;
using System.Collections.Generic;

namespace ExtentionMethods
{
    class Program
    {
        static void Main(string[] args)
        {
            Order[] orders =
            {
                new Order(1, new decimal(23.00)),
                new Order(1, new decimal(23.10)),
                new Order(1, new decimal(23.30))
            };

            var orderGroup = new OrderGroup();
            orderGroup.Orders = orders;

            //smidigt att man anropa en extention istället för att göra beräkningen här eller i en metod
            //tänk 3:e parts klasser eller t.ex. DataTabel export eller dyl som används på många ställen
            //readable, går lätt att upptäcka med intellisense (jämför med en method i en annan klass)

            //decimal total = orderGroup.Total();

            Console.Write(orderGroup.Total());
            Console.Write("...");
            Console.Read();
        }
    }

    public static class ExtentionMethodsForOrders
    {
        public static string orderName(this Order order)
        {
            return "no name Order";
        }

        public static decimal Total(this IEnumerable<Order> orders)
        {
            decimal total = 0;
            foreach (var o in orders)
            {
                total += o.Price;
            }
            return total;
        }
    }

    public class Order
    {
        public decimal Price { get; set; }
        public int Id { get; set; }

        public Order(int id, decimal price)
        {
            Id = id;
            Price = price;
        }
    }
    
    public class OrderGroup : IEnumerable<Order>
    {
        public IEnumerable<Order> Orders { get; set; }

        public IEnumerator<Order> GetEnumerator()
        {
            return Orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
