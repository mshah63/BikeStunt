using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GN_SplashScreen : MonoBehaviour {

	[Header("Scene Selection")]
	public Scenes NextScene;

	[Header("Scene Settings")]
	public float WaitTime;

	void Start () {

		Time.timeScale = 1;
        if (!GameManager.Instance.Initialized) {
			InitializeGame();
		}
        StartCoroutine(StartGame());
        if (SaveData.Instance.IsSoundOn)
        {
            AudioListener.pause = false;
        }
        else
        {
            AudioListener.pause = true;
        }

        Debug.Log("played level are " + PlayerPrefs.GetInt("playedlevels"));

    }
    void InitializeGame() {
		SaveData.Instance = new SaveData();
		GN_SaveLoad.LoadProgress();
		GameManager.Instance.Initialized = true;
	}
	  IEnumerator StartGame(){
		yield return new WaitForSeconds (WaitTime);
		SceneManager.LoadScene(NextScene.ToString());
	}
}
