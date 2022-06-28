using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Grid : MonoBehaviour
{
    public GridItem GridItemPrefab;
    public int GridWidth = 10;
    public int GridHeight = 10;
    public GridItem[,] GridArray;

    [HideInInspector] public Piece CurrentPiece;

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

    public void PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("SpawnPoint") && hit.collider.gameObject.transform.childCount != 0)
            {
                CurrentPiece = hit.collider.GetComponentInChildren<Piece>();
            }
        }
        else print("null");
    }

    public void MoveObject(Vector2 screenPos)
    {
        if (CurrentPiece == null) return;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float t = -ray.origin.z / ray.direction.z;

        CurrentPiece.transform.position = ray.GetPoint(t);
    }

    public void TurnSpawnPoint()
    {
        if (CurrentPiece == null) return;

        CurrentPiece.transform.localPosition = Vector3.zero;
        CurrentPiece = null;
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
