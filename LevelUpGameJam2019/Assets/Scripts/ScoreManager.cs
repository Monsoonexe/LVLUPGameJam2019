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
        scoreTMPro.text = playerScore.ToString();
    }

    public void OnIncorrectOrder()
    {
        ++wrongOrders;
    }

    public void OnSharkAtePizza()
    {
        ++sharksFed;
    }

}
