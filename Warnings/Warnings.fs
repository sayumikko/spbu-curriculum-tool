namespace Checks

open CurriculumParser

module Checks =

    let hours (curriculum: DocxCurriculum) =
        printfn "Здесь будет логика проверки зачетных часов"

    let competences (curriculum: DocxCurriculum) =
        // curriculum.Disciplines
        // |> Seq.iter (fun d ->
        //     d.Implementations
        //     |> Seq.iter (fun d -> d.Competences |> Seq.iter (fun d -> printfn "%s" d.Code)))
        printfn "Здесь будет логика проверки компетенций"

    let checks (curriculum: DocxCurriculum) = competences curriculum