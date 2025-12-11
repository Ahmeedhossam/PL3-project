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




let saveDictionaryAsync (filePath: string) (dict: MyDictionary) =
    async {
        try
            let jsonString = JsonSerializer.Serialize(dict, jsonOptions)

            do! File.WriteAllTextAsync(filePath, jsonString) |> Async.AwaitTask
            
            return Ok "Dictionary saved successfully."
        with
        | ex -> return Error (sprintf "Failed to save to '%s': %s" filePath ex.Message)
    }


let loadDictionaryAsync (filePath: string) =
    async {
        if File.Exists(filePath) then
            try
                let! jsonString = File.ReadAllTextAsync(filePath) |> Async.AwaitTask
                
                let dict = JsonSerializer.Deserialize<MyDictionary>(jsonString, jsonOptions)
                return Ok dict
            with
            | ex -> return Error (sprintf "File is corrupted: %s" ex.Message)
        else
            return Ok Map.empty
    }


let saveDictionary (filePath: string) (dict: MyDictionary) =
    saveDictionaryAsync filePath dict
    |> Async.RunSynchronously

let loadDictionary (filePath: string) =
    loadDictionaryAsync filePath
    |> Async.RunSynchronously