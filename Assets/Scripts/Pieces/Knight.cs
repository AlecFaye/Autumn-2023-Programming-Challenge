using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    private void Awake()
    {
        PieceType = PieceType.Knight;
    }

    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new()
        {
            new Vector2Int(gridPoint.x - 1, gridPoint.y + 2),
            new Vector2Int(gridPoint.x + 1, gridPoint.y + 2),

            new Vector2Int(gridPoint.x + 2, gridPoint.y + 1),
            new Vector2Int(gridPoint.x - 2, gridPoint.y + 1),

            new Vector2Int(gridPoint.x + 2, gridPoint.y - 1),
            new Vector2Int(gridPoint.x - 2, gridPoint.y - 1),

            new Vector2Int(gridPoint.x + 1, gridPoint.y - 2),
            new Vector2Int(gridPoint.x - 1, gridPoint.y - 2)
        };

        return locations;
    }
}
