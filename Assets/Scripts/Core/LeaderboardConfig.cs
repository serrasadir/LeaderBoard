using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LeaderboardConfig", menuName = "Leaderboard/Config")]
public class LeaderboardConfig : ScriptableObject
{

    public int VisibleEntries = 9; //ekranda kaç tane entry gözükeceği
    public int MeVisualIndex = 4;//gözüken entrylerden kaçıncısının (idx olarak) "me" entrysi olacağı
    public float EntrySpacing = 90;
    public float EntryHeight = 70f;
    public Vector3 StartPosition =new Vector3(0,2.7f,0);//containerın start alacağı pozisyon
   
    public float AnimationDuration = 1f;
}
