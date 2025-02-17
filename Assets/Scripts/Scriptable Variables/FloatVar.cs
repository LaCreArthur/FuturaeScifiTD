using UnityEngine;

namespace ScriptableVariables
{
    [CreateAssetMenu(menuName = "SO Variables/FloatVar", fileName = "FloatVar", order = 0)]
    public class FloatVar : SOVar<float>
    {
        protected override void Save() => PlayerPrefs.SetFloat(name, value);
        protected override void Load() => value = PlayerPrefs.GetFloat(name, value);
    }
}
