using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineDeployer : Deployer
{
    [SerializeField] bool _xRotatable;
    [SerializeField] bool _liftable;

    GameObject _pointer;
    RaycastHit _hit;
    float _heightOffset;
    bool _deployable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        if(MachineDeployManager.GetPointer() != null)
        {
            _pointer = Instantiate(MachineDeployManager.GetPointer());
            SetLayersInChildren(_pointer.transform, 2);
        }
        if(!TryGetComponent<Rigidbody>(out _))
        {
            transform.AddComponent<Rigidbody>();
        }
        SetLayersInChildren(transform, 2);
        ChangeMaterialInChildren(transform, MachineDeployManager.GetMaterial(true));
        TriggerizeColliders(transform);
        PlayerMovementManager.Enable(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetHeight();
        SetRotation();
        SetPosition();
        Place();
    }

    void SetLayersInChildren(Transform transform, byte index)
    {
        transform.gameObject.layer = index;
        foreach(Transform child in transform)
        {
            child.gameObject.layer = index;
        }
    }

    void ClearBehaviours(Transform transform)
    {
        foreach (MonoBehaviour behaviour in transform.GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = false;
        }
        foreach (Transform child in transform)
        {
            ClearBehaviours(child);
        }
    }

    void TriggerizeColliders(Transform transform)
    {
        Collider collider;
        if(transform.TryGetComponent<Collider>(out collider))
        {
            collider.isTrigger = true;
        }
        foreach(Transform child in transform)
        {
            TriggerizeColliders(child);
        }
    }

    void ChangeMaterialInChildren(Transform transform, Material material)
    {
        Renderer renderer;
        if(transform.TryGetComponent<Renderer>(out renderer))
        {
            for(int i = 0; i < renderer.materials.Length; ++i)
            {
                renderer.materials[i] = material;
            }
        }

        foreach (Transform child in transform)
        {
            ChangeMaterialInChildren(child, material);
        }
    }

    void SetPosition()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, 100))
        {
            transform.position = _hit.point + Vector3.up * _heightOffset;
            _pointer.transform.position = _hit.point;
        }
    }

    void SetHeight()
    {
        if (_liftable)
        {
            _heightOffset -= Input.mouseScrollDelta.y;
            if(_heightOffset < 0)
            {
                _heightOffset = 0;
            }
        }
    }

    void SetRotation()
    {
        if (_xRotatable)
        {
            Rotate(0, Vector3.right * 5);
            Rotate(1, Vector3.right * -5);
        }
        Rotate(2, Vector3.up * 5);
        Rotate(3, Vector3.up * -5);
    }

    void Rotate(byte index, Vector3 rotation)
    {
        if (Input.GetKeyDown(MachineDeployManager.GetKey(index)))
        {
            transform.eulerAngles += rotation;
        }
    }

    void Place()
    {
        if (_deployable && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Deploy(transform.position, transform.rotation))
            {
                Destroy(_pointer);
                PlayerMovementManager.Enable(true);
            }
        }
    }

    void SetPlaceable(bool deployable)
    {
        if(deployable != _deployable)
        {
            _deployable = deployable;
            ChangeMaterialInChildren(transform, MachineDeployManager.GetMaterial(_deployable));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        SetPlaceable(false);
    }

    private void OnTriggerExit(Collider other)
    {
        SetPlaceable(true);
    }
}
