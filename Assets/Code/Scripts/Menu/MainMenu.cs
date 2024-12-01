using UnityEngine;
using sm = UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame() {
        PlayerPrefs.SetInt("IntroDone",0);
        PlayerPrefs.SetInt("ContinueLevel",0);
        ContinueGame();
    }
    public void ContinueGame() {
        if (PlayerPrefs.GetInt("IntroDone",0)==0) {
            sm.SceneManager.LoadScene("Intro Dialogue");
        } else {
            sm.SceneManager.LoadScene("Main (Dynamic)");
        }
    }
    public void QuitGame()
    {
        Debug.Log("QUITTING!");
        Application.Quit();
    }
}
