using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Square : MonoBehaviour
{
    public int num { get; private set; }
    public bool original { get; private set; }

    private TMP_Text _text;

    private void Awake()
    {
        num = 0;
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = "";
        original = false;
    }

    public void ResetNum()
    {
        num = 0;
        _text.text = "";
    }

    public void SetNum(int newNum)
    {
        num = newNum;
        _text.text = num.ToString();
    }

    public void SetNumInit(int newNum)
    {
        num = newNum;
        _text.text = $"<color=\"red\">{num.ToString()}</color>";
        original = true;
    }
}
