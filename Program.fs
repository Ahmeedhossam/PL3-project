open System
open wordmodel  
open Operations 

[<EntryPoint>]
let main argv =
    printfn "--- Starting Manual Test ---"

    // 1. نبدأ بقاموس فاضي
    let myDict = Map.empty

    // 2. نجرب نضيف كلمة "Apple"
    printfn "\n1. Adding 'Apple'..."
    // النتيجة بتتخزن في متغير اسمه dictAfterAdd
    let dictAfterAdd = 
        match addWord "Apple" "A red fruit" myDict with
        | Ok newDict -> 
            printfn " Success: Apple added!"
            newDict // بنرجع القاموس الجديد عشان نستخدمه في الخطوة الجاية
        | Error e -> 
            printfn " Error: %A" e
            myDict

    // 3. نجرب نبحث عن "apple" (سمول) عشان نختبر الـ clean
    printfn "\n2. Searching for 'apple' (small)..."
    match searchExact "apple" dictAfterAdd with
    | Some (key, meaning) -> printfn "✅ Found: %s -> %s" key meaning
    | None -> printfn " Not Found!"

    // 4. نجرب بحث جزئي عن "ppl"
    printfn "\n3. Partial Search for 'ppl'..."
    let results = searchPartial "ppl" dictAfterAdd
    if results.IsEmpty then
        printfn " No partial matches found."
    else
        printfn " Found %d matches." results.Count
        results |> Map.iter (fun k v -> printfn "   -> %s: %s" k v)

    // 5. نجرب نمسح الكلمة
    printfn "\n4. Deleting 'Apple'..."
    let dictAfterDelete =
        match deleteWord "Apple" dictAfterAdd with
        | Ok newDict -> 
            printfn " Deleted successfully!"
            newDict
        | Error e -> 
            printfn " Delete Failed: %A" e
            dictAfterAdd

    // 6. نتأكد إنها اتمسحت
    printfn "\n5. Checking if 'Apple' still exists..."
    match searchExact "Apple" dictAfterDelete with
    | Some _ -> printfn " Still there!"
    | None -> printfn " Gone (Correct)."

    printfn "\n--- Test Finished. Press Enter to Exit ---"
    Console.ReadLine() |> ignore
    0