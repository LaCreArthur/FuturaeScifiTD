using ScriptableVariables;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class IntVarTMPText : MonoBehaviour
{
    [SerializeField] IntVar var;

    TMP_Text _tmp;

    void Awake() => _tmp = GetComponent<TMP_Text>();

    void OnDestroy() => var.OnChanged -= SetValue;

    void Start()
    {
        var.OnChanged += SetValue;
        SetValue(var.Value);
    }

    void SetValue(int value) => _tmp.text = $"{var.Value.ToString()}";
}
