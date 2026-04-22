using System;

namespace @delegate
{
    // 2. Déclarer un type delegate de signature void/void
    public delegate void delg();

    public class A
    {
        public void ma() { Console.WriteLine("ma"); }
    }

    public class B
    {
        public void mb() { Console.WriteLine("mb"); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1, 6 & 7. Déclarer et allouer de la mémoire pour les objets A et B
            A objetA = new A();
            B objetB = new B();

            // 3 & 8. Instance simple de délégué avec l'adresse de ma
            delg instanceSimple = new delg(objetA.ma);

            // 4 & 9. Tableau de délégués (taille 2)
            delg[] tableauDelegues = new delg[2];
            tableauDelegues[0] = objetA.ma; // Première cellule
            tableauDelegues[1] = objetB.mb; // Deuxième cellule

            // 5 & 10. Délégué Multicast (Abonnements)
            delg multicast = null;
            multicast += objetA.ma; // Premier abonnement
            multicast += objetB.mb; // Deuxième abonnement

            // --- EXÉCUTION ---

            Console.WriteLine("--- Appels directs ---");
            objetA.ma(); // Appeler ma sur l'objet A
            objetB.mb(); // Appeler mb sur l'objet B

            Console.WriteLine("\n--- Invocation simple ---");
            instanceSimple();

            Console.WriteLine("\n--- Boucle sur le tableau ---");
            for (int i = 0; i < tableauDelegues.Length; i++)
            {
                tableauDelegues[i](); // Invoque ma puis mb
            }

            Console.WriteLine("\n--- Invocation Multicast (ma + mb) ---");
            multicast();

            Console.WriteLine("\n--- Désabonnement de mb ---");
            multicast -= objetB.mb; // Désabonnement
            multicast(); // N'invoquera plus que ma

            Console.ReadLine();
        }
    }
}