using UnityEngine;
using UnityEngine.EventSystems;
using sm = UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public AudioSource endSong;
    public EventTrigger.TriggerEvent start;

    void Start()
    {
        BaseEventData eventData = new BaseEventData(EventSystem.current);
        eventData.selectedObject = this.gameObject;
        start.Invoke(eventData);
        endSong?.Play();
    }

    public void MoveToMain()
    {
        PlayerPrefs.SetInt("IntroDone", 1);
        PlayerPrefs.Save();

        sm.SceneManager.LoadScene("Main (Dynamic)");
    }

    public void MoveToMainMenu()
    {
        PlayerPrefs.SetInt("IntroDone", 1);
        PlayerPrefs.Save();
        sm.SceneManager.LoadScene("Main Menu");
    }
}
