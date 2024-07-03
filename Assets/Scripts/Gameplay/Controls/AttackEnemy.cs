using UnityEngine;
using UnityEngine.InputSystem;

public class AttackEnemy : MonoBehaviour
{
    //Таймер кулдауна нажатия
    [SerializeField]
    private float cooldownTimer = 1f;

    //Текущее пройденное время с момента последнего нажатия
    private float timer = 1f;

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
        //Работа с таймером кулдауна нажатия
        timer += Time.deltaTime;
        if (timer < cooldownTimer)
            return;

        //Исключаем работу метода, если нет указателя, не нажат дисплей и включено меню паузы
        if (Pointer.current == null || isPressed == false || uiController.isPaused)
            return;

        //Сохраняем позицию нажатия
        Vector2 touchPostion = Pointer.current.position.ReadValue();

        //Выпускаем луч и создаем необходимые переменные
        Ray ray = Camera.main.ScreenPointToRay(touchPostion);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Проверка на пересечение с коллайдером игрового объекта
            if (hit.collider != null)
            {
                //Если объект имеет компонент предмета, то добавляем в инвентарь
                if (hit.collider.gameObject.TryGetComponent(out EnemyHP enemyHP))
                {
                    //Обнуляем таймер и перебираем варианты возможных интерактивных предметов
                    timer = 0f;
                    //Нанесение урона противнику
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
