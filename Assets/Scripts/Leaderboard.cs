using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class Leaderboard
{
    const int MAX_ENTRIES = 5;
    const bool ONE_SCORE_PER_NAME = true;

    public static List<LeaderboardEntry> LeaderboardEntries = new List<LeaderboardEntry>();

    // if the score is higher than the lowest score in the leaderboard, we have a new leader
    public static bool IsNewLeader() => LeaderboardEntries.Count < MAX_ENTRIES || ScoreManager.Score > LeaderboardEntries[^1].score;

    public static void AddEntry(LeaderboardEntry entry)
    {
        if (ONE_SCORE_PER_NAME)
        {
            int index = LeaderboardEntries.FindIndex(e => e.name == entry.name);

            if (index != -1) // we found an entry with the same name
            {
                LeaderboardEntry existingEntry = LeaderboardEntries[index];
                if (entry.score <= existingEntry.score)
                {
                    Debug.Log($"AddEntry: Score for {entry.name} is lower than existing score");
                    return;
                }
                LeaderboardEntries[index] = entry;
                Debug.Log($"AddEntry: Updated score for {entry.name} to {entry.score}");
            }
            else // we didn't find an entry with the same name, add new entry
                LeaderboardEntries.Add(new LeaderboardEntry { name = entry.name, score = entry.score });
        }
        #pragma warning disable CS0162
        else
        {
            // we don't care about existing entries with the same name, just add new entry
            LeaderboardEntries.Add(new LeaderboardEntry { name = entry.name, score = entry.score });
        }
        #pragma warning restore CS0162

        // Sort and limit entries
        LeaderboardEntries.Sort((a, b) => b.score.CompareTo(a.score)); // Sort descending
        if (LeaderboardEntries.Count > MAX_ENTRIES)
            LeaderboardEntries.RemoveAt(LeaderboardEntries.Count - 1);

        Debug.Log($"New entry added : {entry.name} - {entry.score}");
        SaveLeaderboard();
    }

    [RuntimeInitializeOnLoadMethod] // This method will be called when the game starts
    static void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("leaderboard", "[]");
        LeaderboardEntries = JsonConvert.DeserializeObject<LeaderboardEntry[]>(json).ToList();
        Debug.Log($"Leaderboard loaded with {LeaderboardEntries.Count} entries");
    }

    static void SaveLeaderboard()
    {
        string json = JsonConvert.SerializeObject(LeaderboardEntries);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
        Debug.Log("Leaderboard saved");
    }
}

[Serializable]
public struct LeaderboardEntry
{
    public string name;
    public int score;
}
