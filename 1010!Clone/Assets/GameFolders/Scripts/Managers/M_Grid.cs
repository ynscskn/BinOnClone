using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class M_Grid : MonoBehaviour
{
    public GridItem GridItemPrefab;
    public int GridWidth = 10;
    public int GridHeight = 10;
    public GridItem[,] GridArray;

    List<GridItem> succeedGridItemList;
    List<GridItem> deleteGridItemList;

    [HideInInspector] public Piece CurrentPiece;
    [HideInInspector] public int SpawnPointsCount;
    int placedPieceCount = 0;
    //[HideInInspector] float pickAndMoveOffset = 2f;
    [HideInInspector] Vector3 pickAndMoveOffset = new Vector3(0, 2, 0);


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
        succeedGridItemList = new List<GridItem>();
        deleteGridItemList = new List<GridItem>();

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
                CurrentPiece.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo);
                CurrentPiece.PieceMoveTween = CurrentPiece.transform.DOLocalMove(CurrentPiece.transform.localPosition + pickAndMoveOffset, 0.2f).SetEase(Ease.OutExpo).OnComplete(() => CurrentPiece.PieceMoveTween = null);
            }
        }
        else print("null");
    }

    public void MoveObject(Vector2 screenPos)
    {
        if (CurrentPiece == null) return;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float t = -ray.origin.z / ray.direction.z;

        TweenControl(CurrentPiece.PieceMoveTween);
        CurrentPiece.transform.position = ray.GetPoint(t) + pickAndMoveOffset;
    }

    public void TurnSpawnPoint()
    {
        if (CurrentPiece == null) return;

        TweenControl(CurrentPiece.PieceMoveTween);
        CurrentPiece.PieceMoveTween = CurrentPiece.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutSine);
        CurrentPiece.transform.DOScale(Vector3.one * 0.75f, 0.2f).SetEase(Ease.OutExpo);
        CurrentPiece = null;
    }

    void TweenControl(Tween tween)
    {
        if (tween != null) tween.Kill();
    }

    public void GridControl()
    {
        if (CurrentPiece == null) return;

        if (PieceChildControl(CurrentPiece))
        {
            StartCoroutine(SucceedControl());
        }
        else
        {
            TurnSpawnPoint();
        }
    }

    bool PieceChildControl(Piece piece)
    {
        for (int i = 0; i < piece.PieceChilds.Length; i++)
        {
            int posX = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.x + 0.5f);
            int posY = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.y + 0.5f);

            if (!GridArrayControl(posX, posY) || GridArray[posX, posY].IsFull) return false;
        }
        succeedGridItemList.Clear();
        deleteGridItemList.Clear();

        placedPieceCount++;
        if (placedPieceCount == SpawnPointsCount) { M_Observer.OnPieceSpawn?.Invoke(); placedPieceCount = 0; }

        for (int i = 0; i < piece.PieceChilds.Length; i++)
        {
            int posX = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.x + 0.5f);
            int posY = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.y + 0.5f);

            GridArray[posX, posY].IsFull = true;
            succeedGridItemList.Add(GridArray[posX, posY]);
            piece.PieceChilds[i].transform.SetParent(GridArray[posX, posY].transform);
            piece.PieceChilds[i].transform.localPosition = Vector3.zero;
            GridArray[posX, posY].CurrentPieceChild = piece.PieceChilds[i];
        }
        Destroy(piece.gameObject);

        return true;

    }

    bool GridArrayControl(int posX, int posY)
    {
        if (posX >= 0 && posX < GridWidth && posY >= 0 && posY < GridHeight) return true;
        else return false;
    }

    IEnumerator SucceedControl()
    {
        print("00");

        deleteGridItemList.Clear();
        int succeedCounter = 0;

        for (int i = 0; i < succeedGridItemList.Count; i++)
        {
            print("1");

            int _indexI = succeedGridItemList[i].IndexI;
            int _indexJ = succeedGridItemList[i].IndexJ;

            for (int a = 0; a < GridWidth; a++)
            {
                print("2");

                if (!GridArray[a, _indexJ].IsFull) break;

                else succeedCounter++;
            }
            print("3");

            if (succeedCounter == GridWidth)
            {
                print("4");

                succeedCounter = 0;
                for (int a = 0; a < GridWidth; a++)
                {
                    print("5");

                    if (!GridArray[a, _indexJ].AddDeleteList)
                    {
                        print("6");

                        deleteGridItemList.Add(GridArray[a, _indexJ]);
                        GridArray[a, _indexJ].AddDeleteList = true;
                    }
                }
            }
            else succeedCounter = 0;
            print("7");

            for (int a = 0; a < GridHeight; a++)
            {
                print("8");

                if (!GridArray[_indexI, a].IsFull) break;

                else succeedCounter++;
            }
            print("9");

            if (succeedCounter == GridHeight)
            {
                print("10");

                succeedCounter = 0;
                for (int a = 0; a < GridHeight; a++)
                {
                    print("11");

                    if (!GridArray[_indexI, a].AddDeleteList)
                    {
                        print("12");

                        deleteGridItemList.Add(GridArray[_indexI, a]);
                        GridArray[_indexI, a].AddDeleteList = true;
                    }
                }
            }
            else succeedCounter = 0; print("13");

        }

        if (deleteGridItemList.Count > 0)
        {
            print("14");

            deleteGridItemList = deleteGridItemList.OrderBy(qq => Vector3.Distance(qq.transform.position, CurrentPiece.transform.position)).ToList();

            for (int i = 0; i < deleteGridItemList.Count; i++)
            {
                print("15");

                GridItem _gridItem = deleteGridItemList[i];
                if (_gridItem != null)
                {
                    print("16");

                    //Destroy(_gridItem.CurrentPieceChild.gameObject);
                    _gridItem.CurrentPieceChild.gameObject.SetActive(false);
                    _gridItem.IsFull = false;
                    _gridItem.AddDeleteList = false;
                    _gridItem.CurrentPieceChild = null;

                    yield return new WaitForSeconds(2 / deleteGridItemList.Count); print("17");

                }
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
