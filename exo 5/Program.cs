using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // On commence par une valeur raisonnable (ex: 10 000) 
        // car 300 000 risque de faire planter le Stack immédiatement.
        const int largeSize = 10000;

        AllocateLargeArrayOnHeap(largeSize);
        AllocateLargeArrayOnStack(largeSize);
    }

    static void AllocateLargeArrayOnStack(int size)
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // TO DO: Allocation sur le STACK
            // stackalloc réserve la mémoire directement dans la pile d'exécution
            Span<int> largeArray = stackalloc int[size];

            for (int j = 0; j < 2000; j++)
            {
                largeArray[0] = 0;
                for (int i = 1; i < largeArray.Length; i++)
                {
                    largeArray[i] = largeArray[i - 1];
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Stack : {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Stack allocation not possible: {ex.Message}");
        }
    }

    static void AllocateLargeArrayOnHeap(int size)
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // TO DO: Allocation sur le HEAP (Tas)
            // L'opérateur 'new' alloue toujours sur le Heap en C#
            int[] largeArray = new int[size];

            for (int j = 0; j < 2000; j++)
            {
                largeArray[0] = 0;
                for (int i = 1; i < largeArray.Length; i++)
                {
                    largeArray[i] = largeArray[i - 1];
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Heap : {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            // Note: Une erreur de Heap lèvera souvent une OutOfMemoryException
            Console.WriteLine($"Heap allocation not possible: {ex.Message}");
        }
    }
}