module Tests

open System
open wordmodel
open Operations


let private assertSuccess operationName oldDict result =
    match result with
    | Ok newDict -> 
        printfn " %s: Passed" operationName
        newDict
    | Error e -> 
        printfn " %s: Failed -> %A" operationName e
        oldDict 

let runTests () =
    printfn "\n🚀 Starting Unit Tests...\n"


    let dict0 = Map.empty


    let dict1 = 
        addWord "Apple" "A red fruit" dict0 
        |> assertSuccess "Add 'Apple'" dict0


    match fullsearch "APPLE" dict1 with
    | Some (k, v) -> printfn "SUCCESS Search 'APPLE': Found (%s -> %s)" k v
    | None        -> printfn "FAILED Search 'APPLE': Not Found"


    let partialRes = partialsearch "ppl" dict1
    if partialRes.ContainsKey "apple" then 
        printfn "SUCCESS Partial Search 'ppl': Found 'apple'"
    else 
        printfn "FAILED Partial Search 'ppl': Failed"



    let dict2 = 
        updateWord "Apple" "Green fruit" dict1 
        |> assertSuccess "Update 'Apple'" dict1


    let dict3 = 
        deleteWord "Apple" dict2 
        |> assertSuccess "Delete 'Apple'" dict2


    match fullsearch "apple" dict3 with
    | None   -> printfn "SUCCESS Verification: Word is truly gone."
    | Some _ -> printfn "FAILED Verification: Word still exists!"

    printfn "\n Tests Finished.\n"