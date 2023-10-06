using UnityEngine;

public class ElementIdentifier : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void UpdateElementMaterial(ElementType element)
    {
        meshRenderer.material = ElementManager.Instance.GetMaterial(element);
    }
}
