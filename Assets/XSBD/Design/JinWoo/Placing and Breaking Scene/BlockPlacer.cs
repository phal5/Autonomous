using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField] float placerRange;
    [SerializeField] GameObject CursorSphere;
    [SerializeField] GameObject HoloCube;
    [SerializeField] GameObject PlacedCube;
    [SerializeField] float scrollspeed;
    [SerializeField] float cooldownDelay;
    Ray pointerRay;
    RaycastHit worldCursor;
    float HoloCubeHeight;
    float cooldownTimer;
    bool isPlacing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cursorSphere Controller
        CursorControl();

        //HoloCube Placer
        if(Input.GetKeyDown(KeyCode.Mouse1) && !isPlacing)
        {
            HoloCubeHeight = 0;
            isPlacing = true;
            cooldownTimer = 0;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && isPlacing && (cooldownTimer > cooldownDelay))
        {
            isPlacing = false;
            HoloCube.SetActive(false);
            if(HoloCube.GetComponent<HoloCubeColorChanger>().canplace())
            {
                Instantiate(PlacedCube, HoloCube.transform.position, HoloCube.transform.rotation);
            }
        }
        if (isPlacing)
        {
            if (CursorSphere.activeSelf)
            {
                HoloCube.SetActive(true);
            }
            HoloCubeHeight += Input.mouseScrollDelta.y * scrollspeed;
            HoloCube.transform.position = worldCursor.point + Vector3.down * HoloCubeHeight;
            HoloCube.transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
            if(cooldownTimer <= cooldownDelay)
            {
                cooldownTimer += Time.deltaTime;
            }
        }
    }

    private void OnEnable()
    {
        isPlacing = false;
        HoloCubeHeight = 0;
    }

    void CursorControl()
    {
        pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(pointerRay, out worldCursor, placerRange))
        {
            if (!CursorSphere.activeSelf)
            {
                CursorSphere.SetActive(true);
            }
            CursorSphere.transform.position = worldCursor.point;
        }
        else
        {
            CursorSphere.SetActive(false);
        }
    }

    public float GetplacerRange()
    {
        return placerRange;
    }
}
