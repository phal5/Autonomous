using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        [SerializeField][Tooltip("0 means herbivorous, 1 means small predators and so on. Maximum is 3.")] byte _threatLevel;

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

    public static List<Vector3> _herbivoresPos = new List<Vector3>();
    public static List<Vector3> _predators1Pos = new List<Vector3>();
    public static List<Vector3> _predators2Pos = new List<Vector3>();
    public static List<Vector3> _predators3Pos = new List<Vector3>();

    //Test Variables
    [SerializeField] bool _init = false;

    public static byte _frameCounter = 0;

    [SerializeField] AnimalPrefabs[] _animalPrefabs;
    public static AnimalPrefabs[] _AnimalPrefabs;


    //========================================================================================================================
    // Functions in this class are called before others (-2)
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
                    TransformToPosition(_herbivores, ref _herbivoresPos);
                    _frameCounter++;
                    break;
                }
            case 1:
                {
                    TransformToPosition(_predators1, ref _predators1Pos);
                    _frameCounter++;
                    break;
                }
            case 2:
                {
                    TransformToPosition(_predators2, ref _predators2Pos);
                    _frameCounter++;
                    break;
                }
            case 3:
                {
                    TransformToPosition(_predators3, ref _predators3Pos);
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
                    Debug.LogError
                        (
                        "Frame count module error: _frameCounter value is outside bounds. _framecounter: "
                        + _frameCounter.ToString()
                        );
                    break;
                }
        }

        //Test functions
        if (_init)
        {
            _init = false;
            foreach(AnimalPrefabs animalPrefabs in _animalPrefabs)
            {
                animalPrefabs.InstantiateAnimal(Random.onUnitSphere, 1);
            }
        }
    }

    void TransformToPosition(Transform parent, ref List<Vector3> positions)
    {
        positions.Clear();
        foreach(Transform child in parent)
        {
            positions.Add(child.position);
        }
    }

    public static List<Vector3> Search(Vector3 position, float distance, byte threatThreshold)
    {
        float Abs(float f)
        {
            return (f > 0) ? f : -f;
        }
        List<Vector3> Search(List<Vector3> Positions)
        {
            List<Vector3> result = new List<Vector3>();
            foreach(Vector3 Position in Positions)
            {
                Vector3 v = (position - Position);
                if (Abs(v.x) < distance && Abs(v.y) < distance && Abs(v.z) < distance && v.sqrMagnitude < distance * distance)
                {
                    result.Add(Position - position);
                }
            }
            return result;
        }

        List<Vector3> Result = new List<Vector3>();
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
        return Result;
    }

    public static Vector3 CrudeFlee(Vector3 position, float distance, byte threatThreshold)
    {
        Vector3 Result = Vector3.zero;
        float Abs(float f)
        {
            return (f > 0) ? f : -f;
        }
        Vector3 SearchSum(List<Vector3> Positions)
        {
            Vector3 result = Vector3.zero;
            foreach (Vector3 Position in Positions)
            {
                Vector3 v = (position - Position);
                if (Abs(v.x) < distance && Abs(v.z) < distance && Abs(v.y) < distance && v.sqrMagnitude < distance * distance)
                {
                    result += (distance * distance - v.sqrMagnitude) * v;
                }
            }
            return result;
        }
        switch (threatThreshold)
        {
            case 1:
                Result += SearchSum(_predators1Pos);
                Result += SearchSum(_predators2Pos);
                Result += SearchSum(_predators3Pos);
                break;
            case 2:
                Result += SearchSum(_predators2Pos);
                Result += SearchSum(_predators3Pos);
                break;
            case 3:
                Result += SearchSum(_predators3Pos);
                break;
            default:
                Debug.LogError("threatThreshold maximum is 3, minimum being 1.");
                break;
        }
        return Result + position;
    }
}
