using UnityEngine;

public class ItemInfo: MonoBehaviour
{
    public string itemName;
    public Sprite sprite;
    public GameObject prefab;

    private void Start()
    {
        if (itemName == null && itemName == "")
            itemName = gameObject.name;
    }
}
