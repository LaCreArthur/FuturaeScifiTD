using UnityEngine;

public class CheatLevelUp : MonoBehaviour
{
    [SerializeField] GameObject levelUpPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            levelUpPanel.SetActive(true);
        }
    }
}
