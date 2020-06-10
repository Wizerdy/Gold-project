using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GPGSAuthentification : MonoBehaviour
{
    public static PlayGamesPlatform platform;
    public Text text;

    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Logged in successfully !");
                text.text = success.ToString();
            }
            else
            {
                Debug.Log("Failed ! :(");
                text.text = success.ToString();
            }
        });
    }
}
