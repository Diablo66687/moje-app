using System;
using System.Collections.Generic;

namespace HanojskeVeze

class Program
{
    static void Main(string[] args)
    {
        const string validniOdpovedi = "123";
        const int celkovyPocetKotoucu = 5;
        bool vyhral = false;

        // Věže jsou pro jednodušší práci s nimi uložené ve slovníku
        Dictionary<int, Stack<int>> veze = new Dictionary<int, Stack<int>>();
        veze[1] = new Stack<int>();
        veze[2] = new Stack<int>();
        veze[3] = new Stack<int>();

        // Vytvoření kotoučů na první věži, 1 je nejmenší kotouč a je na vrcholu zásobníku
        for (int i = celkovyPocetKotoucu; i >= 1; i--)
            veze[1].Push(i);

        // Vykreslení věží
        VykresliVeze(veze, celkovyPocetKotoucu);

        while (!vyhral)
        {
            Console.Write("\nPřesunout kotouč z věže: ");
            // Získání věže, ze které se koutouč přesouvá
            Stack<int> pocatecniVez = veze[ZiskejOdpoved(validniOdpovedi)];

            Console.Write("\nPřesunout kotouč na věž: ");
            // Získání věže, na kterou se koutouč přesouvá
            Stack<int> cilovaVez = veze[ZiskejOdpoved(validniOdpovedi)];

            // Ověření, že věž, ze které se koutouč přesouvá, obsahuje nějaký kotouč
            if (pocatecniVez.Count != 0)
            {
                // Ověření, že přesouvaný kotouč je menší než vrchní kotouč věže, na kterou se koutouč přesouvá
                int posledniKotoucCiloveVeze = cilovaVez.Count > 0 ? cilovaVez.Peek() : celkovyPocetKotoucu + 1;
                if (pocatecniVez.Peek() < posledniKotoucCiloveVeze)
                {
                    // Přesunutí koutouče
                    int kotouc = pocatecniVez.Pop();
                    cilovaVez.Push(kotouc);
                }
            }

            // Kontrola výhry = jiná než první věž obsahuje všechny kotouče
            vyhral = veze[2].Count == celkovyPocetKotoucu || veze[3].Count == celkovyPocetKotoucu;

            // Vykreslení věží s přesunutým kotoučem
            VykresliVeze(veze, celkovyPocetKotoucu);
        }

        Console.WriteLine("\nVyhrál jsi!");

        Console.ReadKey();
    }

    static int ZiskejOdpoved(string validniOdpovedi)
    {
        string odpoved;
        bool jeValidniOdpoved;

        do
        {
            odpoved = Console.ReadLine();
            jeValidniOdpoved = validniOdpovedi.Contains(odpoved);

            if (!jeValidniOdpoved)
                Console.Write("\nZadej znovu: ");
        }
        while (!jeValidniOdpoved); // Výzva pro zadání se opakuje, dokud uživatel nezadá validní odpověď (1, 2 nebo 3)

        int odpovedInt;
        int.TryParse(odpoved, out odpovedInt);

        return odpovedInt;
    }

    static void VykresliVeze(Dictionary<int, Stack<int>> veze, int celkovyPocetKotoucu)
    {
        Console.Clear();
        Console.WriteLine("1".PadRight(celkovyPocetKotoucu * 2) + "2".PadRight(celkovyPocetKotoucu * 2) + "3");

        int[,] poleKotoucu = VytvorPoleKotoucu(veze, celkovyPocetKotoucu);

        for (int kotouc = 0; kotouc < poleKotoucu.GetLength(1); kotouc++)
        {
            for (int vez = 0; vez < poleKotoucu.GetLength(0); vez++)
            {
                // Řetězec, který představuje aktuálně vykreslovaný kotouč
                string kotoucRetezec = "";
                int celkovaDelkaRetezce = celkovyPocetKotoucu * 2;
                int delkaRetezceKotouce = poleKotoucu[vez, kotouc] * 2;

                // Vytvoření řětězce pro kotouč
                for (int i = 1; i <= celkovaDelkaRetezce; i++)
                {
                    if (i <= (celkovaDelkaRetezce - delkaRetezceKotouce) / 2 || i > (celkovaDelkaRetezce - delkaRetezceKotouce) / 2 + delkaRetezceKotouce)
                        kotoucRetezec += " ";
                    else
                        kotoucRetezec += "█";
                }
                Console.Write(kotoucRetezec);
            }
            // Přesunutí na další řádek po vykreslení všech kotoučů na aktuálním řádku
            Console.WriteLine();
        }
    }

    static int[,] VytvorPoleKotoucu(Dictionary<int, Stack<int>> veze, int celkovyPocetKotoucu)
    {
        // Vytvoření dvourozměrného pole, které udává, kde se nacházejí jednotlivé kotouče ([věž, řádek])
        int[,] pole = new int[3, celkovyPocetKotoucu];
        int poziceVeze = 0;

        foreach (Stack<int> vez in veze.Values)
        {
            // Pozice, na nichž se žádný kotouč nenachází, zůstanou nevyplněné = 0
            // Začíná se až od pozice, kde je nějaký kotouč
            int radek = celkovyPocetKotoucu - vez.Count;
            foreach (int kotouc in vez)
            {
                pole[poziceVeze, radek] = kotouc;
                radek++;
            }
            poziceVeze++;
        }
        return pole;
    }
}
}
