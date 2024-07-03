using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
[RequireComponent (typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class ARPortal : MonoBehaviour
{
    [SerializeField]
    private List<ARPortalObject> aRPortalObjects = new List<ARPortalObject>();

    public bool isInside = false;
    public bool isBackSide;

    [SerializeField]
    private Shader portalWindowShader;

    private void Awake()
    {
        if (GetComponent<Renderer>().material.shader != portalWindowShader)
            GetComponent<Renderer>().material.shader = portalWindowShader;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        GetComponent<Collider>().isTrigger = true;
    }

    private void Start()
    {
        isInside = false;

        if (isBackSide)
        {
            GetComponent<Renderer>().material.SetFloat("_Stencil", 0);
            return;
        }

        GetComponent<Renderer>().material.SetFloat("_Stencil", 1);

        GetComponent<Renderer>().material.SetFloat("_Culling", (float)CullMode.Back);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("MainCamera"))
            return;

        isInside = !isInside;
        ChangeState();

        if (isBackSide && isInside || !isInside && !isBackSide)
            GetComponent<Renderer>().material.SetFloat("_Stencil", 1);
        else
            GetComponent<Renderer>().material.SetFloat("_Stencil", 0);

        if (isBackSide && isInside || !isInside && !isBackSide)
            foreach (ARPortal arPortal in FindObjectsOfType<ARPortal>())
                if (arPortal == this)
                    arPortal.GetComponent<Renderer>().enabled = false;
                else
                    GetComponent<Renderer>().material.SetFloat("_Culling", (float)CullMode.Front);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("MainCamera"))
            return;

        if (isBackSide && !isInside || isInside && !isBackSide)
            foreach (ARPortal arPortal in FindObjectsOfType<ARPortal>())
                arPortal.GetComponent<Renderer>().enabled = true;
        else
            GetComponent<Renderer>().material.SetFloat("_Culling", (float)CullMode.Back);

        ChangeState();  
       
        if (isBackSide && isInside || !isInside && !isBackSide)
            GetComponent<Renderer>().material.SetFloat("_Stencil", 1);
        else
            GetComponent<Renderer>().material.SetFloat("_Stencil", 0);
    }

    public void ChangeState()
    {
        foreach (ARPortalObject ARObject in aRPortalObjects)
        {
            if (isInside && ARObject.isInsideOfAR || !isInside && !ARObject.isInsideOfAR)
                if (ARObject.isMoving)
                    foreach (Material material in ARObject.materials)
                        material.SetFloat("_StencilComp", (float)CompareFunction.Always);
                else
            foreach (Material material in ARObject.materials)
                    material.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
            else
                foreach (Material material in ARObject.materials)
                    material.SetFloat("_StencilComp", (float)CompareFunction.Equal);
        }
    }

    public void GetARPortalObjects()
    {
        aRPortalObjects.Clear();
        aRPortalObjects = FindObjectsOfType<ARPortalObject>().ToList();
    }

    private void OnEnable()
    {
        GetARPortalObjects();
        ChangeState();
    }
}
