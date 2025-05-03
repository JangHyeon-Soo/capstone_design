using UnityEngine;
using UnityEngine.UI;

public class UIButtonHighlighter : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.7f, 0.2f);

    private static UIButtonHighlighter currentlySelected;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(SelectThisButton);
    }

    void OnEnable()
    {
        if (this != currentlySelected)
        {
            SetNormalColor();
        }
        else
        {
            SetSelectedColor();
        }
    }

    public void SelectThisButton()
    {
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.SetNormalColor();
        }

        SetSelectedColor();
        currentlySelected = this;
    }

    void SetSelectedColor()
    {
        var colors = button.colors;
        colors.normalColor = selectedColor;
        colors.highlightedColor = selectedColor;
        colors.pressedColor = selectedColor;
        button.colors = colors;

        RefreshButtonState();
    }

    void SetNormalColor()
    {
        var colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = normalColor;
        colors.pressedColor = normalColor;
        button.colors = colors;

        RefreshButtonState();
    }

    void RefreshButtonState()
    {
        button.interactable = false;
        button.interactable = true;
    }

    public void ForceSelect()
    {
        SelectThisButton();
    }
}
