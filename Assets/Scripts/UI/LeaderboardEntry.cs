using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro rankText;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject highlightVisual;



    public PlayerData PlayerData { get; private set; }
    public string PlayerId { get; private set; }

    public void Setup(PlayerData data, bool isMe)
    {
        PlayerId = data.id;
        PlayerData = data;

        nameText.text = data.nickname;
        scoreText.text = data.score.ToString();
        rankText.text = $"#{data.rank}";
        BackToDefault();

        if (isMe)
        {
            BringToFront(); //me entrysi diğerlerinden önde olup pembe olacak
            highlightVisual.SetActive(true);
            background.SetActive(false);
        }
    }
    public void SetRank(int rank)
    {
        rankText.text = "#" + rank.ToString();
    }
    public void SetScore(int score)
    {
        scoreText.text = score.ToString(); 
    }

    public void SetPosition(Vector3 position)
    {
        position.z = transform.localPosition.z; 
        transform.localPosition = position;
    }

    public void BringToFront()//me nin diğer entrylerin önünde durmasını istiyorum
    {
        Vector3 pos = transform.localPosition;
        pos.z = -0.5f ;
        transform.localPosition = pos;
    }


    public void BackToDefault() //pooldan çektiğim entrylerde eski ayarlar kaldıysa diye defaulta döndürmek için
    {

        highlightVisual.SetActive(false);
        background.SetActive(true);
        Vector3 pos = transform.localPosition;
        pos.z = 0.0f;
        transform.localPosition = pos;
    }

}
