using System.Collections.Generic;

public class LSystem
{
    public List<Symbol> axiom;
    public int iterations;

    public float angle;
    public float lengthScale;
    public float radiusScale;

    public LSystem(List<Symbol> axiom, int iterations, float angle, float lengthScale, float radiusScale)
    {
        this.axiom = axiom;
        this.iterations = iterations;
        this.angle = angle;
        this.lengthScale = lengthScale;
        this.radiusScale = radiusScale;
    }

    public List<Symbol> Generate()
    {
        List<Symbol> current = new List<Symbol>(axiom);

        for (int i = 0; i < iterations; i++)
        {
            current = Iterate(current);
        }

        return current;
    }

    private List<Symbol> Iterate(List<Symbol> input)
    {
        List<Symbol> output = new List<Symbol>();

        foreach (Symbol s in input)
        {
            if (s.letter == 'F')
            {
                // trunk continuation
                output.Add(new Symbol('F',
                    s.length * lengthScale,
                    s.radius * radiusScale,
                    s.age + 1));

                // branch left
                output.Add(new Symbol('['));
                output.Add(new Symbol('+'));
                output.Add(new Symbol('F',
                    s.length * lengthScale,
                    s.radius * radiusScale,
                    s.age + 1));
                output.Add(new Symbol(']'));

                // branch right
                output.Add(new Symbol('['));
                output.Add(new Symbol('-'));
                output.Add(new Symbol('F',
                    s.length * lengthScale,
                    s.radius * radiusScale,
                    s.age + 1));
                output.Add(new Symbol(']'));
            }
            else
            {
                output.Add(s.Clone());
            }
        }

        return output;
    }
}