using TMPro;
using UnityEngine;

public class ScreenLeaderboard : MonoBehaviour
{
    [SerializeField] Transform entryContainer; // UI Parent for entries
    [SerializeField] GameObject entryTemplate; // Prefab for a single entry

    void OnEnable() => ShowLeaderboard();
    void ShowLeaderboard()
    {
        // Clear previous entries
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // Display leaderboard entries
        foreach (LeaderboardEntry entry in Leaderboard.LeaderboardEntries)
        {
            GameObject entryGO = Instantiate(entryTemplate, entryContainer);
            entryGO.GetComponent<TextMeshProUGUI>().text = $"{entry.name} - {entry.score}";
        }
    }
}
