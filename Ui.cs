using System;
using System.Runtime.CompilerServices;

class Ui
{
    string actionOutput = "";

    public void DefaultConsoleOutput(string output)
    {
        actionOutput = output;
    }

    public void MainMenu(int money)
    {
        Console.Clear();
        Console.WriteLine("\n=== FARM GAME ===");
        Console.WriteLine($"💰 Argent : {money}");
        Console.WriteLine("");
        Console.WriteLine("1 - Planter une graine");
        Console.WriteLine("2 - Voir inventaire");
        Console.WriteLine("3 - Récolter");
        Console.WriteLine("4 - Shop");
        Console.WriteLine("5 - Quitter");
        Console.WriteLine("");
        if (!string.IsNullOrEmpty(actionOutput))
        {
            Console.WriteLine(actionOutput);
            actionOutput = "";
        }
        Console.WriteLine("\n📜 Quête actuelle :");
    }
    
    public void QuestInfo(Quest quest, Func<int, Item> GetItem)
    {
        if (!quest.IsCompleted)
        {
            int currentAmount = GetItem(quest.TargetItem)?.Quantity ?? 0;

            Console.WriteLine($"{quest.Description} ({currentAmount}/{quest.TargetAmount})");
        }
        else
        {
            Console.WriteLine("✅ Quête terminée !");
        }
    }

    public void ShowMainMenu(int money, Quest currentQuest, Func<int, Item> GetItem)
    {
        MainMenu(money);
        QuestInfo(currentQuest, GetItem);
    }

    public void PlantMenu(List<PlantData> plants)
    {
        Console.Clear();
        Console.WriteLine("\n=== PLANTER UNE GRAINE ===");
        Console.WriteLine("\n🌱 Que veux-tu planter ?");

        for (int i = 0; i < plants.Count; i++)
        {
            var p = plants[i];
            Console.WriteLine($"{i + 1} - {p.Name} (⏳ {p.GrowTime}s | 💰 {p.SellPrice}€)");
        }

    }

    public void ShowInventory(List<Item> inventory)
    {
        // Console.Clear();
        Console.WriteLine("\n📦 INVENTAIRE :");

        if (inventory.Count == 0)
        {
            DefaultConsoleOutput("Vide...");
        }

        foreach (var item in inventory)
        {
            Console.WriteLine($"{item.Id}. {item.Name} x{item.Quantity}");
            DefaultConsoleOutput($"- {item.Name} x{item.Quantity}");
        }
    }

    public void ShowShopPrices(List<Price> prices)
    {
        Console.WriteLine("\n🛒 SHOP PRICES");

        foreach (var price in prices)
        {
            Console.WriteLine($"{price.ItemName} : {price.Amount}€");
        }

        Console.WriteLine("\n💰 Que veux-tu vendre ?");
    }

    public void HarvestMenu(List<Plant> plants)
    {
        Console.Clear();
        Console.WriteLine("🌾 Quelle plante veux-tu récolter ?");
        ShowField(plants);
    }

    public void ShopMenu()
    {
        Console.Clear();
        Console.WriteLine("\n🛒 SHOP");
        Console.WriteLine("1 - Acheter graine (2€)");
        Console.WriteLine("2 - Vendre des objets");
        Console.WriteLine("3 - Retour");
    }

    public void ShowField(List<Plant> plants)
    {
        for (int i = 0; i < plants.Count; i++)
        {
            var plant = plants[i];

            string status = plant.IsReady ? "🌾 Prêt" : "🌱 En croissance";
            Console.WriteLine($"{i + 1}. {plant.Data.Name} - {status}");
        }
    }
}