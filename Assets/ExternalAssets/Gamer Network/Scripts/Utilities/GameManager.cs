using UnityEngine;
using System.Collections;

public class GameManager {

	private static GameManager instance;

	private GameManager () {
	}

	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = new GameManager ();
			}
			return instance;
		}
	}

	public bool Initialized = false;
	public int CurrentLevel = 1;
	public int CurrentPlayer = 1;
	public bool isParked = false;
//	public int currentMode=1;
	public string GameStatus;
	public int Objectives;

	public bool steeringControl = false;
	public bool arrowControl = true;
	

	#if UNITY_EDITOR
	public bool EditorSession = true;
	#endif

	public void TaskComplete () {
		if (Objectives > 0)
			Objectives--;
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GN_GameController> ().OnLevelCheck (0);
	}

	public void GameLoose (int reasonIndex = 0) {
		if (GameStatus != "Loose") {
			GameStatus = "Loose";
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GN_GameController> ().OnLevelCheck (reasonIndex);
		} else {
			Debug.LogWarning ("Game loose being called multiple times !");
		}
	}

	public void PauseTimer () {
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GN_GameController> ().TimerPaused = true;
	}

	public void ResumeTimer () {
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GN_GameController> ().TimerPaused = false;
	}

	public void UpdateInventory () {
		//Give items to player here
	}
    #region InApp
    public void CoinsPurchase(int coins)
    {
        SaveData.Instance.Coins += coins;
        GN_SaveLoad.SaveProgress();
    }
    public void RemoveAds()
	{
		SaveData.Instance.RemoveAds = true;
        if (AdManagerAdmob.instance)
        {
           // AdManagerAdmob.instance.DestroyBanner();
        }
        GN_SaveLoad.SaveProgress();
    }

    public void UnlockLevels()
	{
        SaveData.Instance.Level = 20;
        SaveData.Instance.UnlockLevels = true;
        GN_SaveLoad.SaveProgress();
    }

    public void UnlockCars()
	{
		for (int i = 0; i < SaveData.Instance.Cars.Length; i++)
		{
			SaveData.Instance.Cars[i].unlocked = true;
		}
		SaveData.Instance.UnlockCars = true;
        GN_SaveLoad.SaveProgress();
    }

    public void UnlockEverything()
	{
		RemoveAds();
		UnlockLevels();
		UnlockCars();
		SaveData.Instance.UnlockEverything = true;
        GN_SaveLoad.SaveProgress();
    }
    #endregion
}