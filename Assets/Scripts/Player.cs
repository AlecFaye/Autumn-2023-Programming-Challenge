using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> pieces;
    public List<GameObject> capturedPieces;

    public string playerName;
    public int forward;

    public Player(string playerName, bool positiveZMovement)
    {
        this.playerName = playerName;
        pieces = new List<GameObject>();
        capturedPieces = new List<GameObject>();

        if (positiveZMovement == true)
        {
            forward = 1;
        }
        else
        {
            forward = -1;
        }
    }
}
