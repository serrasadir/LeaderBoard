using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonLoader //json dosyasından oyucu datasını yüklüyoruz
{
    private const string FilePath = "player_data";//resources/player_data.json olacak

    public static PlayerData[] LoadPlayers()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(FilePath);
        if (jsonFile == null)
        {
            Debug.LogError("Json file null");
            return null;
        }

        PlayerDataList playerList = JsonUtility.FromJson<PlayerDataList>(jsonFile.text);
        return playerList.players;
    }
}
