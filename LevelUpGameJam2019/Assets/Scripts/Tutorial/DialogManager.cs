using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public GameObject tutPopupscript;

    public TextMeshProUGUI textDisplay;

    public float typingSpeed;

    private void Start()
    {
        StartCoroutine(Type());
    }

    private void Update()
    {
        NextSentence();
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void NextSentence()
    {
        if (textDisplay.text == sentences[index]) //Makes it so player can't spam through the text and break it.
        {
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());
            }
            else
            {
                tutPopupscript.SetActive(true);
            }
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)) //checks for if the next key is pressed
            //{
            //    if (index < sentences.Length - 1)
            //    {
            //        index++;
            //        textDisplay.text = "";
            //        StartCoroutine(TypeLvl1Tut1());
            //    }
            //    else
            //    {
            //        tutPopupscript.SetActive(true);
            //        //textDisplay.text = "";
            //    }
            //}
        }
    }
}
