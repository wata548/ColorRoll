This library is parse and Load CSV data

format:
    first row: field or property name line
    second row: field or property type line
           (int / float / string is wrong, use Int32 / Single / String) 
    over third lines: data line

==================================================

Parsing

--------------------------------------------------
1. Parse raw csv string to 2 dimension list,
(string -> 2d list)

var rawCSVData = File.ReadAllText(path);
CSV.Parse(rawCSVData);

--------------------------------------------------
2. Parse raw csv string or csv list(2d) to your class list
(string / 2d list -> class list(1d))

var rawCSVData = File.ReadAllText(path);
CSV.DeserializeToList<T>(rawCSVData);

(it return object, so you should cast to List<T>)
(it find field and setter property, 
if some column's type name point readonly or don't have setter properties it may cause bug)

--------------------------------------------------
3. Parse raw csv string or csv list(2d) to dictionary
(string / 2d list -> my class dictionary)

var rawCSVData = File.ReadAllText(path);
CSV.DeserializeToDictionary<T>(rawCSVData, "SerialNumber", out var keyType);

(it also return object, so you should cast to Dictionary<keyType, T>)

==================================================

Generating(3 option)

--------------------------------------------------
1. CSVFile(In Assets file) - Path type
generate class file by csv format

    1) in unity create ScriptableObject
        create/Loader/Generator/Path
    2) typing path and result's type name
    3) push 'Generate'Button

    => Scripts/AutoCSVOutputScripts/(TypeName).cs file generated
    
--------------------------------------------------
 2. CSVFile(In Assets file) - Asset type
 generate class file by csv format
 
     1) in unity create ScriptableObject
         create/Loader/Generator/File
     2) drag your file and type result's type name
     3) push 'Generate'Button
 
     => 'Scripts/AutoCSVOutputScripts/(TypeName).cs' file generated   

--------------------------------------------------
3. GoogleSpreadSheet
 generate class file by csv format
 
     1) in unity create ScriptableObject
         create/Loader/Generator/SpreadSheet
     2) type API key(able to get in Google cloud console),
        And spreadSheetPath ("https://docs.google.com/spreadsheets/d/XXX/edit?gid=YYY#gid=ZZZ"'s XXX)
        And PageName
     3) push 'Generate'Button
 
     => 'Scripts/AutoCSVOutputScripts/(TypeName).cs' file generated   
     
     
==================================================

Generating and Sync Scriptable Object(3 option)

--------------------------------------------------
1. CSVFile(In Assets file) - Path type
generate class file by csv format

    1) in unity create ScriptableObject
        create/Loader/Generator/Path
    2) typing path and result's type name
    3) push 'Generate'Button

    => Scripts/AutoCSVOutputScripts/(TypeName).cs file generated
    
--------------------------------------------------
 2. CSVFile(In Assets file) - Asset type
 generate class file by csv format
 
     1) in unity create ScriptableObject
         create/Loader/Generator/File
     2) drag your file and type result's type name
     3) push 'Generate'Button
 
     => 'Scripts/AutoCSVOutputScripts/(TypeName).cs' file generated   

--------------------------------------------------
3. GoogleSpreadSheet
 generate class file by csv format
 
     1) in unity create ScriptableObject
         create/Loader/Generator/SpreadSheet
     2) type API key(able to get in Google cloud console),
        And spreadSheetPath ("https://docs.google.com/spreadsheets/d/XXX/edit?gid=YYY#gid=ZZZ"'s XXX)
        And PageName
     3) push 'Generate'Button
 
     => 'Scripts/AutoCSVOutputScripts/(TypeName).cs' file generated   
     