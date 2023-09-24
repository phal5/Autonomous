using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [System.Serializable] class Key
    {
        [SerializeField] KeyCode _key;
        [SerializeField] string _trigger;
        [SerializeField] Behaviour _behaviour;

        public void SetTrigger(Animator animator, ref string _currentTrigger)
        {
            if (Input.GetKeyDown(_key))
            {
                animator.ResetTrigger(_currentTrigger);
                animator.SetTrigger(_trigger);
                _currentTrigger = _trigger;
            }
        }
    }

    [System.Serializable]
    class Mouse
    {
        [SerializeField] int _button;
        [SerializeField] string _trigger;

        public void SetTrigger(Animator animator, ref string _currentTrigger)
        {
            if (Input.GetMouseButtonDown(_button))
            {
                if(_currentTrigger != "")
                {
                    animator.ResetTrigger(_currentTrigger);
                }
                animator.SetTrigger(_trigger);
                _currentTrigger = _trigger;
            }
        }
    }
    [SerializeField] Animator _animator;
    [SerializeField] Key[] _keys;
    [SerializeField] Mouse[] _clickEvents;

    string _currentTrigger = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Key key in _keys)
        {
            key.SetTrigger(_animator, ref _currentTrigger);
        }
        foreach (Mouse mouse in _clickEvents)
        {
            mouse.SetTrigger(_animator, ref _currentTrigger);
        }
    }
}
