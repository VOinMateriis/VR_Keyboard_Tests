using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    public InputField inputField;

    private void Start()
    {
        Debug.Log("PATH: " + Application.persistentDataPath);
        //C:/Users/xonoz/AppData/LocalLow/DefaultCompany/Keyboard
        Directory.CreateDirectory(Application.persistentDataPath + "/testfolder");
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/testfolder/datosxd.banana");
        
        Name name = new Name();
        name.name = inputField.text;

        bf.Serialize(file, name);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/datosxd.banana"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/datosxd.banana", FileMode.Open);

            Name name = new Name();
            name = (Name)bf.Deserialize(file);
            file.Close();

            inputField.text = name.name;
        }
    }
}

[Serializable]
class Name
{
    public string name;
}


