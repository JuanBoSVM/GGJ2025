
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCanvasManager : MonoBehaviour
{
    // Cambiar de escena
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Activar un Canvas
    public void ActivateCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning ("El Canvas a activar es nulo.");
        }
    }

    // Desactivar un Canvas
    public void DeactivateCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning ("El Canvas a desactivar es nulo.");
        }
    }

    // Alternar el estado de un Canvas (activar/desactivar)
    public void ToggleCanvas(GameObject canvas)
    {
        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
        else
        {
            Debug.LogWarning ("El Canvas a alternar es nulo.");
        }
    }

    // Cerrar el juego
    public void QuitGame()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}
