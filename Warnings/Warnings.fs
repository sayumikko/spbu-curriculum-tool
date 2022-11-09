namespace Checks

open CurriculumParser

module Checks =

    let hours (curriculum: DocxCurriculum) =
        printfn "Здесь будет логика проверки зачетных часов"

    let competences (curriculum: DocxCurriculum) =

        let available_competences = curriculum.Competences |> Seq.map (fun d -> d.Code)

        let competences =
            curriculum.Disciplines
            |> Seq.map (fun d -> d.Implementations)
            |> Seq.concat
            |> Seq.map (fun d -> d.Competences)
            |> Seq.concat
            |> Seq.map (fun d -> d.Code)
            |> Seq.distinct

        for comp in available_competences do    
            if not (Seq.contains comp competences) then
                printfn "Warning! Unused competence!"

    let checks (curriculum: DocxCurriculum) =
        competences curriculum
        hours curriculum