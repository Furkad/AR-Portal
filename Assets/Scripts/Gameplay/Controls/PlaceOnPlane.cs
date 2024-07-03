using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    //������, ������� ����� �������� � ����� �������
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    private GameObject placedPrefab;

    //������������ ������
    private GameObject spawnedObject;

    //���� �� ������ ���������
    private bool isSpawned = false;

    //��������� ������ ����������
    private TouchControls controls;

    //����� �� ������ �����
    private bool isPressed;

    //AR Raycast Manager ��� �������������� � ����������
    private ARRaycastManager aRRaycastManager;
    //������ ������������ ��� ������������ �������� � ����
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //AR Session ��� ����������� ������ ����� ���������� �������
    [SerializeField]
    private ARSession arSession;

    //������ ���������� ���������������� �����������
    private UIController uiController;

    private void Awake()
    {
        //�������� AR Raycast Manager
        aRRaycastManager = GetComponent<ARRaycastManager>();

        //��������� �������� �� ��������� ��������
        controls = TouchControlsEntity.instance;

        //��������� ������� �� ������� �����
        controls.Player.Tap.performed += _ => isPressed = true;
        controls.Player.Tap.canceled += _ => isPressed = false;

        //������� ������ ���������� ���������������� �����������
        uiController = FindObjectOfType<UIController>();
    }

    private void Update()
    {
        //��������� ������ ������, ���� ��� ���������, �� ����� ������� � �������� ���� �����
        if (Pointer.current == null || isPressed == false || uiController.isPaused)
            return;

        //��������� ������� �������
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //���� ���������� ��� ������������ � �������������
        if (aRRaycastManager.Raycast(touchPostion, hits, TrackableType.PlaneWithinPolygon))
        {
            //�������� ��������� �����
            Pose hitPose = hits[0].pose;

            //������� ������ ������ ���� �� ��� �� ��� ��������� �����
            if (spawnedObject == null && !isSpawned)
            {
                //������� ������ � ������������ ��� � ������
                spawnedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.identity);
                spawnedObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, spawnedObject.transform.position.y, Camera.main.transform.position.z));
                spawnedObject.transform.Rotate(new Vector3(0f, 180f, 0f));

                //��������� ����������� � ��������� �������� ������������
                ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false;

                //������ ��� ���������
                isSpawned = true;

                //������ ��������� ��������� � ���������������� ����������
                UIController uiController = FindObjectOfType<UIController>();
                uiController.HintPanelUpdate(2);
            }
        }
    }

    //�������� ����������
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    //��������� ����������
    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
