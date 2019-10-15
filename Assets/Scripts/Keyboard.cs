using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public InputField output;

    public void KeyPressed(string key)
    {
        output.text += key;
    }
}
