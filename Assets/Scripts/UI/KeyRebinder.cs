using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class KeyRebinder : MonoBehaviour
{
    public string actionName = "Move";
    public string bindingMatchName = "Forward";
    public Button rebindButton;
    public TMP_Text bindingDisplay;

    private PlayerInput playerInput;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private int bindingIndex = -1;

    private static KeyRebinder currentlyRebinding = null;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found!");
            return;
        }

        FindBindingIndex();

        if (bindingIndex < 0)
        {
            Debug.LogError($"Binding '{bindingMatchName}' not found in '{actionName}'");
            return;
        }

        rebindButton.onClick.AddListener(StartRebind);
        UpdateBindingDisplay();
    }

    void FindBindingIndex()
    {
        var action = playerInput.actions[actionName];

        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];

            if (!string.IsNullOrEmpty(bindingMatchName) && binding.name == bindingMatchName)
            {
                bindingIndex = i;
                return;
            }

            string path = (binding.overridePath ?? binding.path ?? "").ToLower();
            if (!string.IsNullOrEmpty(bindingMatchName) && path.Contains("/" + bindingMatchName.ToLower()))
            {
                bindingIndex = i;
                return;
            }
        }
    }

    public void StartRebind()
    {
        if (currentlyRebinding != null && currentlyRebinding != this)
        {
            currentlyRebinding.CancelRebind();
        }

        var action = playerInput.actions[actionName];
        action.Disable();

        bindingDisplay.text = "wait...";
        currentlyRebinding = this;

        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnComplete(op =>
            {
                op.Dispose();
                rebindOperation = null;
                currentlyRebinding = null;

                action.Enable();
                UpdateBindingDisplay();
            })
            .Start();
    }

    public void CancelRebind()
    {
        if (rebindOperation != null)
        {
            rebindOperation.Cancel();
            rebindOperation.Dispose();
            rebindOperation = null;
        }

        currentlyRebinding = null;
        playerInput.actions[actionName].Enable();
        UpdateBindingDisplay();
    }

    void UpdateBindingDisplay()
    {
        var action = playerInput.actions[actionName];
        var binding = action.bindings[bindingIndex];
        bindingDisplay.text = InputControlPath.ToHumanReadableString(
            binding.effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }
}

