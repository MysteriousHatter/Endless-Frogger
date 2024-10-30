using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // The static reference to the GameManager, ensuring it's a singleton
    public static ScoreManager instance;
    private Player player;
    [SerializeField] private Text scoreText;
    public int score { get; private set; }

    void Awake()
    {
        score = 0;
        // Singleton pattern - ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this GameObject persists across scenes
            UpdateScoreText();
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Call this function whenever a tile is cleared
    public void TileCleared()
    {
        score += 1;     // Increment the score by a fixed amount (e.g., 10 points)
        UpdateScoreText();  // Update the score on the UI
    }

    // Update the score text on the UI
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();  // Update the score text
    }
}
