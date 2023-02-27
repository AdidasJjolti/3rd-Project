using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    [SerializeField] TextMeshProUGUI _timerText;
    float _curTime;
    bool _isFirstClick;
    [SerializeField] TextMeshProUGUI _mineCountText;
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Image _faceImage;

    const int _mines = 50;

    private bool _isGameOver;

    void Awake()
    {
        _curTime = 0f;
        _isFirstClick = false;
        _mineCountText.text = _mines.ToString();
        _faceImage.sprite = _faceSprites[0];
    }

    void Start()
    {
        CreateButtons();
        SetMines();
    }

    void Update()
    {
        SetTimerUI();
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
            objButton.GetComponent<Button>().onClick.AddListener(() => 
            {
                Search(_buttonList.IndexOf(objButton), _buttonList.IndexOf(objButton));
                _isFirstClick = true;
            });

            //objButton.GetComponent<Button>()
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

    void Search(int origin, int index)
    {
        if (_isGameOver == true)
        {
            return;
        }

        if (index < 0 || index >= (_col * _row))
        {
            return;
        }

        // origin 좌우로 나올 수 있는 최소, 최대 나머지 값을 각각 min, max로 정의
        // origin이 1인 경우, min 값은 0, max 값은 2
        int min = (origin % _col) - 1 < 0 ? 0 : (origin % _col) - 1;
        int max = (origin % _col) + 1 > _col - 1 ? _col - 1 : (origin % _col) + 1;

        if(index % _col < min || index % _col > max)
        {
            return;
        }

        if(_openMap[index])
        {
            return;
        }

        if (_map[index] != (int)_cellType.MINE || (_map[index] == (int)_cellType.MINE && origin == index))
        {
            _openMap[index] = true;
        }

        // 직접 누르지 않은 버튼이 지뢰일 때
        if(_openMap[index] == false)
        {
            return;
        }

        // Todo : 열린 버튼이 지뢰일 때 게임 오버 처리하는 부분 체크, 지뢰 위치 모두 공개하는 함수 추가

        if (_map[index] == (int)_cellType.MINE && _openMap[index] == true)
        {
            _isGameOver = true;
            Time.timeScale = 0f;
            Debug.LogError("Game Over" + " ,Origin : " + origin + " , Index : " + index);
            return;
        }

        _buttonList[index].GetComponent<Button>().interactable = false;
        int mineCount = CountMine(origin, index);
        Debug.Log("MineCount : " + mineCount + " ,Origin : "+ origin + " , Index : " + index);         // 자기 주위 8칸의 지뢰 갯수를 표시하는 로그


        if (mineCount == 0)
        {
            origin = index;

            Search(origin, index - (_col - 1));
            //Debug.Log("Upper Left Searched, Origin is : " + origin);
            Search(origin, index - _col);
            //Debug.Log("Top Searched, Origin is : " + origin);
            Search(origin, index - (_col + 1));
            //Debug.Log("Upper Right Searched, Origin is : " + origin);
            Search(origin, index - 1);
            //Debug.Log("Left Searched, Origin is : " + origin);
            Search(origin, index + 1);
            //Debug.Log("Right Searched, Origin is : " + origin);
            Search(origin, index + (_col - 1));
            //Debug.Log("Lower Left Searched, Origin is : " + origin);
            Search(origin, index + _col);
            //Debug.Log("Down Searched, Origin is : " + origin);
            Search(origin, index + (_col + 1));
            //Debug.Log("Lower Right Searched, Origin is : " + origin);
        }
        else
        {
            TextMeshProUGUI mineText = _buttonList[index].GetComponentInChildren<TextMeshProUGUI>();
            mineText.SetText(mineCount.ToString());

            switch (mineCount)
            {
                case 1:
                    SetHexToColor("#0000FF", mineText);
                    break;
                case 2:
                    SetHexToColor("#44FD44", mineText);
                    break;
                case 3:
                    SetHexToColor("#FF0000", mineText);
                    break;
                case 4:
                    SetHexToColor("#000080", mineText);
                    break;
                default:
                    SetHexToColor("#800000", mineText);
                    break;
            }
        }
    }

    public void SetHexToColor(string hexadecimal, TextMeshProUGUI text)
    {
        Color hexColor;
        ColorUtility.TryParseHtmlString(hexadecimal, out hexColor);
        text.color = hexColor;
    }

    // 찾으려는 버튼 번호(index) 기준으로 8방향 + 자기자신을 탐색하는 함수 CountMine
    int CountMine(int origin, int index)
    {
        int mineResult = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int target = index + i * _row + j;              // 탐색하려는 버튼 위치  좌상단(-1, -1) ~ 우하단(1, 1)

                if (target < 0 || target >= (_col * _row))      // 탐색하려는 버튼 위치가 0보다 작거나 80번을 넘어가면 스킵
                {
                    continue;
                }


                int min = (index % _col) - 1 < 0 ? 0 : (index % _col) - 1;
                int max = (index % _col) + 1 > _col - 1 ? _col - 1 : (index % _col) + 1;

                if (max < target % _col || min > target % _col)
                {
                    continue;
                }

                if(_map[target] == (int)_cellType.MINE)
                {
                    mineResult += 1;
                }
            }
        }

        return mineResult;
    }

    void SetTimerUI()
    {
        if(_isFirstClick == false)
        {
            return;
        }
        _curTime += Time.deltaTime;
        _timerText.text = string.Format($"{_curTime:000}");
    }

    public void ChangeFaceSprite()
    {
        
    }
}
