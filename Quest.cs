class Quest
{
    public string Description;
    public string TargetItem;
    public int TargetAmount;
    public int Reward;
    public bool IsCompleted;

    public Quest(string description, string targetItem, int targetAmount, int reward)
    {
        Description = description;
        TargetItem = targetItem;
        TargetAmount = targetAmount;
        Reward = reward;
        IsCompleted = false;
    }
}