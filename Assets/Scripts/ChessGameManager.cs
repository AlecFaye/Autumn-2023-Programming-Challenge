using System.Collections.Generic;
using UnityEngine;

public enum CaptureType
{
    Strong,
    Weak,
    Neutral
}

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager Instance { get; private set; }

    [SerializeField] private Board board;

    [Header("White Pieces")]
    [SerializeField] private GameObject whiteKing;
    [SerializeField] private GameObject whiteQueen;
    [SerializeField] private GameObject whiteBishop;
    [SerializeField] private GameObject whiteKnight;
    [SerializeField] private GameObject whiteRook;
    [SerializeField] private GameObject whitePawn;

    [Header("Black Pieces")]
    [SerializeField] private GameObject blackKing;
    [SerializeField] private GameObject blackQueen;
    [SerializeField] private GameObject blackBishop;
    [SerializeField] private GameObject blackKnight;
    [SerializeField] private GameObject blackRook;
    [SerializeField] private GameObject blackPawn;

    private GameObject[,] pieces;
    private List<GameObject> movedPawns;

    private Player white;
    private Player black;

    public Player CurrentPlayer;
    public Player OtherPlayer;

    #region Pipeline Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitPlayer();
        InitPieces();
    }

    #endregion

    #region Initialization Functions

    private void InitPlayer()
    {
        white = new("White", true, PlayerColour.White);
        black = new("Black", false, PlayerColour.Black);

        CurrentPlayer = white;
        OtherPlayer = black;
    }

    private void InitPieces()
    {
        pieces = new GameObject[8, 8];
        movedPawns = new List<GameObject>();

        AddPiece(whiteRook, white, 0, 0);
        AddPiece(whiteKnight, white, 1, 0);
        AddPiece(whiteBishop, white, 2, 0);
        AddPiece(whiteQueen, white, 3, 0);
        AddPiece(whiteKing, white, 4, 0);
        AddPiece(whiteBishop, white, 5, 0);
        AddPiece(whiteKnight, white, 6, 0);
        AddPiece(whiteRook, white, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
        }

        AddPiece(blackRook, black, 0, 7);
        AddPiece(blackKnight, black, 1, 7);
        AddPiece(blackBishop, black, 2, 7);
        AddPiece(blackQueen, black, 3, 7);
        AddPiece(blackKing, black, 4, 7);
        AddPiece(blackBishop, black, 5, 7);
        AddPiece(blackKnight, black, 6, 7);
        AddPiece(blackRook, black, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
        }
    }

    #endregion

    #region Movement Functions

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece, CurrentPlayer.PlayerColour);
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        if (piece == null)
            return;

        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.PieceType == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;

        if (pieceComponent.PieceType == PieceType.Pawn
            && ((CurrentPlayer.PlayerColour == PlayerColour.White && gridPoint.y == 7)
            || (CurrentPlayer.PlayerColour == PlayerColour.Black && gridPoint.y == 0)))
        {
            PromotionFeedback.Instance.PlayPromotionFeedback();
            Destroy(piece);

            if (CurrentPlayer.PlayerColour == PlayerColour.White)
                AddPiece(whiteQueen, CurrentPlayer, gridPoint.x, gridPoint.y);
            else
                AddPiece(blackQueen, CurrentPlayer, gridPoint.x, gridPoint.y);
        }
        else
        {
            pieces[gridPoint.x, gridPoint.y] = piece;
            board.MovePiece(piece, gridPoint);
        }
    }

    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }

    public CaptureType CapturePieceAt(GameObject movingPiece, Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        
        Piece movedPiece = movingPiece.GetComponent<Piece>();
        Piece capturedPiece = pieceToCapture.GetComponent<Piece>();

        if (movedPiece.PieceElement.IsStrongAgainst(capturedPiece.PieceElement.ElementType))
        {
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);
            pieces[gridPoint.x, gridPoint.y] = null;

            movedPiece.PlayElementalFeedback();
            capturedPiece.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayStrongFeedback();

            CheckKingCapture(capturedPiece);

            return CaptureType.Strong;
        }
        else if (movedPiece.PieceElement.IsWeakAgainst(capturedPiece.PieceElement.ElementType))
        {
            OtherPlayer.CapturedPieces.Add(movingPiece);
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);

            pieces[gridPoint.x, gridPoint.y] = null;

            movedPiece.PlayElementalFeedback();
            movedPiece.PlayDestroyPieceFeedback();
            capturedPiece.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayWeakFeedback();

            CheckKingCapture(capturedPiece);
            CheckKingCapture(movedPiece);

            return CaptureType.Weak;
        }
        else
        {
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);
            pieces[gridPoint.x, gridPoint.y] = null;

            movedPiece.PlayElementalFeedback();
            capturedPiece.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayNeutralFeedback();

            CheckKingCapture(capturedPiece);

            return CaptureType.Neutral;
        }
    }

    #endregion

    #region Helper Functions

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return CurrentPlayer.Pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public void NextPlayer()
    {
        (OtherPlayer, CurrentPlayer) = (CurrentPlayer, OtherPlayer);

        PlayerTurnUI.Instance.UpdateTurnUI(CurrentPlayer.PlayerColour);
    }

    private void CheckKingCapture(Piece capturedPiece)
    {
        if (capturedPiece.PieceType == PieceType.King)
        {
            WinnerUI.Instance.UpdateWinnerText(CurrentPlayer.PlayerName);

            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
        }
    }

    private void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.Pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    private bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (OtherPlayer.Pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    #endregion
}
