using System;
using System.Threading;

namespace WS_Memoire
{
    
    delegate int DELG2(int v);

    class Q2b
    {
        static void Main(string[] args)
        {
           
            DELG2 d = x =>
            {
                int res = x * x;
                Console.WriteLine($"The square of {x} is {res}");
                return res;
            };

            Thread[] threads = new Thread[10];

            for (int i = 0; i < threads.Length; i++)
            {
              
                int valeurSpecifique = i;

              
                threads[i] = new Thread(() => d(valeurSpecifique));
            }

            
            foreach (Thread t in threads)
            {
                t.Start();
                t.Join();
            }

            Console.WriteLine("Fin du programme.");
            Console.ReadKey();
        }
    }
}