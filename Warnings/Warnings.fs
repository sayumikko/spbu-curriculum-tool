namespace Checks

open CurriculumParser

module Checks =

    let hours (curriculum: DocxCurriculum) =
        let max_semester = curriculum.Disciplines
                            |> Seq.map (fun d -> d.Implementations)
                            |> Seq.concat
                            |> Seq.map (fun s -> s.Semester)
                            |> Seq.max

        for i = 1 to max_semester do
            let mutable labor_intesity = 0
            if i = max_semester then
                for examination in curriculum.Examinations do
                    labor_intesity <-
                        Semester(i, curriculum).LaborIntensity
                        + examination.LaborIntensity
            else
                labor_intesity <- Semester(i, curriculum).LaborIntensity
    
            if labor_intesity <> 30 then
                printfn "Внимание! Количество зачетных единиц (%d) не совпадает с нормой (30)." labor_intesity
                printfn "Проверьте семестр %d!" i

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
                printfn "Внимание! Неиспользованная компетенция %s!" comp

    let all_checks (curriculum: DocxCurriculum) = 
        competences curriculum 
        hours curriculum 

    let checks (curriculum: DocxCurriculum) (argv: string[]) =
        if not(Array.contains "-off" argv) then
            all_checks curriculum
