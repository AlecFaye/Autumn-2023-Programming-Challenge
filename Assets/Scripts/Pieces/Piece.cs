using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType 
{ 
    King, 
    Queen, 
    Bishop, 
    Knight, 
    Rook, 
    Pawn 
};

public abstract class Piece : MonoBehaviour
{
    [HideInInspector] public PieceType PieceType;

    [Header("Element References")]
    [SerializeField] private ElementMaterialIdentifier elementMaterialIdentifier;

    [Header("Piece Feedback References")]
    [SerializeField] private MMFeedbacks selectPieceFeedback;
    [SerializeField] private MMFeedbacks deselectPieceFeedback;
    [SerializeField] private MMFeedbacks destroyPieceFeedback;

    private Element pieceElement;
    public Element PieceElement => pieceElement;

    protected Vector2Int[] RookDirections = {
        new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    protected Vector2Int[] BishopDirections = {
        new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1) };

    private void Start()
    {
        ChooseAndUpdateElement();
    }

    public void PlaySelectPieceFeedback()
    {
        if (selectPieceFeedback != null)
            selectPieceFeedback.PlayFeedbacks();
    }

    public void PlayDeselectPieceFeedback()
    {
        if (deselectPieceFeedback != null)
            deselectPieceFeedback.PlayFeedbacks();
    }

    public void PlayDestroyPieceFeedback()
    {
        if (destroyPieceFeedback != null)
            destroyPieceFeedback.PlayFeedbacks();
    }

    public void PlayElementalFeedback()
    {
        ElementManager.Instance.PlayElementalFeedback(pieceElement.ElementType, transform.position);
    }

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);

    private void ChooseAndUpdateElement()
    {
        pieceElement = ElementManager.Instance.ChooseRandomElement();

        elementMaterialIdentifier.UpdateElementMaterial(pieceElement.ElementType);
    }
}
