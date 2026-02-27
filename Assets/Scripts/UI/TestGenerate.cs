using System.Collections.Generic;
using UnityEngine;

public class TestGenerate : MonoBehaviour
{
    public Turtle3D turtle;

    [Header("Test Params")]
    public int iterations = 5;
    public float angle = 25f;
    public float lengthScale = 0.75f;
    public float radiusScale = 0.7f;

    void Start()
    {
        if (turtle == null)
        {
            Debug.LogError("TestGenerate: Turtle reference not assigned.");
            return;
        }

        // Axiom (starting symbol)
        var axiom = new List<Symbol> { new Symbol('F', 1.2f, 0.08f, 0) };

        // Generate
        LSystem sys = new LSystem(axiom, iterations, angle, lengthScale, radiusScale);
        var result = sys.Generate();

        // Render
        turtle.Interpret(result, angle, clearFirst: true);
    }
}
