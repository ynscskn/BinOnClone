using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class M_Grid : MonoBehaviour
{
    public GridItem GridItemPrefab;
    public int GridWidth = 10;
    public int GridHeight = 10;
    public GridItem[,] GridArray;
    public SpawnPoint[] SpawnPoints;

    public List<GridItem> succeedGridItemList;
    public List<GridItem> deleteGridItemList;

    [HideInInspector] public Piece CurrentPiece;
    [HideInInspector] public int SpawnPointsCount;

    int placedPieceCount = 0;
    Vector3 pickAndMoveOffset = new Vector3(0, 2, -1);


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
    }

    public void MoveObject(Vector2 screenPos)
    {
        if (CurrentPiece == null) return;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float t = -ray.origin.z / ray.direction.z;

        TweenControl(CurrentPiece.PieceMoveTween);
        CurrentPiece.transform.position = ray.GetPoint(t) + pickAndMoveOffset;
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

    public void TurnSpawnPoint()
    {
        if (CurrentPiece == null) return;

        TweenControl(CurrentPiece.PieceMoveTween);
        Camera.main.DOShakeRotation(0.1f, 1, 2, 5);//test
        CurrentPiece.PieceMoveTween = CurrentPiece.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(Ease.OutSine);
        CurrentPiece.transform.DOScale(Vector3.one * 0.75f, 0.2f).SetEase(Ease.OutExpo);
        CurrentPiece = null;
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

        piece.CurrentSpawnPoint.Empty = true;

        placedPieceCount++;
        if (placedPieceCount == SpawnPointsCount) { M_Observer.OnPieceSpawn?.Invoke(); placedPieceCount = 0; }

        for (int i = 0; i < piece.PieceChilds.Length; i++)
        {
            int posX = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.x + 0.5f);
            int posY = Mathf.FloorToInt(piece.PieceChilds[i].transform.position.y + 0.5f);
            GridItem _gridItem = GridArray[posX, posY];
            _gridItem.IsFull = true;
            succeedGridItemList.Add(_gridItem);
            piece.PieceChilds[i].transform.SetParent(_gridItem.transform);
            piece.PieceChilds[i].transform.localPosition = Vector3.zero;
            _gridItem.CurrentPieceChild = piece.PieceChilds[i];

            M_Score.I.Score++;
        }
        M_Score.I.SetScore();
        Destroy(piece.gameObject);

        return true;

    }

    bool GridArrayControl(int posX, int posY)
    {
        if (posX >= 0 && posX < GridWidth && posY >= 0 && posY < GridHeight) return true;
        else return false;
    }

    int totalSucceedCount, succeedCounter, _succeedScore;
    IEnumerator SucceedControl()
    {
        deleteGridItemList.Clear();
        succeedCounter = 0; totalSucceedCount = 0; _succeedScore = 0;

        for (int i = 0; i < succeedGridItemList.Count; i++)
        {
            int _indexI = succeedGridItemList[i].IndexI;
            int _indexJ = succeedGridItemList[i].IndexJ;

            for (int a = 0; a < GridWidth; a++)
            {
                if (!GridArray[a, _indexJ].IsFull) break;
                else succeedCounter++;
            }

            if (succeedCounter == GridWidth)
            {
                succeedCounter = 0;
                for (int a = 0; a < GridWidth; a++)
                {
                    if (!GridArray[a, _indexJ].AddDeleteList)
                    {
                        deleteGridItemList.Add(GridArray[a, _indexJ]);
                        GridArray[a, _indexJ].AddDeleteList = true;
                    }
                }
            }
            else succeedCounter = 0;



            for (int a = 0; a < GridHeight; a++)
            {
                if (!GridArray[_indexI, a].IsFull) break;
                else succeedCounter++;
            }

            if (succeedCounter == GridHeight)
            {
                succeedCounter = 0;
                for (int a = 0; a < GridHeight; a++)
                {
                    if (!GridArray[_indexI, a].AddDeleteList)
                    {
                        deleteGridItemList.Add(GridArray[_indexI, a]);
                        GridArray[_indexI, a].AddDeleteList = true;
                    }
                }
            }
            else succeedCounter = 0;

        }

        if (deleteGridItemList.Count > 0)
        {
            deleteGridItemList = deleteGridItemList.OrderBy(qq => Vector3.Distance(qq.transform.position, CurrentPiece.transform.position)).ToList();

            totalSucceedCount = deleteGridItemList.Count / 10;

            for (int i = 1; i <= totalSucceedCount; i++)
            {
                _succeedScore += i * GridHeight;
            }

            M_Score.I.Score += _succeedScore;
            M_Score.I.SetScore();

            for (int i = 0; i < deleteGridItemList.Count; i++)
            {
                GridItem _gridItem = deleteGridItemList[i];
                if (_gridItem != null)
                {
                    Destroy(_gridItem.CurrentPieceChild.gameObject, 1f);
                    _gridItem.CurrentPieceChild.gameObject.SetActive(false);
                    _gridItem.IsFull = false;
                    _gridItem.AddDeleteList = false;
                    _gridItem.CurrentPieceChild = null;

                    yield return new WaitForSeconds(2 / deleteGridItemList.Count);

                }
            }
        }
        yield return new WaitForSeconds(1);
        GameOverControl();

    }
    /// <summary>
    /// ////////////////////
    /// </summary>
    void GameOverControl()
    {
        bool cýk = false;
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (!SpawnPoints[i].Empty)
            {
                if (cýk) break;
                Piece _piece = SpawnPoints[i].CurrentPiece;

                for (int y = 0; y < GridHeight; y++)
                {
                    if (cýk) break;
                    for (int x = 0; x < GridWidth; x++)
                    {
                        if (cýk) break;
                        if (pieceCntrl(_piece, x, y)) cýk = true;
                    }
                }
            }
        }

        if (!cýk) M_Observer.OnGameFail?.Invoke();
    }
    bool pieceCntrl(Piece piece, int x, int y)
    {
        for (int i = 0; i < piece.PieceChildsPos.Count; i++)
        {
            int posX = Mathf.FloorToInt(x + 0.5f + piece.PieceChildsPos[i].x);
            int posY = Mathf.FloorToInt(y + 0.5f + piece.PieceChildsPos[i].y);
            if (!GridArrayControl(posX, posY) || GridArray[posX, posY].IsFull) return false;
        }
        return true;
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
