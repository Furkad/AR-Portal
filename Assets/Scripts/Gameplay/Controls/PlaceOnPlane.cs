using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    //Префаб, который будет размещен в точке нажатия
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    private GameObject placedPrefab;

    //Заспавненный объект
    private GameObject spawnedObject;

    //Были ли объект заспавнен
    private bool isSpawned = false;

    //Экземпляр класса управления
    private TouchControls controls;

    //Нажат ли сейчас экран
    private bool isPressed;

    //AR Raycast Manager для взаимодействия с окружением
    private ARRaycastManager aRRaycastManager;
    //Список столкновений для отслеживания объектов в мире
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //AR Session для перезапуска сессии после размещения объекта
    [SerializeField]
    private ARSession arSession;

    //Скрипт управления пользовательским интерфейсом
    private UIController uiController;

    private void Awake()
    {
        //Получаем AR Raycast Manager
        aRRaycastManager = GetComponent<ARRaycastManager>();

        //Принимаем значение из созданной сущности
        controls = TouchControlsEntity.instance;

        //Добавляем реакции на события ввода
        controls.Player.Tap.performed += _ => isPressed = true;
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

        //Если выпущенный луч пересекается с поверхностями
        if (aRRaycastManager.Raycast(touchPostion, hits, TrackableType.PlaneWithinPolygon))
        {
            //Выбираем ближайшую точку
            Pose hitPose = hits[0].pose;

            //Создаем объект только если он еще не был заспавнен ранее
            if (spawnedObject == null && !isSpawned)
            {
                //Спавним объект и поворачиваем его к камере
                spawnedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.identity);
                spawnedObject.transform.LookAt(new Vector3(Camera.main.transform.position.x, spawnedObject.transform.position.y, Camera.main.transform.position.z));
                spawnedObject.transform.Rotate(new Vector3(0f, 180f, 0f));

                //Выключаем поверхности и выключаем менеджер поверхностей
                ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false;

                //Объект был заспавнен
                isSpawned = true;

                //Меняем состояние подсказки в пользовательском интерфейсе
                UIController uiController = FindObjectOfType<UIController>();
                uiController.HintPanelUpdate(2);
            }
        }
    }

    //Включаем управление
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    //Выключаем управление
    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
