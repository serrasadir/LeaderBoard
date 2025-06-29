using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string id;
    public string nickname;
    public int score;
    public int rank;
    public bool isMe => id == "me";

    public PlayerData(string id, string nickname, int score)
    {
        this.id = id;
        this.nickname = nickname;
        this.score = score;
    }
}

[System.Serializable]
public class PlayerDataList
{
    public PlayerData[] players;
}