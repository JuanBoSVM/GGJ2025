using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text winnerText;
    public Canvas endGameCanvas;

    [Header("Game Settings")]
    public int pointsToWin = 3;

    private int player1Score = 0;
    private int player2Score = 0;

    private void Start()
    {
        // Initialize UI
        UpdateScoreUI();
        endGameCanvas.gameObject.SetActive(false);
    }

    public void AddPointToPlayer(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score++;
        }
        else if (playerNumber == 2)
        {
            player2Score++;
        }

        UpdateScoreUI();
        CheckForWinner();
    }

    private void UpdateScoreUI()
    {
        player1ScoreText.text = "Jugador 1: " + player1Score;
        player2ScoreText.text = "Jugador 2: " + player2Score;
    }

    private void CheckForWinner()
    {
        if (player1Score >= pointsToWin)
        {
            EndGame("Jugador 1");
        }
        else if (player2Score >= pointsToWin)
        {
            EndGame("Jugador 2");
        }
    }

    private void EndGame(string winner)
    {
        winnerText.text = "Ganador: " + winner;
        endGameCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ContinueGame()
    {
        // Reset scores and UI
        player1Score = 0;
        player2Score = 0;
        UpdateScoreUI();
        endGameCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void ExitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
