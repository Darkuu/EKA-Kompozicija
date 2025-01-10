using UnityEngine;
using UnityEngine.EventSystems;

public class Artwork : MonoBehaviour
{
    [Header("User Defined Properties")]
    public string style;
    public bool isValid;
    public bool isGraded = false;
    public bool wasGradedCorrectly = false;

    [Header("Drag Settings")]
    private bool isDragging = false;
    private Vector3 offset;

    [Tooltip("Specify the LayerMask for objects that block dragging (e.g., StampingTool)")]
    public LayerMask blockLayer;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Validate style on initialization
        if (RatingManager.instance != null)
        {
            ValidateStyle();
        }
    }

    private void Update()
    {
        HandleDrag();
    }

    public void ValidateStyle()
    {
        isValid = string.Equals(style, RatingManager.instance.currentValidStyle, System.StringComparison.OrdinalIgnoreCase);
    }

    private bool CanDragArtwork()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        Vector2 mousePosition = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, blockLayer);

        return hit.collider == null;
    }

    private void OnMouseDown()
    {
        if (CanDragArtwork())
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void HandleDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}