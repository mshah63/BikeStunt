using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    public GameObject loadingScreen;
    public float loadingDuration = 15f;
    public Slider loadingSlider;
    public GameObject _tutorialPanel;
    private bool _istutorialActive;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("tutorials = " + PlayerPrefs.GetInt("TutorialCompleted"));
    }

    void Start()
    {

        _istutorialActive = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
        Debug.Log("isturotiral" + _istutorialActive);
        loadingScreen.SetActive(_istutorialActive);
        if (loadingSlider)
            loadingSlider.value = 0;
        if(PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            StartCoroutine(UpdateSliderCoroutine());
            Invoke(nameof(HideLoadingScreen), loadingDuration);
        }
        
    }

    public void ShowLoadingScreen()
    {
        Debug.Log("Showing Loading Screen");
        loadingScreen.SetActive(true);
        StartCoroutine(UpdateSliderCoroutine()); // Ensure slider starts updating

        // Invoke HideLoadingScreen after loading duration
        Invoke(nameof(HideLoadingScreen), loadingDuration);
    }

    public void HideLoadingScreen()
    {
        Debug.Log("Hiding Loading Screen");
        loadingScreen.SetActive(false);
    }

    IEnumerator UpdateSliderCoroutine()
    {
        if (loadingSlider == null) yield break;

        float totalLoadingTime = loadingDuration;
        float elapsedTime = 0f;

        while (elapsedTime < totalLoadingTime)
        {
            float progress = elapsedTime / totalLoadingTime;
            loadingSlider.value = progress;
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        loadingSlider.value = 1f;
    }
}
