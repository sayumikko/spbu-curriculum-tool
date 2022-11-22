namespace Checks

open CurriculumParser

module Checks =

    let hours (curriculum: DocxCurriculum) =
        let max_semester = curriculum.Disciplines
                            |> Seq.map (fun d -> d.Implementations)
                            |> Seq.concat
                            |> Seq.map (fun s -> s.Semester)
                            |> Seq.max

        let mutable is_last_semester = false 

        for i = 1 to max_semester do
            if i = max_semester then
                is_last_semester <- true
            else
                is_last_semester <- false
            
            let mutable labor_intesity = 0

            if is_last_semester then
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
                printfn "Warning! Unused competence %s!" comp

    let checks (curriculum: DocxCurriculum) =
        competences curriculum
        hours curriculum
