// ============================================================
//  AUTO-CHECKER — you do NOT need to edit or understand this file.
//  It uses "reflection" to look inside your classes and test them.
// ============================================================
using System.Reflection;

namespace OOPPractice.Checker
{
    public class ExerciseChecker
    {
        private readonly string _ns;
        private int _passed, _total;

        public ExerciseChecker(string ns) { _ns = ns; }

        // ---------------- helpers ----------------
        private Type Find(string name) =>
            Assembly.GetExecutingAssembly().GetType($"{_ns}.{name}")
            ?? throw new Exception($"class/interface '{name}' not found");

        private static object New(Type t, params object[] args)
        {
            try { return Activator.CreateInstance(t, args); }
            catch (MissingMethodException)
            {
                string sig = string.Join(", ", args.Select(a => a.GetType().Name));
                throw new Exception($"no public constructor {t.Name}({sig})");
            }
            catch (TargetInvocationException e) { throw e.InnerException ?? e; }
        }

        private static PropertyInfo Prop(object o, string name) =>
            o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
            ?? throw new Exception($"property '{name}' not found on {o.GetType().Name}");

        private static object Get(object o, string name) => Prop(o, name).GetValue(o);

        private static void Set(object o, string name, object value)
        {
            var p = Prop(o, name);
            if (p.SetMethod == null || !p.SetMethod.IsPublic)
                throw new Exception($"{name} has no public setter");
            try { p.SetValue(o, value); }
            catch (TargetInvocationException e) { throw e.InnerException ?? e; }
        }

        private static object Call(object o, string method, params object[] args)
        {
            var m = o.GetType().GetMethod(method, BindingFlags.Public | BindingFlags.Instance)
                    ?? throw new Exception($"method '{method}' not found on {o.GetType().Name}");
            try { return m.Invoke(o, args); }
            catch (TargetInvocationException e) { throw e.InnerException ?? e; }
        }

        private void Check(string description, Func<bool> test, string hint = "")
        {
            _total++;
            try
            {
                if (test())
                {
                    _passed++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  ✅ {description}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  ❌ {description}");
                    if (hint != "") { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"     💡 {hint}"); }
                }
            }
            catch (NotImplementedException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ❌ {description}  (not implemented yet)");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ❌ {description}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"     💡 {e.Message}");
            }
            Console.ResetColor();
        }

        private void Header(string title)
        {
            _passed = 0; _total = 0;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n━━━ {title} ━━━");
            Console.ResetColor();
        }

        private bool Footer()
        {
            Console.ForegroundColor = _passed == _total ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.WriteLine($"  Score: {_passed}/{_total}" + (_passed == _total ? "  🎉 All passed!" : ""));
            Console.ResetColor();
            return _passed == _total;
        }

        // ---------------- exercises ----------------
        public bool Ex1()
        {
            Header("Exercise 1 — Encapsulation: Student");
            Check("Student(\"Mei\") can be created and Name is \"Mei\"",
                () => (string)Get(New(Find("Student"), "Mei"), "Name") == "Mei");
            Check("Name cannot be changed from outside (no public setter)",
                () => { var p = Find("Student").GetProperty("Name"); return p != null && (p.SetMethod == null || !p.SetMethod.IsPublic); },
                "Use  public string Name { get; }  or  { get; private set; }");
            Check("A new student starts with Score 0",
                () => (int)Get(New(Find("Student"), "Mei"), "Score") == 0);
            Check("Score = 85 is stored",
                () => { var s = New(Find("Student"), "Mei"); Set(s, "Score", 85); return (int)Get(s, "Score") == 85; });
            Check("Score = 150 is ignored (stays 85)",
                () => { var s = New(Find("Student"), "Mei"); Set(s, "Score", 85); Set(s, "Score", 150); return (int)Get(s, "Score") == 85; },
                "In the setter: only store value if it is between 0 and 100");
            Check("Score = -5 is ignored (stays 85)",
                () => { var s = New(Find("Student"), "Mei"); Set(s, "Score", 85); Set(s, "Score", -5); return (int)Get(s, "Score") == 85; });
            Check("Score = 0 and Score = 100 are both accepted (edge cases!)",
                () => { var s = New(Find("Student"), "Mei"); Set(s, "Score", 100); bool a = (int)Get(s, "Score") == 100; Set(s, "Score", 0); return a && (int)Get(s, "Score") == 0; },
                "Check your comparison: >= 0 and <= 100");
            Check("Passed is true at 50 and false at 49",
                () => { var s = New(Find("Student"), "Mei"); Set(s, "Score", 50); bool a = (bool)Get(s, "Passed"); Set(s, "Score", 49); return a && !(bool)Get(s, "Passed"); });
            Check("Passed is read-only",
                () => { var p = Find("Student").GetProperty("Passed"); return p != null && (p.SetMethod == null || !p.SetMethod.IsPublic); });
            Check("No public fields (data is hidden)",
                () => Find("Student").GetProperty("Score") != null && Find("Student").GetFields(BindingFlags.Public | BindingFlags.Instance).Length == 0,
                "Fields should be private. Expose data through properties instead.");
            return Footer();
        }

