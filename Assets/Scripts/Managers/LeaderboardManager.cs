using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class LeaderboardManager : MonoBehaviour
    {
        private string __FilePath;
        public readonly List<LeaderboardEntry> __LeaderboardEntries = new();

        private void Start()
        {
            __FilePath = Path.Combine(Application.dataPath, "leaderboard.csv");
            LoadLeaderboard();
        }

        private void LoadLeaderboard()
        {
            __LeaderboardEntries.Clear();
            if (File.Exists(__FilePath))
            {
                string[] _Lines = File.ReadAllLines(__FilePath);
                foreach (string line in _Lines)
                {
                    string[] _Parts = line.Split(',');
                    LeaderboardEntry _Entry = new LeaderboardEntry(_Parts[0], int.Parse(_Parts[1]));
                    __LeaderboardEntries.Add(_Entry);
                }
            }
        }

        public void AddEntry(string playerName, int score)
        {
            // Check if username already exists in the leaderboard
            if (__LeaderboardEntries.Any(entry => entry._PlayerName == playerName))
            {
                Debug.LogWarning("Username already exists in the leaderboard, entry not added.");
                return;
            }

            // Add the new entry to the list of entries
            __LeaderboardEntries.Add(new LeaderboardEntry(playerName, score));

            // Save the updated leaderboard
            SaveLeaderboard();
        }

        private void SaveLeaderboard()
        {
            List<string> _Lines = new();
            
            foreach (LeaderboardEntry _Entry in __LeaderboardEntries)
            {
                _Lines.Add(_Entry._PlayerName + "," + _Entry._Score);
            }
            
            File.WriteAllLines(__FilePath, _Lines.ToArray());
        }
    }

    public class LeaderboardEntry
    {
        public string _PlayerName;
        public int _Score;

        public LeaderboardEntry(string playerName, int score)
        {
            _PlayerName = playerName;
            _Score = score;
        }
    }
}