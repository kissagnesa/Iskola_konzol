using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Tanulo
{
    public int Ev { get; set; }
    public string Osztaly { get; set; }
    public string Nev { get; set; }
    public string Azonosito { get; set; }

    public override string ToString()
    {
        return $"{Ev} {Osztaly} {Nev} - Azonosító: {Azonosito}";
    }
}

public class Iskola
{
    public static void Main(string[] args)
    {
        List<Tanulo> tanulok = BeolvasTanulok("nevek.txt");

        KiirTanulok(tanulok);

        Console.WriteLine($"Tanulók száma: {tanulok.Count}");

        GeneralAzonositok(tanulok);

        Console.WriteLine($"Első tanuló azonosítója: {tanulok.First().Azonosito}");
        Console.WriteLine($"Utolsó tanuló azonosítója: {tanulok.Last().Azonosito}");

        MentesAzonositok(tanulok, "azonosítók.txt");

        Console.ReadKey();
    }

    public static List<Tanulo> BeolvasTanulok(string fajlnev)
    {
        List<Tanulo> tanulok = new List<Tanulo>();
        try
        {
            foreach (string sor in File.ReadLines(fajlnev))
            {
                string[] adatok = sor.Split(';');
                if (adatok.Length == 3)
                {
                    tanulok.Add(new Tanulo
                    {
                        Ev = int.Parse(adatok[0]),
                        Osztaly = adatok[1],
                        Nev = adatok[2]
                    });
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("A 'nevek.txt' fájl nem található.");
        }
        return tanulok;
    }

    public static void KiirTanulok(List<Tanulo> tanulok)
    {
        foreach (Tanulo tanulo in tanulok)
        {
            Console.WriteLine(tanulo);
        }
    }

    public static void GeneralAzonositok(List<Tanulo> tanulok)
    {
        foreach (Tanulo tanulo in tanulok)
        {
            tanulo.Azonosito = GeneralAzonosito(tanulo);            
        }
    }

    public static string GeneralAzonosito(Tanulo tanulo)
    {
        string adat = $"{tanulo.Ev}{tanulo.Osztaly}{tanulo.Nev}";
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(adat));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString().Substring(0, 8);
        }
    }

    public static void MentesAzonositok(List<Tanulo> tanulok, string fajlnev)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(fajlnev))
            {
                foreach (Tanulo tanulo in tanulok)
                {
                    writer.WriteLine($"{tanulo.Nev} {tanulo.Azonosito}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a fájlmentés során: {ex.Message}");
        }
    }
}