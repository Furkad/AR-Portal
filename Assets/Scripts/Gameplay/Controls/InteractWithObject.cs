using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWithObject : MonoBehaviour
{
    //������ �������� �������
    [SerializeField]
    private float cooldownTimer = 1f;

    //������� ���������� ����� � ������� ���������� �������
    private float timer = 1f;

    [SerializeField]
    private InventoryController inventoryController;

    //��������� ������ ����������
    private TouchControls controls;

    //����� �� ������ �����
    private bool isPressed;

    private UIController uiController;

    protected void Start()
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
            if (hit.collider != null)
            {
                //���� ��� ���������� ������������� �������
                if (hit.collider.CompareTag("Interactable"))
                {
                    //�������� ������ � ���������� �������� ��������� ������������� ���������
                    timer = 0f;
                    //���� ������� - ������, �� ���� ��������� ��� ��� ������� ����� � ��������� ���������, ���� ������ ���������
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

                    //���� ������� - ���������, �� ��������� ����� ��� ������� � ��������� ���������
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

                    //���� ������� - �������, �� ��������� ����� ����� ��� ������� � ��������� ���������, ����� �������� ������
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

                    //���� ������� - �����, �� ��������� ����� ����� ��� ������� � ��������� ���������, ����� �������� ������
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
