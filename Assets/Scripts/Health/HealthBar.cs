using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Heallth playerHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthbar;

    private void Start()
    {
       totalHealthbar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update()
    {
        currentHealthbar.fillAmount = playerHealth.currentHealth / 10;
    }
}
