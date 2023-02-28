using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _onPress;
    GameObject target;      // �޽����� ������ ���� ������Ʈ : minesweeper

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPress = true;
        target.SendMessage("ChangeFaceSprite", _onPress);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPress = false;
        target.SendMessage("ChangeFaceSprite", _onPress);
    }

    public void SetTarget(GameObject minesweeper)
    {
        target = minesweeper;
    }
}
