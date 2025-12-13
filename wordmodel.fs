module wordmodel
open System


type MyDictionary = Map<string, string>




 
 


type AppError =
    | WordAlreadyExists of string
    | WordNotFound of string
    | InvalidInput of string
