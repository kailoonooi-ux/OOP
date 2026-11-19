// ============================================================
//  EXERCISE 1 — ENCAPSULATION: Student
//  Check your work:   dotnet run -- 1
// ============================================================
//  Build a class called  Student  so that:
//
//  1. Constructor:  Student(string name)
//  2. Name  — anyone can READ it, nobody outside can CHANGE it.
//  3. Score — an int, stored in a PRIVATE field.
//       • Values from 0 to 100 are accepted.
//       • Anything outside 0..100 is IGNORED (the score stays the same).
//       • A new student starts with Score 0.
//  4. Passed — a read-only bool property: true when Score >= 50.
//  5. The class must have NO public fields.
//
//  Hint: a property with a "set" block lets you check "value"
//        before storing it in the private field.
// ============================================================
namespace OOPPractice.Exercises
{
    public class Student
    {
        // TODO 1: add a private field to store the score


        // TODO 2: add the Name property (read-only from outside)


        public Student(string name)
        {
            // TODO 3: store the name
        }

        // TODO 4: add the Score property with validation in the setter


        // TODO 5: add the Passed property

    }
}
