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
    }

    private void Update()
    {
        HandleDrag();
    }

    /// <summary>
    /// Determines if the artwork can be dragged.
    /// </summary>
    /// <returns>True if draggable; false otherwise.</returns>
    private bool CanDragArtwork()
    {
        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        // Perform a raycast to detect blocking objects
        Vector2 mousePosition = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, blockLayer);

        return hit.collider == null; // Allow dragging if nothing is blocking
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