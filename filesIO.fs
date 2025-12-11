module filesIO

open System.IO
open System.Text.Json
open FSharp.SystemTextJson
open System.Text.Json.Serialization
open wordmodel

// =========================================================
// SECTION 1: Configuration
// =========================================================

let private jsonOptions = 
    let options = JsonSerializerOptions(WriteIndented = true)
    options.Converters.Add(JsonFSharpConverter())
    options

// =========================================================
// SECTION 2: Asynchronous Operations (لعيون الـ GUI)
// =========================================================

/// دالة الحفظ (Async) - دي اللي هتستخدمها في الـ GUI.
/// بتستخدم async block زي سلايد 13
let saveDictionaryAsync (filePath: string) (dict: MyDictionary) =
    async {
        try
            // بنجهز النص عادي
            let jsonString = JsonSerializer.Serialize(dict, jsonOptions)
            
            // بنكتب في الملف بشكل Async عشان الماوس ميهنجش
            // AwaitTask: عشان نحول من C# Task لـ F# Async
            do! File.WriteAllTextAsync(filePath, jsonString) |> Async.AwaitTask
            
            return Ok "Dictionary saved successfully."
        with
        | ex -> return Error (sprintf "Failed to save to '%s': %s" filePath ex.Message)
    }

/// دالة التحميل (Async) - دي اللي هتستخدمها في الـ GUI
/// بتستخدم let! زي سلايد 16
let loadDictionaryAsync (filePath: string) =
    async {
        if File.Exists(filePath) then
            try
                // بنقرأ الملف في الخلفية
                let! jsonString = File.ReadAllTextAsync(filePath) |> Async.AwaitTask
                
                let dict = JsonSerializer.Deserialize<MyDictionary>(jsonString, jsonOptions)
                return Ok dict
            with
            | ex -> return Error (sprintf "File is corrupted: %s" ex.Message)
        else
            return Ok Map.empty
    }

// =========================================================
// SECTION 3: Synchronous Operations (للـ Console)
// =========================================================

/// دالة الحفظ (Sync) - مجرد غلاف
/// بتشغل الـ Async وتستناه يخلص (زي سلايد 11: RunSynchronously)
let saveDictionary (filePath: string) (dict: MyDictionary) =
    saveDictionaryAsync filePath dict
    |> Async.RunSynchronously

/// دالة التحميل (Sync) - مجرد غلاف
let loadDictionary (filePath: string) =
    loadDictionaryAsync filePath
    |> Async.RunSynchronously