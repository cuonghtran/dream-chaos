using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite spriteTabIdle;
    public Sprite spriteTabHover;
    public Sprite spriteTabActive;

    public TabButton selectedTab;
    public List<GameObject> tabsToShow;

    public void SubScribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabPressed(TabButton button)
    {
        ResetTabs();
        if (selectedTab != null && selectedTab != button)
        {
            button.background.sprite = spriteTabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
            selectedTab.DeselectProcesses();

        selectedTab = button;

        selectedTab.SelectProcesses();

        ResetTabs();
        button.background.sprite = spriteTabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i< tabsToShow.Count; i++)
        {
            if (i == index)
                tabsToShow[i].SetActive(true);
            else
                tabsToShow[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && selectedTab == button)
                continue;

            button.background.sprite = spriteTabIdle;
        }
    }

    public TabButton GetTabButtonByIndex(int tabIndex)
    {
        if (tabIndex >= 0 && tabIndex <= 2)
            return tabButtons[tabIndex];

        return tabButtons[0];
    }
}
