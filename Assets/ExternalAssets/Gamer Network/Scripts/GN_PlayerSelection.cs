using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Selection_Elements {
	public GameObject LoadingScreen;
	public Slider FillBar;
	[Header ("Player Attributes")]
	public Image Speed_Bar;
	public Image Handling_Bar;
	public Image Acceleration_Bar;
	public Text InfoText, Price;
	public GameObject buyButton;

	[Header ("UI Buttons")]
	public Button PlayBtn;
	public GameObject NextBtn;
	public GameObject PrevBtn;
	
}

[System.Serializable]
public class PlayerAttributes {
	public string Name;
	[Tooltip ("Text to display when this Player is locked.")]
	[Multiline]
	public string Info;
	public GameObject PlayerObject;
	[Range (0, 100)]
	public int Speed;
	[Range (0, 100)]
	public int Handling;
	[Range (0, 100)]
	public int Acceleration;
	public bool Locked;
	[Tooltip ("Enter price which is required to buy")]
	public int CoinsRequired;

}

public class GN_PlayerSelection : MonoBehaviour {

	[Header ("Scene Selection")]
	public Scenes PreviousScene;
	public Scenes NextScene;

	[Header ("UI Elements")]
	//public GameObject UnlockPopUpPanel;
	public Selection_Elements Selection_UI;
	public Text coinsText;

	[Header ("Player Attributes")]
	public PlayerAttributes[] Players;

	AsyncOperation async = null;
	private int current;
    public static GN_PlayerSelection instance = null;
	private void Awake()
	{
        instance = this;
        InitializeCars();
    }

    void Start () {
		Time.timeScale = 1;
		Selection_UI.FillBar.value = 0;
		Selection_UI.LoadingScreen.SetActive (false);
		checkCoins();
		if (!GameManager.Instance.Initialized) {
			InitializeGame ();
		}
		GetPlayerInfo ();
        if (!SaveData.Instance.UnlockCars)
        {
           // UnlockPopUpPanel.SetActive(true);
        }
		if (AdManagerAdmob.instance)
		{
           // AdManagerAdmob.instance.RequestBanner();
		}
   }

    void InitializeGame () {
		SaveData.Instance = new SaveData ();
		GN_SaveLoad.LoadProgress ();
		GameManager.Instance.Initialized = true;
	}
    void InitializeCars()
    {
        if (SaveData.Instance.FirstTime)
        {
            bool[] data = new bool[Players.Length];
            for (int i = 0; i < Players.Length; i++)
            {
                data[i] = !Players[i].Locked;
                Debug.Log("Car Lock Value :" + data[i]);

            }
            SaveData.Instance.InitializeCars(Players.Length, data);
            SaveData.Instance.FirstTime = false;
            GN_SaveLoad.SaveProgress();
        }
    }
    void Update () {
		if (async != null) {
			Selection_UI.FillBar.value = async.progress;
			if (async.progress >= 0.9f) {
				Selection_UI.FillBar.value = 1.0f;
			}
		}
	}

    public void checkCoins()
    {
        coinsText.text = SaveData.Instance.Coins.ToString();
    }

    public void BuyCar()
	{
		SaveData.Instance.Coins -= Players[current].CoinsRequired;
		checkCoins();
		SaveData.Instance.Cars[current].unlocked = true;
		GN_SaveLoad.SaveProgress();
		GetPlayerInfo();
	}

	public void GetPlayerInfo () {
		for (int i = 0; i < Players.Length; i++) {
			if (i == current) {
				Players [i].PlayerObject.SetActive (true);
			} else if (i != current) {
				Players [i].PlayerObject.SetActive (false);
			}
		}

		Selection_UI.InfoText.text = Players[current].Name;
		Selection_UI.Speed_Bar.fillAmount = Players [current].Speed / 100.0f;
		Selection_UI.Handling_Bar.fillAmount = Players [current].Handling / 100.0f;
		Selection_UI.Acceleration_Bar.fillAmount = Players [current].Acceleration / 100.0f;
        Debug.Log("Car Current : " + current);
        Debug.Log("Car Data : " + SaveData.Instance.Cars[current].unlocked);
		if (!SaveData.Instance.Cars[current].unlocked)
		{
			Selection_UI.PlayBtn.gameObject.SetActive(false);
			Selection_UI.Price.enabled = true;
			Selection_UI.Price.text = Players[current].CoinsRequired.ToString();
			if (Players [current].CoinsRequired <= SaveData.Instance.Coins) {
				Selection_UI.buyButton.GetComponent<Button>().interactable = true;
				Selection_UI.buyButton.SetActive(true);
			} else{
				Selection_UI.buyButton.SetActive(true);
				Selection_UI.buyButton.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			Selection_UI.PlayBtn.gameObject.SetActive(true);
			Selection_UI.Price.enabled = false;
			Selection_UI.PlayBtn.interactable = true;
			Selection_UI.buyButton.SetActive(false);
		}



		if (current == 0) {
            Debug.Log("if");
            Selection_UI.PrevBtn.SetActive (false);
			Selection_UI.NextBtn.SetActive (true);
		} else if (current == Players.Length - 1) {
            Debug.Log("Else if");
            Selection_UI.PrevBtn.SetActive (true);
			Selection_UI.NextBtn.SetActive (false);
		} else {
            Debug.Log("Else");
			Selection_UI.PrevBtn.SetActive (true);
			Selection_UI.NextBtn.SetActive (true);
		}
	}

	public void Previous () {
		current--;
		GetPlayerInfo ();
	}

	public void Next () {
		current++;
		GetPlayerInfo ();
		
	}

	public void PlayLevel () {
		#if UNITY_EDITOR
		GameManager.Instance.EditorSession = false;
		#endif
		GameManager.Instance.CurrentPlayer = current;
		Selection_UI.LoadingScreen.SetActive (true);
        if (AdManagerAdmob.instance && !SaveData.Instance.RemoveAds)
        {
           // AdManagerAdmob.instance.ShowInterstitialAd();
        }
        StartCoroutine (LevelStart ());
		
       // AdManagerAdmob.instance.DestroyBanner();
    }

	IEnumerator LevelStart () {
		yield return new WaitForSeconds (3);
		async = SceneManager.LoadSceneAsync (NextScene.ToString());
		yield return async;
	}

	public void BackBtn () {
		SceneManager.LoadScene (PreviousScene.ToString ());
       // AdManagerAdmob.instance.DestroyBanner();
	}
    #region InApp
    public void UnlockPlayers()
    {
        GameManager.Instance.UnlockCars();
        GetPlayerInfo();
    }
    #endregion
}
