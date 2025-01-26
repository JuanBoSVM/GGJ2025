using UnityEngine;

public class Death_Manager : MonoBehaviour

{
    [Header("Canvas a Mostrar")]
    public GameObject canvas;

    [Header("Tag del Objeto que Detendrá el Tiempo")]
    public string triggeringTag = "Player";

    private bool isTimeStopped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(triggeringTag) && !isTimeStopped)
        {
            ShowCanvas();

            Player player = other.gameObject.GetComponent<Player>();

            if (player == null)
            {
                Debug.LogWarning("No player script");
            }
            else
            {
                player.Kill();
            }
        }
    }

    private void ShowCanvas()
    {
        // Mostrar el canvas
        if (canvas != null)
        {
            canvas.SetActive(true);
        }

        else
        {
            Debug.LogWarning("Canvas no asignado en el inspector.");
        }
    }

    public void ResumeTime()
    {
        // Reanudar el tiempo
        Time.timeScale = 1f;
        isTimeStopped = false;

        // Ocultar el canvas
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }
}
