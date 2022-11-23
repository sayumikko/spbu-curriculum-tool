module Testing

open NUnit.Framework
open FsUnit
open CurriculumParser
open Warnings

[<TestFixture>]
type MyTests () =

    [<SetUp>]
    member __.setup () =
        FSharpCustomMessageFormatter() |> ignore

[<Test>]
let competence_check () = 
    let path = "/home/sayumiko/Курсовая/spbu-curriculum-tool/Testing/test_curricula.docx"
    let curriculum = DocxCurriculum(path)
    Warnings.competences curriculum |> should be null
    
