using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

enum _cellType
{
    FLAG = -2,
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
    int _row = 9;
    int _col = 9;
    int _mines = 10;
    int _flags;

    [SerializeField] GameObject _button;
    [SerializeField] List<GameObject> _buttonList = new List<GameObject>();
    [SerializeField] Transform _buttonParent;
    [SerializeField] List<int> _map = new List<int>();
    [SerializeField] List<bool> _openMap = new List<bool>();
    [SerializeField] List<bool> _flagMap = new List<bool>();
    [SerializeField] TextMeshProUGUI _timerText;
    float _curTime;
    bool _isFirstClick;
    [SerializeField] TextMeshProUGUI _mineCountText;
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Image _faceImage;
    bool _toggleState;
    [SerializeField] GameObject _flagToggle;
    [SerializeField] GameObject _difficultyToggle;

    [SerializeField] Image _buttonBackGround;

    private bool _isGameOver;
    private bool _isVictorious;

    void Awake()
    {
        _curTime = 0f;
        _isFirstClick = false;
        _faceImage.sprite = _faceSprites[0];
    }

    void Start()
    {
        SetNewGame((eDifficultyType.EASY,(-1,-1,-1)));
    }

    void Update()
    {
        SetTimerUI();
        if(_flagMap.Count >0)
        {
            Debug.Log(_flagMap[0]);
        }
    }

    void CreateButtons()
    {
        for(int i = _buttonList.Count - 1; i >= 0; i--)
        {
            Destroy(_buttonList[i]);
        }

        _buttonList.Clear();

        for (int i = 0; i < (_row * _col); i++)
        {
            GameObject objButton = Instantiate(_button);
            if(_map[i] == (int)_cellType.MINE)
            {
                ColorBlock cb = objButton.GetComponent<Button>().colors;
                cb.normalColor = new Color(1, 0, 0);
                objButton.GetComponent<Button>().colors = cb;
            }
            objButton.GetComponent<ButtonUI>().SetTarget(gameObject);
            objButton.transform.parent = _buttonParent;
            objButton.transform.name = $"button[{i}]";
            _buttonList.Add(objButton);
            objButton.GetComponent<Button>().onClick.AddListener(() => 
            {
                CheckFlagToggle();

                // flagToggle ���¸� üũ�ϰ� search �Ǵ� addFlag ����
                if (_toggleState == true)
                {
                    if(_flagMap[_buttonList.IndexOf(objButton)] == false)
                    {
                        AddFlag(_buttonList.IndexOf(objButton));
                    }
                    else
                    {
                        RemoveFlag(_buttonList.IndexOf(objButton));
                    }
                    _mineCountText.text = _flags.ToString();
                }
                else if (_toggleState == false && _flagMap[_buttonList.IndexOf(objButton)] == true)
                {
                    return;
                }
                else
                {
                    Search(_buttonList.IndexOf(objButton), _buttonList.IndexOf(objButton));
                }
                _isFirstClick = true;
            });
        }
    }

    void SetMines()
    {
        _flags = _mines;
        _mineCountText.text = _flags.ToString();
        List<int> mineNumber = new List<int>();
        while(mineNumber.Count < _mines)
        {
            int mine = Random.Range(0, _row * _col);
            if(mineNumber.Contains(mine) == false)
            {
                mineNumber.Add(mine);
                //Debug.Log(mine);
            }
        }

        CreateMap(mineNumber);
    }

