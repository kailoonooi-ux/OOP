// ============================================================
//  OOP PRACTICE — start here
//
//    dotnet run            → menu
//    dotnet run -- basics  → OOP foundations (classes, objects, constructors)
//    dotnet run -- demo    → run the 4-pillar lesson examples
//    dotnet run -- 1       → check Exercise 1 (1 to 6)
//    dotnet run -- all     → check every exercise
// ============================================================
using OOPPractice.Checker;
using OOPPractice.Lessons;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string choice = args.Length > 0 ? args[0].ToLower() : "";

if (choice == "")
{
    Console.WriteLine("""

    ╔══════════════════════════════════════════╗
    ║        OOP PRACTICE  (C#)                ║
    ╠══════════════════════════════════════════╣
    ║  basics  Classes, objects, constructors  ║
    ║  demo    The 4 pillars lesson examples   ║
    ║  1     Encapsulation   – Student         ║
    ║  2     Inheritance     – Vehicles        ║
    ║  3     Polymorphism    – Payroll         ║
    ║  4     Abstraction     – Shapes          ║
    ║  5     Interfaces      – Notifications   ║
    ║  6     All four        – Library (exam)  ║
    ║  all   Check every exercise              ║
    ╚══════════════════════════════════════════╝
    """);
    Console.Write("Type your choice and press Enter: ");
    choice = (Console.ReadLine() ?? "").Trim().ToLower();
}

if (choice == "basics")
{
    OOPPractice.Lessons.Foundations.FoundationsDemo.Run();
}
else if (choice == "demo")
{
    LessonDemo.Run();
}
else if (choice == "all" || choice == "solutions")
{
    string ns = choice == "solutions" ? "OOPPractice.Solutions" : "OOPPractice.Exercises";
    var checker = new ExerciseChecker(ns);
    int done = 0;
    for (int i = 1; i <= 6; i++) if (checker.Run(i)) done++;
    Console.WriteLine($"\nExercises fully passed: {done}/6");
}
else if (int.TryParse(choice, out int n) && n >= 1 && n <= 6)
{
    new ExerciseChecker("OOPPractice.Exercises").Run(n);
}
else
{
    Console.WriteLine("Unknown choice. Try: basics, demo, 1-6, or all");
}
