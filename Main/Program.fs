open Checks
open System.IO
open CurriculumParser

let plansFolder = "../WorkingPlans"

let planNameToCode fileName = FileInfo(fileName).Name.Substring(3, 9)

let planCodeToFileName planCode =
    Directory.EnumerateFiles(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../" + plansFolder)
    |> Seq.find (fun f -> planNameToCode f = planCode)

let print_help () =
    printfn "Это инструмент для работы с учебными планами СПбГУ"
    printfn "Введите номер учебного плана и параметры предупреждений"
    printfn "Имеющиеся учебные планы:"

    Directory.EnumerateFiles(plansFolder)
    |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))
    |> Seq.iter (printf "%s ")

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        print_help ()
    else
        let curriculum = DocxCurriculum(planCodeToFileName argv[0]) //todo: add wrong name handler
        Checks.checks curriculum

    0
