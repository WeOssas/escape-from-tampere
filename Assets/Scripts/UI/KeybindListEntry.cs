using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeybindListEntry : MonoBehaviour
{
    protected InputAction action;
    protected int bindingIndex;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI boundKeyText;
    public Color normalBindingColor = Color.black;
    public float blinkSpeed = 1;
    public Color blinkStartColor = Color.black;
    public Color blinkEndColor = Color.gray;
    private float blinkOffset = 0;

    private List<InputActionRebindingExtensions.RebindingOperation> _rebindingOperations = new();
    
    public void SetBinding(InputAction action, int bindingIndex)
    {
        InputBinding binding = action.bindings[bindingIndex];
        this.action = action;
        this.bindingIndex = bindingIndex;
        nameText.text = action.name + " " + binding.name;
        boundKeyText.text = binding.ToDisplayString();

        if (binding.path == "<Mouse>/delta")
        { 
            // Mouse delta cannot be rebound.
            boundKeyText.text = "Mouse";
            boundKeyText.color = Color.gray;
            boundKeyText.GetComponent<Button>().enabled = false;
            nameText.color = Color.gray;
        }
    }
    
    public void StartRebinding()
    {
        var operation = action.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation => {
                boundKeyText.text = action.bindings[bindingIndex].ToDisplayString();
                boundKeyText.color = normalBindingColor;
                operation.Dispose();
                _rebindingOperations.Remove(operation);
            })
            .Start();
        
        _rebindingOperations.Add(operation);
        blinkOffset = Time.time;
    }

    private void OnDisable()
    {
        foreach (var operation in _rebindingOperations)
        {
            operation.Dispose();
        }
    }

    private void Update()
    {
        if (_rebindingOperations.Count > 0)
        {
            // Blink while rebinding
            boundKeyText.color = Color.Lerp(blinkStartColor, blinkEndColor, Mathf.PingPong(Time.time * blinkSpeed - blinkOffset, 1));
        }
    }
}
