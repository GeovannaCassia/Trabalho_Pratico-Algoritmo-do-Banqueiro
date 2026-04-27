using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Se argumentos forem fornecidos, usa-os como recursos disponíveis.
        int[] resources;

        if (args.Length > 0)
        {
            resources = Array.ConvertAll(args, int.Parse);
        }
        else
        {
            // Valor padrão quando nenhum argumento é passado.
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

        // Cria uma thread para cada cliente e inicia sua execução.
        for (int i = 0; i < numberOfCustomers; i++)
        {
            Customer customer = new Customer(i, banker, numberOfResources);
            threads[i] = new Thread(customer.Run);
            threads[i].Start();
        }
    }
}