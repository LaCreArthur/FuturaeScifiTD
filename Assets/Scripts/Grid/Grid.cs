using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public static float CellSize;
    public static bool HalfCellShift;
    public static Vector2Int GridSize;
    [SerializeField] float cellSize = 1f;
    [SerializeField] bool halfCellShift = true;
    [SerializeField] Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] GameObject cellPrefab;
    public static event Action GridInitialized;
    public static Dictionary<Vector2Int, Cell> Cells { get; } = new Dictionary<Vector2Int, Cell>();

    void Awake()
    {
        CellSize = cellSize;
        GridSize = gridSize;
        HalfCellShift = halfCellShift;
    }

    void Start()
    {
        CleanupGrid();
        InitializeGrid();
    }

    public static Vector3 GetWorldPos(Vector2Int pos) => new Vector3(pos.x, 0, pos.y) * CellSize +
                                                         (HalfCellShift ? new Vector3(1, 0, 1) * (CellSize / 2f) : Vector3.zero);

    public static Vector2Int GetGridPos(Vector3 worldPos) =>
        new Vector2Int(
            Mathf.FloorToInt(worldPos.x / CellSize),
            Mathf.FloorToInt(worldPos.z / CellSize)
        );

    static void CleanupGrid()
    {
        foreach (Cell cell in Cells.Values)
            Destroy(cell.gameObject);
        Cells.Clear();
    }

    void InitializeGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var pos = new Vector2Int(x, y);
                Vector3 worldPos = GetWorldPos(pos);
                GameObject cellGo = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                cellGo.transform.localScale = Vector3.one * CellSize;

                var cell = cellGo.GetComponent<Cell>();
                cell.Initialize(pos, CellType.Ground);
                Cells[pos] = cell;
            }
        }
        GridInitialized?.Invoke();
    }
}
