using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // The static reference to the GameManager, ensuring it's a singleton
    public static GameManager instance;
    private int coinCollection;
    public GameObject GameOverUI;
    public float obstacleSpeed = 5f; // Starting speed
    [SerializeField] private TextMeshProUGUI coinCountText;

    private bool isPaused = false;

    void Awake()
    {
        GameOverUI.SetActive(false);
        // Singleton pattern - ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this GameObject persists across scenes
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }
    }

    private void Update()
    {
        // Check for the Start button press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGameMethod();
            }
        }
    }

    public void PauseGameMethod()
    {
        Time.timeScale = 0;  // Pauses the game
        isPaused = true;
        // Optional: Activate pause UI or overlay here
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;  // Resumes the game
        isPaused = false;
        // Optional: Deactivate pause UI or overlay here
    }
    public void CoinCollectionIncrease(int count)
    {
        coinCollection += count;
        coinCountText.text = coinCollection.ToString();
    }

    public void LoadGameOverScene()
    {
        GameOverUI.SetActive(true);
        FindObjectOfType<Player>().enabled = false;
        FindAnyObjectByType<Mover>().enabled = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }  

    public int getCoinCollection() { return coinCollection; }
    public void SetCoinCollection(int value) { coinCollection = value; }
    
    public float getObstacleSpeed() { return obstacleSpeed; }
}
