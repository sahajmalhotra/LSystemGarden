using System.Collections.Generic;

public enum PlantMode
{
    Tree,
    Bush
}

public class LSystem
{
    public List<Symbol> axiom;
    public int iterations;

    public float angle;
    public float lengthScale;
    public float radiusScale;

    public int leafStartAge = 3;
    public float leafSizeMultiplier = 1f;

    public bool includePitch = true;
    public float pitchChance = 0.35f; // deterministic pattern (not random)

    public PlantMode mode = PlantMode.Tree;

    public LSystem(
        List<Symbol> axiom,
        int iterations,
        float angle,
        float lengthScale,
        float radiusScale)
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
                if (mode == PlantMode.Tree)
                    ExpandTreeF(s, output);
                else
                    ExpandBushF(s, output);
            }
            else
            {
                output.Add(s.Clone());
            }
        }

        return output;
    }

    private void ExpandTreeF(Symbol s, List<Symbol> output)
    {
        // Main trunk continuation
        output.Add(new Symbol('F', s.length * lengthScale, s.radius * radiusScale, s.age + 1));

        // Left branch
        output.Add(new Symbol('['));
        output.Add(new Symbol('+'));
        if (includePitch && ((s.age % 10) < (int)(pitchChance * 10f))) output.Add(new Symbol('&'));
        output.Add(new Symbol('F', s.length * lengthScale * 0.85f, s.radius * radiusScale * 0.85f, s.age + 1));
        if (s.age >= leafStartAge) output.Add(new Symbol('L', leafSizeMultiplier, 0f, s.age));
        output.Add(new Symbol(']'));

        // Right branch
        output.Add(new Symbol('['));
        output.Add(new Symbol('-'));
        if (includePitch && ((s.age % 10) >= (int)(pitchChance * 10f))) output.Add(new Symbol('^'));
        output.Add(new Symbol('F', s.length * lengthScale * 0.85f, s.radius * radiusScale * 0.85f, s.age + 1));
        if (s.age >= leafStartAge) output.Add(new Symbol('L', leafSizeMultiplier, 0f, s.age));
        output.Add(new Symbol(']'));
    }

    private void ExpandBushF(Symbol s, List<Symbol> output)
{
    // Bush goal: less upward trunk, more sideways branching and curl.
    float l = s.length * lengthScale;
    float r = s.radius * radiusScale;
    int a = s.age + 1;

    // Short forward step (keeps it connected but not tall)
    output.Add(new Symbol('F', l * 0.65f, r * 0.95f, a));

    // Left branch (stronger, with pitch)
    output.Add(new Symbol('['));
    output.Add(new Symbol('+'));
    if (includePitch) output.Add(new Symbol('&'));
    output.Add(new Symbol('F', l * 0.85f, r * 0.80f, a));
    if (s.age >= leafStartAge) output.Add(new Symbol('L', leafSizeMultiplier * 1.2f, 0f, s.age));
    output.Add(new Symbol(']'));

    // Right branch (stronger, with pitch)
    output.Add(new Symbol('['));
    output.Add(new Symbol('-'));
    if (includePitch) output.Add(new Symbol('^'));
    output.Add(new Symbol('F', l * 0.85f, r * 0.80f, a));
    if (s.age >= leafStartAge) output.Add(new Symbol('L', leafSizeMultiplier * 1.2f, 0f, s.age));
    output.Add(new Symbol(']'));

    // Optional: add a sideways “filler” leaf cluster (makes it bushy)
    if (s.age >= leafStartAge)
        output.Add(new Symbol('L', leafSizeMultiplier * 1.0f, 0f, s.age));
}
}