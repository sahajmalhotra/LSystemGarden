using System.Collections.Generic;
using UnityEngine;

public class Turtle3D : MonoBehaviour
{
    [Header("Scene")]
    public Transform plantRoot;

    [Header("Base Materials (assigned in Inspector)")]
    public Material branchMaterial;
    public Material barkMaterial;   // optional (can be same as branchMaterial)
    public Material leafMaterial;

    [Header("Rendering")]
    public float defaultAngleDeg = 25f;
    public int maxAgeForColor = 8;

    [Header("Leaf Shape")]
    public float leafWidth = 0.08f;
    public float leafLength = 0.16f;

    private Stack<TurtleState> stack = new Stack<TurtleState>();

    // Call this from a manager
    public void ClearPlant()
    {
        if (plantRoot == null) return;

        for (int i = plantRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(plantRoot.GetChild(i).gameObject);
        }
    }

    public void Interpret(List<Symbol> symbols, float angleDeg, bool clearFirst = true)
    {
        if (plantRoot == null)
        {
            Debug.LogError("Turtle3D: plantRoot not assigned.");
            return;
        }
        if (branchMaterial == null || leafMaterial == null)
        {
            Debug.LogError("Turtle3D: assign branchMaterial and leafMaterial in Inspector.");
            return;
        }

        if (clearFirst) ClearPlant();
        stack.Clear();

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        float currentAngle = (angleDeg > 0f) ? angleDeg : defaultAngleDeg;

        for (int i = 0; i < symbols.Count; i++)
        {
            Symbol s = symbols[i];

            switch (s.letter)
            {
                case 'F':
                    DrawBranch(ref pos, rot, s.length, s.radius, s.age);
                    break;

                case 'L':
                    DrawLeaf(pos, rot, s.length > 0 ? s.length : 1f, s.age);
                    break;

                case '+': // yaw left
                    rot = rot * Quaternion.Euler(0f, currentAngle, 0f);
                    break;

                case '-': // yaw right
                    rot = rot * Quaternion.Euler(0f, -currentAngle, 0f);
                    break;

                case '&': // pitch down
                    rot = rot * Quaternion.Euler(currentAngle, 0f, 0f);
                    break;

                case '^': // pitch up
                    rot = rot * Quaternion.Euler(-currentAngle, 0f, 0f);
                    break;

                case '[':
                    // Save state
                    stack.Push(new TurtleState(pos, rot, s.radius, s.age));
                    break;

                case ']':
                    // Restore state
                    if (stack.Count > 0)
                    {
                        TurtleState st = stack.Pop();
                        pos = st.position;
                        rot = st.rotation;
                    }
                    break;
            }
        }
    }

    private void DrawBranch(ref Vector3 pos, Quaternion rot, float length, float radius, int age)
    {
        if (length <= 0.001f) length = 0.1f;
        if (radius <= 0.001f) radius = 0.01f;

        Vector3 dir = rot * Vector3.up;
        Vector3 start = pos;
        Vector3 end = pos + dir * length;

        // Pick bark vs branch material (optional)
        Material baseMat = (barkMaterial != null) ? barkMaterial : branchMaterial;

        // Create a per-segment instance so we can color by age without recoloring all branches
        Material segMat = new Material(baseMat);
        segMat.color = BarkColorByAge(age);

        GameObject cyl = LGeom.Cylinder(start, end, radius, segMat, "Branch");
        cyl.transform.SetParent(plantRoot, worldPositionStays: true);

        pos = end;

        // Optional: add leaves near tips (age threshold)
        if (age >= 3)
        {
            // tiny chance of leaf clustering; deterministic simple method:
            DrawLeaf(pos, rot, 1f, age);
        }
    }

    private void DrawLeaf(Vector3 pos, Quaternion rot, float sizeMult, int age)
    {
        // Simple leaf polygon (fan triangulation wants a convex-ish outline)
        float w = leafWidth * sizeMult;
        float l = leafLength * sizeMult;

        // Leaf in local plane (XZ), then rotate into world
        List<Vector3> ptsLocal = new List<Vector3>
        {
            new Vector3(0f, 0f, 0f),         // base
            new Vector3(-w, 0f, l * 0.35f),
            new Vector3(-w * 0.6f, 0f, l * 0.8f),
            new Vector3(0f, 0f, l),          // tip
            new Vector3(w * 0.6f, 0f, l * 0.8f),
            new Vector3(w, 0f, l * 0.35f)
        };

        // Orient leaf: face roughly outward from branch
        Quaternion leafRot = rot * Quaternion.Euler(0f, 0f, 90f); // rotate into a nice plane

        List<Vector3> ptsWorld = new List<Vector3>(ptsLocal.Count);
        for (int i = 0; i < ptsLocal.Count; i++)
        {
            ptsWorld.Add(pos + (leafRot * ptsLocal[i]));
        }

        Material leafMatInstance = new Material(leafMaterial);
        leafMatInstance.color = LeafColorByAge(age);

        GameObject leaf = LGeom.FilledPolygon_Fan(ptsWorld, leafMatInstance, "Leaf");
        if (leaf != null)
        {
            leaf.transform.SetParent(plantRoot, worldPositionStays: true);
        }
    }

    private Color BarkColorByAge(int age)
    {
        float t = Mathf.Clamp01((float)age / Mathf.Max(1, maxAgeForColor));
        // darker -> lighter brown
        Color dark = new Color(0.25f, 0.18f, 0.10f);
        Color light = new Color(0.55f, 0.38f, 0.20f);
        return Color.Lerp(dark, light, t);
    }

    private Color LeafColorByAge(int age)
    {
        float t = Mathf.Clamp01((float)age / Mathf.Max(1, maxAgeForColor));
        Color fresh = new Color(0.20f, 0.75f, 0.25f);
        Color old = new Color(0.08f, 0.35f, 0.12f);
        return Color.Lerp(fresh, old, t);
    }
}