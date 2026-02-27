using System;

[Serializable]
public class Symbol
{
    public char letter;

    // Parametric values
    public float length;
    public float radius;
    public int age;

    public Symbol(char letter, float length = 0f, float radius = 0f, int age = 0)
    {
        this.letter = letter;
        this.length = length;
        this.radius = radius;
        this.age = age;
    }

    public Symbol Clone()
    {
        return new Symbol(letter, length, radius, age);
    }

    public override string ToString()
    {
        return $"{letter}({length:F2},{radius:F2},{age})";
    }
}