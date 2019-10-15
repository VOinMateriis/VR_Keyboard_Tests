using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenKeyboard : MonoBehaviour
{

    public GameObject keyboard;

    public void Open()
    {
        keyboard.SetActive(true);
        keyboard.GetComponent<Keyboard>().output = transform.parent.GetComponent<InputField>(); 
    }
}
