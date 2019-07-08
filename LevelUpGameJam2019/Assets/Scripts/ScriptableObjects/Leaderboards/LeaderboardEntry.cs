
[System.Serializable]
public struct LeaderboardEntry 
{
    public string name;
    public int score;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    public LeaderboardEntry(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public override string ToString()
    {
        return name.ToString() + " " + score.ToString();
    }
}
