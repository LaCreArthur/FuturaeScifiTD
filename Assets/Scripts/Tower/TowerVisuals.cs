using System.Collections.Generic;
using UnityEngine;

public class TowerVisuals : MonoBehaviour
{
    [SerializeField] GameObject rangeDisplay;
    [SerializeField] Material previewMat;

    public readonly Dictionary<MeshRenderer, Material[]> _rendererMaterial = new Dictionary<MeshRenderer, Material[]>();
    void Awake()
    {
        //todo: check if it is the right tower
        BuildingManager.StartBuilding += OnStartBuilding;
        //todo: should be refactored because we could cancel the building
        BuildingManager.SuccessBuilding += OnSuccessBuilding;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in renderers)
        {
            _rendererMaterial[meshRenderer] = meshRenderer.sharedMaterials;
        }
    }

    void OnDestroy()
    {
        BuildingManager.StartBuilding -= OnStartBuilding;
        BuildingManager.SuccessBuilding -= OnSuccessBuilding;
    }

    void OnStartBuilding() => TogglePreviewDisplay(true);
    void OnSuccessBuilding() => TogglePreviewDisplay(false);
    void TogglePreviewDisplay(bool show)
    {
        rangeDisplay.SetActive(show);
        SetMaterial(show);
    }

    void SetMaterial(bool show)
    {
        foreach (MeshRenderer r in _rendererMaterial.Keys)
        {
            r.sharedMaterials = show ? new[] { previewMat } : _rendererMaterial[r];
        }
    }
}
