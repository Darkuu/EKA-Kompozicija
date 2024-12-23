using UnityEngine;
using UnityEngine.EventSystems;  // Required for checking UI interaction

public class Artwork : MonoBehaviour
{
    [Header("User Defined Properties")]
    public string colorName;
    public string style;
    public bool isValid;
    public bool isGraded = false;
    public bool wasGradedCorrectly = false;

    [Header("Drag Settings")]
    private bool isDragging = false;
    private Vector3 offset;

    // LayerMask to ignore layers when we are trying to drag
    public LayerMask blockLayer; // Use this to specify the layer of objects that block dragging (e.g., StampingTool)

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;  // Get the main camera
    }

    private void Update()
    {
        HandleDrag();
    }

    // Check if we can drag the artwork
    private bool CanDragArtwork()
    {
        // Ignore if the mouse is over any UI element (like a button)
        if (EventSystem.current.IsPointerOverGameObject())  // Checks if the pointer is over a UI element
        {
            return false;
        }

        // Check if the mouse is over the stamping tool
        RaycastHit2D hit = Physics2D.Raycast(GetMouseWorldPosition(), Vector2.zero, Mathf.Infinity, blockLayer);
        
        // If we hit an object on the block layer (like the stamping tool), prevent dragging
        if (hit.collider != null)
        {
            return false;
        }

        return true; // Allow dragging if nothing blocks it
    }

    private void OnMouseDown()
    {
        if (CanDragArtwork())  // Only allow dragging if nothing is blocking the raycast
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
