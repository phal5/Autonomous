using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public class FoodPrefab
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] Transform _prefabParent;
        public void InstantiateFoodWithVelocity(Vector3 pos, Vector3 velocity)
        {
            Rigidbody rb;
            Transform transform = Instantiate(_prefab, pos, Random.rotation, _prefabParent).transform;
            transform.TryGetComponent<Rigidbody>(out rb);
            rb.velocity = velocity;
        }

        public void InstantiateFood(Vector3 pos)
        {
            Instantiate(_prefab, pos, Random.rotation, _prefabParent);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Transform SearchUnder(Transform foodParent, Vector3 position, float distance, Transform Result)
    {
        float ResultDist = distance;
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
}
