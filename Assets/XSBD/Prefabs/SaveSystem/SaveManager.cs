using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //6: 47

    private SaveData _saveData;

    public static SaveManager _instance { get; private set; }

    private void Awake()
    {
        //Singleton pattern in Unity

        if (_instance != null)
        {
            Debug.LogError("You don't really want two save systems running");
            Destroy(this);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load();
    }

    void NewGame()
    {
        this._saveData = new SaveData();
    }

    void Load()
    {
        //load file using Data handler
        if(_saveData == null)
        {
            NewGame();
        }
    }

    void Save()
    {
        //pass data to other scripts that need them

        //save data to file using Data handler

    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
