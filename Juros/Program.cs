using System.Globalization;

namespace Juros;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Calcular juros ===");

        Console.Write("Digite o valor: ");
        var valor = decimal.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);
        Console.Write("Digite a data de vencimento (dd/MM/yyyy): ");
        var entradaData = Console.ReadLine() ?? string.Empty;

        if (!DateTime.TryParseExact(entradaData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var vencimento))
        {
            Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
            return;
        }

        if (valor <= 0)
        {
            Console.WriteLine("Valor inválido.");
            return;
        }

        var hoje = DateTime.Today;
        var diasAtraso = (hoje - vencimento).Days;

        if (vencimento > hoje)
        {
            Console.WriteLine("Sem juros: a data de vencimento ainda não chegou.");
            return;
        }

        if (diasAtraso <= 0)
        {
            Console.WriteLine("Sem juros: hoje é o mesmo dia do vencimento.");
            return;
        }

        var juros = valor * 0.025m * diasAtraso;
        Console.WriteLine($"Dias em atraso: {diasAtraso}");
        Console.WriteLine($"Juros: {juros:C}");
        Console.WriteLine($"Total: {(valor + juros):C}");
    }
}
