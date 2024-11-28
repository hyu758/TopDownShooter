using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    
    private float minX, maxX, minY, maxY;

    void Awake()
    {
        target = LevelManager.Instance.getPlayer().transform;
    }
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Giới hạn vị trí camera
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, desiredPosition.y, -10), smoothSpeed);
            transform.position = smoothedPosition;
        }
    }


    public void SetBounds(float mapWidth, float mapHeight, float cameraWidth, float cameraHeight)
    {
        // Tính giới hạn
        minX = cameraWidth / 2;
        maxX = mapWidth - cameraWidth / 2;

        minY = cameraHeight / 2;
        maxY = mapHeight - cameraHeight / 2;
    }
}