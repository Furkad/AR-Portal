using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    public int inventoryCapacity = 10;

    public Dictionary<ItemInfo, int> inventoryItems = new Dictionary<ItemInfo, int>();

    [SerializeField]
    private List<Image> imagesOfObjects = new List<Image>();

    [SerializeField]
    private List<Image> imagesOfSelection = new List<Image>();

    [SerializeField]
    private List<TMP_Text> numberOfObjects = new List<TMP_Text>();

    public int currentSelected = -1;

    private void Start()
    {
        UpdateVisuals();
    }

    public void AddItem(ItemInfo item)
    {
        if (inventoryItems.Count < inventoryCapacity)
        {
            bool isInDictionary = false;
            foreach (KeyValuePair<ItemInfo, int> keyValuePair in inventoryItems)
            {
                if (keyValuePair.Key.itemName == item.itemName)
                {
                    inventoryItems[keyValuePair.Key]++;
                    isInDictionary = true;
                    break;
                }
            }
            if (!isInDictionary)
                inventoryItems.Add(item, 1);
        }

        UpdateVisuals();
    }

    public void RemoveItem(ItemInfo item)
    {
        if (inventoryItems.ContainsKey(item))
        {
            inventoryItems[item]--;
        }

        if (inventoryItems[item] == 0)
            inventoryItems.Remove(item);

        inventoryItems = new Dictionary<ItemInfo, int>(inventoryItems);
        UpdateVisuals();
    }

    public void RemoveItemAtIndex(int itemIndex)
    {
        if (inventoryItems.ElementAt(itemIndex).Value > 0)
        {
            RemoveItem(inventoryItems.ElementAt(itemIndex).Key);
        }
    }

    public void SelectItemAtIndex(int itemIndex)
    {
        if (currentSelected == itemIndex)
            currentSelected = -1;
        else
            currentSelected = itemIndex;

        UpdateVisuals();
    }

    //-1 возвращается если нет предмета
    public int ContainsAt(string itemName)
    {
        int result = -1;
        int iteration = 0;

        foreach (KeyValuePair<ItemInfo, int> keyValuePair in inventoryItems)
        {
            if (keyValuePair.Key.itemName == itemName)
            {
                result = iteration;
                break;
            }
            iteration++;
        }

        return result;
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < imagesOfObjects.Count; i++)
        {
            if (i < inventoryItems.Count)
            {
                ItemInfo currentItem = inventoryItems.Keys.ElementAt(i);
                imagesOfObjects[i].gameObject.SetActive(true);
                imagesOfObjects[i].sprite = currentItem.sprite;
                if (inventoryItems.Values.ElementAt(i) > 1)
                {
                    numberOfObjects[i].text = inventoryItems.Values.ElementAt(i).ToString();
                }
                else
                {
                    numberOfObjects[i].text = "";
                }

                if (i == currentSelected)
                {
                    imagesOfSelection[i].gameObject.SetActive(true);
                }
                else
                {
                    imagesOfSelection[i].gameObject.SetActive(false);
                }
            }
            else
            {
                imagesOfObjects[i].gameObject.SetActive(false);
                imagesOfObjects[i].sprite = null;
                numberOfObjects[i].text = "";
                imagesOfSelection[i].gameObject.SetActive(false);
            }
        }
    }
}
