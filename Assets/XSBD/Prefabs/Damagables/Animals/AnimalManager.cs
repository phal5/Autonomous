using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [SerializeField] Transform __herbivores1;
    [SerializeField] Transform __herbivores2;
    [SerializeField] Transform __herbivores3;
    [SerializeField] Transform __predators1;
    [SerializeField] Transform __predators2;
    [SerializeField] Transform __predators3;

    static Transform _herbivores1;
    static Transform _herbivores2;
    static Transform _herbivores3;
    static Transform _predators1;
    static Transform _predators2;
    static Transform _predators3;

    [System.Serializable]
    public class AnimalPrefab
    {
        [SerializeField] GameObject _prefab;
        [SerializeField][Tooltip("0 means herbivorous, 1 means small predators and so on. Maximum is 3.")] byte _size = 0;
        [SerializeField] bool _isPredator = false;

        public void InstantiateAnimal(Vector3 position, byte number)
        {
            Transform parent;
            switch (_size)
            {
                case 0:
                    {
                        parent = _herbivores1;
                        break;
                    }
                case 1:
                    {
                        if (_isPredator)
                        {
                            parent = _predators1;
                        }
                        else
                        {
                            parent = _herbivores1;
                        }
                        break;
                    }
                case 2:
                    {
                        if (_isPredator)
                        {
                            parent = _predators2;
                        }
                        else
                        {
                            parent = _herbivores2;
                        }
                        break;
                    }
                case 3:
                    {
                        if (_isPredator)
                        {
                            parent = _predators3;
                        }
                        else
                        {
                            parent = _herbivores3;
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogError("Minimim value for animal size is 0, Maximum is 3");
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

    public static Vector3[] _herbivoresPos;
    public static Vector3[] _predators1Pos;
    public static Vector3[] _predators2Pos;
    public static Vector3[] _predators3Pos;

    //Test Variables
    [SerializeField] bool _init = false;

    public static byte _frameCounter = 0;

    [SerializeField] AnimalPrefab[] _animalPrefabs;
    public static AnimalPrefab[] _AnimalPrefabs;


    //========================================================================================================================
    // Functions in this class are called before others (-2)
    //========================================================================================================================
    // Start is called before the first frame update
    void Start()
    {
        _herbivores1 = __herbivores1;
        _herbivores2 = __herbivores2;
        _herbivores3 = __herbivores3;
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
                    TransformToPosition(_herbivores1, ref _herbivoresPos);
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
        Init();
    }

    void TransformToPosition(Transform parent, ref Vector3[] positions)
    {
        List<Vector3> tempList = new List<Vector3>();
        foreach(Transform child in parent)
        {
            tempList.Add(child.position);
        }
        positions = tempList.ToArray();
    }

    public static Vector3[] Search(Vector3 position, float distance, byte threatThreshold)
    {
        float Abs(float f)
        {
            return (f > 0) ? f : -f;
        }
        List<Vector3> Search(Vector3[] Positions)
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
        return Result.ToArray();
    }

    public static Vector3 CrudeFlee(Vector3 position, float distance, byte threatThreshold)
    {
        Vector3 Result = Vector3.zero;
        float Abs(float f)
        {
            return (f > 0) ? f : -f;
        }
        Vector3 SearchSum(Vector3[] Positions)
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
                Result += SearchSum(_predators1Pos.ToArray());
                Result += SearchSum(_predators2Pos.ToArray());
                Result += SearchSum(_predators3Pos.ToArray());
                break;
            case 2:
                Result += SearchSum(_predators2Pos.ToArray());
                Result += SearchSum(_predators3Pos.ToArray());
                break;
            case 3:
                Result += SearchSum(_predators3Pos.ToArray());
                break;
            default:
                Debug.LogError("threatThreshold maximum is 3, minimum being 1.");
                break;
        }
        return Result;
    }

    public static void InstantiateAnimal(byte index, Vector3 position)
    {
        _AnimalPrefabs[index].InstantiateAnimal(position, 1);
    }

    //Test Functions======================================================================================================================

    public static Transform GetAnimalParent(byte index)
    {
        switch (index)
        {
            case 1: return _herbivores1;
            case 2: return _herbivores2;
            case 3: return _herbivores3;

            case 11: return _predators1;
            case 12: return _predators2;
            case 13: return _predators3;
        }
        Debug.LogError("Animal Parent Inedx Error: 1, 2, 3 for herbivores, 11, 12, 13 for carnivores");
        return null;
    }

    //Test Functions======================================================================================================================
    void Init()
    {
        if (_init)
        {
            _init = false;
            foreach (AnimalPrefab animalPrefab in _animalPrefabs)
            {
                animalPrefab.InstantiateAnimal(Random.onUnitSphere, 1);
            }
        }
    }
}
