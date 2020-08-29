using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAssistant : MonoBehaviour
{
    public TMP_Text message1;
    public TMP_Text message2;

    private string text1;
    private string text2;

    // Start is called before the first frame update
    void Awake()
    {
        message1.SetText("");
        message2.SetText("");
    }

    public void setText1(string text1)
    {
        this.text1 = text1;
    }

    public void setText2(string text2)
    {
        this.text2 = text2;
    }

    public void SetText()
    {
        TextWriter.AddWriter_Static(message1, text1, .1f, true);
        StartCoroutine(NextText());
    }

    IEnumerator NextText()
    {
        yield return new WaitForSeconds(2);
        TextWriter.AddWriter_Static(message2, text2, .1f, true);
    }
}
