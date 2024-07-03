using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithObject : MonoBehaviour
{
    //Таймер кулдауна нажатия
    [SerializeField]
    private float cooldownTimer = 1f;

    //Текущее пройденное время с момента последнего нажатия
    private float timer = 1f;

    [SerializeField]
    private InventoryController inventoryController;

    //Экземпляр класса управления
    private TouchControls controls;

    //Нажат ли сейчас экран
    private bool isPressed;

    private UIController uiController;

    protected void Start()
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
            if (hit.collider != null)
            {
                //Если тег обозначает интерактивный предмет
                if (hit.collider.CompareTag("Interactable"))
                {
                    //Обнуляем таймер и перебираем варианты возможных интерактивных предметов
                    timer = 0f;
                    //Если предмет - сундук, то либо открываем его при наличии ключа и подчищаем инвентарь, либо меняем состояние
                    if (hit.collider.TryGetComponent(out ChestController currentChest))
                    {
                        if (!currentChest.isUnlocked && inventoryController.currentSelected == inventoryController.ContainsAt("Key_Golden") && inventoryController.currentSelected != -1)
                        {
                            currentChest.UnlockChest();
                            inventoryController.currentSelected = -1;
                            inventoryController.RemoveItemAtIndex(inventoryController.ContainsAt("Key_Golden"));
                            return;
                        }

                        hit.collider.GetComponent<ChestController>().SwitchState();
                        return;
                    }

                    //Если предмет - контейнер, то добавляем зелье при наличии и подчищаем инвентарь
                    if (hit.collider.TryGetComponent(out ContainerController currentController))
                    {
                        if (inventoryController.currentSelected == inventoryController.ContainsAt("Potion_Blue") || inventoryController.currentSelected == inventoryController.ContainsAt("Potion_Green") || inventoryController.currentSelected == inventoryController.ContainsAt("Potion_Red") && inventoryController.currentSelected != -1)
                        {
                            string currentPotion = inventoryController.inventoryItems.ElementAt(inventoryController.currentSelected).Key.itemName;
                            currentController.AddItem(currentPotion);
                            inventoryController.currentSelected = -1;
                            inventoryController.RemoveItemAtIndex(inventoryController.ContainsAt(currentPotion));
                            return;
                        }
                    }

                    //Если предмет - манекен, то добавляем часть брони при наличии и подчищаем инвентарь, также включаем визуал
                    if (hit.collider.TryGetComponent(out ManekenController manekenController))
                    {
                        if (inventoryController.currentSelected == inventoryController.ContainsAt("armor") || inventoryController.currentSelected == inventoryController.ContainsAt("armor_helmet") || inventoryController.currentSelected == inventoryController.ContainsAt("armor_shoulder_pad_left")
                            || inventoryController.currentSelected == inventoryController.ContainsAt("armor_shoulder_pad_right") || inventoryController.currentSelected == inventoryController.ContainsAt("armor_gorget") && inventoryController.currentSelected != -1)
                        {
                            string currentItem = inventoryController.inventoryItems.ElementAt(inventoryController.currentSelected).Key.itemName;
                            manekenController.AddItem(currentItem);
                            inventoryController.currentSelected = -1;
                            inventoryController.RemoveItemAtIndex(inventoryController.ContainsAt(currentItem));
                            manekenController.TurnOnItem(currentItem);
                            return;
                        }
                    }

                    //Если предмет - рычаг, то добавляем часть брони при наличии и подчищаем инвентарь, также включаем визуал
                    if (hit.collider.TryGetComponent(out ColorCaller colorCaller))
                    {
                        colorCaller.ChangeState();
                        return;
                    }
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
