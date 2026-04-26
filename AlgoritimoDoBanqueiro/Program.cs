using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int[] resources;

        if (args.Length > 0)
        {
            resources = Array.ConvertAll(args, int.Parse);
        }
        else
        {
            resources = new int[] { 10, 5, 7 }; 
        }

        int numberOfCustomers = 5;
        int numberOfResources = resources.Length;

        int[,] maximum = new int[,]
        {
            {7,5,3},
            {3,2,2},
            {9,0,2},
            {2,2,2},
            {4,3,3}
        };

        Banker banker = new Banker(resources, maximum);

        Thread[] threads = new Thread[numberOfCustomers];

        for (int i = 0; i < numberOfCustomers; i++)
        {
            Customer customer = new Customer(i, banker, numberOfResources);
            threads[i] = new Thread(customer.Run);
            threads[i].Start();
        }
    }
}