open System
open UI

[<EntryPoint>]
let main argv =
    try
        
        UI.run()
        0 
    with
    | ex -> 
        printfn "Critical Error: %s" ex.Message
        1 





//open System
//open UI
//open Tests 

//[<EntryPoint>]
//let main argv =
//   
//    Tests.runTests()

//    printfn "\nPress Enter to start the GUI..."
//    Console.ReadLine() |> ignore

//    
//    UI.run()
//    0