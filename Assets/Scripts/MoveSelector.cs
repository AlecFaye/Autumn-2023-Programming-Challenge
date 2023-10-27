using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public static MoveSelector Instance { get; private set; }

    [Header("Tile Colour Prefabs")]
    [SerializeField] private GameObject moveLocationPrefab;
    [SerializeField] private GameObject tileHighlightPrefab;
    [SerializeField] private GameObject attackLocationPrefab;

    [Header("Feedback")]
    [SerializeField] private MMFeedbacks normalMovementFeedback;

    private GameObject tileHighlight;
    private Piece movingPiece;

    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    private CaptureType captureType = CaptureType.Neutral;

    #region Pipeline Functions

    private void Awake()
    {
        Instance = this;
    }

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

            HighlightTile(gridPoint);

            if (Input.GetMouseButtonDown(0))
            {
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                captureType = CaptureType.Neutral;
                bool isOnlyMovingPiece = ChessGameManager.Instance.PieceAtGrid(gridPoint) == null;

                if (isOnlyMovingPiece)
                {
                    MovePiece(gridPoint);
                }
                else
                {
                    CapturePiece(gridPoint);
                }

                ExitState();
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

    #endregion

    private void HighlightTile(Vector2Int gridPoint)
    {
        tileHighlight.SetActive(true);
        tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    private void MovePiece(Vector2Int gridPoint)
    {
        ChessGameManager.Instance.Move(movingPiece, gridPoint);

        if (normalMovementFeedback != null)
        {
            normalMovementFeedback.PlayFeedbacks();
        }
    }

    private void CapturePiece(Vector2Int gridPoint)
    {
        captureType = ChessGameManager.Instance.CapturePieceAt(movingPiece, gridPoint);

        if (captureType == CaptureType.Strong || captureType == CaptureType.Neutral)
        {
            ChessGameManager.Instance.Move(movingPiece, gridPoint);
        }
    }

    private void CancelMove()
    {
        enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        tileHighlight.SetActive(false);

        ChessGameManager.Instance.DeselectPiece(movingPiece);
        TileSelector.Instance.EnterState();
    }

    public void EnterState(Piece piece)
    {
        enabled = true;
        movingPiece = piece;

        moveLocations = ChessGameManager.Instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        foreach (Vector2Int moveLocation in moveLocations)
        {
            GameObject highlightPrefab = ChessGameManager.Instance.PieceAtGrid(moveLocation)
                ? attackLocationPrefab
                : moveLocationPrefab;

            GameObject highlight = Instantiate(highlightPrefab, Geometry.PointFromGrid(moveLocation), Quaternion.identity, gameObject.transform);

            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        enabled = false;
        tileHighlight.SetActive(false);

        ChessGameManager.Instance.DeselectPiece(movingPiece);
        
        movingPiece = null;
        
        if (captureType != CaptureType.Strong)
        {
            ChessGameManager.Instance.NextPlayer();
        }

        TileSelector.Instance.EnterState();
        
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
