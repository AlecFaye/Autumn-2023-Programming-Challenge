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
    [SerializeField] private Piece whiteKing;
    [SerializeField] private Piece whiteQueen;
    [SerializeField] private Piece whiteBishop;
    [SerializeField] private Piece whiteKnight;
    [SerializeField] private Piece whiteRook;
    [SerializeField] private Piece whitePawn;

    [Header("Black Pieces")]
    [SerializeField] private Piece blackKing;
    [SerializeField] private Piece blackQueen;
    [SerializeField] private Piece blackBishop;
    [SerializeField] private Piece blackKnight;
    [SerializeField] private Piece blackRook;
    [SerializeField] private Piece blackPawn;

    private Piece[,] pieces;
    private List<Piece> movedPieces;

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
        pieces = new Piece[8, 8];
        movedPieces = new List<Piece>();

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

    public void SelectPiece(Piece piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(Piece piece)
    {
        board.DeselectPiece(piece, CurrentPlayer.PlayerColour);
    }

    public List<Vector2Int> MovesForPiece(Piece piece)
    {
        Vector2Int gridPoint = GridForPiece(piece);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

    public void Move(Piece piece, Vector2Int gridPoint)
    {
        if (piece == null)
            return;

        if (piece.PieceType == PieceType.Pawn && !HasPieceMoved(piece))
        {
            movedPieces.Add(piece);
        }

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;

        if (piece.PieceType == PieceType.Pawn
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

    public bool HasPieceMoved(Piece piece)
    {
        return movedPieces.Contains(piece);
    }

    public CaptureType CapturePieceAt(Piece movingPiece, Vector2Int gridPoint)
    {
        Piece pieceToCapture = PieceAtGrid(gridPoint);
        
        if (movingPiece.PieceElement.IsStrongAgainst(pieceToCapture.PieceElement.ElementType))
        {
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);
            pieces[gridPoint.x, gridPoint.y] = null;

            movingPiece.PlayElementalFeedback();
            pieceToCapture.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayStrongFeedback();

            CheckKingCapture(pieceToCapture);

            return CaptureType.Strong;
        }
        else if (movingPiece.PieceElement.IsWeakAgainst(pieceToCapture.PieceElement.ElementType))
        {
            OtherPlayer.CapturedPieces.Add(movingPiece);
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);

            pieces[gridPoint.x, gridPoint.y] = null;

            movingPiece.PlayElementalFeedback();
            movingPiece.PlayDestroyPieceFeedback();
            pieceToCapture.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayWeakFeedback();

            CheckKingCapture(pieceToCapture);
            CheckKingCapture(movingPiece);

            return CaptureType.Weak;
        }
        else
        {
            CurrentPlayer.CapturedPieces.Add(pieceToCapture);
            pieces[gridPoint.x, gridPoint.y] = null;

            movingPiece.PlayElementalFeedback();
            pieceToCapture.PlayDestroyPieceFeedback();
            StrengthFeedbackManager.Instance.PlayNeutralFeedback();

            CheckKingCapture(pieceToCapture);

            return CaptureType.Neutral;
        }
    }

    #endregion

    #region Helper Functions

    public bool DoesPieceBelongToCurrentPlayer(Piece piece)
    {
        return CurrentPlayer.Pieces.Contains(piece);
    }

    public Piece PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(Piece piece)
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

    private void AddPiece(Piece prefab, Player player, int col, int row)
    {
        Piece pieceObject = board.AddPiece(prefab, col, row);
        player.Pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    private bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        Piece piece = PieceAtGrid(gridPoint);

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
