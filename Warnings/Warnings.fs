namespace Checks

open CurriculumParser

module Checks =

    let competences (curriculum: DocxCurriculum) =
        curriculum.Disciplines
        |> Seq.iter (fun d ->
            d.Implementations
            |> Seq.iter (fun d -> d.Competences |> Seq.iter (fun d -> printfn "%s" d.Code)))

    let checks (curriculum: DocxCurriculum) = competences curriculum
