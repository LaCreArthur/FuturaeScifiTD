using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathGenerator : MonoBehaviour
{
    const int MAX_EXCLUDED_HISTORY = 16;

    [SerializeField] float visualizationDelay = 0.1f;
    [SerializeField] GameObject playerTriggerPrefab;

    readonly Vector2Int[] _directions =
    {
        Vector2Int.up, // Top
        Vector2Int.right, // Right
        Vector2Int.down, // Bottom
        Vector2Int.left, // Left
    };

    readonly Dictionary<Vector2Int, float> _directionsWeights = new Dictionary<Vector2Int, float>
    {
        { Vector2Int.up, 0.4f },
        { Vector2Int.right, 0.1f },
        { Vector2Int.down, 0.4f },
        { Vector2Int.left, 0.5f },
    };

    Vector2Int _gridSize;

    public static event Action PathGenerated;

    public List<Vector2Int> Path { get; } = new List<Vector2Int>();
    public static List<Vector3> PathWorldPositions { get; } = new List<Vector3>();

    void Awake() => Grid.GridInitialized += GeneratePath;

    void OnDestroy() => Grid.GridInitialized -= GeneratePath;

    void GeneratePath()
    {
        _gridSize = Grid.GridSize;
        GeneratePathRoutine();
    }

    [ContextMenu("Reset Path")]
    public void ResetPath()
    {
        foreach (Vector2Int p in Path)
        {
            Grid.Cells[p].SetType(CellType.Ground);
        }
        Path.Clear();
        GeneratePathRoutine();
    }

    void GeneratePathRoutine()
    {
        int startY = Random.Range(1, _gridSize.y - 1);
        var currentPos = new Vector2Int(0, startY);
        var excludedPositions = new Queue<Vector2Int>();

        Path.Add(currentPos);
        Grid.Cells[currentPos].SetType(CellType.Road);

        while (currentPos.x < _gridSize.x - 1)
        {
            List<Vector2Int> validDirections = GetValidDirections(currentPos, excludedPositions);

            if (validDirections.Count == 0)
            {
                if (!BacktrackPath(ref currentPos, excludedPositions))
                    break;
                continue;
            }

            currentPos += GetWeightedRandomDirection(validDirections);
            Path.Add(currentPos);
            Grid.Cells[currentPos].SetType(CellType.Road);
        }

        PoolManager.Spawn(playerTriggerPrefab, Grid.GetWorldPos(currentPos), Quaternion.identity);
        ConvertPathToWorldPositions();
        PathGenerated?.Invoke();
    }

    Vector2Int GetWeightedRandomDirection(List<Vector2Int> validDirections)
    {
        // Calculate the sum of the weights
        float weightSum = 0;
        foreach (Vector2Int dir in validDirections)
            weightSum += _directionsWeights[dir];

        float randomTarget = Random.Range(0, weightSum);
        float runningTotal = 0;

        foreach (Vector2Int direction in validDirections)
        {
            runningTotal += _directionsWeights[direction];
            if (runningTotal >= randomTarget)
                return direction;
        }

        // Fallback to the first direction, should not happen
        Debug.LogWarning("Fallback to the first direction");
        return validDirections[0];
    }

    List<Vector2Int> GetValidDirections(Vector2Int current, Queue<Vector2Int> excluded)
    {
        var valid = new List<Vector2Int>();

        foreach (Vector2Int direction in _directions)
        {
            Vector2Int nextPos = current + direction;

            if (IsPositionValid(nextPos) &&
                !Path.Contains(nextPos) &&
                !excluded.Contains(nextPos) &&
                CountNeighbors(nextPos) < 2)
            {
                valid.Add(direction);
            }
        }

        return valid;
    }

    bool BacktrackPath(ref Vector2Int currentPos, Queue<Vector2Int> excluded)
    {
        if (Path.Count == 0) return false;

        excluded.Enqueue(currentPos);
        if (excluded.Count > MAX_EXCLUDED_HISTORY)
            excluded.Dequeue();

        Path.RemoveAt(Path.Count - 1);
        Grid.Cells[currentPos].SetType(CellType.Ground);

        if (Path.Count == 0) return false;

        currentPos = Path[^1];
        return true;
    }

    bool IsPositionValid(Vector2Int pos) =>
        pos.x >= 1 && pos.x < _gridSize.x &&
        pos.y >= 1 && pos.y < _gridSize.y - 1;

    int CountNeighbors(Vector2Int pos)
    {
        int count = 0;
        foreach (Vector2Int dir in _directions)
            if (Path.Contains(pos + dir))
                count++;
        return count;
    }

    void ConvertPathToWorldPositions()
    {
        PathWorldPositions.Clear();
        foreach (Vector2Int gridPos in Path)
        {
            PathWorldPositions.Add(Grid.GetWorldPos(gridPos));
        }
    }
}
