using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeColor : MonoBehaviour
{
    public List<GameObject> spheres = new List<GameObject>();
    public UnityAction BlockLevelers;

    [SerializeField]
    private List<Color> startingColors = new List<Color>()
    {
        new Color(1f,0f,0f,1f),
        new Color(0f,1f,1f,1f),
        new Color(0f,0f,0f,1f)
    };
    [SerializeField]
    private GameObject portalGO;

    private List<MeshRenderer> renderers = new List<MeshRenderer>();

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip portalSound;

    private void Start()
    {
        foreach (GameObject go in spheres)
        {
            renderers.Add(go.GetComponent<MeshRenderer>());
        }

        int i = 0;
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = startingColors[i];
            i++;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = portalSound;
    }

    public void SwitchColor(int levelerID, ColorCaller.State stateOfLeveler, Color color)
    {
        switch (levelerID)
        {
            case 0:
            {
                switch (stateOfLeveler)
                {
                    case ColorCaller.State.Left:
                            { renderers[1].material.color += color; renderers[2].material.color -= color; break; }
                    case ColorCaller.State.Mid:
                            { renderers[0].material.color += color; renderers[1].material.color -= color; break; }
                    case ColorCaller.State.Right:
                            { renderers[2].material.color += color; renderers[0].material.color -= color; break; }
                }
                break;
            }
            case 1:
            {
                switch (stateOfLeveler)
                {
                    case ColorCaller.State.Left:
                            { renderers[2].material.color += color; renderers[0].material.color -= color; break; }
                    case ColorCaller.State.Mid:
                            { renderers[1].material.color += color; renderers[2].material.color -= color; break; }
                    case ColorCaller.State.Right:
                            { renderers[0].material.color += color; renderers[1].material.color -= color; break; }
                }
                break;
            }
            case 2:
            {
                switch (stateOfLeveler)
                {
                    case ColorCaller.State.Left:
                            { renderers[0].material.color += color; renderers[1].material.color -= color; break; }
                    case ColorCaller.State.Mid:
                            { renderers[2].material.color += color; renderers[0].material.color -= color; break; }
                    case ColorCaller.State.Right:
                            { renderers[1].material.color += color; renderers[2].material.color -= color; break; }
                }
                break;
            }
        }

        if (CheckOpenCondition())
            StartCoroutine(OpenPortal());
    }

    public void InitializeColor(int levelerID, ColorCaller.State stateOfLeveler, Color color)
    {
        switch (levelerID)
        {
            case 0:
                {
                    switch (stateOfLeveler)
                    {
                        case ColorCaller.State.Left:
                            renderers[1].material.color += color; break;
                        case ColorCaller.State.Mid:
                            renderers[0].material.color += color; break;
                        case ColorCaller.State.Right:
                            renderers[2].material.color += color; break;
                    }
                    break;
                }
            case 1:
                {
                    switch (stateOfLeveler)
                    {
                        case ColorCaller.State.Left:
                            renderers[2].material.color += color; break;
                        case ColorCaller.State.Mid:
                            renderers[1].material.color += color; break;
                        case ColorCaller.State.Right:
                            renderers[0].material.color += color; break;
                    }
                    break;
                }
            case 2:
                {
                    switch (stateOfLeveler)
                    {
                        case ColorCaller.State.Left:
                            renderers[0].material.color += color; break;
                        case ColorCaller.State.Mid:
                            renderers[2].material.color += color; break;
                        case ColorCaller.State.Right:
                            renderers[1].material.color += color; break;
                    }
                    break;
                }
        }
    }

    private bool CheckOpenCondition()
    {
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer.material.color != Color.white)
                return false;
        }
        return true;
    }

    private IEnumerator OpenPortal()
    {
        audioSource.Play();
        BlockLevelers();
        yield return new WaitForSeconds(3f);

        GameObject spawnedObject = Instantiate(portalGO, transform.position, transform.rotation);
        spawnedObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, spawnedObject.transform.position.y, Camera.main.transform.position.z));
        spawnedObject.transform.Rotate(new Vector3(0f, 180f, 0f));

        UIController uiController = FindObjectOfType<UIController>();
        uiController.HintPanelUpdate(3);
        Destroy(gameObject);
    }  
}
