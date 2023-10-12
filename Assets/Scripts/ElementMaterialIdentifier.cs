using UnityEngine;

public class ElementMaterialIdentifier : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    public void UpdateElementMaterial(ElementType elementType)
    {
        meshRenderer.material = ElementManager.Instance.GetMaterial(elementType);
    }
}
