using System;
using System.Linq;

public class Banker
{
    private int[] available;
    private int[,] maximum;
    private int[,] allocation;
    private int[,] need;

    private int numCustomers;
    private int numResources;

    private readonly object _lock = new object();

    public Banker(int[] resources, int[,] max)
    {
        available = (int[])resources.Clone();
        maximum = max;

        numCustomers = max.GetLength(0);
        numResources = max.GetLength(1);

        allocation = new int[numCustomers, numResources];
        need = new int[numCustomers, numResources];

        // need = maximum
        for (int i = 0; i < numCustomers; i++)
            for (int j = 0; j < numResources; j++)
                need[i, j] = maximum[i, j];
    }

    public int RequestResources(int customer, int[] request)
    {
        lock (_lock)
        {
            for (int i = 0; i < numResources; i++)
            {
                if (request[i] > need[customer, i])
                    return -1;

                if (request[i] > available[i])
                    return -1;
            }

            for (int i = 0; i < numResources; i++)
            {
                available[i] -= request[i];
                allocation[customer, i] += request[i];
                need[customer, i] -= request[i];
            }

            if (IsSafe())
            {
                PrintState($"✔ Cliente {customer} ALOCADO");
                return 0;
            }
            else
            {
                for (int i = 0; i < numResources; i++)
                {
                    available[i] += request[i];
                    allocation[customer, i] -= request[i];
                    need[customer, i] += request[i];
                }

                PrintState($"❌ Cliente {customer} NEGADO (inseguro)");
                return -1;
            }
        }
    }

    public int ReleaseResources(int customer, int[] release)
    {
        lock (_lock)
        {
            for (int i = 0; i < numResources; i++)
            {
                if (release[i] > allocation[customer, i])
                    return -1;

                available[i] += release[i];
                allocation[customer, i] -= release[i];
                need[customer, i] += release[i];
            }

            PrintState($"🔄 Cliente {customer} LIBEROU");
            return 0;
        }
    }

    private bool IsSafe()
    {
        int[] work = (int[])available.Clone();
        bool[] finish = new bool[numCustomers];

        while (true)
        {
            bool found = false;

            for (int i = 0; i < numCustomers; i++)
            {
                if (!finish[i])
                {
                    bool canExecute = true;

                    for (int j = 0; j < numResources; j++)
                    {
                        if (need[i, j] > work[j])
                        {
                            canExecute = false;
                            break;
                        }
                    }

                    if (canExecute)
                    {
                        for (int j = 0; j < numResources; j++)
                            work[j] += allocation[i, j];

                        finish[i] = true;
                        found = true;
                    }
                }
            }

            if (!found)
                break;
        }

        return finish.All(f => f);
    }

    private void PrintState(string message)
    {
        Console.WriteLine($"\n=== {message} ===");
        Console.WriteLine("Available: " + string.Join(", ", available));
    }
}