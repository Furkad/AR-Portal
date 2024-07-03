using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;

    [SerializeField]
    private float currentHP;

    [SerializeField]
    private Slider hpSlider;

    public UnityAction DeathEvent;

    private void Start()
    {
        if (hpSlider == null)
            hpSlider = FindObjectOfType<Slider>(true);

        if (!hpSlider.gameObject.activeSelf)
            hpSlider.gameObject.SetActive(true);

        hpSlider.maxValue = maxHP;
        hpSlider.minValue = 0f;

        hpSlider.value = maxHP;
    }

    public void Damage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0f)
        {
            Die();
        }

        UpdateVisuals();
    }

    private void Die()
    {
        DeathEvent();
        hpSlider.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void UpdateVisuals()
    {
        hpSlider.value = currentHP;
    }
}
