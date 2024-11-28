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
        sm.SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        Debug.Log("QUITTING!");
        Application.Quit();
    }
}
