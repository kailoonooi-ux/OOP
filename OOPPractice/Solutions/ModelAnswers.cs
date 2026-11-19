// ============================================================
//  MODEL ANSWERS — try each exercise yourself FIRST!
//  Compare after you get all ✅ (there's more than one correct way).
//  The checker can test these with:   dotnet run -- solutions
// ============================================================
namespace OOPPractice.Solutions
{
    // ---------- Exercise 1: Encapsulation ----------
    public class Student
    {
        private int _score;                         // private field = hidden data

        public string Name { get; }                 // get only = read-only outside

        public Student(string name)
        {
            Name = name;
        }

        public int Score
        {
            get { return _score; }
            set
            {
                if (value >= 0 && value <= 100)     // validation guards the data
                {
                    _score = value;
                }
            }
        }

        public bool Passed => Score >= 50;          // computed, read-only
    }

    // ---------- Exercise 2: Inheritance ----------
    public class Vehicle
    {
        public string Brand { get; }
        public int Wheels { get; }

        public Vehicle(string brand, int wheels)
        {
            Brand = brand;
            Wheels = wheels;
        }

        public string Describe()
        {
            return $"{Brand} with {Wheels} wheels";
        }
    }

    public class Car : Vehicle
    {
        public int Doors { get; }

        public Car(string brand, int doors) : base(brand, 4)
        {
            Doors = doors;
        }
    }

    public class Motorcycle : Vehicle
    {
        public Motorcycle(string brand) : base(brand, 2) { }
    }

    // ---------- Exercise 3: Polymorphism ----------
    public class Employee
    {
        public string Name { get; }
        public decimal BaseSalary { get; }

        public Employee(string name, decimal baseSalary)
        {
            Name = name;
            BaseSalary = baseSalary;
        }

        public virtual decimal CalculatePay()
        {
            return BaseSalary;
        }
    }

    public class Manager : Employee
    {
        public Manager(string name, decimal baseSalary) : base(name, baseSalary) { }

        public override decimal CalculatePay()
        {
            return BaseSalary + 1000;
        }
    }

    public class Intern : Employee
    {
        public Intern(string name, decimal baseSalary) : base(name, baseSalary) { }

        public override decimal CalculatePay()
        {
            return BaseSalary * 0.5m;
        }
    }

    public static class Payroll
    {
        public static decimal Total(List<Employee> staff)
        {
            decimal total = 0;
            foreach (Employee e in staff)
            {
                total += e.CalculatePay();          // the right version runs automatically
            }
            return total;
        }
    }

    // ---------- Exercise 4: Abstraction (abstract class) ----------
    public abstract class Shape
    {
        public abstract double Area();
        public abstract double Perimeter();

        public string Describe()
        {
            return $"{GetType().Name} with area {Area():F2}";
        }
    }

    public class Circle : Shape
    {
        private readonly double _radius;
        public Circle(double radius) { _radius = radius; }

        public override double Area() => Math.PI * _radius * _radius;
        public override double Perimeter() => 2 * Math.PI * _radius;
    }

    public class Rectangle : Shape
    {
        private readonly double _width;
        private readonly double _height;

        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public override double Area() => _width * _height;
        public override double Perimeter() => 2 * (_width + _height);
    }

    // ---------- Exercise 5: Abstraction (interface) ----------
    public interface INotifier
    {
        string Send(string message);
    }

    public class EmailNotifier : INotifier
    {
        public string Send(string message) => "Email: " + message;
    }

    public class SmsNotifier : INotifier
    {
        public string Send(string message)
        {
            if (message.Length > 20)
            {
                message = message.Substring(0, 20);
            }
            return "SMS: " + message;
        }
    }

    public class AlertService
    {
        private readonly INotifier _notifier;

        public AlertService(INotifier notifier)
        {
            _notifier = notifier;
        }

        public string Alert(string message)
        {
            return _notifier.Send("ALERT " + message);
        }
    }

    // ---------- Exercise 6: All four pillars ----------
    // ABSTRACTION:   LibraryItem is abstract; LateFee() is abstract.
    // ENCAPSULATION: _daysLate is private; the DaysLate setter blocks negatives;
    //                Library's list is private and only changed through AddItem.
    // INHERITANCE:   Book and Dvd inherit Title/DaysLate from LibraryItem.
    // POLYMORPHISM:  TotalFees() calls item.LateFee() and each type answers differently.
    public abstract class LibraryItem
    {
        private int _daysLate;

        public string Title { get; }

        public int DaysLate
        {
            get => _daysLate;
            set => _daysLate = value < 0 ? 0 : value;
        }

        protected LibraryItem(string title)
        {
            Title = title;
        }

        public abstract decimal LateFee();
    }

    public class Book : LibraryItem
    {
        public Book(string title) : base(title) { }
        public override decimal LateFee() => DaysLate * 0.50m;
    }

    public class Dvd : LibraryItem
    {
        public Dvd(string title) : base(title) { }
        public override decimal LateFee() => Math.Min(DaysLate * 1.00m, 10.00m);
    }

    public class Library
    {
        private readonly List<LibraryItem> _items = new List<LibraryItem>();

        public int Count => _items.Count;

        public void AddItem(LibraryItem item)
        {
            _items.Add(item);
        }

        public decimal TotalFees()
        {
            decimal total = 0;
            foreach (LibraryItem item in _items)
            {
                total += item.LateFee();
            }
            return total;
        }
    }
}
