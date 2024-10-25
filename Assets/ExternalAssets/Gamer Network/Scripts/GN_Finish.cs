using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GN_Finish : MonoBehaviour
{
    int currentlevel;
    // Start is called before the first frame update
    void Start()
    {

        currentlevel = PlayerPrefs.GetInt("playlevel");
    }
    private void OnEnable()
    {
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameManager.Instance.TaskComplete();
            Debug.Log("yhi ha current level " + currentlevel);
            //PlayerPrefs.SetInt("playedlevels", currentLevel);
            //   Debug.Log("current level is " + PlayerPrefs.GetInt("playedlevels"));
            PlayerPrefs.SetInt("playlevel", currentlevel + 1);
            PlayerPrefs.Save();
            Debug.Log("your next unlock level is " + PlayerPrefs.GetInt("playlevel"));
            AdManagerAdmob.instance.DestroyBanner();
        }
      
    }
}
