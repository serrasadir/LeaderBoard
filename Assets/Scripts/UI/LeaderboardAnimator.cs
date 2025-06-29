using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LeaderboardAnimator : MonoBehaviour
{
    public System.Action OnExpansionReady;

    public void AnimateUpdate( int oldMeScore,int newMeScore,int oldMeIndex, int newMeIndex,LeaderboardEntry meEntry,Vector3 meTargetLocalPos,Vector3 containerTargetLocalPos,float duration,Transform container)
    {
        Vector3 meWorldPos = meEntry.transform.position;

        meEntry.transform.SetParent(container.parent, worldPositionStays: false);
        meEntry.transform.position = meWorldPos; //burada bug olmuştu detach ederken, eski pozisyona tekrar sabitlemek zorunda kaldım o yüzden
        DOTween.Kill(meEntry.transform);

        container.DOLocalMove(containerTargetLocalPos, duration).SetEase(Ease.OutCubic); //containerın kaydırma işlemi

        DOTween.To(() => oldMeScore, x => { //scoreun sayaç gibi artıp azalması için
            oldMeScore = x;
            meEntry.SetScore(oldMeScore);
        }, newMeScore, duration)
        .SetEase(Ease.Linear);

        int oldRank = oldMeIndex + 1;
        int newRank = newMeIndex + 1;

        DOTween.To(() => oldRank, x => { //rankın sayaç gibi artıp azalması için
            oldRank = x;
            meEntry.SetRank(oldRank);
        }, newRank, duration)
        .SetEase(Ease.Linear);

        DOVirtual.DelayedCall(duration, () =>
        {
            meEntry.transform.SetParent(container);
            meEntry.transform.DOLocalMove(meTargetLocalPos, duration) //container yerini aldıktan me entrysi gerekirse kendi yerine gidiyor
                .SetEase(Ease.OutBack);
        });
    }

}
