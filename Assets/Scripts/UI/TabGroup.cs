using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> TabButtons;

    public Color TabIdle;
    public Color TabHover;
    public Color TabActive;

    public TabButton SelectedTab;
    public List<GameObject> ObjectsToSwap;

    public PanelGroup PanelGroup;

    private void Start()
    {
        ResetTabs();
    }

    public void OnTabEnter(TabButton tabButton)
    {
        ResetTabs();

        if (SelectedTab == null || tabButton != SelectedTab)
            tabButton.Background.color = TabHover;
    }

    public void OnTabExit(TabButton tabButton)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton)
    {
        if (SelectedTab != null)
            SelectedTab.Deselect();

        SelectedTab = tabButton;
        SelectedTab.Select();

        ResetTabs();

        tabButton.Background.color = TabActive;

        int index = tabButton.ButtonIndex;

        for (int i = 0; i < ObjectsToSwap.Count; i++)
            ObjectsToSwap[i].SetActive(i == index);

        if (PanelGroup != null)
            PanelGroup.SetPageIndex(index);
    }

    public void ResetTabs()
    {
        foreach (TabButton tabButton in TabButtons)
        {
            if (SelectedTab != null && tabButton == SelectedTab)
                continue;

            tabButton.Background.color = TabIdle;
        }
    }

    public void AddTab(TabButton tabButtonToAdd)
    {
        TabButtons.Add(tabButtonToAdd);
        tabButtonToAdd.TabGroup = this;
    }

    public void RemoveTab(TabButton tabButtonToRemove)
    {
        TabButtons.Remove(tabButtonToRemove);
    }

    public void AddObjectToSwap(GameObject objectToSwap)
    {
        ObjectsToSwap.Add(objectToSwap);
    }

    public void RemoveObjectToSwap(GameObject objectToSwap)
    {
        ObjectsToSwap.Remove(objectToSwap);
    }
}
