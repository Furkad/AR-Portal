using System.Collections;
using UnityEngine;
using TMPro;

public class CoinController : MonoBehaviour
{
    public int goalOfCoins;

    private int currentAmountOfCoins = 0;

    [SerializeField]
    private TMP_Text amountOfCoinsText;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private AudioClip coinAudio;

    private bool coinsReturned = false;
    private void Start()
    {
        UpdateVisuals();
    }

    public void AddCoin()
    {
        currentAmountOfCoins++;
        AudioSource.PlayClipAtPoint(coinAudio, Camera.main.transform.position);
        UpdateVisuals();
        if (currentAmountOfCoins == goalOfCoins)
        {
            if (coinsReturned)
                StartCoroutine(EndGame());
            else
                SpawnEnemy();
        }
    }

    private void UpdateVisuals()
    {
        amountOfCoinsText.text = currentAmountOfCoins.ToString() + " / " + goalOfCoins.ToString();
    }

    private IEnumerator EndGame()
    {
        UIController uiController = FindObjectOfType<UIController>();
        uiController.HintPanelUpdate(5);
        yield return new WaitForSeconds(5f);
        FindObjectOfType<GameManager>().ReturnToMainMenu();
    }

    private void SpawnEnemy()
    {
        UIController uiController = FindObjectOfType<UIController>();
        uiController.HintPanelUpdate(4);
        Vector3 spawnPos = Camera.main.transform.position;
        spawnPos.y /= 1.5f;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity/*, GameObject.Find("Room New(Clone)").transform*/);
        currentAmountOfCoins = 0;
        UpdateVisuals();
        FindObjectOfType<EnemyHP>().DeathEvent += ReturnMoney;
    }

    private void ReturnMoney()
    {
        coinsReturned = true;
        currentAmountOfCoins = goalOfCoins-1;
        AddCoin();
    }
}
