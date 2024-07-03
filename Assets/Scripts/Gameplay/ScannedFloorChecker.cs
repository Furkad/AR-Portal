using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScannedFloorChecker : MonoBehaviour
{
    [SerializeField]
    private UIController uiController;

    private ARPlaneManager planeManager;
    private bool isScanned;

    private void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (!isScanned)
        {
            if (planeManager.trackables != null)
            {
                isScanned = true;
                uiController.HintPanelUpdate(1);
            }
        }
    }
}
