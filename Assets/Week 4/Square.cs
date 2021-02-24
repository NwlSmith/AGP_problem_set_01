using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Square : MonoBehaviour
{
    public int num { get; private set; }

    private TMP_Text _text;

    private void Awake()
    {
        num = 0;
        _text = GetComponentInChildren<TMP_Text>();
        _text.text = "";
    }

    public void SetNum(int newNum)
    {
        num = newNum;
        _text.text = num.ToString();
    }
}
