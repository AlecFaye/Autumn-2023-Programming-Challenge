using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Material defaultWhiteMaterial;
    [SerializeField] private Material defaultBlackMaterial;

    [SerializeField] private Material selectedMaterial;

    public Piece AddPiece(Piece piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        Piece newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint), Quaternion.identity, gameObject.transform);
        return newPiece;
    }

    public void MovePiece(Piece piece, Vector2Int gridPoint)
    {
        // TODO: Add animation to piece movement
        piece.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    public void SelectPiece(Piece piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;

        piece.PlaySelectPieceFeedback();
    }

    public void DeselectPiece(Piece piece, PlayerColour playerColour)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();

        if (renderers == null)
            return;

        renderers.material = playerColour == PlayerColour.White
            ? defaultWhiteMaterial
            : defaultBlackMaterial;

        piece.PlayDeselectPieceFeedback();
    }
}
