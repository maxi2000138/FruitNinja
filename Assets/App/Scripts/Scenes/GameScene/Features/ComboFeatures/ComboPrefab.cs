using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ComboPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text _fruitsText;
    [SerializeField] private TMP_Text _setText;
    [SerializeField] private TMP_Text _comboText;
    
    private ComboConfig _comboConfig;
    public void Construct(ComboConfig comboConfig, int comboCount)
    {
        _comboConfig = comboConfig;
        SetText(comboCount);
        DestroyTask();
    }

    private void SetText(int comboCount)
    {
        _fruitsText.text = comboCount + " fruits";
        _setText.text = "combo";
        _comboText.text = "x" + comboCount;
    }

    private async void DestroyTask()
    {
        await Task.Delay((int)(_comboConfig.ComboLifeTime * 1000));
        Destroy(gameObject);
    }
}
