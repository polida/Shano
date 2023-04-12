using System;
using System.Linq;



static Dictionary<char, string> ShannonFano(Dictionary<char, double> symbolProbabilities)
{
    Dictionary<char, string> codes = new Dictionary<char, string>(); // slovník kódových slov pro každý symbol

if (symbolProbabilities.Count == 1)
{
    char symbol = symbolProbabilities.Keys.First();
    codes[symbol] = "0"; // když máme jen jeden symbol, kódové slovo je 0
    return codes;
}

double totalProbabilities = symbolProbabilities.Values.Sum(); // celková pravděpodobnost všech symbolů
double halfProbabilities = 0;
int splitIndex = 0;

var orderedSymbols = symbolProbabilities.OrderByDescending(x => x.Value).ToList(); // seřazený seznam symbolů podle pravděpodobnosti

for (int i = 0; i < orderedSymbols.Count; i++)
{
    halfProbabilities += orderedSymbols[i].Value; // přičtení pravděpodobnosti symbolu k polovině celkových pravděpodobností

    if (halfProbabilities >= totalProbabilities / 2) // pokud jsou poloviny pravděpodobností dostatečně vyrovnané, nastavíme index rozdělení
    {
        splitIndex = i;
        break;
    }
}

// rekursivní aplikace kroků 2 a 3 na obě poloviny symbolů
var leftSymbols = orderedSymbols.GetRange(0, splitIndex + 1).ToDictionary(x => x.Key, x => x.Value);
var rightSymbols = orderedSymbols.GetRange(splitIndex + 1, orderedSymbols.Count - splitIndex - 1).ToDictionary(x => x.Key, x => x.Value);

var leftCodes = ShannonFano(leftSymbols);
var rightCodes = ShannonFano(rightSymbols);

// přidání "0" nebo "1" ke kódovým slovům symbolů v každé polovině
foreach (var code in leftCodes)
{
    codes[code.Key] = "0" + code.Value;
}

foreach (var code in rightCodes)
{
    codes[code.Key] = "1" + code.Value;
}

return codes;
}
Dictionary<char, double> VypoctiPravdepodobnosti(string vstupniSlovo)
{
    Dictionary<char, double> pravdepodobnosti = new Dictionary<char, double>();
    int pocetZnaku = vstupniSlovo.Length;

    foreach (char znak in vstupniSlovo)
    {
        if (pravdepodobnosti.ContainsKey(znak))
        {
            pravdepodobnosti[znak]++;
        }
        else
        {
            pravdepodobnosti[znak] = 1;
        }
    }

    foreach (char znak in pravdepodobnosti.Keys.ToList())
    {
        pravdepodobnosti[znak] /= pocetZnaku;
    }

    return pravdepodobnosti;
}



var pravdepodobnosti = VypoctiPravdepodobnosti(Console.ReadLine());
Console.WriteLine("Pravdepodobnost:");
foreach (var item in pravdepodobnosti)
{
    Console.WriteLine("{0}: {1}", item.Key, item.Value);
}
var kodova_slova = ShannonFano(pravdepodobnosti);
Console.WriteLine("Kodova slova(shannon):");
foreach(var item in kodova_slova)
{
    Console.WriteLine("{0}: {1}", item.Key, item.Value);
}
double  prumernaDelka = 0;
foreach (var item in kodova_slova)
{
    prumernaDelka += pravdepodobnosti[item.Key] * item.Value.Length;
}
Console.WriteLine("Velikost prumerné délky kódovaného slova: {0}", prumernaDelka);
