using System.Collections;
using System.Collections.Generic;
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

    public static Vector3[] Search(Vector3 position, byte threatThreshold)
    {
        void Search(Transform parent)
        {

        }
        Vector3[] Result = new Vector3[1024];
        switch (threatThreshold)
        {
            case 0:

            case 1:

            case 2:

            case 3:

                break;
            default:
                Debug.LogError("threatThreshold maximum is 3");
                break;
        }
        return Result;
    }
}
