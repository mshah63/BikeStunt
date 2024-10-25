using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialImages;
    public Button[] continueButtons;
    private int currentImageIndex = 0;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 1)
        {
            // Tutorial already completed, deactivate the tutorial panel
            this.gameObject.SetActive(false);
            return;
        }
       

        ShowTutorialImage(currentImageIndex);

        // Add listeners to the continue buttons
        for (int i = 0; i < continueButtons.Length; i++)
        {
            int index = i;
            continueButtons[i].onClick.AddListener(() => ContinueTutorial(index));
        }
    }

    public void SkipTutorial()
    {
        this.gameObject.SetActive(false);
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save(); // Ensure prefs are saved
        LoadingScreen.instance.ShowLoadingScreen();
        Debug.Log("tutorials = " + PlayerPrefs.GetInt("TutorialCompleted"));
    }

    private void ShowTutorialImage(int index)
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            tutorialImages[i].SetActive(i == index);
        }
    }

    private void ContinueTutorial(int index)
    {
        currentImageIndex = index + 1;
        if (currentImageIndex < tutorialImages.Length)
        {
            ShowTutorialImage(currentImageIndex);
        }
        else
        {
            // Tutorial completed
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save(); // Ensure prefs are saved
            LoadingScreen.instance.ShowLoadingScreen();
            gameObject.SetActive(false);
            Debug.Log("tutorials = " + PlayerPrefs.GetInt("TutorialCompleted"));
        }
    }
}
