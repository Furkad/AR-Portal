using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    [SerializeField]
    private InventoryController inventoryController;
    [SerializeField]
    private CoinController coinController;

    //��������� ������ ����������
    private TouchControls controls;

    //����� �� ������ �����
    private bool isPressed;

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
            return;

        //��������� ������� �������
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //��������� ��� � ������� ����������� ����������
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;
        ItemInfo item;

        if (Physics.Raycast(ray, out hit))
        {
            //�������� �� ����������� � ����������� �������� �������
            if (hit.collider != null)
            {
                //���� ������ ����� ��������� ��������, �� ��������� � ���������
                if (hit.collider.gameObject.TryGetComponent(out item) && inventoryController.inventoryItems.Count < inventoryController.inventoryCapacity)
                {
                    inventoryController.AddItem(item);
                    Destroy(hit.collider.gameObject);
                    return;
                }
                
                //��������� ���� ��� ��������� ������
                if (hit.collider.CompareTag("Coin"))
                {
                    coinController.AddCoin();
                    Destroy(hit.collider.gameObject);
                    return;
                }
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
