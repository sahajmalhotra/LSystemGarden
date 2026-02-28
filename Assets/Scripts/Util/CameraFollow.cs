using UnityEngine;

/// <summary>
/// CameraFollow
/// ------------
/// Simple third-person camera follow script.
/// 
/// This script keeps the camera positioned at a fixed offset
/// relative to a target Transform (for example, a player or plant).
/// 
/// It also ensures the camera continuously looks at the target,
/// creating a smooth tracking behavior.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// The object the camera should follow.
    /// Typically assigned in the Unity Inspector.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Positional offset from the target.
    /// 
    /// Example:
    /// (0, 2, -6) means:
    /// - 0 units sideways
    /// - 2 units above the target
    /// - 6 units behind the target
    /// </summary>
    public Vector3 offset = new Vector3(0, 2, -6);

    /// <summary>
    /// LateUpdate is called after all Update() calls.
    /// 
    /// This is important for camera movement because:
    /// - The target may move in Update().
    /// - The camera adjusts AFTER the target moves,
    ///   preventing jitter or frame delay.
    /// </summary>
    void LateUpdate()
    {
        // If no target is assigned, do nothing
        if (target == null) return;

        // Position the camera at target position + offset
        transform.position = target.position + offset;

        // Rotate the camera to look slightly above the target's center
        // (1.5f upward creates a more natural framing)
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}