        public bool Ex2()
        {
            Header("Exercise 2 — Inheritance: Vehicles");
            Check("Vehicle(\"Honda\", 3).Describe() returns \"Honda with 3 wheels\"",
                () => (string)Call(New(Find("Vehicle"), "Honda", 3), "Describe") == "Honda with 3 wheels");
            Check("Car inherits from Vehicle",
                () => Find("Car").BaseType == Find("Vehicle"), "class Car : Vehicle");
            Check("Motorcycle inherits from Vehicle",
                () => Find("Motorcycle").BaseType == Find("Vehicle"));
            Check("new Car(\"Toyota\", 4) has Brand \"Toyota\" and 4 Wheels",
                () => { var c = New(Find("Car"), "Toyota", 4); return (string)Get(c, "Brand") == "Toyota" && (int)Get(c, "Wheels") == 4; },
                "Car's constructor should call  : base(brand, 4)");
            Check("Car has its own Doors property",
                () => (int)Get(New(Find("Car"), "Toyota", 2), "Doors") == 2);
            Check("Car.Describe() works without Car defining it (inherited)",
                () => (string)Call(New(Find("Car"), "Toyota", 4), "Describe") == "Toyota with 4 wheels"
                      && Find("Car").GetMethod("Describe")!.DeclaringType == Find("Vehicle"),
                "Don't rewrite Describe() in Car — inherit it.");
            Check("new Motorcycle(\"Yamaha\").Describe() returns \"Yamaha with 2 wheels\"",
                () => (string)Call(New(Find("Motorcycle"), "Yamaha"), "Describe") == "Yamaha with 2 wheels");
            Check("Brand is NOT re-declared in Car (no duplicated code)",
                () => Find("Car").GetProperty("Brand")!.DeclaringType == Find("Vehicle"));
            return Footer();
        }

