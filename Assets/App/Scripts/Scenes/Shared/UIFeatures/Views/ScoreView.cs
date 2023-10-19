using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    public void UpdateText(string text)
    {
        _text.text = text;
    }
}
