using System;

public class ScoreManager
{
    private static int score = 0;

    public static int GetScore()
    {
        return score;
    }

    public static void Add(int ScoreToAdd)
    {
        score = score + ScoreToAdd;
    }

    public static void ResetScore()
    {
        score = 0;
    }
}
