open Checks
open System.IO
open CurriculumParser

let plansFolder = "../WorkingPlans"

let planNameToCode fileName = FileInfo(fileName).Name.Substring(3, 9)

let planCodeToFileName planCode =
    Directory.EnumerateFiles(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../" + plansFolder)
    |> Seq.find (fun f -> planNameToCode f = planCode)

let print_plans () =
    printfn "Имеющиеся учебные планы:"

    Directory.EnumerateFiles(plansFolder)
    |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))
    |> Seq.iter (printf "%s ")

let print_help () = 
    printfn "Данный инструмент предназначен для проверки учебных планов СПбГУ."
    printfn "На данный момент доступны следующие параметры:"
    printfn ""

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        printfn "Передайте учебный план и параметры предупреждений."
        print_plans ()
    else
        let actual_curricula =
            Directory.EnumerateFiles(plansFolder)
            |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

        if Seq.contains argv[0] actual_curricula then
            let curriculum = DocxCurriculum(planCodeToFileName argv[0])
            
            Checks.checks curriculum 
            
        else
            printfn "Передайте первым параметром номер учебного плана"
            print_plans ()

    0
