using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    public GameObject keyboard;
    public SteamVR_Action_Boolean trigger;
    GameObject button = null;

    private bool hover;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if(hit.collider.CompareTag("Button") || hit.collider.CompareTag("InputField"))
            {
                button = hit.collider.gameObject;
                ExecuteEvents.Execute(button, pointer, ExecuteEvents.pointerEnterHandler);
                hover = true;

                if (trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    if (button.CompareTag("InputField"))
                    {
                        OpenKeyboard(button.GetComponent<InputField>());
                    }
                    StartCoroutine(PressButton(pointer));
                }
            }
            
                
        }
        else if (hover)
        {
            ExecuteEvents.Execute(button, pointer, ExecuteEvents.pointerExitHandler);
            hover = false;
        }
            

        Debug.DrawRay(transform.position, transform.forward * 1f);
    }


    private IEnumerator PressButton(PointerEventData pointer)
    {
        ExecuteEvents.Execute(button, pointer, ExecuteEvents.pointerClickHandler);
        ExecuteEvents.Execute(button, pointer, ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(button, pointer, ExecuteEvents.pointerUpHandler);
    }


    public void OpenKeyboard(InputField inputField)
    {
        keyboard.SetActive(true);
        keyboard.GetComponent<Keyboard>().output = inputField;
    }
}
