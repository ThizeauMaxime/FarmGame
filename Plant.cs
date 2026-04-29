class Plant
{
    public PlantData Data;
    public DateTime PlantedAt;
    public bool IsReady;

    public Plant(PlantData data)
    {
        Data = data;
        PlantedAt = DateTime.Now;
        IsReady = false;
    }

    public void Update()
    {
        if (!IsReady)
        {
            var elapsed = DateTime.Now - PlantedAt;

            if (elapsed.TotalSeconds >= Data.GrowTime)
            {
                IsReady = true;
            }
        }
    }
}