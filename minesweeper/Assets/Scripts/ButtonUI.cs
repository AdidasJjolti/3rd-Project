using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _onPress;
    GameObject target;      // �޽����� ������ ���� ������Ʈ : minesweeper
    [SerializeField] Image _flagImage;

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

    // ��ư �����տ� ��� �̹��� ǥ�� on, off
    public void SetFlag()
    {
        _flagImage.gameObject.SetActive(!_flagImage.gameObject.activeSelf);
        //if(_flagImage.gameObject.activeSelf == false)
        //{
        //    string name = gameObject.name;
        //    string number = Regex.Replace(name, @"\D", "");
        //    target.SendMessage("RemoveFlag", int.Parse(number));
        //}
    }
}
