// ============================================================
//  EXERCISE 3 — POLYMORPHISM: Payroll
//  Check your work:   dotnet run -- 3
// ============================================================
//  1. Base class  Employee
//       • Constructor:  Employee(string name, decimal baseSalary)
//       • Read-only properties:  Name, BaseSalary
//       • VIRTUAL method:  decimal CalculatePay()  → returns BaseSalary
//
//  2. Manager : Employee
//       • OVERRIDES CalculatePay()  → BaseSalary + 1000
//
//  3. Intern : Employee
//       • OVERRIDES CalculatePay()  → BaseSalary * 0.5m
//
//  4. Static class  Payroll  with a method:
//       public static decimal Total(List<Employee> staff)
//       → adds up CalculatePay() for everyone in the list.
//
//  KEY TEST: Payroll.Total must NOT use "if (e is Manager)" etc.
//  It just calls e.CalculatePay() and lets polymorphism do the work.
//
//  Remember: decimal numbers need an  m  at the end, e.g. 0.5m
// ============================================================
namespace OOPPractice.Exercises
{
    // Write your classes here



}
