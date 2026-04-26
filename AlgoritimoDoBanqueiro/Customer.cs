using System;
using System.Threading;

public class Customer
{
    private int id;
    private Banker banker;
    private int numResources;
    private Random rand;

    public Customer(int id, Banker banker, int numResources)
    {
        this.id = id;
        this.banker = banker;
        this.numResources = numResources;
        this.rand = new Random(id * DateTime.Now.Millisecond);
    }

    public void Run()
    {
        while (true)
        {
            Thread.Sleep(rand.Next(500, 1500));

            int[] request = GenerateRequest();

            if (banker.RequestResources(id, request) == 0)
            {
                Console.WriteLine($"Cliente {id} conseguiu recursos");

                Thread.Sleep(rand.Next(500, 1500));

                banker.ReleaseResources(id, request);
            }
            else
            {
                Console.WriteLine($"Cliente {id} teve pedido negado");
            }
        }
    }

    private int[] GenerateRequest()
    {
        int[] req = new int[numResources];

        for (int i = 0; i < numResources; i++)
        {
            req[i] = rand.Next(0, 3);
        }

        return req;
    }
}