namespace Checks

open CurriculumParser

module Checks =

    let count_max_semester (curriculum: DocxCurriculum) =
        curriculum.Disciplines
        |> Seq.map (fun d -> d.Implementations)
        |> Seq.concat
        |> Seq.map (fun s -> s.Semester)
        |> Seq.max


    let hours (curriculum: DocxCurriculum) =
        let max_semester = count_max_semester (curriculum)

        for i = 1 to max_semester do
            let mutable labor_intesity = Semester(i, curriculum).LaborIntensity

            if i = max_semester then
                for examination in curriculum.Examinations do
                    labor_intesity <- labor_intesity + examination.LaborIntensity

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
        let semesters =
            curriculum.Disciplines
            |> Seq.map (fun d -> d.Implementations)
            |> Seq.concat
            |> Seq.map (fun i -> i.Semester)
            |> Seq.distinct

        let print_warning (semesters: seq<int>) (number: int) =
            for i = 1 to number do
                if not (Seq.contains i semesters) then
                    printfn "Внимание! Не хватает семестра %d!" i

        let is_level level max_semester =
            match level, max_semester with
            | "бакалавриат", 8 -> print_warning semesters 8
            | "специалитет", 10 -> print_warning semesters 10
            | "магистратура", 4
            | "аспирантура", 4 -> print_warning semesters 4
            | _ -> printfn "Внимание! Неопознанный уровень образования!"

        let level = curriculum.Programme.LevelOfEducation.ToLower()
        let max_semester = count_max_semester (curriculum)

        is_level level max_semester


    let check_codes (curriculum: DocxCurriculum) =
        let disciplines = curriculum.Disciplines

        for discipline in disciplines do
            if discipline.Code.Length <> 6 then
                printfn "Внимание! Код дисциплины \"%s\" содержит не 6 цифр!" discipline.RussianName


    let all_checks (curriculum: DocxCurriculum) =
        competences curriculum
        hours curriculum
        level_of_education curriculum
        check_codes curriculum


    let checks (curriculum: DocxCurriculum) (argv: string[]) =
        if not (Array.contains "-off" argv) then
            all_checks curriculum