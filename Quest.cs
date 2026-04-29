class Quest
{
    public string Description;
    public int TargetItem;
    public int TargetAmount;
    public int Reward;
    public bool IsCompleted;

    public Quest(string description, int targetItem, int targetAmount, int reward)
    {
        Description = description;
        TargetItem = targetItem;
        TargetAmount = targetAmount;
        Reward = reward;
        IsCompleted = false;
    }
}