        public bool Ex3()
        {
            Header("Exercise 3 — Polymorphism: Payroll");
            Type emp = null;
            Check("Employee(\"Ali\", 3000).CalculatePay() returns 3000",
                () => { emp = Find("Employee"); return (decimal)Call(New(emp, "Ali", 3000m), "CalculatePay") == 3000m; });
            Check("CalculatePay is virtual in Employee",
                () => { var m = Find("Employee").GetMethod("CalculatePay"); return m != null && m.IsVirtual && !m.IsFinal; },
                "public virtual decimal CalculatePay()");
            Check("Manager and Intern inherit from Employee",
                () => Find("Manager").BaseType == Find("Employee") && Find("Intern").BaseType == Find("Employee"));
            Check("Manager OVERRIDES CalculatePay (not 'new')",
                () => { var m = Find("Manager").GetMethod("CalculatePay"); return m.DeclaringType == Find("Manager") && m.GetBaseDefinition().DeclaringType == Find("Employee"); },
                "public override decimal CalculatePay()");
            Check("Employee e = new Manager(\"Bo\", 3000);  e.CalculatePay() == 4000",
                () => { var baseMethod = Find("Employee").GetMethod("CalculatePay"); return (decimal)baseMethod.Invoke(New(Find("Manager"), "Bo", 3000m), null) == 4000m; },
                "Calling through an Employee reference must still run Manager's version");
            Check("Employee e = new Intern(\"Cy\", 2000);  e.CalculatePay() == 1000",
                () => { var baseMethod = Find("Employee").GetMethod("CalculatePay"); return (decimal)baseMethod.Invoke(New(Find("Intern"), "Cy", 2000m), null) == 1000m; });
            Check("Payroll.Total([Employee 3000, Manager 3000, Intern 2000]) == 8000",
                () =>
                {
                    var e = Find("Employee");
                    var list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(e));
                    list.Add(New(e, "A", 3000m)); list.Add(New(Find("Manager"), "B", 3000m)); list.Add(New(Find("Intern"), "C", 2000m));
                    var total = Find("Payroll").GetMethod("Total", BindingFlags.Public | BindingFlags.Static)
                                ?? throw new Exception("static method Payroll.Total not found");
                    return (decimal)total.Invoke(null, new object[] { list }) == 8000m;
                });
            Check("Payroll.Total of an empty list is 0",
                () =>
                {
                    var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(Find("Employee")));
                    return (decimal)Find("Payroll").GetMethod("Total").Invoke(null, new[] { list }) == 0m;
                });
            return Footer();
        }

        public bool Ex4()
        {
            Header("Exercise 4 — Abstraction: Shapes");
            Check("Shape is abstract (new Shape() is impossible)",
                () => Find("Shape").IsAbstract, "public abstract class Shape");
            Check("Area() and Perimeter() are abstract in Shape",
                () => Find("Shape").GetMethod("Area")!.IsAbstract && Find("Shape").GetMethod("Perimeter")!.IsAbstract,
                "public abstract double Area();   ← semicolon, no body");
            Check("Circle and Rectangle inherit from Shape",
                () => Find("Circle").BaseType == Find("Shape") && Find("Rectangle").BaseType == Find("Shape"));
            Check("Circle(2).Area() ≈ 12.566",
                () => Math.Abs((double)Call(New(Find("Circle"), 2.0), "Area") - 12.566) < 0.01, "Math.PI * r * r");
            Check("Circle(2).Perimeter() ≈ 12.566",
                () => Math.Abs((double)Call(New(Find("Circle"), 2.0), "Perimeter") - 12.566) < 0.01);
            Check("Rectangle(3, 4) has Area 12 and Perimeter 14",
                () => { var r = New(Find("Rectangle"), 3.0, 4.0); return (double)Call(r, "Area") == 12 && (double)Call(r, "Perimeter") == 14; });
            Check("Circle(1).Describe() returns \"Circle with area 3.14\"",
                () => (string)Call(New(Find("Circle"), 1.0), "Describe") == "Circle with area 3.14",
                "$\"{GetType().Name} with area {Area():F2}\"");
            Check("Describe() is written ONCE in Shape (shared by all shapes)",
                () => Find("Rectangle").GetMethod("Describe")!.DeclaringType == Find("Shape"));
            return Footer();
        }

        public bool Ex5()
        {
            Header("Exercise 5 — Abstraction: Interfaces");
            Check("INotifier is an interface",
                () => Find("INotifier").IsInterface, "public interface INotifier { string Send(string message); }");
            Check("EmailNotifier and SmsNotifier implement INotifier",
                () => Find("INotifier").IsAssignableFrom(Find("EmailNotifier")) && Find("INotifier").IsAssignableFrom(Find("SmsNotifier")));
            Check("Email Send(\"Hi\") returns \"Email: Hi\"",
                () => (string)Call(New(Find("EmailNotifier")), "Send", "Hi") == "Email: Hi");
            Check("SMS Send(\"Hi\") returns \"SMS: Hi\"",
                () => (string)Call(New(Find("SmsNotifier")), "Send", "Hi") == "SMS: Hi");
            Check("SMS cuts long messages to 20 characters",
                () => (string)Call(New(Find("SmsNotifier")), "Send", "abcdefghijklmnopqrstuvwxyz") == "SMS: abcdefghijklmnopqrst",
                "if (message.Length > 20) message = message.Substring(0, 20);");
            Check("SMS with exactly 20 characters is not cut",
                () => (string)Call(New(Find("SmsNotifier")), "Send", "12345678901234567890") == "SMS: 12345678901234567890");
            Check("AlertService constructor takes an INotifier (not a specific class)",
                () => Find("AlertService").GetConstructors().Any(c => c.GetParameters().Length == 1 && c.GetParameters()[0].ParameterType == Find("INotifier")),
                "public AlertService(INotifier notifier)");
            Check("AlertService works with EITHER notifier",
                () =>
                {
                    var a1 = New(Find("AlertService"), New(Find("EmailNotifier")));
                    var a2 = New(Find("AlertService"), New(Find("SmsNotifier")));
                    return (string)Call(a1, "Alert", "Server down") == "Email: ALERT Server down"
                        && (string)Call(a2, "Alert", "Server is on fire right now") == "SMS: ALERT Server is on f";
                });
            return Footer();
        }

        public bool Ex6()
        {
            Header("Exercise 6 — All four pillars: Library");
            Check("LibraryItem is abstract with an abstract LateFee()",
                () => Find("LibraryItem").IsAbstract && Find("LibraryItem").GetMethod("LateFee")!.IsAbstract);
            Check("Book and Dvd inherit from LibraryItem",
                () => Find("Book").BaseType == Find("LibraryItem") && Find("Dvd").BaseType == Find("LibraryItem"));
            Check("new Book(\"Dune\").Title is \"Dune\" and Title is read-only",
                () => { var b = New(Find("Book"), "Dune"); var p = Find("LibraryItem").GetProperty("Title"); return (string)Get(b, "Title") == "Dune" && (p.SetMethod == null || !p.SetMethod.IsPublic); });
            Check("DaysLate = -3 is stored as 0",
                () => { var b = New(Find("Book"), "Dune"); Set(b, "DaysLate", -3); return (int)Get(b, "DaysLate") == 0; });
            Check("Book 4 days late → fee 2.00",
                () => { var b = New(Find("Book"), "Dune"); Set(b, "DaysLate", 4); return (decimal)Call(b, "LateFee") == 2.00m; });
            Check("Dvd 3 days late → fee 3.00",
                () => { var d = New(Find("Dvd"), "Up"); Set(d, "DaysLate", 3); return (decimal)Call(d, "LateFee") == 3.00m; });
            Check("Dvd 15 days late → fee capped at 10.00",
                () => { var d = New(Find("Dvd"), "Up"); Set(d, "DaysLate", 15); return (decimal)Call(d, "LateFee") == 10.00m; },
                "Math.Min(DaysLate * 1.00m, 10.00m)");
            Check("Library: add Book(4 days) + Dvd(15 days) → Count 2, TotalFees 12.00",
                () =>
                {
                    var lib = New(Find("Library"));
                    var b = New(Find("Book"), "Dune"); Set(b, "DaysLate", 4);
                    var d = New(Find("Dvd"), "Up"); Set(d, "DaysLate", 15);
                    Call(lib, "AddItem", b); Call(lib, "AddItem", d);
                    return (int)Get(lib, "Count") == 2 && (decimal)Call(lib, "TotalFees") == 12.00m;
                });
            Check("Library and LibraryItem have no public fields",
                () => Find("Library").GetFields(BindingFlags.Public | BindingFlags.Instance).Length == 0
                   && Find("LibraryItem").GetFields(BindingFlags.Public | BindingFlags.Instance).Length == 0);
            return Footer();
        }

        public bool Run(int n) => n switch
        {
            1 => Ex1(), 2 => Ex2(), 3 => Ex3(), 4 => Ex4(), 5 => Ex5(), 6 => Ex6(),
            _ => false
        };
    }
}
