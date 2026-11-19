// ============================================================
//  EXERCISE 6 — ALL FOUR PILLARS (exam-style): Library
//  Check your work:   dotnet run -- 6
// ============================================================
//  A library lends Books and DVDs. Late items pay a fee.
//
//  1. ABSTRACT class  LibraryItem                     (abstraction)
//       • Constructor:  LibraryItem(string title)
//       • Title — read-only property
//       • DaysLate — int property backed by a private field.
//           A negative value must be stored as 0.     (encapsulation)
//       • ABSTRACT method:  decimal LateFee()
//
//  2. Book : LibraryItem                               (inheritance)
//       • LateFee = 0.50 per day late
//
//  3. Dvd : LibraryItem                                (inheritance)
//       • LateFee = 1.00 per day late, but never more than 10.00
//
//  4. Class  Library
//       • A private List<LibraryItem> to hold items
//       • void AddItem(LibraryItem item)
//       • decimal TotalFees()  → sum of every item's LateFee()
//                                                      (polymorphism)
//       • int Count  — read-only, number of items
//
//  When you finish, write in a comment WHERE each pillar appears.
//  Examiners love asking "identify and explain" questions!
// ============================================================
namespace OOPPractice.Exercises
{
    // Write your classes here



}
