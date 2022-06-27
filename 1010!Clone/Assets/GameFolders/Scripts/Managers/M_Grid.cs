using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Grid : MonoBehaviour
{
    public GridItem GridItemPrefab;
    public int GridWidth = 10;
    public int GridHeight = 10;
    public GridItem[,] GridArray;

    private void Awake()
    {
        II = this;
    }


    private void OnEnable()
    {
        M_Observer.OnGameCreate += GameCreate;
    }
    private void OnDisable()
    {
        M_Observer.OnGameCreate -= GameCreate;
    }

    private void GameCreate()
    {
        GridCreate();
    }

    private void GridCreate()
    {
        GridArray = new GridItem[GridWidth, GridHeight];
        for (int i = 0; i < GridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                GridItem _gridItem = Instantiate(GridItemPrefab, transform);
                _gridItem.transform.position = new Vector3(i, j, 0);
                _gridItem.IndexI = i;
                _gridItem.IndexJ = j;
                GridArray[i, j] = _gridItem;
            }
        }
    }


    public static M_Grid II;

    public static M_Grid I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Grid");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Grid>();
                }
            }

            return II;
        }
    }
}
