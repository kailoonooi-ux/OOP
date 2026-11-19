# OOP Practice in C# — Encapsulation, Inheritance, Polymorphism, Abstraction

Two parts work together:

1. **The interactive tutorial**: `index.html`, deployed on Vercel. You can also open the file directly in a browser. Learn each pillar, try the demos, and do the quizzes.
2. **This folder**, a real C# project. You write the code yourself, and a checker tells you what's right ✅ and what's wrong ❌.

## One-time setup (Mac)

1. Install the **.NET SDK** (version 8 or newer) from <https://dotnet.microsoft.com/download>.
   Pick the **macOS Arm64** installer if the Mac has an Apple chip (M1/M2/M3/M4). Pick **x64** if it has an Intel chip.
2. Install **Visual Studio Code** and its **C# Dev Kit** extension. This is optional but nicer to use.
3. Open Terminal, then run:

   ```bash
   cd ~/Projects/OOP/OOPPractice
   dotnet run
   ```

   A menu should appear.

## How to practise

| Command | What it does |
|---|---|
| `dotnet run -- demo` | Runs the lesson examples. Read `Lessons/LessonExamples.cs` first and **predict** the output. |
| `dotnet run -- 1` | Checks Exercise 1. Use any number from 1 to 6. |
| `dotnet run -- all` | Checks every exercise. |

**Work loop:** open `Exercises/Ex1_Encapsulation.cs`, then read the instructions at the top. Write the code and run `dotnet run -- 1`. Fix whatever shows ❌, then repeat until it says 🎉.

| # | Pillar | Problem |
|---|---|---|
| 1 | Encapsulation | Student with a protected Score (0–100) |
| 2 | Inheritance | Vehicle → Car, Motorcycle |
| 3 | Polymorphism | Employee → Manager, Intern payroll |
| 4 | Abstraction (abstract class) | Shape → Circle, Rectangle |
| 5 | Abstraction (interface) | INotifier → Email/SMS + AlertService |
| 6 | **All four** (exam-style) | Library with Books and DVDs and late fees |

`Solutions/ModelAnswers.cs` has worked answers. **Only look after you've tried.** There's more than one correct answer, so if the checker passes, you're right.

If you get a red compile error, read the **first** error line. It gives the file name and line number, such as `Ex2_Inheritance.cs(25,9)`. Fix that error first, because the other errors are often caused by it.

## The four pillars in one line each (memory hook: **A PIE**)

- **A**bstraction: show *what* something does and hide *how*. (`abstract`, `interface`)
- **P**olymorphism: one call, many behaviours. (`virtual` / `override`)
- **I**nheritance: a child class *is a* kind of parent class and reuses its code. (`class Car : Vehicle`, `base(...)`)
- **E**ncapsulation: keep data `private` and allow changes only through safe, checked methods and properties.
