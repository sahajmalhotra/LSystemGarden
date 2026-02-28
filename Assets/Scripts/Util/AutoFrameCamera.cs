using UnityEngine;

public class AutoFrameCamera : MonoBehaviour
{
    public Transform targetRoot;          // PlantRoot
    public float distanceMultiplier = 2.2f;
    public float heightMultiplier = 0.6f;
    public float minDistance = 3f;
    public float smooth = 10f;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targetRoot == null || targetRoot.childCount == 0) return;

        // Compute bounds of everything under PlantRoot
        Bounds b = new Bounds(targetRoot.position, Vector3.zero);
        bool hasBounds = false;

        var renderers = targetRoot.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            if (!hasBounds)
            {
                b = r.bounds;
                hasBounds = true;
            }
            else b.Encapsulate(r.bounds);
        }
        if (!hasBounds) return;

        Vector3 center = b.center;
        float size = Mathf.Max(b.size.x, b.size.y, b.size.z);

        float dist = Mathf.Max(minDistance, size * distanceMultiplier);
        Vector3 desiredPos = center + new Vector3(0, size * heightMultiplier, -dist);

        // Smooth move + look
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smooth);
        transform.LookAt(center);
    }
}