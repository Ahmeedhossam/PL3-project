module filesIO

open System.IO
open System.Text.Json
open FSharp.SystemTextJson
open System.Text.Json.Serialization
open wordmodel


let private jsonOptions = 
    let options = JsonSerializerOptions(WriteIndented = true)
    options.Converters.Add(JsonFSharpConverter())
    options


let saveDictionary (filePath: string) (dict: MyDictionary) =
    try
        let jsonString = JsonSerializer.Serialize(dict, jsonOptions)
        
        File.WriteAllText(filePath, jsonString)
        
        Ok "Dictionary saved successfully."
    with
    | ex -> Error (sprintf "Failed to save to '%s': %s" filePath ex.Message)

let loadDictionary (filePath: string) =
    if File.Exists(filePath) then
        try
            let jsonString = File.ReadAllText(filePath)
            
            
            let dict = JsonSerializer.Deserialize<MyDictionary>(jsonString, jsonOptions)
            Ok dict
        with
        | ex -> Error (sprintf "File is corrupted: %s" ex.Message)
    else
 
        Ok Map.empty