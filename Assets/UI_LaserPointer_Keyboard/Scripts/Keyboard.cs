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

    public void Delete()
    {
        if(output.text.Length != 0)
            output.text = output.text.Remove(output.text.Length - 1, 1);
    }

    public void Space()
    {
        if (output.text.Length != 0)
            output.text += " ";
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
