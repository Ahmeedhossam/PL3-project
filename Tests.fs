module Tests

open System
open wordmodel
open Operations

let runTests () =
    printfn "🚀 Starting Automated Unit Tests..."
    printfn "================================="

    // الحالة الابتدائية: قاموس فاضي
    let initialDict = Map.empty

    // ---------------------------------------------------------
    // Test 1: Adding a new word
    // ---------------------------------------------------------
    printfn "\n[Test 1] Adding 'Apple'..."
    let dictAfterAdd = 
        match addWord "Apple" "A red fruit" initialDict with
        | Ok d -> 
            printfn "✅ Passed: Word added successfully."
            d
        | Error e -> 
            printfn "❌ Failed: Could not add word. Error: %A" e
            initialDict

    // ---------------------------------------------------------
    // Test 2: Full Search (Exact Match)
    // ---------------------------------------------------------
    printfn "\n[Test 2] Searching for 'Apple' using fullsearch..."
    // بنجرب نبحث بكلمة "APPLE" (كابيتال) عشان نختبر الـ clean كمان
    match fullsearch "APPLE" dictAfterAdd with
    | Some (k, v) when k = "apple" -> 
        printfn "✅ Passed: Found key '%s' with meaning '%s'." k v
    | _ -> 
        printfn "❌ Failed: 'Apple' not found."

    // ---------------------------------------------------------
    // Test 3: Partial Search
    // ---------------------------------------------------------
    printfn "\n[Test 3] Partial search for 'ppl'..."
    let partialResults = partialsearch "ppl" dictAfterAdd
    if partialResults.ContainsKey "apple" then
        printfn "✅ Passed: Found 'apple' in partial results."
    else
        printfn "❌ Failed: Partial search did not return 'apple'."

    // ---------------------------------------------------------
    // Test 4: Updating a word
    // ---------------------------------------------------------
    printfn "\n[Test 4] Updating 'Apple' meaning..."
    let dictAfterUpdate = 
        match updateWord "Apple" "Green or Red fruit" dictAfterAdd with
        | Ok d -> 
            printfn "✅ Passed: Update operation successful."
            d
        | Error e -> 
            printfn "❌ Failed: Update error: %A" e
            dictAfterAdd

    // نتأكد إن المعنى اتغير فعلاً
    match fullsearch "apple" dictAfterUpdate with
    | Some (_, m) when m = "Green or Red fruit" -> 
        printfn "✅ Passed: Meaning updated correctly in memory."
    | _ -> 
        printfn "❌ Failed: Meaning did not change."

    // ---------------------------------------------------------
    // Test 5: Deleting a word
    // ---------------------------------------------------------
    printfn "\n[Test 5] Deleting 'Apple'..."
    let dictAfterDelete = 
        match deleteWord "Apple" dictAfterUpdate with
        | Ok d -> 
            printfn "✅ Passed: Delete operation successful."
            d
        | Error e -> 
            printfn "❌ Failed: Delete error: %A" e
            dictAfterUpdate

    // نتأكد إنها اتمسحت
    match fullsearch "apple" dictAfterDelete with
    | None -> printfn "✅ Passed: Word is gone."
    | Some _ -> printfn "❌ Failed: Word still exists after delete!"

    printfn "\n================================="
    printfn "🏁 All Tests Completed."
    printfn "================================="