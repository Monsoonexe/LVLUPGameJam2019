using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return)) //checks for if the next key is pressed
            {
                Debug.Log("Next Sentence Please");
                if (index < sentences.Length - 1)
                {
                    index++;
                    textDisplay.text = "";
                    StartCoroutine(Type());
                }
                else
                {
                    textDisplay.text = "";
                }
            }
        }
    }
}
