using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Material defaultWhiteMaterial;
    [SerializeField] private Material defaultBlackMaterial;

    [SerializeField] private Material selectedMaterial;

    public GameObject AddPiece(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform);
        return newPiece;
    }

    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
        // TODO: Add animation to piece movement
        piece.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    public void SelectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;

        if (piece.TryGetComponent(out Piece pieceComponent))
        {
            pieceComponent.PlaySelectPieceFeedback();
        }
    }

    public void DeselectPiece(GameObject piece, PlayerColour playerColour)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();

        if (renderers == null)
            return;

        renderers.material = playerColour == PlayerColour.White
            ? defaultWhiteMaterial
            : defaultBlackMaterial;

        if (piece.TryGetComponent(out Piece pieceComponent))
        {
            pieceComponent.PlayDeselectPieceFeedback();
        }
    }
}
