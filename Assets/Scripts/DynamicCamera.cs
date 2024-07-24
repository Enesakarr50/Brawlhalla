using UnityEditor.U2D.Animation;
using UnityEngine;


public class DynamicCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;
    public Camera mainCamera;

   
    private void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        MoveCamera();
        ZoomCamera();
    }

    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players == null || players.Length == 2)
        {
            player1 = players[0].transform;
            player2 = players[1].transform;
        } 
        
    }
    void MoveCamera()
    {
        Vector3 middlePoint = (player1.position + player2.position) / 2;
        Vector3 desiredPosition = middlePoint + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void ZoomCamera()
    {
        float distance = Vector3.Distance(player1.position, player2.position);
        float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, newZoom, Time.deltaTime);
    }
}
