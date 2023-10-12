using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public static ElementManager Instance { get; private set; }

    [Header("Element Materials")]
    [SerializeField] private Material fireMaterial;
    [SerializeField] private Material waterMaterial;
    [SerializeField] private Material earthMaterial;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material darkMaterial;

    [Header("Element Feedbacks")]
    [SerializeField] private MMFeedbacks fireFeedback;
    [SerializeField] private MMFeedbacks waterFeedback;
    [SerializeField] private MMFeedbacks earthFeedback;
    [SerializeField] private MMFeedbacks lightFeedback;
    [SerializeField] private MMFeedbacks darkFeedback;

    private readonly Dictionary<ElementType, Material> elementMaterials = new();
    private readonly Dictionary<ElementType, MMFeedbacks> elementFeedbacks = new();

    private readonly List<Element> elements = new();

    private void Awake()
    {
        Instance = this;

        InitElements();
        InitElementMaterials();
        InitElementFeedbacks();
    }

    public Element ChooseRandomElement()
    {
        int randomIndex = Random.Range(0, elements.Count);
        return elements[randomIndex];
    }

    public Material GetMaterial(ElementType elementType)
    {
        if (elementMaterials.TryGetValue(elementType, out Material elementMaterial))
        {
            return elementMaterial;
        }
        else
        {
            Debug.LogError($"No Material for Element Type: {elementType}");
            return null;
        }
    }

    public void PlayElementalFeedback(ElementType elementType, Vector3 feedbackPosition)
    {
        if (elementFeedbacks.TryGetValue(elementType, out MMFeedbacks elementFeedback))
        {
            elementFeedback.transform.position = feedbackPosition;
            elementFeedback.PlayFeedbacks();
        }
        else
        {
            Debug.LogError($"There is no feedback for Element Type: {elementType}");
        }
    }

    private void InitElements()
    {
        Element fireElement  = new(ElementType.Fire,  ElementType.Earth, ElementType.Water);
        Element waterElement = new(ElementType.Water, ElementType.Fire,  ElementType.Earth);
        Element earthElement = new(ElementType.Earth, ElementType.Water, ElementType.Fire);
        Element lightElement = new(ElementType.Light, ElementType.Dark,  ElementType.Light);
        Element darkElement  = new(ElementType.Dark,  ElementType.Light, ElementType.Dark);

        elements.Add(fireElement);
        elements.Add(waterElement);
        elements.Add(earthElement);
        elements.Add(lightElement);
        elements.Add(darkElement);
    }

    private void InitElementMaterials()
    {
        elementMaterials.Add(ElementType.Fire,  fireMaterial);
        elementMaterials.Add(ElementType.Water, waterMaterial);
        elementMaterials.Add(ElementType.Earth, earthMaterial);
        elementMaterials.Add(ElementType.Light, lightMaterial);
        elementMaterials.Add(ElementType.Dark,  darkMaterial);
    }

    private void InitElementFeedbacks()
    {
        elementFeedbacks.Add(ElementType.Fire,  fireFeedback);
        elementFeedbacks.Add(ElementType.Water, waterFeedback);
        elementFeedbacks.Add(ElementType.Earth, earthFeedback);
        elementFeedbacks.Add(ElementType.Light, lightFeedback);
        elementFeedbacks.Add(ElementType.Dark,  darkFeedback);
    }
}
