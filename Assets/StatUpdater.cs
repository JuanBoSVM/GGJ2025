using UnityEngine;

public class StatUpdater : MonoBehaviour
{
    // Reference to the textmeshpro object
    [SerializeField]
    private TMPro.TextMeshProUGUI m_Score;



    void Awake()
    {
        // Get the game manager instance
        GameManager gameManager = GameManager.Instance;

        // Get the score of the players
        uint p1Score = gameManager.GetScore(0);
        uint p2Score = gameManager.GetScore(1);

        // Update the text
        m_Score.text =
            "Player 1:   " + p1Score.ToString() + "\n\n" +
            "Player 2:   " + p2Score.ToString() + "\n\n";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
