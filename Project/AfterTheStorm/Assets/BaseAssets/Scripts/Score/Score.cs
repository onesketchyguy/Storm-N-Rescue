public class Score
{
    public static int score = 0; //The player's score
    public static string reason;

    public static void Reset()
    {
        score = 0;
        reason = "";
    }

    public static void ModifyScore(int mod, string modificationReason)
    {
        score = score + mod;
        reason = modificationReason;
    }
}