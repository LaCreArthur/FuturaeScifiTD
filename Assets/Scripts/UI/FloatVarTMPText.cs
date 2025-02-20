using ScriptableVariables;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FloatVarTMPText : MonoBehaviour
{
    [SerializeField] FloatVar var;
    [SerializeField] int decimals;

    TMP_Text _tmp;

    void Awake() => _tmp = GetComponent<TMP_Text>();

    void OnDestroy() => var.OnChanged -= SetValue;

    void Start()
    {
        var.OnChanged += SetValue;
        SetValue(var.Value);
    }

    void SetValue(float value) => _tmp.text = $"{var.Value.ToString($"N{decimals}")}";
}
