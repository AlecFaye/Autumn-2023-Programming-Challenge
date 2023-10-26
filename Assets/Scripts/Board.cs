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
        piece.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    public void SelectPiece(Piece piece)
    {
        piece.MeshRenderer.material = selectedMaterial;

        piece.PlaySelectPieceFeedback();
    }

    public void DeselectPiece(Piece piece, PlayerColour playerColour)
    {
        if (piece.MeshRenderer == null)
            return;

        piece.MeshRenderer.material = playerColour == PlayerColour.White
            ? defaultWhiteMaterial
            : defaultBlackMaterial;

        piece.PlayDeselectPieceFeedback();
    }
}
