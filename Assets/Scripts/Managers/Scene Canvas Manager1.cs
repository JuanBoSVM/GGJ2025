using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    [Header("Canvas del Menú")]
    public GameObject menuCanvas;

    private bool isMenuActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (menuCanvas != null)
        {
            isMenuActive = !isMenuActive;
            menuCanvas.SetActive(isMenuActive);

            // Pausar o reanudar el tiempo
            Time.timeScale = isMenuActive ? 0f : 1f;
        }
        else
        {
            Debug.LogWarning("Menu Canvas no asignado en el inspector.");
        }
    }
}

