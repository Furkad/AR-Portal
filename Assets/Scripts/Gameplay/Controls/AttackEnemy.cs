using UnityEngine;
using UnityEngine.InputSystem;

public class AttackEnemy : MonoBehaviour
{
    //������ �������� �������
    [SerializeField]
    private float cooldownTimer = 1f;

    //������� ���������� ����� � ������� ���������� �������
    private float timer = 1f;

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
        //������ � �������� �������� �������
        timer += Time.deltaTime;
        if (timer < cooldownTimer)
            return;

        //��������� ������ ������, ���� ��� ���������, �� ����� ������� � �������� ���� �����
        if (Pointer.current == null || isPressed == false || uiController.isPaused)
            return;

        //��������� ������� �������
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //��������� ��� � ������� ����������� ����������
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //�������� �� ����������� � ����������� �������� �������
            if (hit.collider != null)
            {
                //���� ������ ����� ��������� ��������, �� ��������� � ���������
                if (hit.collider.gameObject.TryGetComponent(out EnemyHP enemyHP))
                {
                    //�������� ������ � ���������� �������� ��������� ������������� ���������
                    timer = 0f;
                    //��������� ����� ����������
                    enemyHP.Damage(Random.Range(15f, 33f));
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
