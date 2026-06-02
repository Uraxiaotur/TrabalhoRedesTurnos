using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    private Camera mainCamera;
    [SerializeField] private float maxSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame

    public void FollowMousePosition()
    {
        transform.position = GetMousePosition();
    }

    private Vector2 GetMousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
