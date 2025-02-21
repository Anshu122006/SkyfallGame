using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image health;
    [SerializeField] private TextMeshProUGUI currHP;
    [SerializeField] private TextMeshProUGUI maxHP;

    private void Update()
    {
        float currHealth = PlayerStats.Instance.GetHP();
        float maxHealth = PlayerStats.Instance.GetMHP();
        currHP.text = currHealth.ToString();
        maxHP.text = maxHealth.ToString();
        health.fillAmount = currHealth / maxHealth;
    }
}
