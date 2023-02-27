using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool _onPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        _onPress = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPress = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
