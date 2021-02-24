using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuSolver : MonoBehaviour
{

    [SerializeField] private TextAsset file;

    [SerializeField] private GameObject _squarePrefab;

    private Square[,] board = new Square[9, 9];

    void Start()
    {
        ConstructBoard();

        PopulateBoard();

        StartCoroutine(SolverEnum());
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
                board[y, x] = square;
            }
        }
    }

    private void PopulateBoard()
    {
        char[] inputFile = file.text.ToCharArray();

        for (int i = 0; i < inputFile.Length; i++)
        {
            if (!inputFile[i].Equals('.'))
                 board[i / 9, i % 9].SetNum(inputFile[i] - 48);
        }
    }

    // I wanted to make this a coroutine so people could see the calculations in progress.
    private IEnumerator SolverEnum()
    {
        yield return null;
    }
}
