using System;

public delegate int MonDelegue(int a, int b);

class Program
{

    static int Addition(int v1, int v2)
    {
        return v1 + v2;
    }

    static void Main(string[] args)
    {

        MonDelegue operation = Addition;


        int resultat = operation(5, 10);

        Console.WriteLine($"Résultat du délégué : {resultat}");
    }
}