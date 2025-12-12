module UI

open System
open System.Windows.Forms
open System.Drawing
open wordmodel 
open Operations
open filesIO

// التعديل هنا: ضفنا "as this" عشان نعرف نستخدم كلمة this جوه
type DictionaryForm() as this =
    inherit Form()

    // App State
    let mutable currentDict : MyDictionary = Map.empty
    let defaultPath = "dictionary.json"

    // GUI Controls
    let lblWord = new Label(Text = "Word:", Location = Point(20, 24), AutoSize = true)
    let txtWord = new TextBox(Location = Point(100, 20), Width = 200)

    let lblMeaning = new Label(Text = "Meaning:", Location = Point(20, 64), AutoSize = true)
    let txtMeaning = new TextBox(Location = Point(100, 60), Width = 200)

    let btnAdd = new Button(Text = "Add", Location = Point(20, 100), Width = 80)
    let btnUpdate = new Button(Text = "Update", Location = Point(110, 100), Width = 80)
    let btnDelete = new Button(Text = "Delete", Location = Point(200, 100), Width = 80)

    let separator = new Label(Text = String.replicate 45 "-", Location = Point(20, 140), AutoSize = true)
    
    let lblSearch = new Label(Text = "Search:", Location = Point(20, 175), AutoSize = true)
    let txtSearch = new TextBox(Location = Point(100, 170), Width = 120)
    let btnSearch = new Button(Text = "Find", Location = Point(230, 170), Width = 70)
    
    let lstResults = new ListBox(Location = Point(20, 210), Width = 280, Height = 150)

    let btnSave = new Button(Text = "Save", Location = Point(20, 380), Width = 130)
    let btnLoad = new Button(Text = "Load", Location = Point(170, 380), Width = 130)

    // Helpers
    let showMsg msg = MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    let showError msg = MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

    let updateList (items: Map<string, string>) =
        lstResults.Items.Clear()
        if items.IsEmpty then lstResults.Items.Add("No words found.") |> ignore
        else items |> Map.iter (fun k v -> lstResults.Items.Add(sprintf "%s -> %s" k v) |> ignore)

    let clearInputs () =
        txtWord.Clear()
        txtMeaning.Clear()
        txtWord.Focus() |> ignore

    // Constructor
    do
        // التعديل: استبدال base بـ this
        this.Text <- "Dictionary App"
        this.Size <- Size(350, 480)
        this.StartPosition <- FormStartPosition.CenterScreen
        this.FormBorderStyle <- FormBorderStyle.FixedSingle
        this.MaximizeBox <- false

        // Add Controls
        let controls : Control[] = [| 
            lblWord; txtWord; lblMeaning; txtMeaning; 
            btnAdd; btnUpdate; btnDelete; 
            separator; lblSearch; txtSearch; btnSearch; 
            lstResults; btnSave; btnLoad 
        |]
        this.Controls.AddRange(controls)

        // Add Logic
        btnAdd.Click.Add(fun _ -> 
            match addWord txtWord.Text txtMeaning.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Added successfully"
                clearInputs()
                updateList currentDict
            | Error (InvalidInput msg) -> showError msg
            | Error (WordAlreadyExists w) -> showError $"Word '{w}' already exists"
            | Error _ -> ()
        )

        btnUpdate.Click.Add(fun _ -> 
            match updateWord txtWord.Text txtMeaning.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Updated successfully"
                clearInputs()
                updateList currentDict
            | Error (WordNotFound w) -> showError $"Word '{w}' not found"
            | Error (InvalidInput msg) -> showError msg
            | Error _ -> ()
        )

        btnDelete.Click.Add(fun _ -> 
            match deleteWord txtWord.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Deleted successfully"
                clearInputs()
                updateList currentDict
            | Error (WordNotFound w) -> showError $"Word '{w}' not found"
            | Error (InvalidInput msg) -> showError msg
            | Error _ -> ()
        )

        btnSearch.Click.Add(fun _ -> 
            let query = txtSearch.Text
            if String.IsNullOrWhiteSpace query then showError "Enter text to search"
            else
                let results = partialsearch query currentDict
                lstResults.Items.Clear()
                if results.IsEmpty then lstResults.Items.Add("No matches.") |> ignore
                else results |> Map.iter (fun k v -> lstResults.Items.Add($"{k} -> {v}") |> ignore)
        )

        btnSave.Click.Add(fun _ -> 
            this.Text <- "Saving..."
            match filesIO.saveDictionary defaultPath currentDict with
            | Ok msg -> showMsg msg
            | Error err -> showError err
            this.Text <- "Dictionary App"
        )

        let loadData () =
            this.Text <- "Loading..."
            match filesIO.loadDictionary defaultPath with
            | Ok dict -> 
                currentDict <- dict
                updateList dict
            | Error err -> showError err
            this.Text <- "Dictionary App"

        btnLoad.Click.Add(fun _ -> loadData())
        this.Load.Add(fun _ -> loadData())

let run () =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    Application.Run(new DictionaryForm())