using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance { get; private set; }

    [SerializeField] private GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    #region Pipeline Functions

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitTileHighlight();
    }

    void Update()
    {
        MoveTileHighlight();
    }

    #endregion

    private void InitTileHighlight()
    {
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);

        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    private void MoveTileHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);

            if (Input.GetMouseButtonDown(0))
            {
                Piece selectedPiece = ChessGameManager.Instance.PieceAtGrid(gridPoint);
                if (ChessGameManager.Instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                {
                    ChessGameManager.Instance.SelectPiece(selectedPiece);
                    ExitState(selectedPiece);
                }
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState(Piece movingPiece)
    {
        enabled = false;
        tileHighlight.SetActive(false);

        MoveSelector.Instance.EnterState(movingPiece);
    }
}
