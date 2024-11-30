using UnityEngine;
using UnityEngine.EventSystems;
using sm = UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    
    public EventTrigger.TriggerEvent start;
    void Start()
    {
        BaseEventData eventData = new BaseEventData(EventSystem.current);
        eventData.selectedObject = this.gameObject;
        start.Invoke(eventData);
    }

    public void MoveToMain() {
        PlayerPrefs.SetInt("IntroDone",1);
        sm.SceneManager.LoadScene("Main (Dynamic)");
    }
}
