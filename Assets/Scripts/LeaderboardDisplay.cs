using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LeaderboardDisplay : MonoBehaviour
{
    [FormerlySerializedAs("leaderboardManager")] public LeaderboardManager __LeaderboardManager;
    [FormerlySerializedAs("leaderboardContent")] public Transform __LeaderboardContent; // Parent object for leaderboard entries
    [FormerlySerializedAs("entryPrefab")] public GameObject __EntryPrefab; // Prefab for individual leaderboard entries
    private IList<Model.LeaderboardEntry> __Entries;
    private void Start()
    {
        UpdateLeaderboardDisplay();
    }

    private void UpdateLeaderboardDisplay()
    {
        __Entries = new List<Model.LeaderboardEntry>();
        
        foreach (Transform _Child in __LeaderboardContent)
        {
            Destroy(_Child.gameObject);
        }

        if (__LeaderboardManager.__LeaderboardEntries?.Count > 0)
        {
            foreach (LeaderboardEntry _Entry in __LeaderboardManager.__LeaderboardEntries)
            {
                __Entries.Add(new Model.LeaderboardEntry
                {
                    Name = _Entry._PlayerName,
                    Score = _Entry._Score
                });
            }
            
            IList<Model.LeaderboardEntry> _SortedEntries = __Entries.OrderByDescending(x => x.Score).ToList();

            for (int i = 0; i < _SortedEntries.Count(); i++)
            {
                _SortedEntries[i].Rank = i + 1;
                GameObject _EntryObject = Instantiate(__EntryPrefab, __LeaderboardContent);
                TMP_Text[] _Texts = _EntryObject.GetComponentsInChildren<TMP_Text>();
                if (_Texts.Length < 3)
                {
                    Debug.LogError("Not enough Text components in children. Found: " + _Texts.Length);
                    return;
                }

                _Texts[0].text = _SortedEntries[i].Rank.ToString();
                _Texts[1].text = _SortedEntries[i].Name;
                _Texts[2].text = _SortedEntries[i].Score.ToString();
            }
        }
    }
}