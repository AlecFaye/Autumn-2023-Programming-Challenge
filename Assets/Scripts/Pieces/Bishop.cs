using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private void Awake()
    {
        PieceType = PieceType.Bishop;
    }

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new();

        foreach (Vector2Int dir in BishopDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int nextGridPoint = new(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);

                if (ChessGameManager.Instance.PieceAtGrid(nextGridPoint))
                {
                    break;
                }
            }
        }

        return locations;
    }
}
