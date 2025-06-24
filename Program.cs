
class Produto
{
    public string? Gtin;
    public string? Descricao;
    public double PrecoVarejo;
    public double? PrecoAtacado;
    public int? QtdeAtacado;
}

class VendaItem
{
    public string? Gtin;
    public int Quantidade;
    public double Parcial;
}

class Program
{
    static void Main()
    {
        List<Produto> catalogo = new List<Produto>
        {
            new Produto { Gtin = "7891024110348", Descricao = "SABONETE OLEO DE ARGAN 90G PALMOLIVE", PrecoVarejo = 2.88, PrecoAtacado = 2.51, QtdeAtacado = 12 },
            new Produto { Gtin = "7891048038017", Descricao = "CHÁ DE CAMOMILA DR.OETKER", PrecoVarejo = 4.40, PrecoAtacado = 4.37, QtdeAtacado = 3 },
            new Produto { Gtin = "7896066334509", Descricao = "TORRADA TRADICIONAL WICKBOLD", PrecoVarejo = 5.19 },
            new Produto { Gtin = "7891700203142", Descricao = "BEBIDA SOJA MAÇÃ ADES", PrecoVarejo = 2.39, PrecoAtacado = 2.38, QtdeAtacado = 6 },
            new Produto { Gtin = "7894321711263", Descricao = "TODDY PÓ ORIGINAL", PrecoVarejo = 9.79 },
            new Produto { Gtin = "7896001250611", Descricao = "ADOÇANTE LINEA", PrecoVarejo = 9.89, PrecoAtacado = 9.10, QtdeAtacado = 10 },
            new Produto { Gtin = "7793306013029", Descricao = "CEREAL SUCRILHOS", PrecoVarejo = 12.79, PrecoAtacado = 12.35, QtdeAtacado = 3 },
            new Produto { Gtin = "7896004400914", Descricao = "COCO RALADO SOCOCO", PrecoVarejo = 4.20, PrecoAtacado = 4.05, QtdeAtacado = 6 },
            new Produto { Gtin = "7898080640017", Descricao = "LEITE ITALAC", PrecoVarejo = 6.99, PrecoAtacado = 6.89, QtdeAtacado = 12 },
            new Produto { Gtin = "7891025301516", Descricao = "DANONINHO", PrecoVarejo = 12.99 },
            new Produto { Gtin = "7891030003115", Descricao = "CREME DE LEITE MOCOCA", PrecoVarejo = 3.12, PrecoAtacado = 3.09, QtdeAtacado = 4 },
        };

        List<VendaItem> vendas = new List<VendaItem>
        {
            new VendaItem { Gtin = "7891048038017", Quantidade = 1, Parcial = 4.40 },
            new VendaItem { Gtin = "7896004400914", Quantidade = 4, Parcial = 16.80 },
            new VendaItem { Gtin = "7891030003115", Quantidade = 1, Parcial = 3.12 },
            new VendaItem { Gtin = "7891024110348", Quantidade = 6, Parcial = 17.28 },
            new VendaItem { Gtin = "7898080640017", Quantidade = 24, Parcial = 167.76 },
            new VendaItem { Gtin = "7896004400914", Quantidade = 8, Parcial = 33.60 },
            new VendaItem { Gtin = "7891700203142", Quantidade = 8, Parcial = 19.12 },
            new VendaItem { Gtin = "7891048038017", Quantidade = 1, Parcial = 4.40 },
            new VendaItem { Gtin = "7793306013029", Quantidade = 3, Parcial = 38.37 },
            new VendaItem { Gtin = "7896066334509", Quantidade = 2, Parcial = 10.38 },
        };

        var agrupado = vendas
            .GroupBy(v => v.Gtin)
            .Select(g =>
            {
                var prod = catalogo.FirstOrDefault(p => p.Gtin == g.Key);
                int totalQtd = g.Sum(x => x.Quantidade);
                double subtotal = g.Sum(x => x.Parcial);
                double desconto = 0;

                if (prod != null && prod.PrecoAtacado.HasValue && prod.QtdeAtacado.HasValue)
                {
                    int gruposAtacado = totalQtd / prod.QtdeAtacado.Value;
                    desconto = gruposAtacado * prod.QtdeAtacado.Value * (prod.PrecoVarejo - prod.PrecoAtacado.Value);
                }

                return new
                {
                    Gtin = g.Key,
                    Desconto = Math.Round(desconto, 2),
                    Subtotal = subtotal
                };
            });

        double totalDesconto = agrupado.Sum(x => x.Desconto);
        double totalBruto = vendas.Sum(x => x.Parcial);
        double totalFinal = totalBruto - totalDesconto;

        Console.WriteLine("Produto\t\tDesconto");
        foreach (var item in agrupado.Where(x => x.Desconto > 0))
            Console.WriteLine($"{item.Gtin}\tR$ {item.Desconto:F2}");

        Console.WriteLine("\nTotal\tValor");
        Console.WriteLine($"(+) Subtotal\tR$ {totalBruto:F2}");
        Console.WriteLine($"(-) Descontos\tR$ {totalDesconto:F2}");
        Console.WriteLine($"(=) Total\tR$ {totalFinal:F2}");
    }
}