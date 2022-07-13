using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public Piece_Data Piece_Data;
    public Piece_Color Piece_Color;
    public SpawnPoint[] SpawnPoints;

    private void OnEnable()
    {
        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnPieceSpawn += PieceSpawn;

    }
    private void OnDisable()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnPieceSpawn -= PieceSpawn;

    }

    private void GameCreate()
    {

    }

    private void GameStart()
    {
        M_Grid.II.SpawnPointsCount = SpawnPoints.Length;

        PieceSpawn();
    }

    private void PieceSpawn()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            int RandomPieceNumber = Random.Range(0, Piece_Data.Piece.Length);
            Piece _piece = Instantiate(Piece_Data.Piece[RandomPieceNumber], SpawnPoints[i].transform);
            SpawnPoints[i].CurrentPiece = _piece;
            SpawnPoints[i].Empty = false;
            _piece.CurrentSpawnPoint = SpawnPoints[i];
            _piece.transform.localPosition = Vector3.zero;
            _piece.transform.localScale = Vector3.one * 0.75f;

            int RandomMaterialNumber = Random.Range(0, Piece_Color.Material.Length);
            for (int j = 0; j < _piece.PieceChilds.Length; j++)
            {
                PieceChild _pieceChild = _piece.PieceChilds[j];
                _pieceChild.GetComponent<MeshRenderer>().sharedMaterial = Piece_Color.Material[RandomMaterialNumber];
            }
        }
    }

}
