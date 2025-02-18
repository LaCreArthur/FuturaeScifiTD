using UnityEngine;

public class TowerVisuals : MonoBehaviour
{
    [SerializeField] GameObject rangeDisplay;
    [SerializeField] Material defaultMat;
    [SerializeField] Material previewMat;

    MeshRenderer[] _renderers;

    void Awake()
    {
        //todo: check if it is the right tower
        BuildingManager.StartBuilding += OnStartBuilding;
        BuildingManager.StopBuilding += OnStopBuilding;
        _renderers = GetComponentsInChildren<MeshRenderer>();
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
        SetMaterial(show ? previewMat : defaultMat);
    }

    void SetMaterial(Material mat)
    {
        foreach (MeshRenderer r in _renderers)
        {
            r.material = mat;
        }
    }
}
