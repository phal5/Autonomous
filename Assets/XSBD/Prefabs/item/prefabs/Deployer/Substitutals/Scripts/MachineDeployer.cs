using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MachineDeployer : Deployer
{
    [SerializeField] bool _xRotatable;
    [SerializeField] bool _liftable;
    [Space(10f)]
    [SerializeField] GameObject _item;
    [SerializeField] bool _stackable;

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
            _pointer.layer = 2;
        }
        if(!TryGetComponent<Rigidbody>(out _))
        {
            transform.AddComponent<Rigidbody>();
        }
        ChangeMaterialInChildren(transform, MachineDeployManager.GetMaterial(true));
        TriggerizeColliders(transform);
        SetLayersInChildren(transform, 2);
        foreach (Transform child in transform)
        {
            ClearBehaviours(child);
            ClearRigidbody(child);
        }
        ClearNavMeshAgent(transform);
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Cancel();
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
            SetLayersInChildren(child, index);
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

    void ClearRigidbody(Transform transform)
    {
        foreach (Rigidbody rigidbody in transform.GetComponents<Rigidbody>())
        {
            Destroy(rigidbody);
        }
        foreach (Transform child in transform)
        {
            ClearRigidbody(child);
        }
    }

    void ClearNavMeshAgent(Transform transform)
    {
        foreach(NavMeshAgent navMeshAgent in transform.GetComponents<NavMeshAgent>())
        {
            Destroy(navMeshAgent);
        }
        foreach(Transform child in transform)
        {
            ClearNavMeshAgent(child);
        }
    }

    void TriggerizeColliders(Transform transform)
    {
        Collider collider;
        if(transform.TryGetComponent<Collider>(out collider))
        {
            if(collider.GetType() == typeof(MeshCollider))
            {
                Destroy(collider);
            }
            else
            {
                collider.isTrigger = true;
            }
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
                renderer.SetMaterials(new List<Material>(Enumerable.Repeat<Material>(material, renderer.materials.Length)));
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
            if(_pointer != null)
            {
                _pointer.transform.position = _hit.point;
            }
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
            }
        }
    }

    void Cancel()
    {
        if (Input.GetKeyDown(MachineDeployManager.GetKey(4)))
        {
            PlayerManager.GetInventoryInstance().MoveToEmptySlot(_item, ParentData.zero(), 1, _stackable);
            Destroy(_pointer);
            Destroy(gameObject);
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
