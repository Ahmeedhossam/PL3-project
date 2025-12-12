module filesIO

open System.IO
open System.Text.Json
open FSharp.SystemTextJson
open System.Text.Json.Serialization
open wordmodel

// =========================================================
// Configuration
// =========================================================

let private jsonOptions = 
    let options = JsonSerializerOptions(WriteIndented = true)
    options.Converters.Add(JsonFSharpConverter())
    options

// =========================================================
// Synchronous Operations (السهل الممتنع)
// =========================================================

/// دالة الحفظ (مباشرة بدون async)
let saveDictionary (filePath: string) (dict: MyDictionary) =
    try
        // 1. حولنا القاموس لنص
        let jsonString = JsonSerializer.Serialize(dict, jsonOptions)
        
        // 2. كتبنا في الملف علطول (شيلنا do! و AwaitTask)
        File.WriteAllText(filePath, jsonString)
        
        Ok "Dictionary saved successfully."
    with
    | ex -> Error (sprintf "Failed to save to '%s': %s" filePath ex.Message)

/// دالة التحميل (مباشرة بدون async)
let loadDictionary (filePath: string) =
    if File.Exists(filePath) then
        try
            // 1. قرأنا الملف علطول (شيلنا let! و AwaitTask)
            let jsonString = File.ReadAllText(filePath)
            
            // 2. حولنا النص لقاموس
            let dict = JsonSerializer.Deserialize<MyDictionary>(jsonString, jsonOptions)
            Ok dict
        with
        | ex -> Error (sprintf "File is corrupted: %s" ex.Message)
    else
        // لو الملف مش موجود، رجع قاموس فاضي
        Ok Map.empty