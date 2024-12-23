using UnityEngine;

public class ClickMoveInOut : MonoBehaviour
{
    public Vector3 inPosition;  
    public Vector3 outPosition; 
    public float moveSpeed = 5f; 
    private bool isMovingOut = false; 

    public GameObject otherObject; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Raycast to check if the click is on the object
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            // If the raycast hits this object or the otherObject, toggle the position
            if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject == otherObject))
            {
                isMovingOut = !isMovingOut;
            }
        }
        if (isMovingOut)
        {
            transform.position = Vector3.MoveTowards(transform.position, outPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, inPosition, moveSpeed * Time.deltaTime);
        }
    }
}