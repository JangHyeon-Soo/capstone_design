using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject displaySettingsPanel;
    public GameObject keySettingsPanel;

    public Button displayTabButton;
    public Button keyTabButton;
    public ScrollRect keyScrollRect;

    void Start()
    {
        displayTabButton.onClick.AddListener(ShowDisplaySettings);
        keyTabButton.onClick.AddListener(ShowKeySettings);

        ShowDisplaySettings();
    }

    public void ShowDisplaySettings()
    {
        displaySettingsPanel.SetActive(true);
        keySettingsPanel.SetActive(false);

        displayTabButton.GetComponent<UIButtonHighlighter>().ForceSelect();
    }

    public void ShowKeySettings()
    {
        displaySettingsPanel.SetActive(false);
        keySettingsPanel.SetActive(true);
        
        if (keyScrollRect != null)
        {
            keyScrollRect.verticalNormalizedPosition = 1f;
        }

        keyTabButton.GetComponent<UIButtonHighlighter>().ForceSelect();
    }
}
