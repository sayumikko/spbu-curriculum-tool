namespace Warnings

open CurriculumParser

module Warnings =
    let level_of_education_semesters (curriculum: DocxCurriculum) =
        let mutable warnings = Seq.empty

        let semesters =
            curriculum.Disciplines
            |> Seq.map (fun d -> d.Implementations)
            |> Seq.concat
            |> Seq.map (fun i -> i.Semester)
            |> Seq.distinct

        let level = curriculum.Programme.LevelOfEducation.ToLower()

        let number_of_semesters =
            match level with
            | "бакалавриат" -> 8
            | "специалитет" -> 10
            | "магистратура" -> 4
            | "аспирантура" -> 4
            | _ -> 0
        
        for i = 1 to number_of_semesters do
            if not (Seq.contains i semesters) then
                warnings <- Seq.append warnings [|i|]
        
        warnings

    let codes (curriculum: DocxCurriculum) =
        curriculum.Disciplines 
        |> Seq.map (fun s -> s.Code) 
        |> Seq.filter (fun s -> s.Length <> 6)

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
    
    let hours_check (curriculum: DocxCurriculum) (error_flag: bool) (output_flag: bool) = 
        let hours_errors = hours curriculum 
        if (Seq.isEmpty hours_errors) then
            if (output_flag) then
                printfn "Проверка количества зачетных единиц проведена успешно."
        else
            printfn "Найдены ошибки в количестве зачетных единиц."
            hours_errors 
            |> Seq.iter (fun a -> match a with (a, b) -> printfn "В семестре %d %d з.е." b a)
            //exit -1 Закомментировано до момента решения реализации парсера иностранных языков
    
    let competence_check (curriculum: DocxCurriculum) (error_flag: bool) (output_flag: bool) = 
        let comp_errors = competences curriculum 
        if (Seq.isEmpty comp_errors) then
            if (output_flag) then
                printfn "Проверка компетенций проведена успешно."
        else
            printfn "Внимание! Найдены неиспользованные компетенции:"
            comp_errors 
            |> Seq.iter (fun a -> printfn "%s" a)
            if error_flag then
                exit -1
    
    let level_of_education_semesters_check (curriculum: DocxCurriculum) (error_flag: bool) (output_flag: bool) = 
        let level_errors = level_of_education_semesters curriculum
        if (Seq.isEmpty level_errors) then
            if (output_flag) then
                printfn "Проверка уровня обучения проведена успешно." 
        else
            printfn "Внимание! Следующие семестры отсутствуют: "
            level_errors |> Seq.iter (fun sem -> printfn "%d" sem)
            if error_flag then
                exit -1
    
    let codes_check (curriculum: DocxCurriculum) (error_flag: bool) (output_flag: bool) = 
        let codes_errors = codes curriculum
        if (Seq.isEmpty codes_errors) then
            if (output_flag) then
                printfn "Проверка кодов предметов проведена успешно"
        else
            printfn "Внимание! Найдены коды, содержащие не 6 цифр:"
            codes_errors |> Seq.iter (fun a -> printfn "%s" a)
            if error_flag then
                exit -1
        
    let all_checks (curriculum: DocxCurriculum) (error_flag: bool) (output_flag: bool) = 
        hours_check curriculum error_flag output_flag
        competence_check curriculum error_flag output_flag
        level_of_education_semesters_check curriculum error_flag output_flag
        codes_check curriculum error_flag output_flag

    let checks (curriculum: DocxCurriculum) (argv: string []) =
        let mutable error_flag = false
        let mutable output_flag = true
        if (Array.contains "-off" argv) then 
            printfn "Все проверки для учебного плана отключены."
        elif (argv.Length > 1) then
            if (Array.contains "-err" argv) then
                error_flag <- true

            if (Array.contains "-nsout" argv) then
                output_flag <- false

            if (Array.contains "-hours" argv) then
                hours_check curriculum error_flag output_flag             

            if (Array.contains "-compet" argv) then
                competence_check curriculum error_flag output_flag
            
            if (Array.contains "-lvsem" argv) then
                level_of_education_semesters_check curriculum error_flag output_flag

            if (Array.contains "-code" argv) then
                codes_check curriculum error_flag output_flag

        else 
            all_checks curriculum error_flag output_flag
            
