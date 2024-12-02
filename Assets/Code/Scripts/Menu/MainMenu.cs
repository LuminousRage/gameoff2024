using TMPro;
using UnityEngine;
using sm = UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        var optionsMenu = FindAnyObjectByType<OptionsMenu>(UnityEngine.FindObjectsInactive.Include);
        optionsMenu.OnStart();
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("IntroDone", 0);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("ContinueLevel", 0);
        PlayerPrefs.Save();

        ContinueGame();
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("IntroDone", 0) == 0)
        {
            sm.SceneManager.LoadScene("Intro Dialogue");
        }
        else
        {
            sm.SceneManager.LoadScene("Main (Dynamic)");
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING!");
        Application.Quit();
    }
}
