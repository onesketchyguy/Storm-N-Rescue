public class Score
{
    public static int score = 0; //The player's score

    public static void Reset()
    {
        score = 0;
    }

    public static void ModifyScore(int mod)
    {
        score = score + mod;
    }
}