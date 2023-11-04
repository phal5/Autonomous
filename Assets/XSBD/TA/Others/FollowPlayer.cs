using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (PlayerManager.GetPlayerPosition().x, transform.position.y, PlayerManager.GetPlayerPosition().z);
    }
}
