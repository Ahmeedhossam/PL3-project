module UI

open System
open System.Windows.Forms
open System.Drawing
// استخدمنا الأسماء الـ small زي ما هي عندك في المشروع
open wordmodel 
open Operations
open filesIO

type DictionaryForm() as form =
    inherit Form()

    // الحالة (State)
    let mutable currentDict : MyDictionary = Map.empty
    let defaultPath = "dictionary.json"

    // ==========================================
    // تعريف العناصر (Controls)
    // ==========================================
    
    let lblWord = new Label(Text = "Word:", Location = Point(20, 20), AutoSize = true)
    let txtWord = new TextBox(Location = Point(100, 20), Width = 200)

    let lblMeaning = new Label(Text = "Meaning:", Location = Point(20, 60), AutoSize = true)
    let txtMeaning = new TextBox(Location = Point(100, 60), Width = 200)

    let btnAdd = new Button(Text = "Add", Location = Point(20, 100), Width = 80)
    let btnUpdate = new Button(Text = "Update", Location = Point(110, 100), Width = 80)
    let btnDelete = new Button(Text = "Delete", Location = Point(200, 100), Width = 80)

    let separator = new Label(Text = "-----------------------------", Location = Point(20, 140), AutoSize = true)
    let lblSearch = new Label(Text = "Search:", Location = Point(20, 170), AutoSize = true)
    let txtSearch = new TextBox(Location = Point(100, 170), Width = 120)
    let btnSearch = new Button(Text = "Find", Location = Point(230, 170), Width = 70)
    
    let lstResults = new ListBox(Location = Point(20, 210), Width = 280, Height = 150)

    let btnSave = new Button(Text = "Save (Async)", Location = Point(20, 380), Width = 130)
    let btnLoad = new Button(Text = "Load (Async)", Location = Point(170, 380), Width = 130)

    // ==========================================
    // دوال مساعدة
    // ==========================================
    
    let showMsg msg = MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    let showError msg = MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

    let updateList (items: Map<string, string>) =
        lstResults.Items.Clear()
        if items.IsEmpty then
            lstResults.Items.Add("Dictionary is empty.") |> ignore
        else
            items |> Map.iter (fun k v -> lstResults.Items.Add(sprintf "%s -> %s" k v) |> ignore)

    let clearInputs () =
        txtWord.Text <- ""
        txtMeaning.Text <- ""
        txtWord.Focus() |> ignore

    // ==========================================
    // الأحداث (Events)
    // ==========================================

    do
        form.Text <- "My F# Dictionary"
        form.Size <- Size(350, 480)
        form.StartPosition <- FormStartPosition.CenterScreen
        form.FormBorderStyle <- FormBorderStyle.FixedSingle
        form.MaximizeBox <- false
        
        // حل مشكلة المصفوفة: بنحول كل عنصر لـ Control
        form.Controls.AddRange([| 
            lblWord :> Control; txtWord :> Control; lblMeaning :> Control; txtMeaning :> Control;
            btnAdd :> Control; btnUpdate :> Control; btnDelete :> Control;
            separator :> Control; lblSearch :> Control; txtSearch :> Control; btnSearch :> Control;
            lstResults :> Control; btnSave :> Control; btnLoad :> Control 
        |])

        // زرار Add
        btnAdd.Click.Add(fun _ -> 
            match addWord txtWord.Text txtMeaning.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Word Added Successfully!"
                clearInputs()
                updateList currentDict
            | Error (InvalidInput msg) -> showError msg
            | Error (WordAlreadyExists w) -> showError (sprintf "Word '%s' already exists!" w)
            | Error (WordNotFound _) -> () 
        )

        // زرار Update
        btnUpdate.Click.Add(fun _ -> 
            match updateWord txtWord.Text txtMeaning.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Word Updated!"
                clearInputs()
                updateList currentDict
            | Error (WordNotFound w) -> showError (sprintf "Cannot update. Word '%s' not found." w)
            | Error (InvalidInput msg) -> showError msg
            | Error _ -> ()
        )

        // زرار Delete
        btnDelete.Click.Add(fun _ -> 
            match deleteWord txtWord.Text currentDict with
            | Ok newDict -> 
                currentDict <- newDict
                showMsg "Word Deleted!"
                clearInputs()
                updateList currentDict
            | Error (WordNotFound w) -> showError (sprintf "Word '%s' not found." w)
            | Error (InvalidInput msg) -> showError msg
            | Error _ -> ()
        )

        // زرار Search
        btnSearch.Click.Add(fun _ -> 
            let query = txtSearch.Text
            if String.IsNullOrWhiteSpace query then
                showError "Please enter text to search."
            else
                // ====================================================
                // التعديل هنا: استخدمنا اسم الدالة بتاعك partialsearch
                // ====================================================
                let results = partialsearch query currentDict
                
                lstResults.Items.Clear()
                if results.IsEmpty then
                    lstResults.Items.Add("No matches found.") |> ignore
                else
                    results |> Map.iter (fun k v -> lstResults.Items.Add(sprintf "%s -> %s" k v) |> ignore)
        )

        // زرار Save (Async)
        btnSave.Click.Add(fun _ -> 
            async {
                btnSave.Enabled <- false
                form.Text <- "Saving..." 
                // استخدمنا filesIO سمول
                let! result = filesIO.saveDictionaryAsync defaultPath currentDict
                match result with
                | Ok msg -> showMsg msg
                | Error err -> showError err
                form.Text <- "My F# Dictionary"
                btnSave.Enabled <- true
            } |> Async.StartImmediate
        )

        // دالة Load المساعدة
        let loadData () = 
            async {
                btnLoad.Enabled <- false
                form.Text <- "Loading..."
                // استخدمنا filesIO سمول
                let! result = filesIO.loadDictionaryAsync defaultPath
                match result with
                | Ok dict -> 
                    currentDict <- dict
                    updateList dict
                | Error err -> showError err
                form.Text <- "My F# Dictionary"
                btnLoad.Enabled <- true
            } 

        // زرار Load وتشغيل التحميل عند الفتح
        btnLoad.Click.Add(fun _ -> loadData() |> Async.StartImmediate)
        form.Load.Add(fun _ -> loadData() |> Async.StartImmediate)

let run () =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)
    Application.Run(new DictionaryForm())