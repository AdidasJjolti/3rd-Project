using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _onPress;
    GameObject target;      // 메시지를 전달할 게임 오브젝트 : minesweeper
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

    // 버튼 프리팹에 깃발 이미지 표시 on, off
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