    void CreateMap(List<int> mineList)
    {
        _map.Clear();
        _openMap.Clear();
        _flagMap.Clear();

        for (int i = 0; i < (_row * _col); i++)
        {
            _map.Add((int)_cellType.ZERO);
            _openMap.Add(false);
            _flagMap.Add(false);

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

        // origin �¿�� ���� �� �ִ� �ּ�, �ִ� ������ ���� ���� min, max�� ����
        // origin�� 1�� ���, min ���� 0, max ���� 2
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

        // ���� ������ ���� ��ư�� ������ ��
        if(_openMap[index] == false)
        {
            return;
        }

        // Todo : ���� ��ư�� ������ �� ���� ���� ó���ϴ� �κ� üũ, ���� ��ġ ��� �����ϴ� �Լ� �߰�

        if (_map[index] == (int)_cellType.MINE && _openMap[index] == true)
        {
            _isGameOver = true;
            SetGameOver();
            Debug.LogError("Game Over" + " ,Origin : " + origin + " , Index : " + index);
            return;
        }

        _buttonList[index].GetComponent<Button>().interactable = false;
        int mineCount = CountMine(index);
        //Debug.Log("MineCount : " + mineCount + " ,Origin : "+ origin + " , Index : " + index);         // �ڱ� ���� 8ĭ�� ���� ������ ǥ���ϴ� �α�


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


        // ToDo : ���� �¸� ���� �����ϱ�
        if(CountOpenButtons() + CountFlags() == _row * _col)
        {
            SetVictorious();
        }
    }

    public void SetHexToColor(string hexadecimal, TextMeshProUGUI text)
    {
        Color hexColor;
        ColorUtility.TryParseHtmlString(hexadecimal, out hexColor);
        text.color = hexColor;
    }

    // ã������ ��ư ��ȣ(index) �������� 8���� + �ڱ��ڽ��� Ž���ϴ� �Լ� CountMine
    int CountMine(int index)
    {
        int mineResult = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int target = index + i * _row + j;              // Ž���Ϸ��� ��ư ��ġ  �»��(-1, -1) ~ ���ϴ�(1, 1)

                if (target < 0 || target >= (_col * _row))      // Ž���Ϸ��� ��ư ��ġ�� 0���� �۰ų� 80���� �Ѿ�� ��ŵ
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

        if(_isGameOver == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            _curTime += Time.deltaTime;
            _timerText.text = string.Format($"{_curTime:000}");
        }
    }

    public void ChangeFaceSprite(bool onPress)
    {
        if (onPress == true)
        {
            _faceImage.sprite = _faceSprites[1];
        }
        else
        {
            _faceImage.sprite = _faceSprites[0];
        }
    }

    // FlagToggle�� ��� ���¸� ��û�ϴ� �Լ�
    public void CheckFlagToggle()
    {
        _flagToggle.SendMessage("SendToggleState");
    }

    // FlagToggle���� ��� ���� �޾ƿ��� �Լ�
    public void ReceiveFlagToggle(bool flagToggleState)
    {
        _toggleState = flagToggleState;
    }

    // FlagToggle�� on ������ �� onClick �̺�Ʈ�� ��� �������� ǥ���� �Լ�
    public void AddFlag(int index)
    {
        _buttonList[index].SendMessage("SetFlag");
        _flagMap[index] = true;
        if (_flags <= 0)
        {
            return;
        }
        _flags--;
    }

    public void RemoveFlag(int index)
    {
        _buttonList[index].SendMessage("SetFlag");
        _flagMap[index] = false;
        _flags++;
    }

    // ToDo : ���̵� ������ ����
    public void SetNewGame((eDifficultyType difficulty, (int col, int row, int mine)info)data)
    {
        switch(data.difficulty)
        {
            case eDifficultyType.EASY:
                _row = 9;
                _col = 9;
                _mines = 10;
                break;
            case eDifficultyType.NORMAL:
                _row = 16;
                _col = 16;
                _mines = 40;
                break;
            case eDifficultyType.HARD:
                _row = 16;
                _col = 30;
                _mines = 99;
                break;
            case eDifficultyType.CUSTOM:          
                _row = data.info.row;
                _col = data.info.col;
                _mines = data.info.mine;
                break;
        }
        SetBackgroundSize(_row, _col);
        SetMines();
        CreateButtons();
    }

    // �� ��ư�� ������ 9x9 �� �������� �ʱ�ȭ
    public void SetEasyGame()
    {
        GameObject.Find("UICanvas").transform.Find("gameOverUI").gameObject.SetActive(false);
        Time.timeScale = 1;
        _isGameOver = false;
        _curTime = 0f;
        _isFirstClick = false;
        _timerText.text = "000";
        SetBackgroundSize(9, 9);
        SetMines();
        CreateButtons();

    }

    // width = left padding + right padding + (buttons - 1) * spacing + buttons * buttonsize
    // height = top padding + down padding + (buttons - 1) * spacing + buttons * buttonsize
    public void SetBackgroundSize(int row, int col)
    { 
        _buttonBackGround.rectTransform.sizeDelta = new Vector2(4 + (col - 1) * 30 + col * 62, 4 + (row - 1) * 30 + row * 62);
    }

    // ���� ���� �� ���� ���� UI Ȱ��ȭ, ���� ���� UI���� SetEasyGame�� ȣ���ϴ� ��ư ����
    void SetGameOver()
    {
        Time.timeScale = 0;
        GameObject.Find("UICanvas").transform.Find("gameOverUI").gameObject.SetActive(true);
    }

    // ���� �¸� ���� : _flags = 0�̰� open ��ư�� _row*_col - _mines = 71�� ��
    void SetVictorious()
    {
        Debug.Log("Victorious!");
    }

    // open ������ ��ư ������ �������� �Լ�, CountFlags �Լ��� ���ϰ����� ���� _row*_col�̸� �¸� ����
    int CountOpenButtons()
    {
        return 0;
    }

    // �ʿ� ǥ���� ��� ������ �������� �Լ�,
    int CountFlags()
    {
        return 0;
    }
}
