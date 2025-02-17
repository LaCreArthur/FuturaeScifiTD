using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] float cellSize = 1f;
    [SerializeField] Vector2Int gridSize = new Vector2Int(10, 10);
    [SerializeField] GameObject cellPrefab;
    
    public static float CellSize;
    public static Vector2Int GridSize;
    public static Dictionary<Vector2Int, Cell> Cells { get; } = new Dictionary<Vector2Int, Cell>();
    public static event Action GridInitialized;
    
    public static Vector3 GetWorldPos(Vector3 originPos, float cellSize, int x, int y, bool halfCellShift = false) =>
        new Vector3(x, 0, y) *
        cellSize + originPos +
        (halfCellShift ? new Vector3(1, 0, 1) * (cellSize / 2f) : Vector3.zero);

    public static Vector2Int GetGridPos(Vector3 originPos, float cellSize, Vector3 worldPos) =>
        new Vector2Int(
            Mathf.FloorToInt((worldPos - originPos).x / cellSize),
            Mathf.FloorToInt((worldPos - originPos).z / cellSize)
        );

    void Awake()
    {
        CellSize = cellSize;
        GridSize = gridSize;
    }

    void Start()
    {
        CleanupGrid();
        InitializeGrid();
    }

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
                var position = new Vector2Int(x, y);
                Vector3 worldPos = GetWorldPos(Vector3.zero, cellSize, x, y, true);
                GameObject cellGo = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                cellGo.transform.localScale = Vector3.one * cellSize;
                
                var cell = cellGo.GetComponent<Cell>();
                cell.Initialize(position, CellType.Ground);
                Cells[position] = cell;
            }
        }
        GridInitialized?.Invoke();
    }
}
