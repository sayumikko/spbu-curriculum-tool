namespace Checks

open CurriculumParser

module Checks =

    let count_max_semester(curriculum: DocxCurriculum) = 
        curriculum.Disciplines
                            |> Seq.map (fun d -> d.Implementations)
                            |> Seq.concat
                            |> Seq.map (fun s -> s.Semester)
                            |> Seq.max


    let hours (curriculum: DocxCurriculum) =
        let max_semester = count_max_semester(curriculum)

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

    
    let level_of_education (curriculum: DocxCurriculum) =
        let is_level level max_semester = 
            match level, max_semester with  
            | "бакалавриат", 8
            | "специалитет", 10
            | "магистратура", 4 
            | "аспирантура", 4 -> true
            | _ -> false
        
        let level = curriculum.Programme.LevelOfEducation.ToLower()
        let max_semester = count_max_semester(curriculum)

        if not (is_level level max_semester) then 
            printfn "Внимание! Несоответствие уровня образования и количества семестров!" 


    let all_checks (curriculum: DocxCurriculum) = 
        competences curriculum 
        hours curriculum
        level_of_education curriculum 


    let checks (curriculum: DocxCurriculum) (argv: string[]) =
        if not(Array.contains "-off" argv) then
            all_checks curriculum