using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject deathUI;
    public GameObject winUI;
    public GameObject pauseUI;

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (deathUI.activeSelf || winUI.activeSelf) return;

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // 🪦 PLAYER DIED
    public void ShowDeath()
    {
        if (deathUI != null)
            deathUI.SetActive(true);

        UnlockCursor();
        Time.timeScale = 0f;
    }

    // 🏁 GAME COMPLETE
    public void ShowWin()
    {
        if (winUI != null)
            winUI.SetActive(true);

        UnlockCursor();
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        Time.timeScale = 1f; // IMPORTANT: unpause game
        SceneManager.LoadScene("Level1");
    }

    // ⏸ PAUSE
    public void Pause()
    {
        if (pauseUI != null)
            pauseUI.SetActive(true);

        UnlockCursor();
        Time.timeScale = 0f;
        isPaused = true;
    }

    // ▶ RESUME
    public void Resume()
    {
        if (pauseUI != null)
            pauseUI.SetActive(false);

        LockCursor();
        Time.timeScale = 1f;
        isPaused = false;
    }

    // 🔄 Restart
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // IMPORTANT: unpause game
        SceneManager.LoadScene("MainMenu");
    }

    // ❌ Quit
    public void QuitGame()
    {
        Application.Quit();
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}