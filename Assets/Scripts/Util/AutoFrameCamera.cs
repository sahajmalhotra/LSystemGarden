using UnityEngine;

/// <summary>
/// AutoFrameCamera
/// ----------------
/// Automatically positions and orients the camera so that
/// all geometry under a target root (e.g., PlantRoot)
/// stays visible in frame.
/// 
/// This is useful for procedural systems (like L-systems)
/// where the size of the generated object changes dynamically.
/// 
/// The script:
/// 1. Calculates the combined bounding box of all Renderers under targetRoot.
/// 2. Determines the size and center of the object.
/// 3. Positions the camera at a scaled distance.
/// 4. Smoothly interpolates toward that position.
/// 5. Continuously looks at the center.
/// </summary>
public class AutoFrameCamera : MonoBehaviour
{
    /// <summary>
    /// Root transform that contains all generated objects.
    /// Example: PlantRoot in your L-system scene.
    /// </summary>
    public Transform targetRoot;

    /// <summary>
    /// Multiplies object size to determine camera distance.
    /// Larger value = camera farther away.
    /// </summary>
    public float distanceMultiplier = 2.2f;

    /// <summary>
    /// Controls how high above the center the camera sits.
    /// Relative to object size.
    /// </summary>
    public float heightMultiplier = 0.6f;

    /// <summary>
    /// Minimum camera distance regardless of object size.
    /// Prevents camera from getting too close.
    /// </summary>
    public float minDistance = 3f;

    /// <summary>
    /// Controls interpolation speed.
    /// Higher value = faster camera movement.
    /// </summary>
    public float smooth = 10f;

    // Cached reference to Camera component
    Camera cam;

    /// <summary>
    /// Awake is called when the script instance is loaded.
    /// Here we cache the Camera component for potential future use.
    /// </summary>
    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    /// <summary>
    /// LateUpdate ensures camera updates after all object movements.
    /// This avoids jitter and ensures accurate framing.
    /// </summary>
    void LateUpdate()
    {
        // If no target assigned or nothing generated, exit
        if (targetRoot == null || targetRoot.childCount == 0) return;

        // ============================================================
        // Step 1: Compute combined bounding box of all Renderers
        // ============================================================

        Bounds b = new Bounds(targetRoot.position, Vector3.zero);
        bool hasBounds = false;

        // Get all Renderer components under targetRoot
        var renderers = targetRoot.GetComponentsInChildren<Renderer>();

        foreach (var r in renderers)
        {
            if (!hasBounds)
            {
                // First renderer initializes bounds
                b = r.bounds;
                hasBounds = true;
            }
            else
            {
                // Expand bounds to include this renderer
                b.Encapsulate(r.bounds);
            }
        }

        // If no renderers found, exit safely
        if (!hasBounds) return;

        // ============================================================
        // Step 2: Extract center and overall size
        // ============================================================

        Vector3 center = b.center;

        // Use the largest dimension to determine camera distance
        float size = Mathf.Max(b.size.x, b.size.y, b.size.z);

        // ============================================================
        // Step 3: Compute desired camera position
        // ============================================================

        float dist = Mathf.Max(minDistance, size * distanceMultiplier);

        Vector3 desiredPos = center + new Vector3(
            0,
            size * heightMultiplier,
            -dist
        );

        // ============================================================
        // Step 4: Smooth movement and look-at
        // ============================================================

        // Smoothly interpolate position
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            Time.deltaTime * smooth
        );

        // Always look at object center
        transform.LookAt(center);
    }
}