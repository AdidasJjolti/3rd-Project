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

    // ��� on, off ���¿� ���� selectedColor ���� ����
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

    //minesweeper�� ��� ���¸� �����ϴ� �Լ�
    public void SendToggleState()
    {
        _target.SendMessage("ReceiveFlagToggle", _flagToggle.isOn);
    }
}
