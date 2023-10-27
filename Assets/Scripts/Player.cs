using System.Collections.Generic;

public enum PlayerColour
{
    White,
    Black
}

public class Player
{
    public List<Piece> Pieces;
    public List<Piece> CapturedPieces;

    public string PlayerName;
    public int Forward;
    public PlayerColour PlayerColour;

    public Player(string playerName, bool positiveZMovement, PlayerColour playerColour)
    {
        PlayerName = playerName;
        PlayerColour = playerColour;

        Pieces = new();
        CapturedPieces = new();

        Forward = positiveZMovement 
            ? 1 
            : -1;
    }
}
