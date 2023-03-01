using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eDifficultyType
{
    EASY = 0,
    NORMAL,
    HARD,
    CUSTOM
}

public class DifficultyToggle : MonoBehaviour
{
    Toggle _difficultyToggle;
    [SerializeField] Toggle[] _difficultyToggles;
    [SerializeField] GameObject _minesweeper;
    eDifficultyType _difficulty;
    [SerializeField] Slider _colSlider;
    [SerializeField] Slider _rowSlider;
    [SerializeField] Slider _mineSlider;

    void Start()
    {
        _difficultyToggles = GetComponentsInChildren<Toggle>();
        foreach (var toggle in _difficultyToggles)
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                _difficultyToggle = toggle;
                ChangeColor(isOn);
            });
        }

        _colSlider.onValueChanged.AddListener(delegate { SetCustomMine(); });
        _rowSlider.onValueChanged.AddListener(delegate { SetCustomMine(); });
    }

    void OnEnable()
    {
        if(_difficultyToggles != null)
        {
            foreach (var toggle in _difficultyToggles)
            {
                toggle.isOn = false;
                _difficultyToggle = toggle;
                ChangeColor(toggle.isOn);
            }
        }
    }

    void ChangeColor(bool isOn)
    {
        ColorBlock cb = _difficultyToggle.colors;
        cb.selectedColor = isOn ? SetHexToColor("#3DBF67") : SetHexToColor("#FFFFFF");
        cb.normalColor = isOn ? SetHexToColor("#3DBF67") : SetHexToColor("#FFFFFF");
        _difficultyToggle.colors = cb;
    }

    public Color SetHexToColor(string hexadecimal)
    {
        Color hexColor;
        ColorUtility.TryParseHtmlString(hexadecimal, out hexColor);
        return hexColor;
    }

    public void SetDifficulty ()
    {
        foreach (var toggle in _difficultyToggles)
        {
            if(toggle.isOn)
            {
                _difficulty = (eDifficultyType)toggle.transform.GetSiblingIndex();
                break;
            }
        }

        (int col, int row, int mine) info = (-1, -1, -1);

        if (_difficulty == eDifficultyType.CUSTOM)
        {
            info = GetCustomMine();
        }

        (eDifficultyType difficulty, (int col, int row, int mine) info) data = (_difficulty, info);
        _minesweeper.SendMessage("SetNewGame", data);
    }

    public void SetCustomMine()
    {
        _mineSlider.maxValue = _colSlider.value * _rowSlider.value * 0.3f;
    }

    public (int col, int row, int mine) GetCustomMine()
    {
        return ((int)_colSlider.value, (int)_rowSlider.value, (int)_mineSlider.value);
    }
}
