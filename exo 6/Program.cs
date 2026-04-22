public class DatabaseConnection : IDisposable // 1. On implémente l'interface
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
        // On appelle Close pour s'assurer que la ressource est libérée
        Close();
        // On indique au Garbage Collector qu'il n'a plus besoin de surveiller cet objet
        GC.SuppressFinalize(this);
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

            } // <--- Dispose() est appelé AUTOMATIQUEMENT ici (à la sortie des accolades)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("Program finished.");
    }
}