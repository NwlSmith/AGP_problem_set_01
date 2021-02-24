using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class SMITH_PS_W4 : MonoBehaviour
{
    /*
     * Create a function that helps takes in four bytes (a, b, c, d), and returns an integer that represents those four
     * bytes in order (abcd).
     *
     * For example:
     *     - If you got in (1, 1, 1, 1), you would return 00000001 00000001 00000001 00000001, or 16843009.
     *     - (2, 1, 1, 1)     =>     00000010 00000001 00000001 00000001     =>    33620225 
     *     - (0, 0, 1, 0)     =>     00000000 00000000 00000001 00000000     =>    256
     * 
     */

    private int BytesToInt(byte a, byte b, byte c, byte d)
    {
        return (a << 24) + (b << 16) + (c << 8) + d;
    }

    private int PowerOfTwo(int power)
    {
        return 0;
    }

    /*
     * Define two functions - one that finds the smallest prime factor of a number (SmallestPrimeFactor()), and one that
     * returns the number of digits (NumberOfDigits()).  Then, write an changing function (ChangingFunction) that returns
     * the result of NumberOfDigits, unless the answer is three, and then after will always return the smallest prime factor.
     *
     * Use the function "Initialize()" if you have anything that needs to be reset - treat it like a start function.
     *
     * Assume number of digits always gets a positive number.
     * 
     */

    delegate int MathFunction(int input);
    private MathFunction currentFunction;

    public int SmallestPrimeFactor(int input)
    {

        /* initial approach. It's slow, so let's optimize.
        // Goes through every number from 2 to input. That's O(N)

        for (int i = 2; i <= input; i++)
        {
            if (input % i == 0)
                return i;
        }
        return input;
        */


        /* if we take care of 2 immediately, we can make the loop twice as fast.
        // This is ~ twice as fast, but we can do better.
        // Goes through every OTHER number from 2 to input. That's O(N/2), but because of how Big O notation works, it is still considered O(N).
        // You can think of Big O working like a graph with the input on the X axis and the amount of calculations on the Y.
        // For this graph, the line is 1/2 as steep as it was originally, but it is still linear, and thus it is still O(N).

        if (input % 2 == 0)
            return 2;

        for (int i = 3; i <= input; i += 2)
        {
            if (input % i == 0)
                return i;
        }
        */

        /* Again, this is ~ twice as fast, but we can do better.
        // Goes through every Other number from 2 to half of the input. That's O(N/4), but because of how Big O notation works, it is still considered O(N).
        if (input % 2 == 0)
            return 2;

        int half = input / 2;
        // The least devisor of a number will never be more than half of the number.
        for (int i = 3; i < half; i += 2)
        {
            if (input % i == 0)
                return i;
        }
        */

        // We can cut this down even more, because the smallest prime factor will actually never be more than the square root of a number.
        // Goes through every Other number from 2 to the square root of input. It is now O(sqrt(n)/2), which IS better than O(N).
        if (input % 2 == 0)
            return 2;

        int sqrt = (int)Mathf.Sqrt(input);
        // The least devisor of a number will never be more than half of the number.
        for (int i = 3; i < sqrt; i += 2)
        {
            if (input % i == 0)
                return i;
        }
        
        // The only way to make this more efficient would be to cache Inputs and their return values so you never do the same calculation twice.


        return input;
    }

    // (I think) you could actually extend this to any base, not just base 10.
    // All you would need to do is make the base of the log the wanted base, ie, if you want to know how many hex digits are in a number, just make the base 16.
    public int NumberOfDigits(int input)
    {
        return (int)Mathf.Log10(input) + 1;
    }

    // Imagine this is your "Start()" function
    public void Initialize()
    {
        
    }

    public int ChangingFunction(int input)
    {
        // Will need delegate or something.
        return 0;
    }
    
    
    // =========================== DON'T EDIT BELOW THIS LINE =========================== //

    public TextMeshProUGUI bytesToIntTest, delegateTest;

    private void Update()
    {
        bytesToIntTest.text = "Bytes to Int\n<align=left>\n";
        
        bytesToIntTest.text += Success(BytesToInt(0, 0, 1, 0) == 256) + " Correct for (0, 0, 1, 0).\n";
        bytesToIntTest.text += Success(BytesToInt(0, 42, 1, 0) == 2752768) + " Correct for (0, 42, 1, 0).\n";
        bytesToIntTest.text += Success(BytesToInt(1, 1, 1, 1) == 16843009) + " Correct for (1, 1, 1, 1).\n";
        bytesToIntTest.text += Success(BytesToInt(2, 1, 1, 1) == 33620225) + " Correct for (2, 1, 1, 1).\n";
        
        delegateTest.text = "Delegate Test\n<align=left>\n";

        delegateTest.text +=
            Success(NumberOfDigits(10) == 2 && NumberOfDigits(4431) == 4 && NumberOfDigits(123842) == 6 &&
                    NumberOfDigits(100) == 3) + " Number of digits works.\n";
        delegateTest.text +=
            Success(SmallestPrimeFactor(4) == 2 && SmallestPrimeFactor(17) == 17 && SmallestPrimeFactor(95) == 5) +
            " Smallest prime number is correct.\n";
        
        Initialize();
        delegateTest.text +=
            Success(ChangingFunction(12) == 2 && ChangingFunction(39243) == 5 &&
                    ChangingFunction(2313) == 4 && ChangingFunction(333) == 3 && ChangingFunction(5) == 5) + " Changes when input is 3.\n";

        delegateTest.text += Success(ChangingFunction(9) == 3 && ChangingFunction(4) == 2) + " Doesn't change back when the input is 3 again.";

    }

    private string Success(bool test)
    {
        return test ? "<color=\"green\">PASS</color>" : "<color=\"red\">FAIL</color>";
    }
}