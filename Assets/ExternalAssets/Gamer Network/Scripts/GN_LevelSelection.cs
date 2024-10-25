using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using JetBrains.Annotations;

public class GN_LevelSelection : MonoBehaviour
{

	[Header ("Scene Selection")]
	public Scenes PreviousScene;
	public Scenes NextScene;

	[Header ("Settings")]
	public bool Locked;
	private int unlocklevel;
	public Sprite _levelLockSprite;

	[Header ("UI Panels")]
	//public GameObject UnlockPopUpPanel;
    public GameObject LoadingScreen;
	public GameObject LevelsPanel;

	[Header ("Audio Settings")]
	public AudioSource ButtonClick;

	public List<Button> LevelButtons = new List<Button> ();
	AsyncOperation async = null;
    
    public static GN_LevelSelection instance = null;
	private void Awake()
	{
        instance = this;
		// PlayableLevels = PlayerPrefs.GetInt("currentlevel") + 1;
		//Debug.Log("playablelevels are " + PlayableLevels);
		// PlayerPrefs.SetInt("unlocklevel",0);
		
    }

	void Start ()
	{
        
        //PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
		LoadingScreen.SetActive (false);
		if (!GameManager.Instance.Initialized) {
			InitializeGame ();
		}//CacheButtons ();
		
        if (!SaveData.Instance.UnlockLevels)
        {
           // UnlockPopUpPanel.SetActive(true);
        }
		unlocklevel = PlayerPrefs.GetInt("playlevel");
	
        Debug.Log("unlock level" + unlocklevel);
        LevelsInit();
    }

	void InitializeGame ()
	{
		SaveData.Instance = new SaveData ();
		GN_SaveLoad.LoadProgress ();
		GameManager.Instance.Initialized = true;

        
}
	/*void CacheButtons ()
	{
		Button[] levelButtons = LevelsPanel.transform.GetComponentsInChildren <Button> ();
		for (int i = 0; i < levelButtons.Length; i++) {
			LevelButtons.Add (levelButtons [i]);		
		}
		LevelButtons = LevelButtons.OrderBy (x => Int32.Parse (x.gameObject.name)).ToList ();
		for (int i = 0; i < LevelButtons.Count; i++) {
			int LevelIndex = i + 1;
			LevelButtons [i].onClick.AddListener (() => PlayLevel (LevelIndex));
			LevelButtons [i].onClick.AddListener (() => ButtonClick.Play ());

		}
	}*/

	public void LevelsInit ()
	{
        /*if (!Locked) {
			for (int i = 0; i < LevelButtons.Count; i++) {
				if (i < PlayableLevels)
					LevelButtons [i].interactable = true;
				else
					LevelButtons [i].interactable = false;
			}
		} else {
			for (int i = 0; i < LevelButtons.Count; i++) {
				LevelButtons [i].interactable = false;
			}
			for (int i = 0; i < LevelButtons.Count; i++) {
				if (i < SaveData.Instance.Level && i < PlayableLevels) {
					LevelButtons [i].interactable = true;
				}
			}
		}*/
        for (int i = 0; i < LevelButtons.Count; i++)
        {
            if (i == 0) // Unlock the first level button
            {
                LevelButtons[i].interactable = true;
            }
            else if (i < unlocklevel)
            {
                LevelButtons[i].interactable = true;
            }
            else
            {
                LevelButtons[i].interactable = false;
				LevelButtons[i].image.sprite = _levelLockSprite;
            }
        }
    }

	public void PlayLevel (int level)
	{
		Time.timeScale = 1;
		#if UNITY_EDITOR
		GameManager.Instance.EditorSession = false;
		#endif
		GameManager.Instance.CurrentLevel = level;
		LoadingScreen.SetActive (true);
        if (AdManagerAdmob.instance && !SaveData.Instance.RemoveAds)
        {
           // AdManagerAdmob.instance.ShowInterstitialAd();
            //AdsManager.instance.RequestBanner();
        }
        StartCoroutine (LevelStart ());
		Debug.Log("level " + level);
		PlayerPrefs.SetInt("playlevel", level);
		//AdsManager.instance.RequestBanner();

    }

	IEnumerator LevelStart ()
	{
		yield return new WaitForSeconds (3);

		async = SceneManager.LoadSceneAsync (NextScene.ToString ());
		yield return async;
	}

	public void BackBtn ()
	{
		SceneManager.LoadScene (0);
        //AdManagerAdmob.instance.RequestBanner();
	}
    #region InApp
    public void UnlockLevels()
    {
        GameManager.Instance.UnlockLevels();
		//mshah
       // LevelsInit();
    }
    #endregion

}
