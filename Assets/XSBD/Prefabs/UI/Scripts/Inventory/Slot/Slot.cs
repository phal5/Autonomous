using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Slot : MonoBehaviour
{
    [SerializeField] protected GameObject _item;
    [SerializeField] protected ParentData _parentData = new ParentData();
    [SerializeField] protected byte _quantity = 0;
    [SerializeField] protected bool _stackable;

    protected TextMeshProUGUI _textMeshProUGUI;
    protected Transform _itemInstance;
    protected Vector3 _scaleDivisor;

    private void Awake()
    {
        SetLayer();
        SetText();
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetScale();
    }

    public void SetSlotData(GameObject item, ParentData parentData, byte quantity, bool stackable)
    {
        _item = item;
        _parentData = parentData;
        _quantity = quantity;
        _stackable = stackable;
        SetText();
        InstanceItem();
    }

    protected void SetScale()
    {
        _scaleDivisor.x = 1f / transform.lossyScale.x;
        _scaleDivisor.y = 1f / transform.lossyScale.y;
        _scaleDivisor.z = 1f / transform.lossyScale.z;
        if (_quantity != 0)
        {
            InstanceItem();
        }
    }

    protected void SetLayer()
    {
        if (transform.parent)
        {
            SetLayerInChildren(transform, transform.parent.gameObject.layer);
        }
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected void SetText()
    {
        if (_textMeshProUGUI != null)
        {
            if (_quantity > 0)
            {
                _textMeshProUGUI.text = _quantity.ToString();
            }
            else
            {
                _textMeshProUGUI.text = "";
            }
        }
    }

    protected void InstanceItem()
    {
        if (_itemInstance)
        {
            Destroy(_itemInstance.gameObject);
        }

        if(_quantity > 0)
        {
            _itemInstance = Instantiate(_item, transform).transform;

            if(_itemInstance.TryGetComponent<Deployer>(out Deployer deployer))
            {
                _parentData = deployer.GetParentData();
            }

            if (_scaleDivisor == Vector3.zero)
            {
                _scaleDivisor.x = 1f / transform.lossyScale.x;
                _scaleDivisor.y = 1f / transform.lossyScale.y;
                _scaleDivisor.z = 1f / transform.lossyScale.z;
            }

            SetLayerInChildren(_itemInstance, gameObject.layer);
            ClearColliders(_itemInstance);
            ClearBehaviours(_itemInstance);
            ClearNavAgents(_itemInstance);
            ClearRigidBody(_itemInstance);

            _itemInstance.localScale = _scaleDivisor * 0.7f;
            _itemInstance.position = transform.position - transform.forward * 0.5f;
            _itemInstance.localRotation = Quaternion.Euler(Vector3.up * 180);
        }
    }

    protected void SetLayerInChildren(Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            SetLayerInChildren(child, layer);
        }
    }

    void ClearColliders(Transform transform)
    {
        foreach (Collider collider in transform.GetComponents<Collider>())
        {
            Destroy(collider);
        }
        foreach (Transform child in transform)
        {
            ClearColliders(child);
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

    void ClearNavAgents(Transform transform)
    {
        foreach (NavMeshAgent navMeshAgent in transform.GetComponents<NavMeshAgent>())
        {
            Destroy(navMeshAgent);
        }
        foreach (Transform child in transform)
        {
            ClearNavAgents(child);
        }
    }

    void ClearRigidBody(Transform transform)
    {
        foreach (Rigidbody rigidbody in transform.GetComponents<Rigidbody>())
        {
            Destroy(rigidbody);
        }
        foreach (Transform child in transform)
        {
            ClearRigidBody(child);
        }
    }
}
