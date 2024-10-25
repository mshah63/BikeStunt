using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NiobiumStudios;
using DG.Tweening;
//using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TimedRewards))]
public class TimedReward : MonoBehaviour
{
    [Range(0, 5)]
    [Tooltip("Initial Spin delay, wait before spin.")]
    public float InitialSpinDelay = 2f;
    [Tooltip("Timer Text")]
    public Text timer;
    [Tooltip("Spin Button, Button when user click spin starts")]
    public Button SpinButton;
    [Header("Reward Panel")]
    public GameObject RewardPanel;
    [Tooltip("Reward Description Text, The Text shown on Reward")]
    [TextArea(5, 10)]
    public string RewardPanelDescriptionText = "You have been rewarded with {0} {1}. Stay Tuned to get more Free Spin!";
    [Tooltip("Reward Description, Description text where we show user reward")]
    public Text RewardPanelDescription;
    [Tooltip("Image Icon on Reward Panel, Reward Icon image reference so Image update at runtime")]
    public Image PanelImageIcon;

    [Header("Unity Events")]
    public UnityEvent SpinCompleted;

    bool isFirstTime = false;
    private BaseSpin spin;
    private TimedRewards timedRewards;

    private void Awake()
    {
        timedRewards = GetComponent<TimedRewards>();
        spin = GetComponent<BaseSpin>();
        RewardPanel.SetActive(false);
    }

    /// <summary>
    /// calls when object enable
    /// </summary>
    void OnEnable()
    {
        timedRewards.onCanClaim += OnCanClaim;
        timedRewards.onInitialize += OnInitialize;
        spin.OnSpinCompleted += OnSpinCompleted;
    }

    /// <summary>
    /// calls when object is disable
    /// </summary>
    void OnDisable()
    {
        if (timedRewards != null)
        {
            timedRewards.onCanClaim -= OnCanClaim;
            timedRewards.onInitialize -= OnInitialize;
            spin.OnSpinCompleted += OnSpinCompleted;
        }
    }

    /// <summary>
    /// button subscribed on click claim button clicked
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoClaim()
    {
        yield return new WaitForSeconds(InitialSpinDelay);
        // make a spin and start spin time and play animation
        spin.MakeASpin();
    }

    /// <summary>
    /// When spin completed reward given
    /// </summary>
    /// <param name="index">reward index</param>
    void OnSpinCompleted(int index)
    {
        timer.transform.DOKill();
        timedRewards.ClaimReward(index);
        RewardPanel.SetActive(true);
        var reward = timedRewards.GetReward(index);

        PanelImageIcon.sprite = reward.sprite;
        PanelImageIcon.SetNativeSize();
        RewardPanelDescription.text = string.Format(RewardPanelDescriptionText, reward.reward, reward.unit);
        SpinButton.onClick.RemoveAllListeners();
        SpinButton.gameObject.SetActive(false);

        if (isFirstTime)
        {
            StartCoroutine(TickTime());
        }

        // Unity Event Call
        SpinCompleted.Invoke();

        //GSF_SaveLoad.SaveProgress();
    }

    /// <summary>
    /// calls when timer end and spin is availaible
    /// </summary>
    private void OnCanClaim()
    {
        SpinButton.gameObject.SetActive(true);
        timer.text = "Free Spin";
        timer.transform.DOScale(1.2f, 0.25f).SetLoops(10000, LoopType.Yoyo);
        SpinButton.onClick.RemoveAllListeners();
        SpinButton.onClick.AddListener(() => { StartCoroutine(DoClaim()); });
    }

    /// <summary>
    /// calls when Timed Reward Initialized
    /// </summary>
    /// <param name="error">error if any</param>
    /// <param name="errorMessage">error message</param>
    private void OnInitialize(bool error, string errorMessage)
    {
        if (!error)
        {
            // free first spin
            if (PlayerPrefs.GetInt("IsFirstSpin") == 0)
            {
                OnCanClaim();
                PlayerPrefs.SetInt("IsFirstSpin", 1);
                PlayerPrefs.Save();
                isFirstTime = true;
            }
            else if (PlayerPrefs.GetInt("IsFirstSpin") == 1)
            {
                SpinButton.gameObject.SetActive(false);
                StartCoroutine(TickTime());
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// check timer and update timer text
    /// </summary>
    /// <returns></returns>
    private IEnumerator TickTime()
    {
        for (; ; )
        {
            // Updates the timer UI
            timedRewards.TickTime();
            UpdateTextInfo();
            yield return null;
        }
    }

    /// <summary>
    /// Update timer text
    /// </summary>
    private void UpdateTextInfo()
    {
        if (timedRewards.timer.TotalSeconds > 0)
            timer.text = timedRewards.GetFormattedTime();
    }

    /// <summary>
    /// Reset Timed reward.
    /// Cheat use when rewarded video seen. :P
    /// </summary>
    public void ResetTime()
    {
        timedRewards.Reset();
    }
}
