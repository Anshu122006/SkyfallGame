using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreValue;

    private void Update()
    {
        scoreValue.text = PlayerStats.Instance.GetScore().ToString();
    }
}
