using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;

public class M_Camera : MonoBehaviour
{
    public List<CameraPreset> CameraPresets;
    public int selectedCameraPresetIndex;
    public GameObject TargetContainer;
    public Camera MainCamera;
    CameraMultiTarget cameraMultiTarget;
    GameObject[] CurrentTargets;
    public float CameraYawSpeed;
    bool gameStart;
    bool IsFollow;

    public Transform RocketManTransform;
    private void Awake()
    {
        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameComplete += GameComplete;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGameReady += GameReady;

        CurrentTargets = new GameObject[TargetContainer.transform.childCount];
        GetTargets();
        SetCameraMT();
    }


    private void OnDestroy()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameComplete -= GameComplete;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGameReady -= GameReady;
    }

    private void GameReady()
    {

    }

    private void GameStart()
    {

    }

    private void GameFail()
    {

    }

    private void GameComplete()
    {

    }

    private void GameCreate()
    {

    }

    void Start()
    {

    }

    public void ChancePreset(int _selectedCameraPresetIndex)
    {
        selectedCameraPresetIndex = _selectedCameraPresetIndex;
    }

    float velA;
    float velB;
    float velC;
    float velD;
    float velE;
    float velF;
    float velG;
    bool qwe;
    void Update()
    {
        if (gameStart)
        {
            cameraMultiTarget.Pitch = Mathf.SmoothDamp(cameraMultiTarget.Pitch,
        CameraPresets[selectedCameraPresetIndex].Pitch, ref velA,
        CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.Yaw = Mathf.SmoothDamp(cameraMultiTarget.Yaw, CameraPresets[selectedCameraPresetIndex].Yaw,
                ref velB, CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.Roll = Mathf.SmoothDamp(cameraMultiTarget.Roll, CameraPresets[selectedCameraPresetIndex].Roll,
                ref velC, CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.PaddingLeft = Mathf.SmoothDamp(cameraMultiTarget.PaddingLeft,
                CameraPresets[selectedCameraPresetIndex].PaddingLeft, ref velD,
                CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.PaddingRight = Mathf.SmoothDamp(cameraMultiTarget.PaddingRight,
                CameraPresets[selectedCameraPresetIndex].PaddingRight, ref velE,
                CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.PaddingUp = Mathf.SmoothDamp(cameraMultiTarget.PaddingUp,
                CameraPresets[selectedCameraPresetIndex].PaddingUp, ref velF,
                CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);
            cameraMultiTarget.PaddingDown = Mathf.SmoothDamp(cameraMultiTarget.PaddingDown,
                CameraPresets[selectedCameraPresetIndex].PaddingDown, ref velG,
                CameraPresets[selectedCameraPresetIndex].MoveSmoothTime);

            if (IsFollow)
            {
                if (RocketManTransform.position.y < 15)
                {
                    TargetContainer.transform.position = RocketManTransform.position;
                }
                else
                {
                    TargetContainer.transform.position = new Vector3(RocketManTransform.position.x, 15, RocketManTransform.position.z);
                }

            }

        }
        float CamaraHight = 50f;//40
        if (qwe)
        {
            if (RocketManTransform.position.y > 5)
            {
                cameraMultiTarget.PaddingLeft = Mathf.SmoothDamp(cameraMultiTarget.PaddingLeft, Mathf.Clamp(RocketManTransform.position.y, -CamaraHight, CamaraHight), ref velD, 0.5f);
                cameraMultiTarget.PaddingRight = Mathf.SmoothDamp(cameraMultiTarget.PaddingRight, Mathf.Clamp(RocketManTransform.position.y, -CamaraHight, CamaraHight), ref velD, 0.5f);
                cameraMultiTarget.PaddingUp = Mathf.SmoothDamp(cameraMultiTarget.PaddingUp, Mathf.Clamp(RocketManTransform.position.y, -CamaraHight, CamaraHight), ref velD, 0.5f);
            }
            else
            {
                ChancePreset(1);
            }
        }
    }

    void SetCameraMT()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
            cameraMultiTarget = MainCamera.GetComponent<CameraMultiTarget>();
        }
        else
        {
            cameraMultiTarget = MainCamera.GetComponent<CameraMultiTarget>();
        }
    }

    void GetTargets()
    {
        for (int i = 0; i < TargetContainer.transform.childCount; i++)
        {
            CurrentTargets[i] = TargetContainer.transform.GetChild(i).gameObject;
        }
    }

    void CameraTargetsPos()
    {
        float _x = 0;
        float _y = 0;
        float _z = 0;
        int _counter = 0;
        for (int i = 0; i < CurrentTargets.Length; i++)
        {
            CurrentTargets[i].transform.localPosition = new Vector3(_x, _y, _z);
            //_x += M_Slot.I.ColumnCount;
            _counter++;
            //if (_counter == 2)
            //{
            //    _x = 0;
            //    _y += M_Slot.I.RowCount;
            //}
        }
        cameraMultiTarget.SetTargets(CurrentTargets);
    }
}

[Serializable]
public class CameraPreset
{
    public string Name;
    public float Pitch;
    public float Yaw;
    public float Roll;
    public float PaddingLeft;
    public float PaddingRight;
    public float PaddingUp;
    public float PaddingDown;
    public float MoveSmoothTime = 0.19f;
}