using UnityEngine;

public class VFXElementManager : MonoBehaviour
{
    [SerializeField] private GameObject fireElement;
    [SerializeField] private GameObject waterElement;
    [SerializeField] private GameObject earthElement;
    [SerializeField] private GameObject lightElement;
    [SerializeField] private GameObject darkElement;

    public void ToggleVFX(ElementType elementType, bool isActive = true)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                fireElement.SetActive(isActive);
                break;
            case ElementType.Water:
                waterElement.SetActive(isActive);
                break;
            case ElementType.Earth:
                earthElement.SetActive(isActive);
                break;
            case ElementType.Light:
                lightElement.SetActive(isActive);
                break;
            case ElementType.Dark:
                darkElement.SetActive(isActive);
                break;
        }
    }
}
