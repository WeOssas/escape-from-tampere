using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeybindList : MonoBehaviour
{
    /// <summary>
    /// Prefab for one entry in the list.
    /// </summary>
    public GameObject listEntryPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get an array of all keybinds.
        InputAction[] allActions = Actions.GetAllActions();

        int i = 0;
        // Loop through all indexes of the array.
        foreach (InputAction action in allActions)
        {
            int bindingIndex = -1;
            foreach (InputBinding binding in action.bindings)
            {
                bindingIndex++;
                
                if (binding.isComposite && !binding.isPartOfComposite) continue; // Skip composites.

                // Spawn a new entry to this scroll area from a prefab.
                GameObject listEntry = Instantiate(listEntryPrefab, transform);
                // Tell the entry which keybind it should represent.
                listEntry.GetComponent<KeybindListEntry>().SetBinding(action, bindingIndex);
            
                RectTransform rectTransform = (RectTransform) listEntry.transform;
                // Move the entry below all other entries.
                listEntry.transform.localPosition -= new Vector3(0, i * rectTransform.sizeDelta.y);
                // Expand the size of the scroll area to fit the new entry.
                ((RectTransform)transform).sizeDelta += new Vector2(0, rectTransform.sizeDelta.y);
                
                i++;
            }
        }
    }
}
