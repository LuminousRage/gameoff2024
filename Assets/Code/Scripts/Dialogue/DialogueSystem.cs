using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;
    private bool isWriting = false;
    private float writingStartTime = 0;
    public float writeRate = 30; //chars per sec
    public EventTrigger.TriggerEvent onCloseds;
    
    [System.Serializable]
    public struct Dialog {
        public TMPro.TMP_FontAsset font;
        public String text;
    }

    public Dialog[] remainingDialogue;


    public void StartDialogue() {
        popUpBox.GetComponent<Image>().enabled = true;
        popUpText.text = "";
        popUpText.font = remainingDialogue[0].font;
        animator.SetTrigger("open");
    }

    public void OnOpened() {
        isWriting= true;
        writingStartTime = Time.realtimeSinceStartup;
    }
    public void OnClosed() {
        BaseEventData eventData= new BaseEventData(EventSystem.current);
        eventData.selectedObject=this.gameObject;
        onCloseds.Invoke(eventData);
        
        popUpBox.GetComponent<Image>().enabled = false;

    }
    public void SkipForward() {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || !animator.GetCurrentAnimatorStateInfo(0).IsName("open")) 
            return;
        
        if (isWriting) {
            isWriting = false;
            popUpText.text = remainingDialogue[0].text;
        }
        else if (remainingDialogue.Count()>1) {
            isWriting = true;
            writingStartTime = Time.realtimeSinceStartup;
            remainingDialogue = remainingDialogue.Skip(1).ToArray();
            popUpText.text = "";
            popUpText.font = remainingDialogue[0].font;
        } else {
            animator.SetTrigger("close");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isWriting) return;

        var currentChar = Mathf.FloorToInt(writeRate*(Time.realtimeSinceStartup-writingStartTime));
        if (currentChar < remainingDialogue[0].text.Count()) {
            popUpText.text = remainingDialogue[0].text[..currentChar];
        } else {
            popUpText.text = remainingDialogue[0].text;
            isWriting = false;
        }
    }
}
