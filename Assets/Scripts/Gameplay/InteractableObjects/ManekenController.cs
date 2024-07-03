using System.Collections.Generic;
using UnityEngine;

public class ManekenController : ContainerController
{
    public List<GameObject> visualsOfArmor = new List<GameObject>();

    public void TurnOnItem(string itemName)
    {
        foreach (GameObject item in visualsOfArmor)
        {
            if (item.name == itemName)
            {
                item.SetActive(true);
                return;
            }
        }
    }
}
