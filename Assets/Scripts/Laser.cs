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
    GameObject currentUI = null;

    private bool hovering;
    private LineRenderer laser;
    private GameObject dot;

    private void Start()
    {
        laser = GetComponent<LineRenderer>();
        dot = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.parent.position, transform.parent.forward);

        RaycastHit hit;
        PointerEventData pointer = new PointerEventData(EventSystem.current);


        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.collider.CompareTag("Button") || hit.collider.CompareTag("InputField"))
            {
                currentUI = hit.collider.gameObject;
                ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerEnterHandler);
                hovering = true;

                if (trigger.GetStateDown(SteamVR_Input_Sources.Any))
                {
                    if (currentUI.CompareTag("InputField"))
                    {
                        OpenKeyboard(currentUI.GetComponent<InputField>());
                    }
                    StartCoroutine(PressButton(pointer));
                }
            }


        }
        else if (hovering)
        {
            ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerExitHandler);
            hovering = false;
        }

        //Debug.DrawRay(transform.parent.position, transform.parent.forward * 1f);

        Vector3[] laserPoints = new Vector3[2];

        laserPoints[0] = transform.parent.position;

        if (hit.collider != null)
            laserPoints[1] = hit.point;
        else
            laserPoints[1] = transform.parent.position + transform.parent.forward;

        laser.SetPositions(laserPoints);


        dot.transform.position = laserPoints[1];
    }


    private IEnumerator PressButton(PointerEventData pointer)
    {
        ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerClickHandler);
        ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.deselectHandler);
    }


    public void OpenKeyboard(InputField inputField)
    {
        keyboard.SetActive(true);
        keyboard.GetComponent<Keyboard>().output = inputField;
    }
}