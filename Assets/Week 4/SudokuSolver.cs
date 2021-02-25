using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuSolver : MonoBehaviour
{

    [SerializeField] private TextAsset file;

    [SerializeField] private GameObject _squarePrefab;

    private Square[] board = new Square[81];

    private bool initFinished = false;
    private float timeAtStartOfAlgorithm = 0f;

    void Start()
    {
        //ConstructBoard();
        StartCoroutine(ConstructBoardEnum());
        //PopulateBoard();
        //StartCoroutine(PopulateBoardEnum());

        //StartCoroutine(Solver1Enum());
        //StartCoroutine(StartTimerEnum());
        //initFinished = SolverRecurse(0);
    }

    private IEnumerator StartTimerEnum()
    {
        while (!initFinished)
        {
            yield return null;
        }
        Debug.Log($"Time after start: {Time.time - timeAtStartOfAlgorithm}");
    }

    private void ConstructBoard()
    {
        float topRightX = -380f;
        float topRightY = 380f;
        float spacing = 85f;

        Canvas canvas = GetComponentInParent<Canvas>();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Square square = Instantiate(_squarePrefab).GetComponent<Square>();
                square.transform.SetParent(transform, false);
                square.GetComponent<RectTransform>().localPosition = new Vector3(topRightX + spacing * x, topRightY - spacing * y);
                board[y * 9 + x] = square;
            }
        }
    }

    private IEnumerator ConstructBoardEnum()
    {
        float topRightX = -380f;
        float topRightY = 380f;
        float spacing = 85f;

        Canvas canvas = GetComponentInParent<Canvas>();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Square square = Instantiate(_squarePrefab).GetComponent<Square>();
                square.transform.SetParent(transform, false);
                square.GetComponent<RectTransform>().localPosition = new Vector3(topRightX + spacing * x, topRightY - spacing * y);
                board[y * 9 + x] = square;
                yield return null;
            }
        }

        StartCoroutine(PopulateBoardEnum());
    }

    private void PopulateBoard()
    {
        char[] inputFile = file.text.ToCharArray();

        for (int i = 0; i < inputFile.Length; i++)
        {
            if (!inputFile[i].Equals('.'))
                board[i].SetNumInit(inputFile[i] - 48);
        }
    }

    private IEnumerator PopulateBoardEnum()
    {
        char[] inputFile = file.text.ToCharArray();

        for (int i = 0; i < inputFile.Length; i++)
        {
            if (!inputFile[i].Equals('.'))
                board[i].SetNumInit(inputFile[i] - 48);
            yield return null;
        }

        timeAtStartOfAlgorithm = Time.time;
        StartCoroutine(StartTimerEnum());
        initFinished = SolverRecurse(0);
    }

    // I wanted to make this a coroutine so people could see the calculations in progress.
    // This algorithm uses a brute-force approach, there are absolutely more efficient algorithms.
    private IEnumerator Solver1Enum()
    {
        // The current index of the board in 1d form.
        //int curIndex = 0;

        // for each square on the board
        for(int i = 0; i < 81; i++)
        {
            // If it was not one of the original numbers...
            if (!board[i].original)
            {

            }
        }

        yield return null;
    }

    private bool Solver1Recurse(int index)
    {

        return false;
    }

    // This must be done recursively, otherwise I would 81 nested for loops
    private bool SolverRecurse(int index)
    {
        if (index >= 81) return true;

        // If this square is an original, just move onto the next.
        if (board[index].original)
            return SolverRecurse(index + 1);

        for(int num = 1; num <= 9; num++)
        {
            board[index].SetNum(num);
            if (ValidPlacement(index))
            {
                if (SolverRecurse(index + 1))
                    return true;
            }
        }
        board[index].ResetNum();

        return false;
    }
    
    private bool ValidPlacement(int curIndex)
    {
        return ValidPlacementLine(curIndex) && ValidPlacementSq(curIndex);
    }

    private bool ValidPlacementLine(int curIndex)
    {
        // From the left of the row to the right.
        // This loop has good locality.
        int left = curIndex - (curIndex % 9);
        int right = left + 9;
        for (int i = left; i < right; i++)
        {
            // If this square is the same as this number, AND this square is not looking at itself, return false;
            if (Conflict(curIndex, i))
            {
                return false;
            }
        }

        // This loop has HORRIBLE locality.
        // From the top of the column to the bottom.
        for (int i = curIndex % 9; i < 81; i += 9)
        {
            // If this square is the same as this number, AND this square is not looking at itself, return false;
            if (Conflict(curIndex, i))
                return false;
        }
        return true;
    }

    private bool ValidPlacementSq(int curIndex)
    {
        int xComponent = ((curIndex % 9) / 3) * 3;
        int yComponent = (curIndex / 27) * 3;

        int upperLeft = yComponent * 9 + xComponent;
        int lowerRight = upperLeft + 18;
        for (int y = upperLeft; y <= lowerRight;  y += 9)
        {
            for (int x = 0; x < 3; x++)
            {
                int i = y + x;

                if (Conflict(curIndex, i))
                    return false;
            }
        }

        return true;
    }

    private bool Conflict(int curIndex, int lookIndex)
    {
        return board[lookIndex].num == board[curIndex].num && lookIndex != curIndex;
    }
}
