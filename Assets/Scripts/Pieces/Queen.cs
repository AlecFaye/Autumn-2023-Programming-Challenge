using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    private void Awake()
    {
        PieceType = PieceType.Queen;
    }

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new();
        List<Vector2Int> directions = new(BishopDirections);
        directions.AddRange(RookDirections);

        foreach (Vector2Int dir in directions)
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
