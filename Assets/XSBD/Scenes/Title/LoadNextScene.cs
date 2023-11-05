using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] int _index;
    public void Load()
    {
        SceneManager.LoadScene(_index);
    }
}
