module Operations

open wordmodel
open System
open System.Text.RegularExpressions



let private isInvalid (s: string) = String.IsNullOrWhiteSpace(s)

let private clean (s: string) =
    if String.IsNullOrWhiteSpace(s) then ""
    else
        s.Trim()
        |> fun str -> Regex.Replace(str, @"\s+", " ")
        |> fun str -> Regex.Replace(str, @"[^a-zA-Z\s]", "")
        |> fun str -> str.ToLower()
        |> fun str -> str.Trim()


let addWord (word: string) (meaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid meaning then
        Error (InvalidInput "Word and meaning must be not empty")
    else
        let key = clean word
        let cleanMeaning = meaning.Trim() 

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only")
        else
            match dict.ContainsKey key with
            | true -> Error (WordAlreadyExists word)
            | false -> Ok (dict.Add(key, cleanMeaning)) 


let updateWord (word: string) (newMeaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid newMeaning then
        Error (InvalidInput "Word and meaning must be not empty")
    else
        let key = clean word
        
        let cleanNewMeaning = newMeaning.Trim()

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only")
        else
            match dict.TryFind key with
            | Some _ -> Ok (dict.Add(key, cleanNewMeaning))
            | None -> Error (WordNotFound word)


let deleteWord (word: string) (dict: MyDictionary) =
    if isInvalid word then
        Error (InvalidInput "Word must be not empty")
    else
        let key = clean word

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only")
        else
            match dict.TryFind key with
            | Some _ -> Ok (dict.Remove key)
            | None -> Error (WordNotFound word)




let fullsearch (query: string) (dict: MyDictionary) =
    if isInvalid query then
        None
    else
        let key = clean query

        if String.IsNullOrWhiteSpace(key) then
            None
        else
            match dict.TryFind key with
            | Some meaning -> Some (key, meaning)
            | None -> None


let partialsearch (query: string) (dict: MyDictionary) =
    if isInvalid query then
        Map.empty

    else
        let partialSearchQuery = clean query

        if String.IsNullOrWhiteSpace(partialSearchQuery) then
            Map.empty // why here we make map.empty ?   because if the cleaned query is empty, it means there are no valid letters to search for, so we return an empty map.  
        else
            dict
            |> Map.filter (fun key _ -> key.Contains(partialSearchQuery))