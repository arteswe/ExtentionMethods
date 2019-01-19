using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace ExtentionMethods
{
    class Program
    {
        //smidigt att man anropa en extention istället för att göra beräkningen här eller i en metod
        //tänk 3:e parts klasser eller t.ex. DataTabel export eller dyl som används på många ställen
        //readable, går lätt att upptäcka med intellisense (jämför med en method i en annan klass)

        static void Main(string[] args)
        {
            Order[] orders =
            {
                new Order(1, new decimal(133.00), "xyz"),
                new Order(1, new decimal(23.10), "abc"),
                new Order(1, new decimal(23.30), "def")
            };

            var orderGroup = new OrderGroup();
            orderGroup.Orders = orders;

            //man kan använda denna metod på alla typer som implementera IEnumerable<Order>
            ////även på denna enkla array orders
            Console.WriteLine(orderGroup.Total());
            Console.WriteLine(orders.FilterByPrice(30).Total());

            //returnerar totalen för alla ordrar som börjar på a 
            Func<Order,bool> nameFilter = delegate(Order order)
            {
                return order?.orderName()?[0] == 'a';

            };
            Console.WriteLine(orderGroup.Orders.Filter(nameFilter).Total());

            Console.Read();
        }
    }

    public static class ExtentionMethodsForOrders
    {
        public static string orderName(this Order order)
        {
            return order.Name;
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

        public static IEnumerable<Order> FilterByPrice(this IEnumerable<Order> orders, decimal minPrice)
        {
            foreach (Order o in orders)
            {
                if((o?.Price ?? 0) >= minPrice){
                    yield return o;
                }
            }
        }

        //elegant med en delegat funktion 
        public static IEnumerable<Order> Filter(this IEnumerable<Order> orders, Func<Order, bool> selector)
        {
            foreach (var order in orders)
            {
                if (selector(order))
                {
                    yield return order;
                }
            }
        }


    }
    public class Order
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }

        public Order(int id, decimal price, string name)
        {
            Id = id;
            Name = name;
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
