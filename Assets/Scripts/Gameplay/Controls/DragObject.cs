using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    //Экземпляр класса управления
    private TouchControls controls;

    //Нажат ли сейчас экран
    private bool isPressed;

    //Расстояние между игровым объектом и камерой
    private float zCoord;

    //Игровой объект передвигается сейчас или был последним передвинут
    private GameObject currentGO;

    private UIController uiController;

    private void Start()
    {
        //Принимаем значение из созданной сущности
        controls = TouchControlsEntity.instance;

        //Добавляем реакции на события ввода
        controls.Player.Tap.started += _ => isPressed = true;
        controls.Player.Tap.canceled += _ => isPressed = false;

        //Находим скрипт управления пользовательским интерфейсом
        uiController = FindObjectOfType<UIController>();
    }

    private void Update()
    {
        //Исключаем работу метода, если нет указателя, не нажат дисплей и включено меню паузы
        if (Pointer.current == null || isPressed == false || uiController.isPaused)
        {
            //Возвращаем объекту изначальные параметры и очищаем текущий объект
            if (currentGO != null)
            {
                currentGO.GetComponent<Rigidbody>().isKinematic = false;
                currentGO = null;
            }
            return;
        }

        //Сохраняем позицию нажатия
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //Выпускаем луч и создаем необходимые переменные
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Если объект имеет тег Movable и есть нажатие
            if (hit.collider != null && hit.collider.CompareTag("Movable") && isPressed)
            {
                //Двигаем объект
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
