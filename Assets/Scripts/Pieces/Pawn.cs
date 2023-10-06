using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new();

        int forwardDirection = ChessGameManager.Instance.currentPlayer.forward;

        Vector2Int forwardOne = new(gridPoint.x, gridPoint.y + forwardDirection);
        if (ChessGameManager.Instance.PieceAtGrid(forwardOne) == false)
        {
            locations.Add(forwardOne);
        }

        Vector2Int forwardTwo = new(gridPoint.x, gridPoint.y + 2 * forwardDirection);
        if (ChessGameManager.Instance.HasPawnMoved(gameObject) == false && ChessGameManager.Instance.PieceAtGrid(forwardTwo) == false)
        {
            locations.Add(forwardTwo);
        }

        Vector2Int forwardRight = new(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (ChessGameManager.Instance.PieceAtGrid(forwardRight))
        {
            locations.Add(forwardRight);
        }

        Vector2Int forwardLeft = new(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (ChessGameManager.Instance.PieceAtGrid(forwardLeft))
        {
            locations.Add(forwardLeft);
        }

        return locations;
    }
}
