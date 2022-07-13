using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{

    public PieceChild[] PieceChilds;

    public Tween PieceMoveTween;

    public List<Vector2> PieceChildsPos;

    public SpawnPoint CurrentSpawnPoint;
}
