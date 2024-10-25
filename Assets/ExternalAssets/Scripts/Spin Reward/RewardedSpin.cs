using UnityEngine;
using System.Collections;

public class RewardedSpin : MonoBehaviour
{

    [Header("Offer Wall Settings")]
    [Space]
    public bool EnabeOfferWall = false;

    [Header("UI Panels")]
    public GameObject OfferWall;
    TimedReward TimedReward;

    [Header("Ad Sequence ID")]
    public int SequenceID;

    private void Awake()
    {
        //ConsoliAds.Instance.LoadRewarded(SequenceID);
    }

    void OnEnable()
    {
        //if (ConsoliAds.Instance)
        //{
        //    ConsoliAds.Instance.LoadRewarded(SequenceID);
        //    ConsoliAds.onRewardedVideoAdCompletedEvent += RewardedVideoCompleted;
        //}
    }

    void OnDisable()
    {
        //if (ConsoliAds.Instance)
        //    ConsoliAds.onRewardedVideoAdCompletedEvent -= RewardedVideoCompleted;
    }

    void Start()
    {
        OfferWall.SetActive(false);
    }

    public void ShowRewardedVideo()
    {
    }

    public void RewardedVideoCompleted()
    {
        if (TimedReward == null)
        {
            Debug.LogError("No Reference of Timed reward");
            return;
        }
        TimedReward.ResetTime();
        OfferWall.SetActive(false);
    }

    IEnumerator CheckRewardedVideo()
    {
        yield return new WaitForSeconds(2.0f);
        //if (ConsoliAds.Instance.IsRewardedVideoAvailable(SequenceID))
        //{
        //    OfferWall.SetActive(true);
        //}
        //else
        //{
        //    ConsoliAds.Instance.LoadRewarded(SequenceID);
        //}
    }

    public void CheckRewardedVideo(TimedReward timedReward)
    {
        TimedReward = timedReward;
        StartCoroutine(CheckRewardedVideo());
    }
}
