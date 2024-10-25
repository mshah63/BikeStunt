using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using JetBrains.Annotations;

public class LevelSelection : MonoBehaviour
{

    [Header("Scene Selection")]
    public Scenes PreviousScene;
    public Scenes NextScene;

    [Header("Settings")]
    public bool Locked;
    public int levelplayed;
    public int PlayableLevels = 1;

    [Header("UI Panels")]
    //public GameObject UnlockPopUpPanel;
    public GameObject LoadingScreen;
    public GameObject LevelsPanel;

    [Header("Audio Settings")]
    public AudioSource ButtonClick;

    private List<Button> LevelButtons = new List<Button>();
    AsyncOperation async = null;

    public static LevelSelection instance = null;
    /// -------------------------------------------------------------------/
    public Button[] levelButtons;
    public int unlockedBtn;
    public void OnClickedLevelButton(int levelindex)
    {
        Debug.Log("levelindex = " + levelindex);
#if UNITY_EDITOR
        GameManager.Instance.EditorSession = false;
#endif
        GameManager.Instance.CurrentLevel = levelindex;
        LoadingScreen.SetActive(true);
        if (AdManagerAdmob.instance && !SaveData.Instance.RemoveAds)
        {
           // AdManagerAdmob.instance.ShowInterstitialAd();
        }
        StartCoroutine(LevelStart());
        Debug.Log("level index is " + levelindex);
        PlayerPrefs.SetInt("levelindex", levelindex);
    }
    private void Update()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < unlockedBtn)
                levelButtons[i].interactable = true;
            else
                levelButtons[i].interactable = false;
        }

    }
    /// -------------------------------------------------------------------/
    private void Awake()
    {
        instance = this;
       
    }

    void Start()
    {
       // PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        LoadingScreen.SetActive(false);
        if (!GameManager.Instance.Initialized)
        {
            InitializeGame();
        }
    //    CacheButtons();
        LevelsInit();
        if (!SaveData.Instance.UnlockLevels)
        {
            // UnlockPopUpPanel.SetActive(true);
        }
        unlockedBtn = PlayerPrefs.GetInt("currentlevel");
        Debug.Log("unlockedBtn = " + unlockedBtn);
    }

    void InitializeGame()
    {
        SaveData.Instance = new SaveData();
        GN_SaveLoad.LoadProgress();
        GameManager.Instance.Initialized = true;
    }
    void CacheButtons()
    {
        Button[] levelButtons = LevelsPanel.transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            LevelButtons.Add(levelButtons[i]);
        }
        LevelButtons = LevelButtons.OrderBy(x => Int32.Parse(x.gameObject.name)).ToList();
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            int LevelIndex = i + 1;
            LevelButtons[i].onClick.AddListener(() => PlayLevel(LevelIndex));

            LevelButtons[i].onClick.AddListener(() => ButtonClick.Play());
        }
    }

    public void LevelsInit()
    {
        if (!Locked)
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                if (i < PlayableLevels)
                    LevelButtons[i].interactable = true;
                else
                    LevelButtons[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                LevelButtons[i].interactable = false;
            }
            for (int i = 0; i < LevelButtons.Count; i++)
            {
                if (i < SaveData.Instance.Level && i < PlayableLevels)
                {
                    LevelButtons[i].interactable = true;
                }
            }
        }
    }

    public void PlayLevel(int level)
    {
/*#if UNITY_EDITOR
        GameManager.Instance.EditorSession = false;
#endif
        GameManager.Instance.CurrentLevel = level;
        LoadingScreen.SetActive(true);
        if (AdsManager.instance && !SaveData.Instance.RemoveAds)
        {
            AdsManager.instance.ShowInterstitialAd();
        }
        StartCoroutine(LevelStart());
        Debug.Log("level index is " + level);*/
    }

    IEnumerator LevelStart()
    {
        yield return new WaitForSeconds(3);

        async = SceneManager.LoadSceneAsync(NextScene.ToString());
        yield return async;
    }

    public void BackBtn()
    {
        SceneManager.LoadScene(PreviousScene.ToString());
    }
    #region InApp
    public void UnlockLevels()
    {
        GameManager.Instance.UnlockLevels();
        LevelsInit();
    }
    #endregion

}
