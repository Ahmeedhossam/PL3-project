module Tests

open System
open wordmodel
open Operations

// ==========================================
// دالة مساعدة (تم تعديل الترتيب عشان الـ Pipe)
// الترتيب الجديد: الاسم -> القاموس القديم -> النتيجة
// ==========================================
let private assertSuccess operationName oldDict result =
    match result with
    | Ok newDict -> 
        printfn "✅ %s: Passed" operationName
        newDict
    | Error e -> 
        printfn "❌ %s: Failed -> %A" operationName e
        oldDict // بنرجع القديم عشان السلسلة متقفش

// ==========================================
// كود الاختبارات
// ==========================================
let runTests () =
    printfn "\n🚀 Starting Unit Tests...\n"

    // 1. البداية
    let dict0 = Map.empty

    // 2. اختبار الإضافة
    // لاحظ: بعتنا dict0 لدالة assertSuccess كمان
    let dict1 = 
        addWord "Apple" "A red fruit" dict0 
        |> assertSuccess "Add 'Apple'" dict0

    // 3. اختبار البحث الكامل
    match fullsearch "APPLE" dict1 with
    | Some (k, v) -> printfn "✅ Search 'APPLE': Found (%s -> %s)" k v
    | None        -> printfn "❌ Search 'APPLE': Not Found"

    // 4. اختبار البحث الجزئي
    let partialRes = partialsearch "ppl" dict1
    if partialRes.ContainsKey "apple" then 
        printfn "✅ Partial Search 'ppl': Found 'apple'"
    else 
        printfn "❌ Partial Search 'ppl': Failed"

    // 5. اختبار التعديل
    // لاحظ: بنبعت dict1 (آخر نسخة) للدالة وللـ assert
    let dict2 = 
        updateWord "Apple" "Green fruit" dict1 
        |> assertSuccess "Update 'Apple'" dict1

    // 6. اختبار الحذف
    // لاحظ: بنبعت dict2
    let dict3 = 
        deleteWord "Apple" dict2 
        |> assertSuccess "Delete 'Apple'" dict2

    // 7. التأكد النهائي
    match fullsearch "apple" dict3 with
    | None   -> printfn "✅ Verification: Word is truly gone."
    | Some _ -> printfn "❌ Verification: Word still exists!"

    printfn "\n🏁 Tests Finished.\n"