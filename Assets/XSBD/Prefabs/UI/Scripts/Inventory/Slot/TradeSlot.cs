using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[System.Serializable] public class TradeSlot : Slot
{
    [SerializeField] TradeInventoryInstance _tradeInventoryInstance;

    private void Start()
    {
        SetTradeInventoryInstance();
        SetText();
    }

    public void PointerDownEvent()
    {
        _tradeInventoryInstance.Trade();
    }

    void SetTradeInventoryInstance()
    {
        transform.parent.TryGetComponent<TradeInventoryInstance>(out _tradeInventoryInstance);
    }
}
