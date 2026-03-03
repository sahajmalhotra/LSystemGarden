CPSC 565 – Assignment 4
3D L-System Garden (Unity)
Name: Sahaj Malhotra
Course: CPSC 565 – Emergent Computing
Instructor: Christian Jacob
Engine: Unity
Language: C#
Final Testing Scene: Final Scene

Project Overview

This project implements a 3D deterministic context-free L-System (D0L-System) with bracketing in Unity. The system generates procedural plant structures using string rewriting and interprets them into 3D geometry using a turtle graphics approach.
The implementation includes:

Full D0L-System with bracketing
Two distinct plant rule sets
Parametric L-System extension
Progressive growth over time (complex feature)
Adjustable morphology parameters
Real-time UI interaction
Visually pleasing 3D plant rendering

The scene used for grading and final testing is: Final Scene

Formal Definition of the L-System

The implemented system follows the formal definition: G=(Σ,P,α)

Where:
Σ (Alphabet) = Set of symbols
α (Axiom) = Starting string
P (Production Rules) = Deterministic mapping from symbols to strings

This is a D0L-System, meaning:

Deterministic: One production rule per symbol
Context-free: Rules do not depend on neighboring symbols
Bracketing supported using stack-based turtle state

Rule Sets
Plant 1 – Tree

Production Rule:
F → F[+F]F[-F]F

Explanation:
Each branch grows forward and creates two symmetric child branches, forming a vertically dominant tree structure with natural tapering.

Adjustable Parameters:
Iteration count
Branch angle

Effect of parameters:
Higher iterations → Increased structural complexity
Larger angle → Wider canopy spread

Plant 2 – Bush / Vine

Production Rule:
F → F[+F][−F]

Explanation:
Each segment creates two lateral branches without extending the main trunk significantly, resulting in a dense outward-spreading morphology.

Adjustable Parameters:
Iteration count
Branch angle

Effect:
Higher iterations → Dense compact foliage
Lower angle → Tighter clustering

Parametric L-System Extension

To meet the extension requirement, this project implements a Parametric L-System.

Instead of representing symbols as simple characters, each symbol stores additional numeric parameters. These include:
Length
Radius
Age

This extension allows the system to modify geometric properties during interpretation. For example:

Branch thickness decreases as depth increases (tapering effect)
Color can change depending on age
Leaves can appear only near branch tips
Growth can be controlled dynamically
Both plant rule sets use this parametric extension.

Complex Feature – Growth Over Time

To achieve an A+ level of complexity, the project includes animated plant growth over time.

Instead of rendering the entire plant instantly, the system:
Generates the full L-System string.
Interprets only a portion of the symbols.
Gradually increases the number of interpreted symbols each frame.

This makes the plant appear as if it is growing naturally. The user can:
Enable or disable growth animation
Adjust growth speed
This feature demonstrates emergent behavior and adds dynamic realism to the system.

Turtle Graphics System

The turtle graphics system is responsible for interpreting the generated string and converting it into 3D geometry.

The turtle maintains:
Position in 3D space
Orientation (rotation)
Current branch thickness
Age information

When a branch symbol is encountered, a 3D cylinder is created. When a leaf symbol is encountered, a polygon mesh is generated.
Bracketing uses a stack structure to save and restore turtle states. This enables realistic branching and hierarchical plant structure.

Visual Design

The visual appearance of the plants was designed to be both plausible and aesthetically pleasing.

Branches:
Rendered as cylinders
Gradually decrease in thickness
Colored from darker brown to lighter brown depending on age

Leaves:
Rendered as simple polygon meshes
Positioned near terminal branches
Colored using green gradients

Scene Setup:
Ground plane
Directional lighting
Organized hierarchy under a root object

User Interface

The system includes an interactive UI that allows the user to:
Select plant type
Adjust iteration count
Adjust branch angle
Control growth speed
Toggle growth animation

This allows exploration of plant morphology and emergent complexity.

System Structure

The project is organized into separate components:

A manager that handles string generation and growth logic
A turtle interpreter that builds geometry
A UI manager that connects interface controls
Helper functions for geometry creation
This separation improves clarity and maintainability.

Special Features & Enhancements
This section describes additional design choices and improvements that make the system more polished and visually strong.

Dynamic Camera Framing 
To improve the presentation, the camera can automatically adjust its position as the plant grows.

This matters As the number of iterations increases, the plant becomes larger and more complex. A fixed camera may:
Cut off the top of the plant, Zoom too close, Show too much empty space

To solve this, the camera can:
Calculate the bounds of all generated geometry
Move backward based on plant size
Slightly adjust height
Always look at the center of the plant

This creates:
A more professional demo
A smoother growth animation
Better visual clarity at all iteration levels
This feature is especially useful when Growth Over Time is enabled, because the camera adapts as the plant expands.


Branch Tapering
Branch thickness decreases over recursion depth.

This mimics real tree behavior where:
The trunk is thick
Secondary branches are thinner
Tertiary branches are very thin
Without tapering, the plant would look artificial.

Clean Architecture
The project is structured clearly:
Generation logic is separate from rendering
UI is separate from system logic
Geometry helpers are isolated
Branch state is handled with a stack
This makes the system easier to understand and extend.

Possible Future Extensions
If extended further, the system could include:

Wind sway animation
Random variation in angle per branch
Seasonal color changes
Multiple plant instances in a garden
Environmental interaction
Procedural terrain

Conclusion

This project demonstrates how deterministic rewriting systems can generate complex three-dimensional plant structures. By extending the base D0L-System with parametric symbols and animated growth, the system moves beyond static procedural generation into dynamic simulation.
The final result is a structured, interactive, and visually engaging procedural plant generator built entirely in Unity.
