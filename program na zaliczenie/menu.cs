using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using DnsClient;

public class Menu
{
    public Menu()
    {
        Console.WriteLine("Witamy w serwisie e-flota! Wybierz co chcesz zrobić.");
        Console.WriteLine();
        Console.WriteLine("1 - dodać, edytować, usunąć auta amerykańskie ");
        Console.WriteLine("2 - dodać, edytować, usunąć auta niemieckie ");
        Console.WriteLine();
    }

    public void menu2()
    {
        Console.WriteLine("1. Dodaj samochód");
        Console.WriteLine("2. Edytuj samochód");
        Console.WriteLine("3. Usuń samochód");
        Console.WriteLine("4. Wyświetl samochody");
        Console.WriteLine("5. Wyjście");
        Console.WriteLine();
    }
    
}
public class Ameryka
{
    static MongoClient client = new MongoClient("mongodb://localhost:27017");
    static IMongoDatabase database = client.GetDatabase("CarDatabase");
    IMongoCollection<BsonDocument> americanCarsCollection = database.GetCollection<BsonDocument>("AmericanCars");

    public void AddAmericanCar()
    {
        Console.Write("Podaj markę amerykańskiego samochodu: ");
        string brand = Console.ReadLine();
        Console.Write("Podaj model amerykańskiego samochodu: ");
        string model = Console.ReadLine();

        var car = new BsonDocument
        {
            { "Marka", brand },
            { "Model", model },
        };

        americanCarsCollection.InsertOne(car);

        Console.WriteLine("Amerykański samochód dodany pomyślnie!");
    }

     public void EditAmericanCar()
    {
        Console.Write("Podaj markę amerykańskiego samochodu do edycji: ");
        string brand = Console.ReadLine();

        Console.Write("Podaj model amerykańskiego samochodu do edycji: ");
        string model = Console.ReadLine();

        Console.Write("Podaj nową markę amerykańskiego samochodu: ");
        string newBrand = Console.ReadLine();

        Console.Write("Podaj nowy model amerykańskiego samochodu: ");
        string newModel = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("Marka", brand);
        var updatebrand = Builders<BsonDocument>.Update.Set("Marka", newBrand);
        var updatemodel = Builders<BsonDocument>.Update.Set("Model", newModel);

        var result = americanCarsCollection.UpdateOne(filter, updatebrand);
        var result1 = americanCarsCollection.UpdateOne(filter, updatemodel);

        if (result.ModifiedCount > 0)
            Console.WriteLine("Amerykański samochód zaktualizowany pomyślnie!");
        else
            Console.WriteLine("Nie znaleziono amerykańskiego samochodu o podanej marce.");
    }

     public void DeleteAmericanCar()
    {
        Console.Write("Podaj markę amerykańskiego samochodu do usunięcia: ");
        string brand = Console.ReadLine();
        Console.Write("Podaj model amerykańskiego samochodu do usunięcia: ");
        string model = Console.ReadLine();

        var filter = Builders<BsonDocument>.Filter.Eq("Marka", brand);
        var filter1 = Builders<BsonDocument>.Filter.Eq("Model", model);

        var result = americanCarsCollection.DeleteOne(filter);

        if (result.DeletedCount > 0)
            Console.WriteLine("Amerykański samochód usunięty pomyślnie!");
        else
            Console.WriteLine("Nie znaleziono amerykańskiego samochodu o podanej marce.");
    }

    public void DisplayAmericanCars()
    {
        var cars = americanCarsCollection.Find(new BsonDocument()).ToList();

        Console.WriteLine("Amerykańskie samochody:");

        foreach (var car in cars)
        {
            Console.WriteLine($"Marka: {car["Marka"]}, Model: {car["Model"]}");
        }
    }
}
      
class Program
{
    static void Main()
    {
        Menu menu = new Menu();

        Console.Write("Wybierz opcję: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        menu.menu2();
        Console.Write("Wybierz opcję: ");
        int choice2 = Convert.ToInt32(Console.ReadLine());

        if (choice == 1)
        {
            Ameryka ameryka = new Ameryka();

            if (choice2 == 1)
            {
                ameryka.AddAmericanCar();
            }
            if (choice2 == 2) 
            {
                ameryka.EditAmericanCar();
            }
            if (choice2 == 3) 
            {
                ameryka.DeleteAmericanCar();
            }
            if (choice2 == 4)
            {
                ameryka.DisplayAmericanCars();
            }
            if (choice2 == 5)
            {
                Environment.Exit(0);
            }
        }
        if (choice == 2)
        {
            Niemcy niemcy = new Niemcy();

            if (choice2 == 1)
            {
                niemcy.AddGermanCar();
            }
            //if (choice2 == 2)
            //{
            //    niemcy.EditGermanCar();
            //}
            //if (choice2 == 3)
            //{
            //    ameryka.DeleteAmericanCar();
            //}
            //if (choice2 == 4)
            //{
            //    ameryka.DisplayAmericanCars();
            //}
            if (choice2 == 5)
            {
                Environment.Exit(0);
            }
        }
    }    
}
public class Niemcy
{
    public static string nazwaSerwera = @"DESKTOP-9UQGKV3";
    public static string nazwaBazyDanych = "BazaSamochodowNiemieckich";
    public static string ustawieniaPolaczenia = @"Data Source=" + @"DESKTOP-9UQGKV3" + ";Initial Catalog=" + "AutaNiemieckie" + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public StringBuilder dane = new StringBuilder();

    public string zapytanie;

    public SqlConnection polaczenie = new SqlConnection(ustawieniaPolaczenia);

    public void AddGermanCar()
    {
        try
        {
            polaczenie.Open();

            Console.Write("Podaj markę niemieckiego samochodu: ");
            string brand = Console.ReadLine();
            Console.Write("Podaj model niemieckiego samochodu: ");
            string model = Console.ReadLine();

            dane.Clear();  // Wyczyszczenie poprzednich danych
            dane.Append("INSERT INTO SamochodyNiemieckie (Marka, Model) VALUES (@Marka, @Model)");

            zapytanie = dane.ToString();

            using (SqlCommand komenda = new SqlCommand(zapytanie, polaczenie))
            {
                komenda.Parameters.AddWithValue("@Marka", brand);
                komenda.Parameters.AddWithValue("@Model", model);

                komenda.ExecuteNonQuery();
            }

            Console.WriteLine("Niemiecki samochód dodany pomyślnie!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Wystąpił błąd: " + ex.Message);
        }
        finally
        {
            polaczenie.Close();
        }
    }
}


