using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class LightTorchAtNight : MonoBehaviour
{
    [SerializeField] GameObject _torch;
    [SerializeField] Material _flame;
    [SerializeField] RotationConstraint[] constraints;
    Rig _armControl;
    Light _torchLight;
    const float _oneThirds = 1f / 3f;
    float _initialRange;
    float _timer = 0;
    bool _day = false;
    bool _setTorch = false;

    // Start is called before the first frame update
    void Start()
    {
        _armControl = GetComponent<Rig>();
        SearchLightInHirearchy(_torch.transform, ref _torchLight);
        _initialRange = _torchLight.range;
    }

    // Update is called once per frame
    void Update()
    {
        if(_day != Calendar.IsDaytime())
        {
            _day = Calendar.IsDaytime();
            _setTorch = true;
        }
        if (_setTorch)
        {
            Torch(!_day);
        }
    }

    void Constrain(bool constrain)
    {
        foreach(RotationConstraint constraint in constraints)
        {
            constraint.constraintActive = constrain;
        }
    }

    void Torch(bool pull)
    {
        if (pull)
        {
            _torch.SetActive(true);
            Constrain(true);
        }

        _timer += Time.deltaTime;

        if(_timer > 1)
        {
            _timer = 0;
            _setTorch = false;

            if (pull)
            {
                _torchLight.range = _initialRange;
                _flame.SetFloat("_Height", 1);
                _armControl.weight = 1;
            }
            else
            {
                _torchLight.range = 0;
                _flame.SetFloat("_Height", 0);
                _torch.SetActive(false);
                Constrain(false);
                _armControl.weight = 0;
            }
        }
        else
        {
            float smooth = (_timer * _timer * 0.5f - _timer * _timer * _timer * _oneThirds) * 6;
            if (smooth > 1) Debug.Log(smooth);
            _torchLight.range = ((pull)? smooth : 1 - smooth) * _initialRange;
            _flame.SetFloat("_Height", ((pull) ? smooth : 1 - smooth));
            _armControl.weight = (pull) ? smooth : 1 - smooth;
        }
    }

    void SearchLightInHirearchy(Transform transform, ref Light light)
    {
        if (transform.TryGetComponent(out light)) return;
        else
        {
            foreach(Transform child in transform)
            {
                SearchLightInHirearchy(child, ref light);
            }
        }
    }
}
