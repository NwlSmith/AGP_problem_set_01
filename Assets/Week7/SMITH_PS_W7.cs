﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SMITH_PS_W7 : MonoBehaviour
{
    /*
     * Below are a series of problems to solve with recursion.  You may need to make additional functions to do so.
     * Do not solve these problems with loops.
     */

    // Return the reversed version of the input.
    public string ReverseString(string toReverse) => (toReverse.Length == 1) ? toReverse : toReverse[toReverse.Length - 1] + ReverseString(toReverse.Substring(0, toReverse.Length - 1));

    // Return whether or not the string is a palindrome
    public bool IsPalindrome(string toCheck) => toCheck.Length <= 1 ? true : toCheck[0].Equals(toCheck[toCheck.Length - 1]) && IsPalindrome(toCheck.Substring(1, toCheck.Length - 2));


    private class CharCache
    {

        Dictionary<string, string[]> charactersToStringCache = new Dictionary<string, string[]>();

        private string CharToString(params char[] characters)
        {
            string key = "";
            foreach (char c in characters)
            {
                key += c;
            }
            return key;
        }

        public bool Exists(params char[] characters)
        {

            string key = CharToString(characters);

            Debug.Log($"Does cache contain {key}? {charactersToStringCache.ContainsKey(key)}");

            return charactersToStringCache.ContainsKey(key);
        }

        public string[] Get(char[] characters)
        {
            string key = CharToString(characters);

            string[] str = charactersToStringCache[key];

            string toPrintS = "";

            foreach (string s in str)
            {
                toPrintS += s + ", ";
            }

            Debug.Log($"Getting cache[{key}]. returning {toPrintS}");

            return charactersToStringCache[key];
        }

        public void Set(char[] characters, string[] strings)
        {
            string key = CharToString(characters);

            string toPrintS = "";

            foreach (string s in strings)
            {
                toPrintS += s + ", ";
            }

            Debug.Log($"Setting cache[{key}] to {toPrintS}");


            charactersToStringCache.Add(key, strings);
        }

    }

    private CharCache charCache = new CharCache();

    // Return all strings that can be made from the set characters using all characters.
    public string[] AllStringsFromCharacters(params char[] characters)
    {
        string toPrint = "";

        foreach (char c in characters)
        {
            toPrint += c;
        }
        Debug.Log($"Called AllStringsFromCharacters(characters): {toPrint}");


        if (characters.Length == 0)
            return new string[0];

        if (characters.Length == 1)
        {
            Debug.Log($"characters.Length == 1, returning {characters[0].ToString()}");
            string[] ret = new string[1];
            ret[0] = characters[0].ToString();
            return ret;
        }

        if (charCache.Exists(characters))
            return charCache.Get(characters);
        

        List<string> toReturnList = new List<string>();
        
        foreach (char c in characters)
        {
            // call allstringsfromchars on char array minus c, add c to every element from the returned allstringsfromchars

            Debug.Log($"Looking at c = {c}");

            char[] charsMinusC = new char[characters.Length - 1];

            int curCharacterIndex = 0;
            int curCharsMinusCIndex = 0;
            while (curCharacterIndex < characters.Length)
            {
                if (characters[curCharacterIndex].Equals(c))
                {
                    curCharacterIndex++;
                }
                else
                {
                    charsMinusC[curCharsMinusCIndex] = characters[curCharacterIndex];
                    curCharacterIndex++;
                    curCharsMinusCIndex++;
                }
            }

            toPrint = "";
            foreach (char c2 in charsMinusC)
            {
                toPrint += c2;
            }

            Debug.Log($"charsMinusC = {toPrint}");

            string[] allStrings = AllStringsFromCharacters(charsMinusC);

            foreach (string str in allStrings)
            {
                toReturnList.Add(c + str);
                Debug.Log($"added c + str = {c + str}");
            }
        }

        // SUUUUUPER inefficient.

        charCache.Set(characters, toReturnList.ToArray());

        return toReturnList.ToArray();
    }
    
    // Return the sum of all the numbers given.

    public int SumOfAllNumbers(params int[] numbers)
    {
        return 0;
    }
    
    /*
     * Solve the following problem with recursion:
     *
     * A new soda company is doing a promotion - they'll buy back cans.  But they're not sure how much to charge per can,
     * or how much to pay out for cans.  Write a function that can determine how many cans someone can purchase for
     * a given amount of money, assuming they always return all the cans and then buy as much soda as they can.
     */

    public int TotalCansPurchasable(float money, float price, float canRefund)
    {
        return 0;
    }
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI recursionTest, sodaTest;
    
    private void Update()
    {
        recursionTest.text =  "Recursion Problems\n<align=left>\n";
        recursionTest.text += Success(ReverseString("TEST") == "TSET") + " string reverser worked correctly.\n";
        recursionTest.text += Success(!IsPalindrome("TEST") && IsPalindrome("ASDFDSA") && IsPalindrome("ASDFFDSA")) + " palindrome test worked correctly.\n";



        Debug.Log($"NEW CALL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        string[] strs = AllStringsFromCharacters('A', 'B', 'C', 'D', 'E');
        string toPrint = "";
        foreach(string str in strs)
        {
            toPrint += str + ", ";
            Debug.Log(str);
        }
        Debug.Log($"In the end, all strings = {toPrint}");
        charCache = new CharCache();
        //recursionTest.text += Success(AllStringsFromCharacters('A', 'B').Length == 2 && AllStringsFromCharacters('A').Length == 1 && AllStringsFromCharacters('A', 'B', 'C').Length == 6 && AllStringsFromCharacters('A', 'B', 'C', 'D', 'E', 'F', 'G').Length == 5040) + " all strings test worked correctly.\n";
        recursionTest.text += Success(SumOfAllNumbers(1, 2, 3, 4, 5) == 15 && SumOfAllNumbers(1, 2, 3, 4, 5, 6, 7) == 28) + " sum test worked correctly.\n";

        sodaTest.text = "Soda Problem\n<align=left>\n";
        
        sodaTest.text += Success(TotalCansPurchasable(4, 2, 1) == 3) + " soda test works correctly w/out change.\n";
        sodaTest.text += Success(TotalCansPurchasable(5, 2, 1) == 4) + " soda test works correctly w/change.\n";
    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}