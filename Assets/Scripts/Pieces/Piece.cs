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
    [SerializeField] private ElementIdentifier elementIdentifier;

    [Header("Feedback References")]
    [SerializeField] private MMFeedbacks selectPieceFeedback;
    [SerializeField] private MMFeedbacks deselectPieceFeedback;
    [SerializeField] private MMFeedbacks destroyPieceFeedback;

    protected Vector2Int[] RookDirections = {
        new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    protected Vector2Int[] BishopDirections = {
        new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1) };

    private Element pieceElement;
    public Element PieceElement => pieceElement;

    private void Start()
    {
        pieceElement = ElementManager.Instance.ChooseRandomElement();
        elementIdentifier.UpdateElementMaterial(pieceElement.ElementType);
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

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
}
