using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;

    private void Update()
    {
        StartTut();
    }

    private void StartTut()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[i].SetActive(true);
            } else
            {
                popUps[i].SetActive(false);
            }
        }
        if (popUpIndex == 0)
        {
            Debug.Log("The Tut has started");
            //Have the Random Rabbit slide out and welcome you to the game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                popUpIndex++;
            }
        } else if (popUpIndex == 1)
        {
            Debug.Log("Second Part of Tut");
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                popUpIndex++;
            }
        } else if (popUpIndex == 2)
        {
            Debug.Log("Third Part of Tut");
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                popUpIndex++;
            }
        }
    }

}
