using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_TextFeedback : MonoBehaviour
{
    public TextFeedbackData[] ComboTexts;

    private void Awake()
    {
        M_Observer.OnComboEvent += GetCombo;
    }
    private void OnDestroy()
    {
        M_Observer.OnComboEvent -= GetCombo;

    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GetCombo(int comboNumber)
    {
        GameObject _a = ComboTexts[comboNumber].TextFeedback[Random.Range(0, 5)];
        GameObject _g = Instantiate(_a.gameObject, transform);
        Destroy(_g, 1f);
    }

}
