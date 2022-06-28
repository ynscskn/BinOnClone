using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void OnEnable()
    {
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
    }

    void OnDisable()
    {
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
    }

    private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex != 0) return;
        M_Grid.II.PickObject(fingerPos);
    }

    private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex != 0) return;
        M_Grid.II.MoveObject(fingerPos);

    }

    private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        if (fingerIndex != 0) return;
        M_Grid.II.TurnSpawnPoint();
    }
}
