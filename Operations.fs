module Operations

open wordmodel
open System
open System.Text.RegularExpressions



let private clean (s: string) = 
    if String.IsNullOrWhiteSpace(s) then "" 
    else 
        s.Trim()
        // الـ Regex ده بيشيل أي مسافات متكررة ويخليها مسافة واحدة
        |> fun str -> Regex.Replace(str, @"\s+", " ") 
        |> fun str -> str.ToLower()

// دالة للتحقق من المدخلات الفاضية
let private isInvalid (s: string) = String.IsNullOrWhiteSpace(s)

// ==============================
// 2. CRUD Operations (Matching Style)
// ==============================
// هنا حافظنا على أسلوب السكشن باستخدام match

let addWord (word: string) (meaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid meaning then // el word wl meaning lazem yb2a fehum data 
        Error (InvalidInput "Word and meaning must be non-empty.")
    else
        let key = clean word
        // بنشوف هل المفتاح موجود ولا لأ باستخدام match
        match dict.ContainsKey key with
        | true -> Error (WordAlreadyExists word)
        | false -> Ok (dict.Add(key, meaning))

let updateWord (word: string) (newMeaning: string) (dict: MyDictionary) =
    if isInvalid word || isInvalid newMeaning then  //el word wl meaning lazem yb2a fehum data 
        Error (InvalidInput "Word and meaning must be non-empty.")
    else
        let key = clean word
        // بنستخدم TryFind عشان نستخدم match مع Some/None
        match dict.TryFind key with
        | Some _ -> Ok (dict.Add(key, newMeaning)) // لقيناه -> عدله
        | None -> Error (WordNotFound word)         // ملقيناهوش -> Error

let deleteWord (word: string) (dict: MyDictionary) =
    if isInvalid word then
        Error (InvalidInput "Word must be non-empty.")
    else
        let key = clean word
        match dict.TryFind key with
        | Some _ -> Ok (dict.Remove key)     // لقيناه -> امسحه
        | None -> Error (WordNotFound word)  // ملقيناهوش -> Error

// ==============================
// 3. Search Operations
// ==============================

let searchExact (query: string) (dict: MyDictionary) =
    if isInvalid query then 
        None
    else
        let key = clean query
        // Matching مباشر للنتيجة
        match dict.TryFind key with
        | Some meaning -> Some (key, meaning)
        | None -> None

let searchPartial (query: string) (dict: MyDictionary) =
    if isInvalid query then 
        Map.empty
    else
        let cleanQuery = clean query
        dict
        |> Map.filter (fun key _ -> key.Contains(cleanQuery))