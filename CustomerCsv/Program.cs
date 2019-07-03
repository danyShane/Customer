using CsvHelper;
using CustomerCsv.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomerCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                object lockToread = new object();

                List<CustomersCsv> lCustomers;

                lock (lockToread)
                {
                    TextReader reader = new StreamReader("Customers.csv", Encoding.GetEncoding("iso-8859-1"));
                    var csvReader = new CsvReader(reader);
                    var customers = csvReader.GetRecords<CustomersCsv>();

                    lCustomers = customers.ToList();
                }

                var context = new NorthwindContext(); using (context)
                {

                    var listaCustomersdb = context.CustomersCsv.ToList();

                    foreach (var item in lCustomers)
                    {
                        if (listaCustomersdb.Exists(c => c.Id == item.Id))
                        {
                            Console.WriteLine("Customer with Id: " + item.Id +" already exist.");
                        }
                        else
                        {
                            context.CustomersCsv.Add(item);
                            Console.WriteLine("Customer: " + item.Name + " just added.");
                        }
                    }

                    context.SaveChanges();

                    Console.WriteLine("Pulse ENTER para terminar.");
                    Console.ReadLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
