using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Método para cargar la escena del juego
    public void PlayGame()
    {
        // Reemplaza "GameScene" con el nombre de la escena que quieres cargar
        SceneManager.LoadScene("nivlel");
        Time.timeScale = 1f;
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego..."); // Solo visible en el editor
    }
}
