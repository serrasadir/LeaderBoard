using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeaderboardUpdateButton : MonoBehaviour
{
    private LeaderboardManager leaderboardManager;

    public void Initialize(LeaderboardManager manager)
    {
        this.leaderboardManager = manager;
    }


    private void OnMouseDown()
    {
        Debug.Log("click");
        if (leaderboardManager != null)
        {
            leaderboardManager.RequestLeaderboardUpdate();
        }
    }
}
