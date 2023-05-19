using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using System.Text.RegularExpressions;
using TMPro;

public class CloseDateInput : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void OnValueChanged()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = string.Empty;
        }
        else
        {
            string input = inputField.text;
            string MatchPattern = @"^((\d{2}/){0,2}(\d{1,2})?)$";
            string ReplacementPattern = "$1/$3";
            string ToReplacePattern = @"((\.?\d{2})+)(\d)";

            input = Regex.Replace(input, ToReplacePattern, ReplacementPattern);
            Match result = Regex.Match(input, MatchPattern);
            if (result.Success)
            {
                inputField.text = input;
                inputField.caretPosition++;
            }
        }
    }
}