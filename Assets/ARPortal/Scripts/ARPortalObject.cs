using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class ARPortalObject : MonoBehaviour
{
    public List<Material> materials = new List<Material>();

    [SerializeField]
    private Shader gameObjectShader;

    public bool isInsideOfAR = true;

    public bool isMoving = false;

    private void Awake()
    {
        GetChildMaterials();
        ChangeSelfState();
        foreach (Material material in materials)
        {
            material.SetFloat("_Stencil", 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out ARPortal portal))
            return;

        foreach (Material material in materials)
        {
            material.SetFloat("_StencilComp", (float)CompareFunction.Always);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out ARPortal portal))
            return;

        if (portal.isBackSide)
        {
            isInsideOfAR = !isInsideOfAR;
            ChangeSelfState();
        }
    }

    private void ChangeSelfState()
    {
        if (isInsideOfAR == FindObjectOfType<ARPortal>().isInside)
        {
            foreach (Material material in materials)
            {
                material.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
            }
        }
        else
        {
            foreach (Material material in materials)
            {
                material.SetFloat("_StencilComp", (float)CompareFunction.Equal);
            }
        }
    }

    private void GetChildMaterials()
    {
        if (materials.Count == 0)
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true))
            {
                foreach (Material mat in renderer.materials)
                {
                    materials.Add(mat);
                    materials.Last().shader = gameObjectShader;
                }
            }
        }
    }

    private void OnEnable()
    {
        GetChildMaterials();
        ChangeSelfState();

        ARPortal[] portals = FindObjectsOfType<ARPortal>();
        foreach(ARPortal portal in portals)
        {
            portal.GetARPortalObjects();
            portal.ChangeState();
        }
    }


}
