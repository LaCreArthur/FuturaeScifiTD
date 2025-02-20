using UnityEngine;

public class UIClickPanelManager : MonoBehaviour
{
    [SerializeField] RectTransform panelToShow;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] float edgePadding = 10f; // Padding from screen edges
    RectTransform _canvasRectTransform;
    void Awake()
    {
        _canvasRectTransform = mainCanvas.transform as RectTransform;
        panelToShow.gameObject.SetActive(false);
    }

    public void ShowPanelAtCell(Vector2Int cellPosition)
    {
        // Convert cell position to world position
        Vector3 worldPosition = Grid.GetWorldPos(cellPosition);

        // Convert world position to screen position
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        // Convert screen position to canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            screenPoint,
            mainCanvas.worldCamera,
            out Vector2 localPoint);

        // localPoint += offset;

        // Get panel and canvas dimensions
        Vector2 panelSize = panelToShow.sizeDelta;
        Vector2 canvasSize = _canvasRectTransform.sizeDelta;

        float cellSize = Grid.CellSize;
        float cellHalfWidth = cellSize / 2f;

        // Calculate minimum required offset (cell edge + padding + panel half-width)
        float minimumOffset = cellHalfWidth + edgePadding + panelSize.x / 2f;

        // Calculate available space with corrected offsets
        float rightEdge = canvasSize.x / 2 - edgePadding - panelSize.x / 2;
        float leftEdge = -canvasSize.x / 2 + edgePadding + panelSize.x / 2;

        // Determine placement based on true available space
        if (localPoint.x + minimumOffset <= rightEdge)
        {
            // Place right with cell edge + padding
            localPoint.x += cellHalfWidth + edgePadding + panelSize.x / 2;
        }
        else if (localPoint.x - minimumOffset >= leftEdge)
        {
            // Place left with cell edge + padding
            localPoint.x -= cellHalfWidth + edgePadding + panelSize.x / 2;
        }
        else
        {
            // Handle edge case with smart positioning
            float rightPush = rightEdge - panelSize.x / 2;
            float leftPush = leftEdge + panelSize.x / 2;
            localPoint.x = Mathf.Abs(localPoint.x - rightPush) < Mathf.Abs(localPoint.x - leftPush)
                ? rightPush
                : leftPush;
        }

        // Final position clamp
        localPoint.x = Mathf.Clamp(
            localPoint.x,
            leftEdge + panelSize.x / 2,
            rightEdge - panelSize.x / 2
        );

        panelToShow.localPosition = localPoint;
        panelToShow.gameObject.SetActive(true);
    }
}
