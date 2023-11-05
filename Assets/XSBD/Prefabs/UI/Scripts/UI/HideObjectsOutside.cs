using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideObjectsOutside : MonoBehaviour
{
    [SerializeField] bool _done = false;

    private void Update()
    {
        if (!_done)
        {
            SetChildrenRenderQueue(transform);
        }
    }

    void SetChildrenRenderQueue(Transform transform)
    {
        if (transform.TryGetComponent(out Renderer renderer) && (renderer is SkinnedMeshRenderer || renderer is MeshRenderer))
        {
            foreach (Material material in renderer.materials)
            {
                material.renderQueue = 3002;
                _done = true;
            }
        }
        foreach (Transform child in transform)
        {
            SetChildrenRenderQueue(child);
        }
    }
}
