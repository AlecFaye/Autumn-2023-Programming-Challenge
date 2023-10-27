using System.Collections.Generic;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
    public List<GameObject> Panels;
    public TabGroup TabGroup;

    public int PanelIndex;

    private void Start()
    {
        ShowCurrentPanel();
    }

    private void ShowCurrentPanel()
    {
        for (int i = 0; i < Panels.Count; i++)
        {
            Panels[i].SetActive(PanelIndex == i);
        }
    }

    public void SetPageIndex(int index)
    {
        PanelIndex = index;
        ShowCurrentPanel();
    }

    public void AddPanel(GameObject panelToAdd)
    {
        Panels.Add(panelToAdd);
    }

    public void RemovePanel(GameObject panelToRemove)
    {
        Panels.Remove(panelToRemove);
    }
}
