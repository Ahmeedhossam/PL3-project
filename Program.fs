//open System
//open UI // لازم نفتح المودل بتاع الواجهة

//[<EntryPoint>]
//let main argv =
//    try
//        // بننادي دالة التشغيل اللي عملناها في آخر ملف UI.fs
//        UI.run()
//        0 // رجع 0 يعني البرنامج قفل بسلام
//    with
//    | ex -> 
//        // لو حصلت مصيبة والبرنامج مش عارف يفتح
//        printfn "Critical Error: %s" ex.Message
//        1 // رجع 1 يعني فيه خطأ حصل

//===========================================
open System
open UI
open Tests // افتح ملف الاختبارات

[<EntryPoint>]
let main argv =
    // شغل الاختبارات الأول
    Tests.runTests()

    printfn "\nPress Enter to start the GUI..."
    Console.ReadLine() |> ignore

    // بعد ما تخلص، شغل الواجهة عادي
    UI.run()
    0