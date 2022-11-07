using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NameInputBehavior : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] List<Button> buttonList;
    void Awake()
    {
        nameInputField = GetComponent<TMP_InputField>();
        nameInputField.onValueChanged.AddListener((text) => { OnNameInputFieldChange(text); });

    }
    public void OnNameInputFieldChange(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            foreach (var button in buttonList)
            {
                button.interactable = false;
            }
        }
        else
        {
            foreach (var button in buttonList)
            {
                button.interactable = true;
            }
        }
    }

}