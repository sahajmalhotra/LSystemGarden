using System.Collections.Generic;
using UnityEngine;

// Simple test component that generates and renders an L-System plant at startup
public class TestGenerate : MonoBehaviour
{
    // Reference to the Turtle3D renderer that interprets L-System symbols
    public Turtle3D turtle;

    [Header("Test Params")]

    // Number of L-System iterations (growth generations)
    public int iterations = 5;

    // Branching angle in degrees
    public float angle = 25f;

    // Length reduction multiplier per generation
    public float lengthScale = 0.75f;

    // Radius (thickness) reduction multiplier per generation
    public float radiusScale = 0.7f;

    void Start()
    {
        // Ensure the turtle reference is assigned in the Inspector
        if (turtle == null)
        {
            Debug.LogError("TestGenerate: Turtle reference not assigned.");
            return;
        }

        // Axiom (starting symbol)
        // Creates the initial trunk segment:
        // 'F' = forward draw, length = 1.2, radius = 0.08, age = 0
        var axiom = new List<Symbol> { new Symbol('F', 1.2f, 0.08f, 0) };

        // Generate the L-System using provided parameters
        LSystem sys = new LSystem(axiom, iterations, angle, lengthScale, radiusScale);

        // Produce the final symbol sequence after all iterations
        var result = sys.Generate();

        // Render the generated structure using the turtle interpreter
        // clearFirst: true ensures previous geometry is cleared before drawing
        turtle.Interpret(result, angle, clearFirst: true);
    }
}