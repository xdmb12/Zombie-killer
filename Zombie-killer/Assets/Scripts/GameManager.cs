using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector ]public int score;
    [SerializeField] private TMP_Text scoreText;


    private void Update()
    {
        ShowText();
    }

    private void ShowText()
    {
        scoreText.text = $"Kills: {score}";
    }
}
