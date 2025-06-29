using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class LeaderboardManager : MonoBehaviour
{
    public LeaderboardConfig config;
    public LeaderboardEntry entryPrefab;

    private List<PlayerData> allPlayers;
    private PlayerData mePlayer;
    private int meIndex;

    private LeaderboardView view;
    public int OldMeScore { get; private set; } //animasyon için me entrysinin eski scoreu
    public LeaderboardEntry GetEntryPrefab() => entryPrefab;
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        view = GetComponent<LeaderboardView>();
        if (view == null)
        {
            Debug.LogError("leaderboardview not found!!!!");
            return;
        }

        view.Initialize(config, this);

        LoadPlayers();
        SortAndRankPlayers();
        DisplayPlayers();
    }

    void LoadPlayers()
    {
        allPlayers = JsonLoader.LoadPlayers().ToList();
        mePlayer = allPlayers.FirstOrDefault(p => p.isMe);
    }

    void SortAndRankPlayers()//playerlaroı sıralar ve ranklarını verir
    {
        allPlayers = allPlayers.OrderByDescending(p => p.score).ToList();

        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].rank = i + 1;
        }

        meIndex = allPlayers.IndexOf(mePlayer);
    }

    void DisplayPlayers()
    {
        view.DisplayLeaderboard(allPlayers, meIndex);
    }

    public void RequestLeaderboardUpdate()//leaderboardu pdatelemk için kullanıyorum 
    {                                     // eğer "me"nin rankı değişmediyse yalnızca güncelleme yapar, değiştiyse animasyonla günceller
        int oldMeIndex = meIndex;
        OldMeScore = mePlayer.score;
        UpdateScores();
        SortAndRankPlayers();

        if (oldMeIndex == meIndex)
        {
            view.DisplayLeaderboard(allPlayers, meIndex);
        }
        else
        {
            view.DisplayLeaderboardAnimated(allPlayers, meIndex, oldMeIndex);
        }
    }

    void UpdateScores() //her oyuncuya rastgele puan ekler-çıkarır
    {
        foreach (var player in allPlayers)
        {
              player.score += Random.Range(-100, 201);
              player.score = Mathf.Max(0, player.score);
            
        }

    }

}
