// ============================================================
//  EXERCISE 5 — ABSTRACTION (interface): Notifications
//  Check your work:   dotnet run -- 5
// ============================================================
//  1. INTERFACE  INotifier
//       • string Send(string message);
//
//  2. EmailNotifier : INotifier
//       • Send returns  "Email: " + message
//
//  3. SmsNotifier : INotifier
//       • Send returns  "SMS: " + message
//       • BUT an SMS message is limited to 20 characters:
//         if message is longer than 20 characters, keep only the
//         first 20.  (Hint: message.Substring(0, 20))
//
//  4. Class  AlertService
//       • Constructor:  AlertService(INotifier notifier)
//         (store it in a private field)
//       • Method:  string Alert(string message)
//         → returns notifier.Send("ALERT " + message)
//
//  Notice: AlertService has NO idea whether it is sending email
//  or SMS. It only knows the INotifier contract. That is abstraction.
// ============================================================
namespace OOPPractice.Exercises
{
    // Write your interface and classes here



}
