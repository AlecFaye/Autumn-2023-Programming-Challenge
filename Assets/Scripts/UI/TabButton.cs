using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup TabGroup;
    public Image Background;
    public int ButtonIndex;

    public UnityEvent<TabButton> OnTabSelected;
    public UnityEvent OnTabDeselected;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TabGroup.OnTabExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TabGroup.OnTabSelected(this);
    }

    public void Select()
    {
        OnTabSelected?.Invoke(this);
    }

    public void Deselect()
    {
        OnTabDeselected?.Invoke();
    }
}
