using System;
using System.Runtime.CompilerServices;
// using Plant.cs;

class Program
{
    static bool isRunning = true;
    static List<Plant> plants = new List<Plant>();
    static int money = 5;
    static List<Item> inventory = new List<Item>();
    static Dictionary<string, int> prices = new Dictionary<string, int>();
    
    static void InitPrices()
    {
        foreach (var plant in plantCatalog)
        {
            prices[plant.Name] = plant.SellPrice;
        }
    }

    static Quest currentQuest = new Quest(
        "Récolter 5 Blés",
        "Blé",
        5,
        40
    );

    static List<PlantData> plantCatalog = new List<PlantData>()
    {
        new PlantData("Blé", 10, 3),
        new PlantData("Carotte", 5, 2),
        new PlantData("Maïs", 15, 8)
    };

    
    static void Main()
    {
        InitPrices();

        while(isRunning)
        {
            UpdatePlants();
            CheckQuest();

            ShowMenu();
            HandleInput();
        }
    }

    static void ShowMenu()
    {
        // Console.Clear();
        Console.WriteLine("\n=== FARM GAME ===");
        Console.WriteLine($"💰 Argent : {money}");
        // Console.WriteLine($"🌾 Graines : {seeds}");
        Console.WriteLine("\n📜 Quête actuelle :");
        if (!currentQuest.IsCompleted)
        {
            int currentAmount = GetItem(currentQuest.TargetItem)?.Quantity ?? 0;

            Console.WriteLine($"{currentQuest.Description} ({currentAmount}/{currentQuest.TargetAmount})");
        }
        else
        {
            Console.WriteLine("✅ Quête terminée !");
        }
        Console.WriteLine("1 - Planter une graine");
        Console.WriteLine("2 - Voir inventaire");
        Console.WriteLine("3 - Récolter");
        Console.WriteLine("4 - Shop");
        Console.WriteLine("5 - Quitter");
        Console.Write("Choix : ");
    }

