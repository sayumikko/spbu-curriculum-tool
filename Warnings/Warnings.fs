namespace Warnings

open CurriculumParser

module Warnings =

    let hours (curriculum: DocxCurriculum) =
        let mutable warnings = Seq.empty

        let max_semester =
            curriculum.Disciplines
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
                warnings <- Seq.append warnings [|labor_intesity, i|]
        
        warnings

    let competences (curriculum: DocxCurriculum) =

        let available_competences =
            curriculum.Competences
            |> Seq.map (fun d -> d.Code)

        let competences =
            curriculum.Disciplines
            |> Seq.map (fun d -> d.Implementations)
            |> Seq.concat
            |> Seq.map (fun d -> d.Competences)
            |> Seq.concat
            |> Seq.map (fun d -> d.Code)
            |> Seq.distinct

        available_competences 
        |> Seq.except competences
    
    let hours_check (curriculum: DocxCurriculum) (error_flag: bool) = 
        let hours_errors = hours curriculum 
        if (Seq.isEmpty hours_errors) then
            printfn "Проверка количества зачетных единиц проведена успешно."
        else
            printfn "Найдены ошибки в количестве зачетных единиц."
            hours_errors 
            |> Seq.iter (fun a -> match a with (a, b) -> printfn "В семестре %d %d з.е." b a)
            //failwithf "" Закомментировано до момента решения реализации парсера иностранных языков
    
    let competence_check (curriculum: DocxCurriculum) (error_flag: bool) = 
        let comp_errors = competences curriculum 
        if (Seq.isEmpty comp_errors) then
            printfn "Проверка компетенций проведена успешно."
        else
            printfn "Найдены неиспользованные компетенции:"
            comp_errors 
            |> Seq.iter (fun a -> printfn "%s" a)
            if error_flag then
                failwithf "Пожалуйста, исправьте неиспользуемые компетенции.\n"
        
    let all_checks (curriculum: DocxCurriculum) (error_flag: bool) = 
        hours_check curriculum error_flag
        competence_check curriculum error_flag

    let checks (curriculum: DocxCurriculum) (argv: string []) =
        if (Array.contains "-off" argv) then 
            printfn "Все проверки для учебного плана отключены."
        else
            let mutable error_flag = false

            if (Array.contains "-err" argv) then
                error_flag <- true

            if (Array.contains "-hours" argv) then
                hours_check curriculum error_flag

            if (Array.contains "-compet" argv) then
                competence_check curriculum error_flag

            else 
                all_checks curriculum error_flag
            
