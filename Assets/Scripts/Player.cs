using System.Collections.Generic;
using UnityEngine;

public enum PlayerColour
{
    White,
    Black
}

public class Player
{
    public List<GameObject> Pieces;
    public List<GameObject> CapturedPieces;

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
