using System.Text.Json;

namespace Comissao;

public static class Program
{
    private static string GetCaminhoDados(string arquivo)
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dados", arquivo));
    }

    public static void Main()
    {
        Console.WriteLine("=== Cálculo de comissão ===");

        var lJson = File.ReadAllText(GetCaminhoDados("vendas.json"));
        var lVendas = JsonDocument.Parse(lJson).RootElement.GetProperty("estoque");
        var lComissoes = new Dictionary<string, decimal>();

        foreach (var item in lVendas.EnumerateArray())
        {
            var vendedor = item.GetProperty("vendedor").GetString() ?? "Desconhecido";
            var valor = item.GetProperty("valor").GetDecimal();

            if (!lComissoes.ContainsKey(vendedor))
                lComissoes[vendedor] = 0m;

            var comissao = valor switch
            {
                >= 500m => valor * 0.05m,
                >= 100m => valor * 0.01m,
                _ => 0m
            };

            lComissoes[vendedor] += comissao;
        }

        foreach (var item in lComissoes)
            Console.WriteLine($"{item.Key}: {item.Value:C}");
    }
}
