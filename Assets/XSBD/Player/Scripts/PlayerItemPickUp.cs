using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private float pickUpDistance;
    [SerializeField] InventoryInstance inventory;
    [SerializeField] KeyCode pickUpKey;
    void Start()
    {
        if(player == null)
        {
            player = gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pickUpKey))
        {
            Collider[] colls = Physics.OverlapSphere(player.transform.position, pickUpDistance);

            foreach(Collider coll in colls)
            {
                if(coll.gameObject.TryGetComponent<Item>(out Item item))
                {
                    GameObject _item;
                    ParentData _parentData;
                    byte _quantity;
                    bool _stackable;
                    item.GetItemData(out _item, out _parentData, out _quantity, out _stackable);
                    inventory.MoveToEmptySlot(_item, _parentData, _quantity, _stackable);
                }
            }

        }
    }
}
