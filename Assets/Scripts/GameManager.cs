using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour, IUnityAdsInitializationListener
{

    public int best;
    public int score;
    public int currentStage = 0;
    public static GameManager singleton;
    public GameObject restartScreen;
    private string gameId = "5715718"; // Replace with your actual game ID
    private bool testMode = true; // Set to false when releasing the game

    // Start is called before the first frame update
    void Awake()
    {
        InitializeAds();

        if (singleton == null)

        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
        best = PlayerPrefs.GetInt("Highscore");
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        HelixController helix = FindObjectOfType<HelixController>();

        // if (currentStage > helix.allStages.Count)
        // {
        //     currentStage = 0;
        // }
        helix.LoadStage(currentStage);
    }
    public void InitializeAds()
    {
        Advertisement.Initialize(gameId, testMode, this);
    }


    public void RestartLevel()
    {
        restartScreen.SetActive(false);
        Advertisement.Show("Interstitial_Android");
        singleton.score = 0;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);

        //
    }

    public void GameOver()
    {
        restartScreen.SetActive(true);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("Highscore", score);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
