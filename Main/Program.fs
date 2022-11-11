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

[<EntryPoint>]
let main argv =
    if argv.Length = 0 then
        printfn "Передайте учебный план и параметры предупреждений."
        print_plans ()
    else
        let actual_curricula =
            Directory.EnumerateFiles(plansFolder)
            |> Seq.map (fun p -> FileInfo(p).Name.Substring(3, 9))

        let semesters = [ 1..8 ]

        if Seq.contains argv[0] actual_curricula then
            if argv.Length = 2 then
                if List.exists (fun elem -> elem = (int argv[1])) semesters then
                    let curriculum = DocxCurriculum(planCodeToFileName argv[0])
                    Checks.checks curriculum (int argv[1])
        else
            printfn "Передайте первым параметром номер учебного плана"
            printfn "Передайте вторым параметром номер семестра"
            print_plans ()

    0
