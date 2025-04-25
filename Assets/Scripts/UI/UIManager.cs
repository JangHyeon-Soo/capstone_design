using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenuUIPrefab;      
    public Transform canvasTransform;         
    private GameObject pauseMenuUIInstance;   
    private bool isPaused = false;

    void Start()
    {
        pauseMenuUIInstance = Instantiate(pauseMenuUIPrefab, canvasTransform);
        pauseMenuUIInstance.transform.localPosition = Vector3.zero;
        pauseMenuUIInstance.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        pauseMenuUIInstance.transform.localPosition = Vector3.zero; //
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }
}
