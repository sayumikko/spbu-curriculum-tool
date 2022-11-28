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

    let checks (curriculum: DocxCurriculum) (argv: string []) =
        hours curriculum