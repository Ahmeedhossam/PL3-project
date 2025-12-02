module wordmodel
open System


// شكل الديكش. المطلوب 
type MyDictionary = Map<string, string>



//هنا استخدمنا الي اتشرح في السكشن 
// Discriminated Unions
//بدل ما ارمي exceptions مباشرا
 
 


type AppError =
    | WordAlreadyExists of string
    | WordNotFound of string
    | InvalidInput of string
