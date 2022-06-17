using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LayoutPicker : MonoBehaviour {
    public TMP_Text LayoutNumber;
    public Slider IndicatorSlider;
    public TMP_Text IndicatorVal;
    public Slider ContractionSlider;
    public TMP_Text ContractVal;
    public GameObject[] Layouts;

    private int _currentLayout = 0;

    public void Start() {
        IndicatorSlider.onValueChanged.AddListener(x => {
            MenuButtonTween.IndicateExpansion = x;
            IndicatorVal.text = $"Indicator Expansion: '{x}'";
        });
        ContractionSlider.onValueChanged.AddListener(x => {
            MenuButtonTween.ClickedContraction = x;
            ContractVal.text = $"Click Contraction: '{x}'";
        });

        IndicatorSlider.value = MenuButtonTween.IndicateExpansion;
        ContractionSlider.value = MenuButtonTween.ClickedContraction;
    }

    public void NextLayout() {
        ShowLayout((_currentLayout + 1) % Layouts.Length);
    }
    public void PreviousLayout() {
        ShowLayout(_currentLayout == 0 ? Layouts.Length - 1 : _currentLayout - 1);
    }

    public void ShowLayout(int a) {
        if (Layouts.Length == 0) {
            _currentLayout = -1;
        }

        for (int i = 0; i < Layouts.Length; i++) {
            if (a == i) {
                Layouts[i].SetActive(true);
            } else {
                Layouts[i].SetActive(false);
            }
        }
        _currentLayout = a;
        LayoutNumber.text = Layouts[_currentLayout].name;
    }
}
