using TMPro;
using UnityEngine;

public class SetupTextView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    public void SetupText(string text)
    {
        _text.text = text;
    }
}
