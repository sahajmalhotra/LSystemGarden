using UnityEngine;

/*
 * TurtleState.cs
 * --------------
 * Purpose:
 * Represents the complete state of the turtle at a specific moment in time.
 * 
 * In an L-system using bracketed branching (e.g., [ and ]),
 * we must SAVE the turtle’s state when we encounter '['
 * and RESTORE it when we encounter ']'.
 * 
 * This struct stores all information needed to restore that state.
 * 
 * Why use a struct?
 * - Lightweight value type
 * - Efficient to push/pop from a stack
 * - Perfect for temporary state snapshots
 */

public struct TurtleState
{
    // --------------------------
    // Turtle Transform Data
    // --------------------------

    // Current world position of the turtle
    // This is where the next branch segment will start.
    public Vector3 position;

    // Current orientation of the turtle
    // This determines the direction of growth.
    public Quaternion rotation;

    // --------------------------
    // Procedural Attributes
    // --------------------------

    // Current branch radius (used for tapering branches).
    // As branches get deeper in recursion, this typically decreases.
    public float radius;

    // Age of the current branch segment.
    // This can be used for:
    // - Color variation
    // - Leaf spawning
    // - Thickness variation
    // - Growth logic
    public int age;

    /*
     * Constructor
     * -----------
     * Initializes a complete turtle state snapshot.
     * 
     * Called when pushing state onto the stack:
     * stack.Push(new TurtleState(...));
     * 
     * This allows exact restoration later.
     */
    public TurtleState(Vector3 position, Quaternion rotation, float radius, int age)
    {
        this.position = position;
        this.rotation = rotation;
        this.radius = radius;
        this.age = age;
    }
}