using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    [SerializeField] private GameObject moveLocationPrefab;
    [SerializeField] private GameObject tileHighlightPrefab;
    [SerializeField] private GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;

    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    private void Start()
    {
        enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)), Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);

            CaptureType captureType = CaptureType.Neutral;

            if (Input.GetMouseButtonDown(0))
            {
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                if (ChessGameManager.Instance.PieceAtGrid(gridPoint) == null)
                {
                    ChessGameManager.Instance.Move(movingPiece, gridPoint);
                }
                else
                {
                    captureType = ChessGameManager.Instance.CapturePieceAt(movingPiece, gridPoint);
                    
                    if (captureType == CaptureType.Strong || captureType == CaptureType.Neutral)
                    {
                        ChessGameManager.Instance.Move(movingPiece, gridPoint);
                    }
                }
                ExitState(captureType == CaptureType.Strong);
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1) && movingPiece != null)
        {
            CancelMove();
        }
    }

    private void CancelMove()
    {
        enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        ChessGameManager.Instance.DeselectPiece(movingPiece);
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        enabled = true;

        moveLocations = ChessGameManager.Instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (ChessGameManager.Instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState(bool canGoAgain)
    {
        enabled = false;
        tileHighlight.SetActive(false);

        TileSelector selector = GetComponent<TileSelector>();
        ChessGameManager.Instance.DeselectPiece(movingPiece);
        
        movingPiece = null;
        
        if (!canGoAgain)
            ChessGameManager.Instance.NextPlayer();

        selector.EnterState();
        
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
