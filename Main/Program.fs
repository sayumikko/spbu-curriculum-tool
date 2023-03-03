open Warnings
open System.IO
open CurriculumParser

let plansFolder = "../WorkingPlans"

let planNameToCode fileName = FileInfo(fileName).Name.Substring(3, 9)

let planCodeToFileName planCode =
    Directory.EnumerateFiles(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../" + plansFolder)
    |> Seq.find (fun f -> planNameToCode f = planCode)

let print_plans () =
    let plans =
        Directory.EnumerateFiles(plansFolder)
        |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

    printfn "Имеющиеся учебные планы:"

    Seq.iter (printf "%s ") plans

let print_checks () =
    printfn "На данный момент доступны следующие параметры:"
    printfn "-off - отключает указанные вместе с параметром проверки."
    printfn "-err - вывести все предупреждения как ошибки (по умолчанию выводится текст в консоль)."
    printfn "-nout - вывести в консоль только ошибки."
    printfn "-hours - проверить количества зачетных единиц в семестрах."
    printfn "-compet - проверить соответствие списка компетенций используемым компетенциям."
    printfn "-lvsem - проверить соответствие количества семестров уровню образования и вывести недостающие, если есть."
    printfn "-code - проверить соответствие всех кодов дисциплины шестизначному шаблону."

let print_help () =
    printfn "Данный инструмент предназначен для проверки учебных планов СПбГУ."
    printfn "Чтобы начать, передайте первым параметром номер учебного плана."
    printfn "Далее передайте желаемые параметры предупреждений."
    printfn "Отсутствие параметра предупреждений равносильно выполнению всех проверок."
    printfn "Чтобы увидеть доступные параметры проверок введите -checks."

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        try
            print_help ()
            print_plans ()
        with :? DirectoryNotFoundException ->
            printfn
                "Невозможно начать работу, так как каталог %s не найден. Пожалуйста, поместите учебные планы туда."
                (System.AppDomain.CurrentDomain.BaseDirectory + plansFolder)
    elif argv[0] = "-help" then
        print_help ()
    elif argv[0] = "-checks" then
        print_checks ()
    else
        try
            let actual_curricula =
                Directory.EnumerateFiles(plansFolder)
                |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

            if Seq.contains argv[0] actual_curricula then
                let curriculum = DocxCurriculum(planCodeToFileName argv[0])
                Warnings.checks curriculum argv
            else
                printfn "Передайте первым параметром номер учебного плана."
                print_plans ()

        with
        | :? DirectoryNotFoundException ->
            printfn
                "Каталог %s не найден. Пожалуйста, поместите учебные планы туда."
                (System.AppDomain.CurrentDomain.BaseDirectory + plansFolder)
        | :? InvalidDataException ->
            printfn "Данный файл имеет расширение, отличное от формата .docx. Пожалуйста, передайте правильный файл."
        | :? CurriculumParser.CurriculumParsingException -> printfn "Ошибка парсинга учебного плана."

    0
