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
        BuildingManager.StopBuilding += OnStopBuilding;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in renderers)
        {
            _rendererMaterial[meshRenderer] = meshRenderer.sharedMaterials;
        }
    }

    void OnDestroy()
    {
        BuildingManager.StartBuilding -= OnStartBuilding;
        BuildingManager.StopBuilding -= OnStopBuilding;
    }

    void OnStartBuilding() => TogglePreviewDisplay(true);
    void OnStopBuilding() => TogglePreviewDisplay(false);
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
