using UnityEngine;
using Cinemachine;

public class DynamicCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    public bool IsTwo = false;

    private CinemachineTargetGroup targetGroup;

    private void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Update()
    {
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        if (players.Length == 2)
        {
            virtualCamera.transform.position = new Vector3((
                   players[0].transform.position.x + players[1].transform.position.x) / 2
                , (players[0].transform.position.y + players[1].transform.position.y) / 2
                , -10);
            targetGroup.m_Targets[0].target = players[0].transform;
            targetGroup.m_Targets[1].target = players[1].transform;
            IsTwo = true;
        }
        else
        {
            IsTwo = false;
        }
  
    }
    private void LateUpdate()
    {
        if (targetGroup == null || virtualCamera == null) return;

        AdjustZoom();
    }

    void AdjustZoom()
    {
        if (IsTwo)
        {
            float distance = GetGreatestDistance();
            float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
            virtualCamera.m_Lens.OrthographicSize = newZoom;
        }
        

    }

    float GetGreatestDistance()
    {
        if (targetGroup.m_Targets.Length < 2) return 0f;

        Bounds bounds = new Bounds(targetGroup.m_Targets[0].target.position, Vector3.zero);

        for (int i = 1; i < targetGroup.m_Targets.Length; i++)
        {
            bounds.Encapsulate(targetGroup.m_Targets[i].target.position);
        }

        return bounds.size.x;
    }
}