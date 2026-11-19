// ============================================================
//  LESSON 0 — OOP FOUNDATIONS (read this BEFORE the 4 pillars)
//  Run with:   dotnet run -- basics
// ============================================================
namespace OOPPractice.Lessons.Foundations
{
    // ------------------------------------------------------------
    // ANATOMY OF A CLASS
    // ------------------------------------------------------------
    public class Book                                       // ① class declaration
    {
        private int _timesBorrowed;                         // ② FIELD: data stored inside each object

        public string Title { get; }                        // ③ PROPERTY: controlled access to data
        public string Author { get; }
        public int TimesBorrowed => _timesBorrowed;
        public static int TotalBooks { get; private set; }  // ④ STATIC: ONE value shared by all books

        public Book(string title, string author)            // ⑤ CONSTRUCTOR: runs once, on "new"
        {
            Title = title;
            Author = author;
            TotalBooks++;
        }

        public void Borrow()                                // ⑥ METHOD: something the object can DO
        {
            _timesBorrowed++;
        }

        public override string ToString()                   // ⑦ every class inherits from object
            => $"{Title} by {Author} (borrowed {_timesBorrowed}x)";
    }

    // ------------------------------------------------------------
    // CONSTRUCTORS
    // ------------------------------------------------------------
    public class Player
    {
        public static int Count { get; private set; }       // how many Players exist

        public string Name { get; }
        public int Level { get; }
        public int Health { get; }

        // 1) Main constructor: validates, so a Player can NEVER start in a broken state
        public Player(string name, int level)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");
            if (level < 1)
                throw new ArgumentException("Level must be at least 1");

            Name = name;
            Level = level;
            Health = level * 100;                           // derived starting value
            Count++;
        }

        // 2) Overloaded constructor: chains to the main one with ": this(...)"
        public Player(string name) : this(name, 1) { }
    }

    public class Point
    {
        private int x, y;

        public Point(int x, int y)
        {
            this.x = x;     // this.x = the FIELD,  x = the PARAMETER
            this.y = y;
        }

        public override string ToString() => $"({x}, {y})";
    }

    // ------------------------------------------------------------
    // KINDS OF CLASSES
    // ------------------------------------------------------------
    public class Dog { public string Name = "Rex"; }                // CONCRETE: can be new-ed

    public abstract class Animal { public abstract string Speak(); } // ABSTRACT: must be inherited

    public class Cat : Animal { public override string Speak() => "Meow"; }

    public sealed class Password                                    // SEALED: nobody can inherit
    {
        private readonly string _value;
        public Password(string value) { _value = value; }
        public bool Matches(string attempt) => attempt == _value;
    }
    // public class StrongPassword : Password { }   // ❌ CS0509: cannot derive from sealed type

    public static class Converter                                   // STATIC: a toolbox, no objects
    {
        public static double CToF(double c) => c * 9 / 5 + 32;
    }
    // var c = new Converter();                     // ❌ CS0712: cannot create an instance of a static class

    public interface IPrintable { void Print(); }                   // INTERFACE: a contract

    public partial class Report { public string Title = "Q4"; }     // PARTIAL: split across files
    public partial class Report { public void Print() => Console.WriteLine($"  Report {Title}"); }

    public static class FoundationsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== OBJECTS & REFERENCES ===");
            Book a = new Book("Dune", "Herbert");
            Book b = a;                 // NOT a copy: b points to the SAME object
            Book c = new Book("Dune", "Herbert");
            b.Borrow();
            Console.WriteLine($"  a: {a}");
            Console.WriteLine($"  c: {c}");
            Console.WriteLine($"  a == b? {a == b}   a == c? {a == c}");
            Console.WriteLine($"  Book.TotalBooks = {Book.TotalBooks}");

            Console.WriteLine("\n=== CONSTRUCTORS ===");
            var p1 = new Player("Aisha", 5);
            var p2 = new Player("Ben");
            Console.WriteLine($"  {p1.Name}: level {p1.Level}, health {p1.Health}");
            Console.WriteLine($"  {p2.Name}: level {p2.Level}, health {p2.Health}");
            try { var p3 = new Player("", 3); }
            catch (ArgumentException e) { Console.WriteLine($"  new Player(\"\", 3) → {e.Message}"); }
            Console.WriteLine($"  Player.Count = {Player.Count}");
            Console.WriteLine($"  new Point(3, 4) → {new Point(3, 4)}");

            Console.WriteLine("\n=== KINDS OF CLASSES ===");
            Console.WriteLine($"  Concrete: new Dog().Name = {new Dog().Name}");
            Animal pet = new Cat();
            Console.WriteLine($"  Abstract: Animal pet = new Cat(); pet.Speak() = {pet.Speak()}");
            Console.WriteLine($"  Sealed:   new Password(\"abc\").Matches(\"abc\") = {new Password("abc").Matches("abc")}");
            Console.WriteLine($"  Static:   Converter.CToF(100) = {Converter.CToF(100)}");
            new Report().Print();
        }
    }
}
