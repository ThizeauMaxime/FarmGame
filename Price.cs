class Price
{
    public int ItemId;
    public string ItemName;
    public int Amount;

    public Price(PlantData plant, int amount)
    {
        ItemId = plant.Id;
        ItemName = plant.Name;
        Amount = amount;
    }
}