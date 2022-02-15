using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler
{
    public TabGroup tabGroup;

    public Image background;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private void Awake()
    {
        tabGroup.SubScribe(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.Play("Button2");
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tabGroup.OnTabPressed(this);
    }

    public void SelectProcesses()
    {
        if (onTabSelected != null)
            onTabSelected.Invoke();
    }

    public void DeselectProcesses()
    {
        if (onTabDeselected != null)
            onTabDeselected.Invoke();
    }
}
