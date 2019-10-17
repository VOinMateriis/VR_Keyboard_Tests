using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Laser : MonoBehaviour
{
    public GameObject hand;
    public GameObject keyboard;
    public SteamVR_Action_Boolean trigger;

    private GameObject currentUI = null;
    private LineRenderer laser;
    private GameObject dot;
    private SteamVR_Input_Sources _hand;
    private bool hovering;

    private void Start()
    {
        transform.parent = hand.transform;

        laser = GetComponent<LineRenderer>();
        dot = transform.GetChild(0).gameObject;

        if (hand.name == "RightHand")
            _hand = SteamVR_Input_Sources.RightHand;
        else if (hand.name == "LeftHand")
            _hand = SteamVR_Input_Sources.LeftHand;
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

                if (trigger.GetStateDown(_hand))
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
        ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.deselectHandler);   //Makes the UI element to get back its unhovered style
    }


    public void OpenKeyboard(InputField inputField)
    {
        keyboard.SetActive(true);
        keyboard.GetComponent<Keyboard>().output = inputField;
    }
}