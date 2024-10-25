using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GN_MainMenu : MonoBehaviour {

	[Header ("Scene Selection")]
	public Scenes NextScene;

	[Header ("UI Panels")]
	//public GameObject UnlockPopUpPanel;
	public GameObject storePanel;
	public GameObject settingPanel;
	public GameObject rewardPanel;
	public GameObject ExitDialogue;
	public Text coinsText;
   // public Slider loadingSlider;
   // public GameObject loadingScreen;
    [Header ("Live Main Menu objects")]
    public bool isLiveMainMenu= true;
	public GameObject[] LiveMainMenuObjects;

    public static GN_MainMenu instance = null;
	void Awake () {
        instance = this;
    }

	void Start () {
      
        Time.timeScale = 1;
		if (!GameManager.Instance.Initialized) {
			InitializeGame ();
		}
		InitializeUI ();
        if (!SaveData.Instance.UnlockEverything)
        {
           // UnlockPopUpPanel.SetActive(true);
        }
    }
    private void Update()
    {
        if (AdManagerAdmob.instance._isbannershown)
        {
            AdManagerAdmob.instance.DestroyBanner();
        }
    }

    void InitializeGame () {
		SaveData.Instance = new SaveData ();
		GN_SaveLoad.LoadProgress ();
		GameManager.Instance.Initialized = true;
	}
	void InitializeUI ()
	{
		checkCoins();
        if (isLiveMainMenu)
        {
            foreach(GameObject gm in LiveMainMenuObjects)
            {
                gm.SetActive(false);
            }
            if (LiveMainMenuObjects.Length > 0)
            {
                LiveMainMenuObjects[Random.Range(0, LiveMainMenuObjects.Length)].SetActive(true);
            }
        }
        else
        {
            foreach (GameObject gm in LiveMainMenuObjects)
            {
                gm.SetActive(false);
            }
        }
		ExitDialogue.SetActive (false);
	}

	public void PlayBtn () {
		Debug.Log(NextScene.ToString());
		SceneManager.LoadScene (NextScene.ToString ());
       // AdManagerAdmob.instance.DestroyBanner();
	}

	public void checkCoins()
	{
		coinsText.text = SaveData.Instance.Coins.ToString();
	}
	public void soundOn()
	{
        SaveData.Instance.IsSoundOn = true;
		GN_SaveLoad.SaveProgress();
        AudioListener.pause = false;
	}
	public void soundOff()
	{
        SaveData.Instance.IsSoundOn = false;
        GN_SaveLoad.SaveProgress();
        AudioListener.pause = true;
    }

	public void selectControl(int control)
	{
		if (control == 1)
		{
			GameManager.Instance.steeringControl = true;
			GameManager.Instance.arrowControl = false;
		}
		else if(control == 2)
		{
			GameManager.Instance.arrowControl = true;
			GameManager.Instance.steeringControl = false;
		}
	}


	public void ShowRateUs () {
		Application.OpenURL("");
	}
    public void MoreGamesBtn()
    {
        Application.OpenURL("");
    }
    public void Exit () {
		Application.Quit ();
	}
    
    public void ResetSaveData () {
		SaveData.Instance = null;
		GN_SaveLoad.DeleteProgress ();
		SaveData.Instance = new SaveData ();
		GN_SaveLoad.LoadProgress ();
	}
    #region Ui Buttons
    public void ShowStore()
    {
        storePanel.SetActive(true);
    }
    public void CloseStore()
    {
        storePanel.SetActive(false);
    }
    public void ShowSetting()
    {
        settingPanel.SetActive(true);
        //if (AdManagerAdmob.instance._isbannershown == false)
           // AdManagerAdmob.instance.RequestBanner();
    }
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
    public void ShowReward()
    {
        rewardPanel.SetActive(true);
    }
    public void CloseReward()
    {
        rewardPanel.SetActive(false);
        //if (AdsManager.instance._isbannershown == false)
       // AdManagerAdmob.instance.RequestBanner();
    }
    #endregion
    #region InApp
    public void CoinsPurchase(int coins)
    {
        GameManager.Instance.CoinsPurchase(coins);
       checkCoins();
    }

    public void RemoveAds()
    {
        GameManager.Instance.RemoveAds();
        if (AdManagerAdmob.instance)
        {
           // AdManagerAdmob.instance.DestroyBanner();
        }
    }
    public void UnlockEverything()
    {
        GameManager.Instance.UnlockEverything();
    }
    #endregion
   /* IEnumerator UpdateSliderCoroutine()
    {
        // Define the total duration of loading
        float totalLoadingTime = 5f;
        float elapsedTime = 0f;

        // Loop until the loading is complete
        while (elapsedTime < totalLoadingTime)
        {
            // Calculate the progress as a value between 0 and 1
            float progress = elapsedTime / totalLoadingTime;

            // Update the slider value
            loadingSlider.value = progress;

            // Wait for a short time before updating again
            yield return new WaitForSeconds(0.1f);

            // Update the elapsed time
            elapsedTime += 0.1f;
        }

        // Ensure the slider value is set to 1 when loading is complete
        loadingSlider.value = 1f;
        if(loadingSlider.value > 0.9f)
        {
            loadingScreen.SetActive(false);
        }
    }*/
}
