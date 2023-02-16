using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum _cellType
{
    MINE = -1,
    ZERO,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT
}

public class MineSweeper : MonoBehaviour
{
    const int _row = 9;
    const int _col = 9;
    [SerializeField] GameObject _button;
    [SerializeField] List<GameObject> _buttonList = new List<GameObject>();
    [SerializeField] Transform _buttonParent;
    [SerializeField] List<int> _map = new List<int>();
    [SerializeField] List<bool> _openMap = new List<bool>();


    const int _mines = 10;

    void Awake()
    {

    }

    void Start()
    {
        CreateButtons();
        SetMines();
    }

    void CreateButtons()
    {
        _buttonList.Clear();

        for (int i = 0; i < (_row * _col); i++)
        {
            GameObject objButton = Instantiate(_button);
            objButton.transform.parent = _buttonParent;
            objButton.transform.name = $"button[{i}]";
            _buttonList.Add(objButton);
        }
    }

    void SetMines()
    {
        List<int> mineNumber = new List<int>();

        while(mineNumber.Count < _mines)
        {
            int mine = Random.Range(0, _row * _col);
            if(mineNumber.Contains(mine) == false)
            {
                mineNumber.Add(mine);
                Debug.Log(mine);
            }
        }

        CreateMap(mineNumber);
    }

    void CreateMap(List<int> mineList)
    {
        _map.Clear();
        _openMap.Clear();

        for (int i = 0; i < (_row * _col); i++)
        {
            _map.Add((int)_cellType.ZERO);
            _openMap.Add(false);

            if (mineList.Contains(i))
            {
                _map[i] = (int)_cellType.MINE;
            }
        }

        string debugMap = string.Empty;

        for(int i = 0; i < _row; i++)
        {
            for(int j = 0; j < _col; j++)
            {
                debugMap += (-1 * _map[(i * _col) + j]).ToString();
                debugMap += " ";
            }
            debugMap += "\n";
        }

        Debug.Log(debugMap);
    }

    void Search(int index)
    {
        if(index < 0 || index >= (_col * _row))
        {
            return;
        }

        if(_openMap[index])
        {
            return;
        }
    }
}
