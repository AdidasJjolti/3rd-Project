using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagToggle : MonoBehaviour
{
    Toggle _flagToggle;
    [SerializeField] GameObject _target;

    void Start()
    {
        _flagToggle = GetComponent<Toggle>();
        _flagToggle.onValueChanged.AddListener((isOn) => 
        {
            ChangeColor(isOn);
        });
    }

    // 토글 on, off 상태에 따라 selectedColor 값을 변경
    void ChangeColor(bool isOn)
    {
        ColorBlock cb = _flagToggle.colors;
        cb.selectedColor = isOn ? SetHexToColor("#3DBF67") : SetHexToColor("#FFFFFF");
        cb.normalColor = isOn ? SetHexToColor("#3DBF67") : SetHexToColor("#FFFFFF");
        _flagToggle.colors = cb;
    }

    public Color SetHexToColor(string hexadecimal)
    {
        Color hexColor;
        ColorUtility.TryParseHtmlString(hexadecimal, out hexColor);
        return hexColor;
    }

    //minesweeper에 토글 상태를 전달하는 함수
    public void SendToggleState()
    {
        _target.SendMessage("ReceiveFlagToggle", _flagToggle.isOn);
    }
}
