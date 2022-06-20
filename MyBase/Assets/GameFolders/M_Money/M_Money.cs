using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class M_Money : MonoBehaviour
{
    public Canvas MoneyCanvas;
    public GameObject GoldImageCont;
    public RectTransform MoneyCont;
    public TextMeshProUGUI MoneyTMP;
    public TextMeshProUGUI ScoreText;
    public RectTransform ScoreCont;
    public Money MoneyPrefab;
    public RectTransform MoneyTransform;

    [HideInInspector] public int MyMoney;

    private void Awake()
    {
        II = this;

        M_Observer.OnGameReady += GameReady;

        M_Observer.OnGameStart += GameStart;
    }

    private void OnDestroy()
    {
        M_Observer.OnGameReady -= GameReady;

        M_Observer.OnGameStart -= GameStart;
    }


    private void GameStart()
    {
        //MoneyCont.gameObject.SetActive(true);
    }

    private void GameReady()
    {
        // ScoreCont.gameObject.SetActive(true);
        //ScoreCont.DOAnchorPos3DY(-320, 0.25f).SetEase(Ease.OutExpo);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("My Money"))
        {
            MyMoney = PlayerPrefs.GetInt("My Money");
        }
        else
        {
            MyMoney = 1000;
        }
    }

    private void Update()
    {
        if (MoneyTMP.text != MyMoney.ToString())
        {
            MoneyTMP.text = MyMoney.ToString();
            PlayerPrefs.SetInt("My Money", MyMoney);//**
        }
    }

    public static M_Money II;

    public static M_Money I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Money");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Money>();
                }
            }

            return II;
        }
    }
}
