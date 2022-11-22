namespace Checks

open CurriculumParser

module Checks =

    let hours (curriculum: DocxCurriculum) (number_of_semester: int) (is_last_semester: bool) =

        let mutable labor_intesity = 0

        if is_last_semester then
            for examination in curriculum.Examinations do
                labor_intesity <-
                    Semester(number_of_semester, curriculum).LaborIntensity
                    + examination.LaborIntensity
        else
            labor_intesity <- Semester(number_of_semester, curriculum).LaborIntensity

        if labor_intesity <> 30 then
            printfn "Wraning! Labor intensity (%d) does not match with norm (30) in %d semester!" labor_intesity number_of_semester

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

    let checks (curriculum: DocxCurriculum) (number_of_semester: int) =
        competences curriculum
        hours curriculum number_of_semester
