open Warnings
open System.IO
open CurriculumParser

let plansFolder = "WorkingPlans" 

let planNameToCode fileName = FileInfo(fileName).Name.Substring(3, 9)

let planCodeToFileName planCode =
    Directory.EnumerateFiles(System.AppDomain.CurrentDomain.BaseDirectory + plansFolder)
    |> Seq.find (fun f -> planNameToCode f = planCode)

let print_plans () =
    let plans = Directory.EnumerateFiles(plansFolder) |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

    printfn "Имеющиеся учебные планы:"

    Seq.iter (printf "%s ") plans

let print_checks () = 
    printfn "На данный момент доступны следующие параметры:"
    printfn "-off -- отключить все проверки."

let print_help () = 
    printfn "Данный инструмент предназначен для проверки учебных планов СПбГУ."
    printfn "Чтобы начать, передайте первым параметром номер учебного плана."
    printfn "Далее передайте желаемые параметры предупреждений."
    printfn "Отсутствие параметра предупреждений равносильно выполнению всех проверок."

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        try
            print_help ()
            print_plans ()
        with 
        | :? DirectoryNotFoundException -> printfn "Невозможно начать работу, так как каталог %s не найден. Пожалуйста, поместите учебные планы туда." 
                                                (System.AppDomain.CurrentDomain.BaseDirectory + plansFolder)
    elif argv[0] = "-help" then 
        print_help ()
    else
        try
            let actual_curricula =
                Directory.EnumerateFiles(plansFolder)
                |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

            if Seq.contains argv[0] actual_curricula then
                let curriculum = DocxCurriculum(planCodeToFileName argv[0])
                Warnings.checks curriculum argv |> Seq.iter (fun s -> printfn "Some error")
            else
                printfn "Передайте первым параметром номер учебного плана."
                print_plans ()
        
        with 
        | :? DirectoryNotFoundException -> printfn "Каталог %s не найден. Пожалуйста, поместите учебные планы туда." 
                                                (System.AppDomain.CurrentDomain.BaseDirectory + plansFolder)
        | :? InvalidDataException -> printfn "Данный файл имеет расширение, отличное от формата .docx. Пожалуйста, передайте правильный файл."
        | :? CurriculumParser.CurriculumParsingException -> printfn "Ошибка парсинга учебного плана."

    0
