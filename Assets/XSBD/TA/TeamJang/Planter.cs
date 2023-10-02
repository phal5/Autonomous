using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planter : MonoBehaviour
{
    [SerializeField] Transform[] planters;
    [SerializeField] Transform _parent;
    [SerializeField] GameObject _plantPrefab;
    [SerializeField] byte _layer;
    [Space(10)]
    [SerializeField] bool _plant;
    [SerializeField] bool _undo;

    RaycastHit _hit;
    List<GameObject> gameObjects = new List<GameObject>();

    void Update()
    {
        if (_plant)
        {
            gameObjects.Clear();
            if(planters.Length > 0)
            {
                foreach (Transform planter in planters)
                {
                    gameObjects.Add(Plant(planter, _parent));
                }
            }
            else
            {
                foreach(Transform child in transform)
                {
                    gameObjects.Add(Plant(child, _parent));
                }
            }
            _plant = false;
        }
        if (_undo)
        {
            foreach(GameObject plant in gameObjects)
            {
                DestroyImmediate(plant);
            }
            gameObjects.Clear();
            _undo = false;
        }
    }

    GameObject Plant(Transform from, Transform parent)
    {
        if(Physics.Raycast(from.position, Vector3.down, out _hit) && _hit.transform.gameObject.layer == _layer)
        {
            Quaternion rotation =
                Quaternion.LookRotation(_hit.normal, Vector3.back)
                * Quaternion.Euler(Vector3.right * 90)
                * Quaternion.Euler(Vector3.up * Random.value * 360);

            return Instantiate(_plantPrefab, _hit.point, rotation, parent);
        }
        return null;
    }
}
