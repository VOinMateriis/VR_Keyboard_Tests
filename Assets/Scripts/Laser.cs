using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    public GameObject keyboard;
    public SteamVR_Action_Boolean trigger;
    public GameObject hand;
    GameObject currentUI = null;

    private bool hover;
    private GameObject laser;
    public float thickness = 0.002f;

    private void Start()
    {
        transform.parent = hand.transform;
        //transform.localPosition = Vector3.zero;

        /*GameObject laser = transform.GetChild(0).gameObject;
        laser.SetActive(true);*/


        /*laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        laser.gameObject.name = "Laser";
        laser.transform.parent = transform;
        laser.transform.localScale = new Vector3(thickness, thickness, 100f);
        laser.transform.localPosition = new Vector3(0f, 0f, 50f);
        laser.transform.localRotation = Quaternion.identity;*/

        LineRenderer laser = GetComponent<LineRenderer>();
        //Transform sphere = transform.GetChild(0);

        Vector3[] laserPoints = new Vector3[2];

        laserPoints[0] = transform.parent.position;


        //if (hit.collider != null)
            //laserPoints[1] = hit.point;
        //else
            laserPoints[1] = transform.parent.forward * 3;


        laser.SetPositions(laserPoints);
        //sphere.position = laserPoints[1];
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.parent.position, transform.parent.forward);

        RaycastHit hit;
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if(hit.collider.CompareTag("Button") || hit.collider.CompareTag("InputField"))
            {
                currentUI = hit.collider.gameObject;

                ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerEnterHandler);
                hover = true;

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
        else if (hover)
        {
            ExecuteEvents.Execute(currentUI, pointer, ExecuteEvents.pointerExitHandler);
            hover = false;
        }


        //Debug.DrawRay(transform.position, transform.forward * 1f);

        /*holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;*/


        //BoxCollider collider = laser.GetComponent<BoxCollider>();

        /*LineRenderer laser = GetComponent<LineRenderer>();
        Transform sphere = transform.GetChild(0);

        Vector3[] laserPoints = new Vector3[2];

        laserPoints[0] = transform.parent.position;
        

        if(hit.collider != null)
            laserPoints[1] = hit.point;
        else
            laserPoints[1] = transform.parent.forward * 3;


        laser.SetPositions(laserPoints);
        sphere.position = laserPoints[1];*/

        LineRenderer laser = GetComponent<LineRenderer>();

        Vector3[] laserPoints = new Vector3[2];

        laserPoints[0] = transform.parent.position;
        laserPoints[1] = transform.parent.forward * 3;


        laser.SetPositions(laserPoints);

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
