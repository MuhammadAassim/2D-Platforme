using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    // PlayerMovement ka reference — player ki death check karne ke liye

    [Header("GameOver Canvas")]
    [SerializeField] private GameObject gameOverCanvas;
    // Game Over ka UI jo player ke marne pe show hoga

    [Header("Pause Canvas")]
    [SerializeField] private GameObject pauseCanvas;
    // Pause menu ka canvas — jab Escape dabao to show hota hai

    private bool isPaused = false;
    // Track karega ke game paused hai ya nahi

    private void Update()
    {
        // Har frame check karo — agar player mar gaya ho aur gameOver UI already active nahi
        if (playerMovement.PlayerDied && !gameOverCanvas.activeSelf)
        {
            gameOverCanvas.SetActive(true);
            // Game Over canvas ko show karo
        }

        // Escape key dabane par pause ya resume toggle karo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            // Pause aur resume ke beech toggle karne wala method call karo
        }
    }

    public void Restart()
    {
        // Abhi jo scene chal raha hai usko dobara load karo
        Scene currentScene = SceneManager.GetActiveScene();
        // Current scene ka reference lo

        SceneManager.LoadScene(currentScene.name);
        // Us scene ko reload karo (reset effect ke liye)
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        // Boolean flag ko flip karo — agar true hai to false banega, aur vice versa

        if (isPaused)
        {
            pauseCanvas.SetActive(true);
            // Pause ka canvas UI show karo
            Time.timeScale = 0f;
            // Game ko freeze kar do — sab kuch ruk jaayega (physics, movement etc)
        }
        else
        {
            pauseCanvas.SetActive(false);
            // Pause UI ko hide karo
            Time.timeScale = 1f;
            // Game resume kar do — time normal speed pe wapas
        }
    }

    public void MainMenu()
    {
        // SceneManager ka use karke "Menu" scene load karo
        // Ensure karo ke "Menu" scene Build Settings mein added ho
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        // Agar Editor mein hain to Play Mode band karo
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // Agar Build version hai to game band karo
    Application.Quit();
#endif
    }

}
