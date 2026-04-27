using System;
using System.Linq;

public class Banker
{
    // Recursos atualmente disponíveis no sistema.
    private int[] available;

    // Quantidade máxima que cada cliente pode solicitar.
    private int[,] maximum;

    // Recursos que já foram alocados para cada cliente.
    private int[,] allocation;

    // Recursos que cada cliente ainda precisa para completar.
    private int[,] need;

    private int numCustomers;
    private int numResources;

    // Objeto para garantir exclusão mútua entre threads.
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
            // Verifica se o pedido é válido e se existem recursos disponíveis.
            for (int i = 0; i < numResources; i++)
            {
                if (request[i] > need[customer, i])
                    return -1;

                if (request[i] > available[i])
                    return -1;
            }

            // Reserva temporariamente os recursos.
            for (int i = 0; i < numResources; i++)
            {
                available[i] -= request[i];
                allocation[customer, i] += request[i];
                need[customer, i] -= request[i];
            }

            // Testa se o novo estado ainda é seguro.
            if (IsSafe())
            {
                PrintState($"✔ Cliente {customer} ALOCADO");
                return 0;
            }
            else
            {
                // Se não é seguro, desfaz a alocação.
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
        // Verifica se existe uma sequência segura de clientes que podem terminar.
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