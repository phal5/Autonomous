using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public void SetHittability(int _enable)
    {
        Hitter.SetColliderEnability(_enable != 0);
    }
}
