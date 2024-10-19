using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.singleton.score.ToString();
        bestScoreText.text = "Best: " + GameManager.singleton.best;
    }
}
