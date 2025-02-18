using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInput : MonoBehaviour
{
    Camera _mainCamera;
    Cell _cellOver;
    GameObject _activeTower;

    public static event Action<Cell> CellClicked;
    public static event Action<Cell> CellOver;

    void Awake() => _mainCamera = Camera.main;

    void Update()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition(_mainCamera);
        Vector2Int gridPosition = Grid.GetGridPos(mouseWorldPos);

        // Check if mouse is over an existing cell
        if (Grid.Cells.TryGetValue(gridPosition, out Cell cell))
        {
            if (_cellOver != cell) UpdateCellOver(cell);
            if (Input.GetMouseButtonDown(0))
            {
                // Check if pointer is over UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (IsClickValid(gridPosition))
                {
                    Debug.Log($"Clicked cell: {gridPosition} ({cell.type})");
                    CellClicked?.Invoke(cell);
                }
            }
        }
    }

    void UpdateCellOver(Cell cell)
    {
        if (cell != null)
        {
            if (_cellOver != null)
            {
                _cellOver.ResetMaterial();
            }
            cell.SetValidMaterial();
            _cellOver = cell;
            CellOver?.Invoke(cell);
        }
    }

    bool IsClickValid(Vector2Int pos) =>
        pos.x >= 0 && pos.x < Grid.GridSize.x &&
        pos.y >= 0 && pos.y < Grid.GridSize.y;

    Vector3 GetMouseWorldPosition(Camera cam)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
