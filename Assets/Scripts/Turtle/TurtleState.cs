using UnityEngine;

public struct TurtleState
{
    public Vector3 position;
    public Quaternion rotation;
    public float radius;
    public int age;

    public TurtleState(Vector3 position, Quaternion rotation, float radius, int age)
    {
        this.position = position;
        this.rotation = rotation;
        this.radius = radius;
        this.age = age;
    }
}