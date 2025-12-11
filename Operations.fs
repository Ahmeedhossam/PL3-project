module Operations

open wordmodel
open System
open System.Text.RegularExpressions



// دالة للتحقق من المدخلات الفاضية قبل الـ clean
let private isInvalid (s: string) = String.IsNullOrWhiteSpace(s)

// دالة لتنضيف الكلمة (Trim + Lowercase + Remove symbols)
let private clean (s: string) =
    if String.IsNullOrWhiteSpace(s) then ""
    else
        s.Trim()
        |> fun str -> Regex.Replace(str, @"\s+", " ")          // شيل مسافات زيادة
        |> fun str -> Regex.Replace(str, @"[^a-zA-Z\s]", "")   // شيل الرموز والأرقام
        |> fun str -> str.ToLower()
        |> fun str -> str.Trim()



let addWord (word: string) (meaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid meaning then
        Error (InvalidInput "Word and meaning must be non-empty.")
    else
        let key = clean word

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only.")
        else
            match dict.ContainsKey key with
            | true -> Error (WordAlreadyExists word)
            | false -> Ok (dict.Add(key, meaning))


let updateWord (word: string) (newMeaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid newMeaning then
        Error (InvalidInput "Word and meaning must be non-empty.")
    else
        let key = clean word

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only.")
        else
            match dict.TryFind key with
            | Some _ -> Ok (dict.Add(key, newMeaning))
            | None -> Error (WordNotFound word)


let deleteWord (word: string) (dict: MyDictionary) =
    if isInvalid word then
        Error (InvalidInput "Word must be non-empty.")
    else
        let key = clean word

        if String.IsNullOrWhiteSpace(key) then
            Error (InvalidInput "Word must contain valid letters only.")
        else
            match dict.TryFind key with
            | Some _ -> Ok (dict.Remove key)
            | None -> Error (WordNotFound word)


// Search Operations

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
        let cleanQuery = clean query

        if String.IsNullOrWhiteSpace(cleanQuery) then
            Map.empty
        else
            dict
            |> Map.filter (fun key _ -> key.Contains(cleanQuery))
