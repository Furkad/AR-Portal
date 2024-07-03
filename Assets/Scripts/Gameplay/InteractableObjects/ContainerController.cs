using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    [SerializeField]
    protected List<string> prefabsNames = new List<string>();
    [SerializeField]
    protected List<bool> isInside = new List<bool>();

    [SerializeField]
    protected GameObject spawnPrefab;

    [SerializeField]
    protected Transform transformOfCoinSpawn;

    protected AudioSource audioSource;
    [SerializeField]
    protected AudioClip addedItemClip;

    private bool finishedPotion = false;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = addedItemClip;
    }

    public void AddItem(string itemName)
    {
        if (prefabsNames.Contains(itemName))
        {
            int i = prefabsNames.IndexOf(itemName);
            isInside[i] = true;
            audioSource.Play();
        }
        TrySpawnItem();
    }

    private void TrySpawnItem()
    {
        if (finishedPotion)
            return;

        if (isInside.Contains(false))
            return;

        Instantiate(spawnPrefab, transformOfCoinSpawn.position, Quaternion.identity);
        finishedPotion = true;
    }

}
