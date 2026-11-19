// ============================================================
//  LESSON EXAMPLES — the same code you see in the tutorial page.
//  Run them with:   dotnet run -- demo
//  Read each section, predict the output, THEN run it.
// ============================================================
namespace OOPPractice.Lessons
{
    // ------------------------------------------------------------
    // 1. ENCAPSULATION — protect the data, expose safe actions
    // ------------------------------------------------------------
    public class BankAccount
    {
        private decimal _balance;               // hidden: nobody outside can touch it

        public string Owner { get; }            // read-only after the constructor
        public decimal Balance => _balance;     // others can READ, not WRITE

        public BankAccount(string owner, decimal openingDeposit)
        {
            Owner = owner;
            Deposit(openingDeposit);
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine($"  Rejected: deposit must be positive (got {amount})");
                return;
            }
            _balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0 || amount > _balance)
            {
                Console.WriteLine($"  Rejected: cannot withdraw {amount} (balance {_balance})");
                return false;
            }
            _balance -= amount;
            return true;
        }
    }

    // ------------------------------------------------------------
    // 2. INHERITANCE — "is-a": a Warrior IS A Character
    // ------------------------------------------------------------
    public class Character
    {
        public string Name { get; }
        protected int Health;                   // protected: children can use it, outsiders can't

        public Character(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
            Console.WriteLine($"  {Name} has {Health} HP left");
        }

        // 3. POLYMORPHISM — virtual = "children may replace this"
        public virtual string Attack()
        {
            return $"{Name} punches weakly.";
        }
    }

    public class Warrior : Character
    {
        public int Strength { get; }

        // base(...) runs the parent's constructor first
        public Warrior(string name) : base(name, 150)
        {
            Strength = 20;
        }

        public override string Attack()
        {
            return $"{Name} swings a sword for {Strength} damage!";
        }
    }

    public class Mage : Character
    {
        public int Mana { get; private set; } = 100;

        public Mage(string name) : base(name, 80) { }

        public override string Attack()
        {
            if (Mana < 30) return $"{Name} is out of mana... " + base.Attack();
            Mana -= 30;
            return $"{Name} casts a fireball! (mana left: {Mana})";
        }
    }

    // ------------------------------------------------------------
    // 4. ABSTRACTION — show WHAT, hide HOW
    // ------------------------------------------------------------
    public abstract class Shape
    {
        public abstract double Area();          // no body: every child MUST provide one

        public string Describe()                // normal method: shared by all children
        {
            return $"{GetType().Name} with area {Area():F2}";
        }
    }

    public class Circle : Shape
    {
        private readonly double _radius;
        public Circle(double radius) { _radius = radius; }
        public override double Area() => Math.PI * _radius * _radius;
    }

    public class Rectangle : Shape
    {
        private readonly double _w, _h;
        public Rectangle(double w, double h) { _w = w; _h = h; }
        public override double Area() => _w * _h;
    }

    // An interface is a pure contract: "anything that can do this"
    public interface IHealable
    {
        void Heal(int amount);
    }

    public class Cleric : Character, IHealable
    {
        public Cleric(string name) : base(name, 100) { }

        public void Heal(int amount)
        {
            Health += amount;
            Console.WriteLine($"  {Name} heals to {Health} HP");
        }

        public override string Attack() => $"{Name} smites with holy light!";
    }

    public static class LessonDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 1. ENCAPSULATION ===");
            var acc = new BankAccount("Aisha", 100);
            acc.Deposit(-50);          // rejected
            acc.Withdraw(500);         // rejected
            acc.Withdraw(30);
            Console.WriteLine($"  {acc.Owner}'s balance: {acc.Balance}");
            // acc._balance = 1000000;   <- would NOT compile: _balance is private
            // acc.Balance = 1000000;    <- would NOT compile: Balance has no setter

            Console.WriteLine("\n=== 2. INHERITANCE ===");
            var w = new Warrior("Ren");
            Console.WriteLine($"  {w.Name} (inherited Name) has Strength {w.Strength} (own property)");
            w.TakeDamage(40);          // inherited method

            Console.WriteLine("\n=== 3. POLYMORPHISM ===");
            List<Character> party = new List<Character>
            {
                new Warrior("Ren"),
                new Mage("Yuki"),
                new Character("Villager", 50),
                new Cleric("Sol")
            };
            foreach (Character c in party)
            {
                Console.WriteLine("  " + c.Attack());   // same call, different behaviour
            }

            Console.WriteLine("\n=== 4. ABSTRACTION ===");
            List<Shape> shapes = new List<Shape> { new Circle(1), new Rectangle(3, 4) };
            foreach (Shape s in shapes) Console.WriteLine("  " + s.Describe());
            // Shape s = new Shape();   <- would NOT compile: cannot create an abstract class

            IHealable healer = new Cleric("Sol");
            healer.Heal(25);
        }
    }
}
