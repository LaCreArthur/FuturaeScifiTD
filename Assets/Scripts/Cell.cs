using System;
using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellType type;
    public Vector2Int position;
    
    public Material groundMaterial;
    public Material roadMaterial;
    public Material buildingMaterial;
    public Material validMaterial;
    public Material invalidMaterial;
    
    MeshRenderer _meshRenderer;
    Material _lastMaterial;

    public void Initialize(Vector2Int position, CellType type)
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        this.position = position;
        gameObject.name = $"Cell {position}";
        SetType(type);
    }
    
    public void SetType(CellType type)
    {
        this.type = type;
        switch (type)
        {
            case CellType.Ground:
                _meshRenderer.material = groundMaterial;
                break;
            case CellType.Road:
                _meshRenderer.material = roadMaterial;
                break;
            case CellType.Building:
                _meshRenderer.material = buildingMaterial;
                break;
        }
    }
    public void SetValidMaterial()
    {
        _lastMaterial = _meshRenderer.material;
        _meshRenderer.material = type == CellType.Ground ? validMaterial : _meshRenderer.material = invalidMaterial;
    }
    
    public void ResetMaterial()
    {
        _meshRenderer.material = _lastMaterial;
    }
}

public enum CellType
{
    Ground,
    Road,
    Building,
}
