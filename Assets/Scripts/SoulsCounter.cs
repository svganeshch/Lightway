using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsCounter : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    int currentScore = 0;

    public void SetScore()
    {
        currentScore += 1;
    }

    public void PrepareHud(int particleLen)
    {
        scoreText.text = currentScore.ToString() + " / " + particleLen.ToString();
    }

    public void ResetScore()
    {
        currentScore = 0;
    }
}
