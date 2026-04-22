# Exercices C# - Programmation Avancée

Ce projet contient une série d'exercices en C# démontrant différents concepts de programmation avancée : délégués, expressions lambda, types anonymes, gestion de la mémoire (Heap vs Stack), et le pattern IDisposable.

## Prérequis

- .NET 9.0
- Visual Studio Code ou Visual Studio

## Compilation et exécution

Pour compiler tous les projets :
```bash
dotnet build
```

Pour exécuter un projet spécifique :
```bash
dotnet run --project "nom_du_projet"
```

## Exercice 1 : Délégué

### Énoncé
Soit la méthode `int methode(int v1, int v2)`. Cette méthode additionne deux valeurs et retourne le résultat. Écrire le délégué qui invoquera cette méthode.

### Implémentation
```csharp
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
```

### Explication
- Un délégué `MonDelegue` est déclaré pour représenter une méthode qui prend deux entiers et retourne un entier
- La méthode `Addition` est assignée au délégué
- L'invocation du délégué produit le résultat : 15

**Résultat :** `Résultat du délégué : 15`

## Exercice 2b : Expression Lambda

### Énoncé
Construire à l'aide d'une expression lambda une méthode qui calculera le carré d'un nombre.

### Implémentation
```csharp
Func<int, int> calculeCarre = x => x * x;
int nombre = 4;
int resultatCarre = calculeCarre(nombre);

Console.WriteLine($"Le carré de {nombre} est : {resultatCarre}");
```

### Explication
- Une expression lambda `x => x * x` est utilisée pour créer une fonction qui calcule le carré
- Le type `Func<int, int>` représente une fonction prenant un int et retournant un int
- L'expression lambda est exécutée avec la valeur 4

**Résultat :** `Le carré de 4 est : 16`

## Exercice 2a : Expression Lambda dans une boucle for et thread

### Énoncé
Analyser le code suivant, expliquer pourquoi le programme affiche 10 fois « The square of 9 is 81 ». Apporter une correction pour qu'il affiche le carré pour i allant de 0 à 9.

### Problème du code original
```csharp
for (int i = 0; i < Threads.Length; i++)
{
    j = i;
    Threads[j] = new Thread(() => d(j)); // Problème : closure sur j
}
```

**Pourquoi le problème ?**
La variable `j` est capturée par la closure lambda. Au moment où les threads s'exécutent, `j` vaut toujours 9 (dernière valeur de la boucle), donc tous les threads affichent le carré de 9.

### Correction appliquée
```csharp
for (int i = 0; i < threads.Length; i++)
{
    int valeurSpecifique = i; // Variable locale pour chaque itération
    threads[i] = new Thread(() => d(valeurSpecifique));
}
```

### Implémentation complète
```csharp
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
                int valeurSpecifique = i; // Correction : variable locale
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
```

### Explication
- Chaque thread reçoit une valeur spécifique grâce à une variable locale `valeurSpecifique`
- Les threads sont démarrés et attendus avec `Start()` et `Join()`
- Chaque thread calcule et affiche le carré de son numéro (0 à 9)

**Résultat :**
```
The square of 0 is 0
The square of 1 is 1
The square of 2 is 4
...
The square of 9 is 81
Fin du programme.
```

## Exercice 3 : Type anonyme

### Énoncé
Implémenter un type anonyme qui comporte un `int` et un `string`. Exposer son utilisation.

### Implémentation
```csharp
var etudiant = new { Id = 1, Nom = "Aymen" };

Console.WriteLine($"ID: {etudiant.Id}, Nom: {etudiant.Nom}");

// Note : Les propriétés d'un type anonyme sont en lecture seule (read-only).
```

### Explication
- Un objet anonyme est créé avec `new { Id = 1, Nom = "Aymen" }`
- Les propriétés sont automatiquement inférées par le compilateur
- Les types anonymes sont utiles pour des objets temporaires
- Les propriétés sont en lecture seule

**Résultat :** `ID: 1, Nom: Aymen`

## Exercice 4 : Délégués

### Énoncé
Utiliser les classes A et B fournies pour démontrer :
- Déclaration d'objets A et B
- Type delegate void/void
- Instance simple de délégué
- Tableau de délégués
- Délégué multicast
- Allocation mémoire et assignation d'adresses
- Invocation des différentes formes de délégués

### Implémentation
```csharp
using System;

namespace @delegate
{
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
            A objetA = new A();
            B objetB = new B();

            delg instanceSimple = new delg(objetA.ma);

            delg[] tableauDelegues = new delg[2];
            tableauDelegues[0] = objetA.ma;
            tableauDelegues[1] = objetB.mb;

            delg multicast = null;
            multicast += objetA.ma;
            multicast += objetB.mb;

            Console.WriteLine("--- Appels directs ---");
            objetA.ma();
            objetB.mb();

            Console.WriteLine("\n--- Invocation simple ---");
            instanceSimple();

            Console.WriteLine("\n--- Boucle sur le tableau ---");
            for (int i = 0; i < tableauDelegues.Length; i++)
            {
                tableauDelegues[i]();
            }

            Console.WriteLine("\n--- Invocation Multicast (ma + mb) ---");
            multicast();

            Console.WriteLine("\n--- Désabonnement de mb ---");
            multicast -= objetB.mb;
            multicast();
        }
    }
}
```

