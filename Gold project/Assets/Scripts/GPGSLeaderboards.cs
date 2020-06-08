using UnityEngine;

public class GPGSLeaderboards : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void UpdateLeaderboardScore()
    {
        if (PlayerPrefs.GetInt("ScoreToUpdate", 0) == 0)
        {
            return;
        }

        Social.ReportScore(PlayerPrefs.GetInt("ScoreToUpdate", 1), GPGSIds.leaderboard_victories, (bool success) =>
        {
            if (success)
            {
                PlayerPrefs.SetInt("ScoreToUpdate", 0);
            }
        });
    }
}
