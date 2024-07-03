using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    //��������� ������ ����������
    private TouchControls controls;

    //����� �� ������ �����
    private bool isPressed;

    //���������� ����� ������� �������� � �������
    private float zCoord;

    //������� ������ ������������� ������ ��� ��� ��������� ����������
    private GameObject currentGO;

    private UIController uiController;

    private void Start()
    {
        //��������� �������� �� ��������� ��������
        controls = TouchControlsEntity.instance;

        //��������� ������� �� ������� �����
        controls.Player.Tap.started += _ => isPressed = true;
        controls.Player.Tap.canceled += _ => isPressed = false;

        //������� ������ ���������� ���������������� �����������
        uiController = FindObjectOfType<UIController>();
    }

    private void Update()
    {
        //��������� ������ ������, ���� ��� ���������, �� ����� ������� � �������� ���� �����
        if (Pointer.current == null || isPressed == false || uiController.isPaused)
        {
            //���������� ������� ����������� ��������� � ������� ������� ������
            if (currentGO != null)
            {
                currentGO.GetComponent<Rigidbody>().isKinematic = false;
                currentGO = null;
            }
            return;
        }

        //��������� ������� �������
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //��������� ��� � ������� ����������� ����������
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //���� ������ ����� ��� Movable � ���� �������
            if (hit.collider != null && hit.collider.CompareTag("Movable") && isPressed)
            {
                //������� ������
                currentGO = hit.transform.gameObject;

                if (!currentGO.GetComponent<Rigidbody>().isKinematic)
                    currentGO.GetComponent<Rigidbody>().isKinematic = true;

                zCoord = Camera.main.WorldToScreenPoint(hit.collider.transform.position).z;

                Vector3 pointPos = touchPostion;
                pointPos.z = zCoord;
                Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(pointPos);

                hit.transform.position = pointerWorldPos;
            }
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
