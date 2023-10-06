using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance { get; private set; }

    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material waterMaterial;
    [SerializeField] private Material earthMaterial;

    private readonly Dictionary<ElementType, Material> elementMaterials = new();
    private readonly List<Element> elements = new();

    private void Awake()
    {
        Instance = this;

        InitElements();
        InitElementMaterials();
    }

    public Element ChooseRandomElement()
    {
        int randomIndex = Random.Range(0, elements.Count);
        return elements[randomIndex];
    }

    public Material GetMaterial(ElementType elementType)
    {
        return elementMaterials[elementType];
    }

    private void InitElements()
    {
        Element fireElement = new(ElementType.Fire, ElementType.Earth, ElementType.Water);
        Element waterElement = new(ElementType.Water, ElementType.Fire, ElementType.Earth);
        Element earthElement = new(ElementType.Earth, ElementType.Water, ElementType.Fire);

        elements.Add(fireElement);
        elements.Add(waterElement);
        elements.Add(earthElement);
    }

    private void InitElementMaterials()
    {
        elementMaterials.Add(ElementType.Fire, fireMaterial);
        elementMaterials.Add(ElementType.Water, waterMaterial);
        elementMaterials.Add(ElementType.Earth, earthMaterial);
    }
}
