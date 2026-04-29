using System;
using System.Runtime.CompilerServices;
// using Plant.cs;

class Program
{
    static Ui UI = new Ui();
    static bool isRunning = true;
    static List<Plant> plants = new List<Plant>();
    static int money = 5;
    static List<Item> inventory = new List<Item>();
    static List<Price> prices = new List<Price>();
    
    static void InitPrices()
    {
        foreach (var plant in plantCatalog)
        {
            prices.Add(new Price(plant, plant.SellPrice));
        }
    }

    static Quest currentQuest = new Quest(
        "Récolter 5 Blés",
        1, // Blé
        5, // Quantité à obtenir
        40 // 40€ de récompense
    );

    static List<PlantData> plantCatalog = new List<PlantData>()
    {
        new PlantData(1, "Blé", 10, 3),
        new PlantData(2, "Carotte", 5, 2),
        new PlantData(3, "Maïs", 15, 8)
    };

    
    static void Main()
    {
        InitPrices();

        while(isRunning)
        {
            UpdatePlants();
            CheckQuest();

            UI.ShowMainMenu(money, currentQuest, GetItem);
            HandleInput();
        }
    }

    static string GetUserInput()
    {
        Console.Write("> ");
        return Console.ReadLine();
    }

    static void HandleInput()
    {
        string input = GetUserInput();

        switch (input)
        {
            case "1":
                PlantCrop();
                break;
            
            case "2":
                UI.ShowInventory(inventory);
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
                UI.DefaultConsoleOutput("Choix Invalide !");
                break;
        }
    }

    static void PlantCrop()
    {
        UI.PlantMenu(plantCatalog);

        string input = GetUserInput();

        if (int.TryParse(input, out int index))
        {
            index--;

            if (index >= 0 && index < plantCatalog.Count)
            {
                var selectedPlant = plantCatalog[index];

                if (!RemoveItem(1, 1))
                {
                    UI.DefaultConsoleOutput("❌ Pas de graines !");
                    return;
                }

                plants.Add(new Plant(selectedPlant));

                UI.DefaultConsoleOutput($"🌱 Tu plantes {selectedPlant.Name} !");
            }
        }
    }

    static void HarvestPlant()
    {
        if (plants.Count == 0)
        {
            UI.DefaultConsoleOutput("❌ Pas de plantes à récolter !");
            return;
        }

        UI.HarvestMenu(plants);

        string input = GetUserInput();

        if (int.TryParse(input, out int index))
        {
            index--;

            if (index >= 0 && index < plants.Count)
            {
                var plant = plants[index];

                if (plant.IsReady)
                {
                    UI.DefaultConsoleOutput($"✅ Tu récoltes {plant.Data.Name} !");

                    AddItem(plant.Data.Id, plant.Data.Name, 1);

                    plants.RemoveAt(index);
                }
                else
                {
                    UI.DefaultConsoleOutput("⏳ Cette plante n'est pas prête !");
                }
            }
            else
            {
                UI.DefaultConsoleOutput("❌ Index invalide");
            }
        }
        else
        {
            UI.DefaultConsoleOutput("❌ Entrée invalide");
        }
    }

    static void OpenShop()
    {
        UI.ShopMenu();

        string input = GetUserInput();

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
                UI.DefaultConsoleOutput("❌ Entrée invalide");
                break;
        }
    }

    static void BuySeed()
    {
        int price = 2;

        if (money >= price)
        {
            money -= price;
            AddItem(1, "Graine", 1);

            UI.DefaultConsoleOutput("✅ Tu as acheté une graine !");
        }
        else
        {
            UI.DefaultConsoleOutput("❌ Pas assez d'argent !");
        }
    }

    static void SellItem()
    {
        UI.ShowShopPrices(prices);
        UI.ShowInventory(inventory);

        if (inventory.Count == 0)
        {
            return;
        }

        UI.DefaultConsoleOutput("");

        string inputId = GetUserInput();

        if (!int.TryParse(inputId, out int id))
        {
            UI.DefaultConsoleOutput("❌ ID invalide");
            return;
        }

        Item item = GetItem(id);

        if (item == null)
        {
            UI.DefaultConsoleOutput("❌ Objet introuvable");
            return;
        }

        Price price = prices.Find(p => p.ItemId == item.Id);

        if (price == null)
        {
            UI.DefaultConsoleOutput("❌ Cet objet ne peut pas être vendu");
            return;
        }

        Console.WriteLine("Quantité : ");
        string inputQty = GetUserInput();

        if (int.TryParse(inputQty, out int qty))
        {
            if (qty <= 0 || qty > item.Quantity)
            {
                UI.DefaultConsoleOutput("❌ Quantité invalide");
                return;
            }

            int total = price.Amount * qty;

            RemoveItem(item.Id, qty);
            money += total;

            UI.DefaultConsoleOutput($"✅ Vendu {qty}x {item.Name} pour {total}€ !");
        }
        else
        {
            UI.DefaultConsoleOutput("❌ Entrée invalide");
        }
    }

    static void QuitGame()
    {
        UI.DefaultConsoleOutput("Au revoir");
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

            UI.DefaultConsoleOutput($"\n🎉 Quête accomplie !\n💰 Récompense : {currentQuest.Reward}€");
        }
    }

    static Item GetItem(int id)
    {
        return inventory.Find(i => i.Id == id);
    }

    static void AddItem(int id, string name, int quantity)
    {
        Item item = GetItem(id);

        if (item == null)
        {
            inventory.Add(new Item(id, name, quantity));
        }
        else
        {
            item.Quantity += quantity;
        }
    }

    static bool RemoveItem(int id, int quantity)
    {
        Item item = GetItem(id);

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