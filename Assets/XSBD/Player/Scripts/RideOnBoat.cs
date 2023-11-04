using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RideOnBoat : MonoBehaviour
{
    [SerializeField] Transform _rFootTarget;
    [SerializeField] Transform _rHip;
    [Space(5f)]
    [SerializeField] Transform _lFootTarget;
    [SerializeField] Transform _lHip;
    [Space(10f)]
    [SerializeField] Vector3 _rCastOffset;
    [SerializeField] Vector3 _lCastOffset;

    RaycastHit _hit;
    bool _buffer = false;
    bool _set = false;

    private void Update()
    {
        if (_buffer)
        {
            _buffer = false;
            MoveFeet();
        }
        if (_set)
        {
            _set = false;
            _buffer = true;
        }
    }

    Vector3 Localize(Vector3 world)
    {
        return world.x * transform.right + world.y * transform.up + world.z * transform.forward;
    }

    void Cast(Transform foot, Transform hip, Vector3 offset)
    {
        if (Physics.Raycast(hip.position + Localize(offset), Vector3.down, out _hit)) foot.transform.position = _hit.point;
    }

    void MoveFeet()
    {
        Cast(_rFootTarget, _rHip, _rCastOffset);
        Cast(_lFootTarget, _lHip, _lCastOffset);
    }

    public void SetFoot()
    {
        _set = true;
    }
}
