using System.Text.Json;

namespace Estoque;

public static class Program
{
    private static string GetDadosCaminho(string arquivo)
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dados", arquivo));
    }

    public static void Main()
    {
        Console.WriteLine("=== Movimentações de estoque ===");

        var lCaminho = GetDadosCaminho("estoque.json");
        var lJson = File.ReadAllText(lCaminho);
        var lEstoque = JsonDocument.Parse(lJson).RootElement.GetProperty("estoque");
        var lista = new List<(int Codigo, string Descricao, int Quantidade)>();

        foreach (var item in lEstoque.EnumerateArray())
        {
            lista.Add((
                item.GetProperty("codigoProduto").GetInt32(),
                item.GetProperty("descricaoProduto").GetString() ?? string.Empty,
                item.GetProperty("estoque").GetInt32()));
        }

        foreach (var item in lista)
            Console.WriteLine($"{item.Codigo} - {item.Descricao} - {item.Quantidade} unidades");

        Console.Write("Digite o código do produto: ");
        var codigo = int.Parse(Console.ReadLine() ?? "0");

        var produto = lista.FirstOrDefault(x => x.Codigo == codigo);
        if (produto == default)
        {
            Console.WriteLine("Produto não encontrado.");
            return;
        }

        Console.Write("Digite o tipo da movimentação (Entrada/Saida): ");
        var tipo = Console.ReadLine() ?? string.Empty;
        Console.Write("Digite a quantidade: ");
        var quantidade = int.Parse(Console.ReadLine() ?? "0");

        if (quantidade <= 0)
        {
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        if (tipo.Equals("Entrada", StringComparison.OrdinalIgnoreCase))
        {
            produto = (produto.Codigo, produto.Descricao, produto.Quantidade + quantidade);
        }
        else if (tipo.Equals("Saida", StringComparison.OrdinalIgnoreCase))
        {
            if (quantidade > produto.Quantidade)
            {
                Console.WriteLine("Estoque insuficiente.");
                return;
            }

            produto = (produto.Codigo, produto.Descricao, produto.Quantidade - quantidade);
        }
        else
        {
            Console.WriteLine("Tipo inválido.");
            return;
        }

        var indice = lista.FindIndex(x => x.Codigo == codigo);
        lista[indice] = produto;

        Console.WriteLine($"Quantidade final em estoque: {produto.Quantidade}");
    }
}
