using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    [SerializeField]
    private InventoryController inventoryController;
    [SerializeField]
    private CoinController coinController;

    //Экземпляр класса управления
    private TouchControls controls;

    //Нажат ли сейчас экран
    private bool isPressed;

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
            return;

        //Сохраняем позицию нажатия
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //Выпускаем луч и создаем необходимые переменные
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;
        ItemInfo item;

        if (Physics.Raycast(ray, out hit))
        {
            //Проверка на пересечение с коллайдером игрового объекта
            if (hit.collider != null)
            {
                //Если объект имеет компонент предмета, то добавляем в инвентарь
                if (hit.collider.gameObject.TryGetComponent(out item) && inventoryController.inventoryItems.Count < inventoryController.inventoryCapacity)
                {
                    inventoryController.AddItem(item);
                    Destroy(hit.collider.gameObject);
                    return;
                }
                
                //Сравнение тега для получения монеты
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
