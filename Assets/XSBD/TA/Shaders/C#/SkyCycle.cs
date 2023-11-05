using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyCycle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = Resources.Load<Material>("Sky 2");
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Time_d", Calendar.GetTimer());
        RenderSettings.skybox.SetVector("_Sunlight", transform.forward);
    }
}
