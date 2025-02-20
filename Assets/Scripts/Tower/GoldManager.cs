using ScriptableVariables;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    static IntVar s_goldVar;
    [SerializeField] IntVar goldVar;

    public static int CurrentGold
    {
        get => s_goldVar.Value;
        private set => s_goldVar.Value = value;
    }

    // cheap "singleton" 
    void Awake() => s_goldVar = goldVar;

    public static bool CanAfford(int amount) => CurrentGold >= amount;

    public static void AddGold(int amount) => CurrentGold += amount;

    public static bool SubtractGold(int amount)
    {
        if (CanAfford(amount))
        {
            CurrentGold -= amount;
            return true;
        }
        Debug.Log("Not enough gold!");
        return false;
    }
}
