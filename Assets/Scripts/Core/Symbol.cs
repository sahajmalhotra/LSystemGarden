using System;

/// <summary>
/// Symbol
/// ------
/// Represents a single symbol in a parametric L-system.
/// 
/// Unlike simple L-systems that only store characters (e.g., 'F', '+'),
/// this class supports parameters such as:
/// - length  → branch length
/// - radius  → branch thickness
/// - age     → used for coloring or growth simulation
/// 
/// This allows the plant system to simulate more realistic,
/// biologically inspired growth.
/// </summary>
[Serializable] // Allows Unity to serialize this class (visible in Inspector if used in lists)
public class Symbol
{
    /// <summary>
    /// The core character of the symbol.
    /// Examples:
    /// 'F' = draw forward
    /// '+' = rotate
    /// '[' = push state
    /// etc.
    /// </summary>
    public char letter;

    // ============================================================
    // Parametric Values
    // ============================================================

    /// <summary>
    /// Length parameter.
    /// Commonly used for branch segment length.
    /// </summary>
    public float length;

    /// <summary>
    /// Radius (thickness) parameter.
    /// Used when generating cylindrical branch geometry.
    /// </summary>
    public float radius;

    /// <summary>
    /// Age parameter.
    /// Used for:
    /// - Color interpolation
    /// - Growth stages
    /// - Leaf spawning decisions
    /// </summary>
    public int age;

    /// <summary>
    /// Constructor for creating a parametric symbol.
    /// 
    /// Default parameters are zero, allowing creation of
    /// simple non-parametric symbols if desired.
    /// </summary>
    /// <param name="letter">Symbol character</param>
    /// <param name="length">Branch length</param>
    /// <param name="radius">Branch thickness</param>
    /// <param name="age">Growth age</param>
    public Symbol(char letter, float length = 0f, float radius = 0f, int age = 0)
    {
        this.letter = letter;
        this.length = length;
        this.radius = radius;
        this.age = age;
    }

    /// <summary>
    /// Creates a deep copy of this Symbol.
    /// 
    /// Important in L-systems to avoid modifying original
    /// symbols during rewriting/production steps.
    /// </summary>
    public Symbol Clone()
    {
        return new Symbol(letter, length, radius, age);
    }

    /// <summary>
    /// Returns a formatted string representation of the symbol.
    /// 
    /// Example output:
    /// F(1.00,0.05,3)
    /// 
    /// Useful for:
    /// - Debug logging
    /// - Visualizing generated strings
    /// - Testing production rules
    /// </summary>
    public override string ToString()
    {
        return $"{letter}({length:F2},{radius:F2},{age})";
    }
}