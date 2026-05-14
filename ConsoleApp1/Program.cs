using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Prueba de Tasks & Concurrencia\n");

        // 1. Datos de entrada para la simulación
        var idsParaProcesar = Enumerable.Range(1, 10).ToList();

        // 2. Colección concurrente para guardar resultados de forma segura desde múltiples hilos
        var resultadosSeguros = new ConcurrentBag<string>();

        Console.WriteLine($"Procesando {idsParaProcesar.Count} elementos...\n");

        // 3. Paralelización: Usamos Task.Run para no bloquear el hilo principal y dentro procesamos en paralelo.
        await Task.Run(() =>
        {
            Parallel.ForEach(idsParaProcesar, id =>
            {
                // 4. Llamada a un método asíncrono simulado
                string resultado = ProcesarDatoAsync(id).GetAwaiter().GetResult();

                // 5. Guardado seguro en la colección concurrente
                resultadosSeguros.Add(resultado);
            });
        });

        Console.WriteLine("\nResultados:");
        foreach (var res in resultadosSeguros)
        {
            Console.WriteLine(res);
        }

        Console.WriteLine("\nFinalizado.");
    }

    // 3. Uso de async y await
    static async Task<string> ProcesarDatoAsync(int id)
    {
        // Simulamos una tarea que consume tiempo (como una petición API o DB)
        await Task.Delay(1000);

        string mensaje = $"[ID: {id}] procesado en el Hilo #{Environment.CurrentManagedThreadId}";
        Console.WriteLine($"   Completado: {mensaje}");

        return mensaje;
    }
}