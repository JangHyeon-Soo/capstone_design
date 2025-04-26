using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUIPrefab;
    public Transform canvasTransform;
    private GameObject pauseMenuUIInstance;
    private bool isPaused = false;

    private Button playButton;
    private Button restartButton;
    private Button quitButton;

    void Start()
    {
        pauseMenuUIInstance = Instantiate(pauseMenuUIPrefab, canvasTransform);
        pauseMenuUIInstance.transform.localPosition = Vector3.zero;
        pauseMenuUIInstance.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playButton = pauseMenuUIInstance.transform.Find("Play").GetComponent<Button>();
        playButton.onClick.AddListener(ResumeGame);

        restartButton = pauseMenuUIInstance.transform.Find("Restart").GetComponent<Button>();
        restartButton.onClick.AddListener(RestartGame);

        quitButton = pauseMenuUIInstance.transform.Find("Quit").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUIInstance.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUIInstance.SetActive(true);
        pauseMenuUIInstance.transform.localPosition = Vector3.zero;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void RestartGame()
    {
        pauseMenuUIInstance.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
