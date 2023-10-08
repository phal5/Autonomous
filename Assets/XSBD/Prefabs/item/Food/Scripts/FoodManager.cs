using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodManager;

public class FoodManager : MonoBehaviour
{
    [System.Serializable] public class FoodPrefab
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] Transform _prefabParent;

        public void InstantiateFoodWithVelocity(Vector3 pos, Vector3 velocity)
        {
            Rigidbody rb;
            Transform transform = Instantiate(_prefab, pos, Random.rotation, _prefabParent).transform;
            if (transform.TryGetComponent<Rigidbody>(out rb))
            {
                rb.velocity = velocity;
            }
            
        }

        public void InstantiateFood(Vector3 pos)
        {
            Instantiate(_prefab, pos, Random.rotation, _prefabParent);
        }
    }
    [SerializeField] FoodPrefab[] _food;
    static FoodPrefab[] __food;
    [SerializeField] bool _init = false;//Delete after test


    static Transform[] _foodTypes;

    // Start is called before the first frame update
    void Start()
    {
        SetFoodParents();
        __food = _food;
    }

    // Update is called once per frame
    void Update()
    {
        if (_init)
        {
            InitiateAll();
            _init = false;
        }
    }

    public static Transform SearchUnder(Transform foodParent, Vector3 position, float distance, Transform Result)
    {
        float ResultDist = distance * distance;
        float Sqr(float f)
        {
            return f * f;
        }
        foreach(Transform food in foodParent)
        {
            Vector3 tempV = food.position - position;
            if(Sqr(tempV.x) < ResultDist && Sqr(tempV.z) < ResultDist && tempV.sqrMagnitude < ResultDist)
            {
                Result = food;
                ResultDist = tempV.sqrMagnitude;
            }
        }
        return Result;
    }

    void InitiateAll()
    {
        foreach(FoodPrefab foodPrefab in _food)
        {
            foodPrefab.InstantiateFood(Vector3.zero);
        }
    }

    void SetFoodParents()
    {
        _foodTypes = new Transform[transform.childCount];
        byte index = 0;
        foreach (Transform child in transform)
        {
            _foodTypes[index] = child;
            ++index;
        }
    }

    public static Transform[] GetFoodParents(byte[] indexes)
    {
        Transform[] result = new Transform[indexes.Length];
        byte index = 0;
        foreach(byte id in indexes)
        {
            Debug.Log(id);
            result[index] = _foodTypes[id];
            Debug.Log(new Vector2(index, id));
            ++index;
        }
        return result;
    }

    public static Transform GetFoodParent(byte index)
    {
        return _foodTypes[index];
    }

    public static void InstantiateFood(byte index, Vector3 position)
    {
        __food[index].InstantiateFood(position);
    }

    public static void InstantiateFoodWithVelocity(byte index, Vector3 position, Vector3 velocity)
    {
        __food[index].InstantiateFoodWithVelocity(position, velocity);
    }
}
