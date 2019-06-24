using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("---Score---")]
    [SerializeField]
    private int playerScore;

    [Header("---Tallys---")]
    [SerializeField]
    private int shotsFired;

    [SerializeField]
    private int customersSatisfied;

    [SerializeField]
    private int customersHit;

    [SerializeField]
    private int customersMissed;

    [SerializeField]
    private int wrongOrders;

    [SerializeField]
    private int sharksFed;

    [Header("---UI Elements---")]
    [SerializeField]
    private TextMeshProUGUI scoreTMPro;


    // Start is called before the first frame update
    void Awake()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Update UI visuals with current values.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreTMPro.text = playerScore.ToString();
    }

    public void OnShotFired()
    {
        ++shotsFired;
    }

    public void OnCustomerSatisfied()
    {
        ++customersSatisfied;
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        UpdateScoreText();
    }

    public void OnIncorrectOrder()
    {
        ++wrongOrders;
    }

    public void OnSharkAtePizza()
    {
        ++sharksFed;
    }

    public void OnCustomerHit()
    {
        ++customersHit;
    }

    /// <summary>
    /// Set all scores to 0.
    /// </summary>
    public void ResetScores()
    {
        playerScore = 0;
        shotsFired = 0;
        customersSatisfied = 0;
        customersHit = 0;
        customersMissed = 0;
        wrongOrders = 0;
        sharksFed = 0;
    }

}
