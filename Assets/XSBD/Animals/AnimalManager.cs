using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] Transform __herbivores;
    [SerializeField] Transform __predators1;
    [SerializeField] Transform __predators2;
    [SerializeField] Transform __predators3;

    static Transform _herbivores;
    static Transform _predators1;
    static Transform _predators2;
    static Transform _predators3;

    [System.Serializable]
    public class AnimalPrefabs
    {
        [SerializeField] GameObject _prefab;
        [Tooltip("0 means herbivorous, 1 means small predators and so on. Maximum is 3.")]
        [SerializeField] byte _threatLevel;

        AnimalPrefabs(GameObject prefab, byte threatLevel)
        {
            _prefab = prefab;
            _threatLevel = threatLevel;
        }

        public void InstantiateAnimal(Vector3 position, byte number)
        {
            Transform parent;
            switch (_threatLevel)
            {
                case 0:
                    {
                        parent = _herbivores;
                        break;
                    }
                case 1:
                    {
                        parent = _predators1;
                        break;
                    }
                case 2:
                    {
                        parent = _predators2;
                        break;
                    }
                case 3:
                    {
                        parent = _predators3;
                        break;
                    }
                default:
                    {
                        Debug.LogError("À§Çù ÃÖ´ñ°ªÀº 3ÀÌ¿¡¿ä");
                        parent = _predators3;
                        break;
                    }
            }
            if (number + parent.childCount > 1024)
            {
                number = (byte)(1024 - parent.childCount);
            }
            for (int i = 0; i < number; i++)
            {
                GameObject o = Instantiate(_prefab, parent, true);
                o.transform.position = position;
            }
        }
    }

    public static Vector3[] _herbivoresPos = new Vector3[1024];
    public static Vector3[] _predators1Pos = new Vector3[1024];
    public static Vector3[] _predators2Pos = new Vector3[1024];
    public static Vector3[] _predators3Pos = new Vector3[1024];

    public static byte _frameCounter = 0;

    [SerializeField] AnimalPrefabs[] _animalPrefabs;
    public static AnimalPrefabs[] _AnimalPrefabs;


    //========================================================================================================================
    // Functions in this class are called at -2
    //========================================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        _herbivores = __herbivores;
        _predators1 = __predators1;
        _predators2 = __predators2;
        _predators3 = __predators3;
        _AnimalPrefabs = _animalPrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_frameCounter)
        {
            case 0:
                {
                    TransformToPosition(_herbivores, _herbivoresPos);
                    _frameCounter++;
                    break;
                }
            case 1:
                {
                    TransformToPosition(_predators1, _predators1Pos);
                    _frameCounter++;
                    break;
                }
            case 2:
                {
                    TransformToPosition(_predators2, _predators2Pos);
                    _frameCounter++;
                    break;
                }
            case 3:
                {
                    TransformToPosition(_predators3, _predators3Pos);
                    _frameCounter++;
                    break;
                }
            case 4:
                {
                    _frameCounter = 0;
                    break;
                }
            default:
                {
                    Debug.LogError("Frame count module error: _frameCounter value is outside bounds");
                    Debug.Log(_frameCounter);
                    break;
                }
        }
    }

    void TransformToPosition(Transform parent, Vector3[] positions)
    {
        int i = 0;
        foreach(Transform child in parent)
        {
            positions[i++] = child.position;
        }
    }

    public static Vector3[] Search(Vector3 position, float distance, byte threatThreshold)
    {
        float Abs(float f)
        {
            return (f > 0) ? f : -f;
        }
        List<Vector3> Search(Vector3[] Positions)
        {
            List<Vector3> Result = null;
            foreach(Vector3 Position in Positions)
            {
                Vector3 v = (position - Position);
                if (Abs(v.x) + Abs(v.y) + Abs(v.z) <= distance * 3 && v.sqrMagnitude <= distance)
                {
                    Result.Add(Position);
                }
            }
            return Result;
        }

        List<Vector3> Result = null;
        switch (threatThreshold)
        {
            case 1:
                Result.AddRange(Search(_predators1Pos));
                Result.AddRange(Search(_predators2Pos));
                Result.AddRange(Search(_predators3Pos));
                break;
            case 2:
                Result.AddRange(Search(_predators2Pos));
                Result.AddRange(Search(_predators3Pos));
                break;
            case 3:
                Result.AddRange(Search(_predators3Pos));
                break;
            default:
                Debug.LogError("threatThreshold maximum is 3, minimum being 1.");
                break;
        }
        return Result.ToArray();
    }
}