### Explication
- **Instance simple** : Un délégué pointant vers une seule méthode
- **Tableau de délégués** : Collection de délégués invoqués en boucle
- **Multicast** : Un délégué pouvant invoquer plusieurs méthodes (+= pour s'abonner, -= pour se désabonner)
- Les délégués multicast invoquent toutes les méthodes abonnées en séquence

**Résultat :**
```
--- Appels directs ---
ma
mb

--- Invocation simple ---
ma

--- Boucle sur le tableau ---
ma
mb

--- Invocation Multicast (ma + mb) ---
ma
mb

--- Désabonnement de mb ---
ma
```

## Exercice 5 : Heap vs Stack

### Énoncé
Après avoir complété le code, faire des comparaisons entre Heap et Stack concernant :
- La vitesse d'exécution
- Taille maximale (en jouant sur la valeur de largeSize)

### Implémentation
```csharp
using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int largeSize = 10000; // Réduit pour éviter StackOverflowException

        AllocateLargeArrayOnHeap(largeSize);
        AllocateLargeArrayOnStack(largeSize);
    }

    static void AllocateLargeArrayOnStack(int size)
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Span<int> largeArray = stackalloc int[size]; // Allocation sur le Stack

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

            int[] largeArray = new int[size]; // Allocation sur le Heap

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
            Console.WriteLine($"Heap allocation not possible: {ex.Message}");
        }
    }
}
```

### Comparaisons Heap vs Stack

| Aspect | Heap | Stack |
|--------|------|-------|
| **Vitesse** | Plus lent (allocation + GC) | Plus rapide (allocation directe) |
| **Taille maximale** | Très grande (limitée par RAM) | Petite (1-8 MB typiquement) |
| **Gestion** | Automatique (Garbage Collector) | Automatique (désallocation à la fin du scope) |
| **Syntaxe** | `new int[size]` | `stackalloc int[size]` ou `Span<T>` |
| **Thread-safe** | Oui | Non (lié au thread) |

**Résultat :** `Heap : 45 ms` / `Stack : 74 ms`

*Note : Le Stack peut sembler plus lent ici à cause de la copie Span<T> vers la pile, mais en général il est plus rapide pour les petites allocations.*

## Exercice 6 : IDisposable

### Énoncé
Le code ci-dessous n'utilise pas IDisposable pour gérer la connexion. Modifier la classe DatabaseConnection pour qu'elle implémente IDisposable. Simplifier ensuite Main avec l'utilisation de `using`.

### Implémentation
```csharp
using System;

public class DatabaseConnection : IDisposable // 1. Implémentation de l'interface
{
    private bool isOpen = false;

    public DatabaseConnection()
    {
        Console.WriteLine("Database connection opened.");
        isOpen = true;
    }

    public void ExecuteQuery(string query)
    {
        if (!isOpen) throw new InvalidOperationException("The connection is closed.");
        Console.WriteLine($"Executing query: {query}");
    }

    public void Close()
    {
        if (isOpen)
        {
            Console.WriteLine("Database connection closed.");
            isOpen = false;
        }
    }

    // 2. Méthode obligatoire de l'interface IDisposable
    public void Dispose()
    {
        Close(); // Appeler Close pour libérer la ressource
        GC.SuppressFinalize(this); // Indiquer au GC qu'il n'a plus besoin de surveiller
    }
}

class Program
{
    static void Main()
    {
        try
        {
            // Le bloc using gère tout le cycle de vie
            using (DatabaseConnection connection = new DatabaseConnection())
            {
                connection.ExecuteQuery("SELECT * FROM Users");
                // On peut faire d'autres opérations ici...
            } // Dispose() est appelé AUTOMATIQUEMENT ici
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Program finished.");
    }
}
```

### Explication
- **IDisposable** : Interface pour gérer les ressources non-managées
- **Dispose()** : Méthode appelée automatiquement à la fin du bloc `using`
- **using** : Garantit l'appel à Dispose() même en cas d'exception
- **GC.SuppressFinalize()** : Optimise le Garbage Collector

**Résultat :**
```
Database connection opened.
Executing query: SELECT * FROM Users
Database connection closed.
Program finished.
```

## Structure du projet

```
exos/
├── exos.sln
├── exos/           # Exercice 1 : Délégué
├── exo 2/          # Exercice 2b : Expression Lambda
├── exo3/           # Exercice 2a : Lambda + Threads
├── exo4/           # Exercice 3 : Type anonyme
├── exo 5/          # Exercice 5 : Heap vs Stack
├── exo 6/          # Exercice 6 : IDisposable
└── vrai exo 4/     # Exercice 4 : Délégués avancés
```

## Concepts clés abordés

- **Délégués** : Types référence pour méthodes
- **Expressions Lambda** : Fonctions anonymes
- **Types anonymes** : Objets sans déclaration de classe
- **Multicast Delegates** : Délégués invoquant plusieurs méthodes
- **Gestion mémoire** : Heap vs Stack
- **IDisposable** : Pattern de libération de ressources
- **Threading** : Programmation multi-thread
- **Closures** : Capture de variables dans les lambdas</content>
<parameter name="filePath">f:\exos\exos\README.md
