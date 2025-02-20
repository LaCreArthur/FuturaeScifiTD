using UnityEngine;

namespace ScriptableVariables
{
    [CreateAssetMenu(menuName = "SO Variables/IntVar", fileName = "IntVar", order = 0)]
    public class IntVar : SOVar<int>
    {
        protected override void Save() => PlayerPrefs.SetInt(name, value);
        protected override void Load() => Value = PlayerPrefs.GetInt(name, value);
    }
}
