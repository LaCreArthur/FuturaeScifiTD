using UnityEngine;

public class UIClickPanelManager : MonoBehaviour
{
    [SerializeField] RectTransform panelToShow;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] float edgePadding = 10f; // Padding from screen edges
    [SerializeField] float offsetFromCell = 50f; // Fixed offset in canvas units

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

        // Convert to screen position
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        // Convert to canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            screenPoint,
            mainCanvas.worldCamera,
            out Vector2 localPoint);

        // Get panel and canvas dimensions
        Vector2 panelSize = panelToShow.sizeDelta;
        Vector2 canvasSize = _canvasRectTransform.sizeDelta;

        // Calculate canvas bounds
        float leftBound = -canvasSize.x / 2 + edgePadding;
        float rightBound = canvasSize.x / 2 - edgePadding;

        // Desired offset direction (right by default)
        float offsetDirection = 1f; // 1 for right, -1 for left
        float adjustedOffset = offsetFromCell;

        // Check if placing on the right exceeds the right bound
        float rightSidePos = localPoint.x + offsetFromCell + panelSize.x / 2;
        if (rightSidePos > rightBound)
        {
            offsetDirection = -1f; // Flip to left
        }

        // Check if placing on the left exceeds the left bound
        float leftSidePos = localPoint.x - offsetFromCell - panelSize.x / 2;
        if (leftSidePos < leftBound && offsetDirection == 1f)
        {
            offsetDirection = 1f; // Stay right if both sides fail, adjust later
        }

        // Apply the offset
        localPoint.x += offsetDirection * (offsetFromCell + panelSize.x / 2);

        // Clamp to keep fully on-screen
        float minX = leftBound + panelSize.x / 2;
        float maxX = rightBound - panelSize.x / 2;
        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);

        // Set position and show
        panelToShow.localPosition = localPoint;
        panelToShow.gameObject.SetActive(true);
    }
}
