using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LeaderboardView : MonoBehaviour
{
    private LeaderboardConfig config;
    private LeaderboardManager leaderboardManager;
    private ObjectPool<LeaderboardEntry> entryPool;
    private List<LeaderboardEntry> activeEntries = new List<LeaderboardEntry>();
    private Transform containerTransform;
    [SerializeField] private LeaderboardUpdateButton updateButton;
    private LeaderboardAnimator animator;

    public void Initialize(LeaderboardConfig config, LeaderboardManager manager)
    {
        this.config = config;
        this.leaderboardManager = manager;
        animator = GetComponent<LeaderboardAnimator>();

        SetupContainer();
        SetupPool();
        SetupUpdateButton();
    }

    private void SetupUpdateButton()
    {
        updateButton.Initialize(leaderboardManager);
    }

    private void SetupContainer() //entrylerin gözükeceği containerı oluşturdum
    {
        GameObject container = new GameObject("LeaderboardContainer");
        container.transform.SetParent(transform);
        container.transform.localPosition = config.StartPosition;
        containerTransform = container.transform;
    }

    private void SetupPool()//pool oluşturduk, gözükecek entry sayısında biraz fazla koydum buffer olsun diye
    {
        entryPool = new ObjectPool<LeaderboardEntry>(leaderboardManager.GetEntryPrefab(), containerTransform, config.VisibleEntries + 5 ); 
    }

    public void DisplayLeaderboard(List<PlayerData> players, int meIndex)//animasyon olmadan leaderboardu statik olarak gösteriyoruz
                                                                            //(me ortada - ya da configte seçilen idxte - olacak şekilde)
    {
        entryPool.ReturnAll(activeEntries);
        int startIndex = Mathf.Max(0, meIndex - config.MeVisualIndex);
        int endIndex = Mathf.Min(players.Count, startIndex + config.VisibleEntries);
        startIndex = Mathf.Max(0, endIndex - config.VisibleEntries);

        for (int i = startIndex; i < endIndex; i++)
        {
            var player = players[i];
            var entry = entryPool.Get();
            entry.Setup(player, player.isMe);
            Vector3 position = CalculateEntryPosition(i - meIndex + 4);
            entry.SetPosition(position);
            activeEntries.Add(entry);
        }

        containerTransform.localPosition = config.StartPosition;
    }

    public void DisplayLeaderboardAnimated(List<PlayerData> players, int newMeIndex, int oldMeIndex)//me entrysinin sıralaması değiştiğinde leaderboardu animasyonlu güncelliyoruz
    {
        PlayerData mePlayer = players.FirstOrDefault(p => p.isMe);
        int meOriginalRank = mePlayer.rank;
        mePlayer.rank = oldMeIndex + 1;
        int meNewScore = mePlayer.score;
        int meOldScore = leaderboardManager.OldMeScore;

        LeaderboardEntry existingMeEntry = activeEntries.FirstOrDefault(e => e.PlayerId == "me");

        if (existingMeEntry != null) activeEntries.Remove(existingMeEntry);

        entryPool.ReturnAll(activeEntries);

        var (startIndex, endIndex) = CalculateExpansionRange(oldMeIndex, newMeIndex, players.Count);

        ExpandContainerForAnimation(players, startIndex, endIndex, oldMeIndex, existingMeEntry);

        LeaderboardEntry meEntry = activeEntries.FirstOrDefault(e => e.PlayerId == "me");


        int firstVisibleIdx = newMeIndex - 4;

       
        float offset = CalculateContainerOffset(startIndex, endIndex);


        Vector3 containerTargetPos = newMeIndex < oldMeIndex //eğer me yükseldiyse container aşağı gelecek, me alçaldıysa container yukarı gelecek
            ? config.StartPosition - new Vector3(0, offset, 0)
            : config.StartPosition + new Vector3(0, offset, 0);


        float meTargetZ = -0.5f;
        Vector3 meTargetPos = CalculateEntryPosition((meOriginalRank ) - firstVisibleIdx - 4) - containerTargetPos;
        meTargetPos.z = meTargetZ;//me entry hala önde kalsın istiyorum o yüzden z pozisyonunu korudum

        animator.AnimateUpdate(meOldScore, meNewScore,oldMeIndex, newMeIndex,meEntry, meTargetPos,containerTargetPos, config.AnimationDuration, containerTransform );
    }

    private void ExpandContainerForAnimation(List<PlayerData> players, int startIndex, int endIndex, int oldMeIndex, LeaderboardEntry existingMeEntry)
    {
        for (int i = startIndex-1; i <= endIndex+1; i++) //me'nin eski ve yeni rankı arasında kalan entryleri ekleyerek leaderboardu büyütüyorum
                                                            //sonra animasyonla kaydıracağız 
        {
            var player = players[i];
            LeaderboardEntry entry;

            if (player.isMe && existingMeEntry != null)
            {
                entry = existingMeEntry;
                entry.Setup(player, true);
            }
            else
            {
                entry = entryPool.Get();
                entry.Setup(player, player.isMe);
                Vector3 entryPos = CalculateEntryPosition(i - oldMeIndex + 4);
                entry.SetPosition(entryPos);
            }

            activeEntries.Add(entry);
        }
    }

    private (int startIndex, int endIndex) CalculateExpansionRange(int oldMeIndex, int newMeIndex, int totalCount)//hangi aralıktaki entryleri tekrar oluşturcağımı hesaplıyorum
    {
        int start = Mathf.Max(0, Mathf.Min(oldMeIndex - 4, newMeIndex - 4));
        int end = Mathf.Min(totalCount, Mathf.Max(oldMeIndex + 4, newMeIndex + 4));
        return (start, end);
    }

    private float CalculateContainerOffset(int startIndex, int endIndex) //containerı ne kadar kaydırmam gerektiği hesaplıyorum
    {
        float containerHeight = (endIndex - startIndex + 1) * config.EntrySpacing * 0.01f;
        float visibleHeight = config.VisibleEntries * config.EntrySpacing * 0.01f;
        return containerHeight - visibleHeight;
    }

    private Vector3 CalculateEntryPosition(int localIndex)//yeni eklenen entrylerin nereye gelmesi gerektiğini hesaplıyorum
    {
        float y = -localIndex * config.EntrySpacing * 0.01f;
        return new Vector3(0, y, 0);
    }
}
