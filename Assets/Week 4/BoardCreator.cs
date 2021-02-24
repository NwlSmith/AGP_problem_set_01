using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{

    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private GameObject _canvas;

    

    // Start is called before the first frame update
    void Start()
    {
        float topRightX = -375f;
        float topRightY = 375f;
        float spacing = 85f;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                GameObject square = Instantiate(_squarePrefab) as GameObject;
                square.transform.SetParent(transform, false);
                square.GetComponent<RectTransform>().localPosition = new Vector3(topRightX + spacing*x, topRightY - spacing*y);

            }
        }
    }
}