    static void HandleInput()
    {
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                PlantCrop();
                break;
            
            case "2":
                ShowInventory();
                break;
            
            case "3":
                HarvestPlant();
                break;

            case "4":
                OpenShop();
                break;
            
            case "5":
                QuitGame();
                break;

            default:
                Console.WriteLine("Choix Invalide !");
                break;
        }
    }

    static void PlantCrop()
    {
        Console.WriteLine("\n🌱 Que veux-tu planter ?");

        for (int i = 0; i < plantCatalog.Count; i++)
        {
            var p = plantCatalog[i];
            Console.WriteLine($"{i + 1} - {p.Name} (⏳ {p.GrowTime}s | 💰 {p.SellPrice}€)");
        }

        Console.Write("Choix : ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index))
        {
            index--;

            if (index >= 0 && index < plantCatalog.Count)
            {
                var selectedPlant = plantCatalog[index];

                if (!RemoveItem("Graine", 1))
                {
                    Console.WriteLine("❌ Pas de graines !");
                    return;
                }

                plants.Add(new Plant(selectedPlant));

                Console.WriteLine($"🌱 Tu plantes {selectedPlant.Name} !");
            }
        }
    }

    static void ShowInventory()
    {
        // Console.Clear();
        Console.WriteLine("\n📦 INVENTAIRE :");

        if (inventory.Count == 0)
        {
            Console.WriteLine("Vide...");
        }

        foreach (var item in inventory)
        {
            Console.WriteLine($"- {item.Name} x{item.Quantity}");
        }
    }

    static void ShowField()
    {
        if (plants.Count == 0)
        {
            Console.WriteLine("Vide...");
            return;
        }

        for (int i = 0; i < plants.Count; i++)
        {
            var plant = plants[i];

            string status = plant.IsReady ? "🌾 Prêt" : "🌱 En croissance";
            Console.WriteLine($"{i + 1}. {plant.Data.Name} - {status}");
        }
    }

    static void HarvestPlant()
    {
        Console.WriteLine("\n🌾 Quelle plante veux-tu récolter ?");
        ShowField();

        if (plants.Count == 0)
        {
            return;
        }

        Console.Write("Numéro : ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index))
        {
            index--;

            if (index >= 0 && index < plants.Count)
            {
                var plant = plants[index];

                if (plant.IsReady)
                {
                    Console.WriteLine($"✅ Tu récoltes {plant.Data.Name} !");

                    AddItem(plant.Data.Name, 1);

                    plants.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("⏳ Cette plante n'est pas prête !");
                }
            }
            else
            {
                Console.WriteLine("❌ Index invalide");
            }
        }
        else
        {
            Console.WriteLine("❌ Entrée invalide");
        }
    }

    static void OpenShop()
    {
        Console.Clear();
        Console.WriteLine("\n🛒 SHOP");
        Console.WriteLine("1 - Acheter graine (2€)");
        Console.WriteLine("2 - Vendre des objets");
        Console.WriteLine("3 - Retour");
        Console.Write("Choix : ");

        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                BuySeed();
                break;

            case "2":
                SellItem();
                break;
            
            case "3":
                return;
            
            default:
                Console.WriteLine("❌ Entrée invalide");
                break;
        }
    }

    static void BuySeed()
    {
        int price = 2;

        if (money >= price)
        {
            money -= price;
            AddItem("Graine", 1);

            Console.WriteLine("✅ Tu as acheté une graine !");
        }
        else
        {
            Console.WriteLine("❌ Pas assez d'argent !");
        }
    }

    static void SellItem()
    {
        Console.WriteLine("\n🛒 SHOP PRICES");

        foreach (var kvp in prices)
        {
            Console.WriteLine($"{kvp.Key} : {kvp.Value}€");
        }

        Console.WriteLine("\n💰 Que veux-tu vendre ?");
        ShowInventory();

        if (inventory.Count == 0)
        {
            return;
        }

        Console.Write("Nom de l'objet : ");
        string name = Console.ReadLine();

        Item item = GetItem(name);

        if (item == null)
        {
            Console.WriteLine("❌ Objet introuvable");
            return;
        }

        if (!prices.ContainsKey(name))
        {
            Console.WriteLine("❌ Cet objet ne peut pas être vendu");
            return;
        }

        Console.Write("Quantité : ");
        string inputQty = Console.ReadLine();

        if (int.TryParse(inputQty, out int qty))
        {
            if (qty <= 0 || qty > item.Quantity)
            {
                Console.WriteLine("❌ Quantité invalide");
                return;
            }

            int total = prices[name] * qty;

            RemoveItem(name, qty);
            money += total;

            Console.WriteLine($"✅ Vendu {qty}x {name} pour {total}€ !");
        }
        else
        {
            Console.WriteLine("❌ Entrée invalide");
        }
    }

    static void QuitGame()
    {
        Console.WriteLine("Au revoir");
        isRunning = false;
    }

    static void UpdatePlants()
    {
        foreach (var plant in plants)
        {
            plant.Update();
        }
    }

    static void CheckQuest()
    {
        if (currentQuest.IsCompleted)
        {
            return;
        }

        int currentAmount = GetItem(currentQuest.TargetItem)?.Quantity ?? 0;

        if (currentAmount >= currentQuest.TargetAmount)
        {
            currentQuest.IsCompleted = true;
            money += currentQuest.Reward;

            Console.WriteLine($"\n🎉 Quête accomplie !");
            Console.WriteLine($"💰 Récompense : {currentQuest.Reward}€");
        }
    }

    static Item GetItem(string name)
    {
        return inventory.Find(i => i.Name == name);
    }

    static void AddItem(string name, int quantity)
    {
        Item item = GetItem(name);

        if (item == null)
        {
            inventory.Add(new Item(name, quantity));
        }
        else
        {
            item.Quantity += quantity;
        }
    }

    static bool RemoveItem(string name, int quantity)
    {
        Item item = GetItem(name);

        if (item != null && item.Quantity >= quantity)
        {
            item.Quantity -= quantity;

            if (item.Quantity == 0)
            {
                inventory.Remove(item);
            }

            return true;
        }

        return false;
    }
}