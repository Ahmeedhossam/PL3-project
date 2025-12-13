```mermaid
flowchart TD

    %% =========================
    %% Application Start
    %% =========================
    A[Application Starts] --> B[UI Form Load]

    %% =========================
    %% Load Flow
    %% =========================
    B --> C[UI calls loadDictionary]
    C --> D[filesIO reads JSON file]
    D -->|Ok| E[Deserialize JSON to Map]
    D -->|Error| F[Return Error]
    E --> G[UI sets currentDict]
    F --> G
    G --> H[UI refreshes ListBox]

    %% =========================
    %% User Actions
    %% =========================
    H --> I{User Action}

    %% =========================
    %% Add Update Delete Flow
    %% =========================
    I -->|Add Update Delete| J[UI reads word and meaning]
    J --> K[UI calls Operations]
    K --> L[Clean input]
    L --> M{Input valid}

    M -->|No| N[Return AppError]
    M -->|Yes| O[Apply operation]

    O -->|Ok| P[Return new dictionary]
    O -->|Fail| Q[Return AppError]

    P --> R[UI replaces currentDict]
    R --> S[UI refreshes ListBox]

    N --> T[UI shows error]
    Q --> T

    %% =========================
    %% Search Flow
    %% =========================
    I -->|Search| U[UI reads search text]
    U --> V[UI calls search function]
    V --> W[Clean query]
    W --> X{Query empty}

    X -->|Yes| Y[Return empty result]
    X -->|No| Z[Search in dictionary]

    Z --> AA[Return results]
    Y --> AB[UI shows No matches]
    AA --> AC[UI displays results]

    %% =========================
    %% Save Flow
    %% =========================
    I -->|Save| AD[UI calls saveDictionary]
    AD --> AE[Serialize Map to JSON]
    AE --> AF[Write JSON to disk]
    AF -->|Ok| AG[Return success]
    AF -->|Error| AH[Return error]
    AG --> AI[UI shows message]
    AH --> AI
