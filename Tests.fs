module Tests

open System
open wordmodel
open Operations

let private assertSuccess operationName oldDict result =
    match result with
    | Ok newDict -> 
        printfn "%s: Success" operationName
        newDict
    | Error e -> 
        printfn "%s: Failed Error details: %A" operationName e
        oldDict 

let runTests () =
    printfn "\nStarting unit tests
    \n"

    let dict0 = Map.empty

    let dict1 = 
        addWord "Apple" "A red fruit" dict0 
        |> assertSuccess "Adding new word 'Apple'" dict0

    let dict2 = 
        updateWord "Apple" "Green fruit" dict1 
        |> assertSuccess "Updating meaning of 'Apple'" dict1

    let dict3 = 
        deleteWord "Apple" dict2 
        |> assertSuccess "Deleting word 'Apple'" dict2

    match fullsearch "apple" dict3 with
    | None    -> printfn "Verification: The word was successfully removed."
    | Some _ -> printfn "Verification: Error, the word still exists."

    printfn "\nTests completed\n"
