using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Texture2D image;
    public Material material;

    public int x = 0;
    public int y = 0;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("COLOR: " + image.GetPixel(x, y));
        material.color = image.GetPixel(x, y);
    }
}
