module Testing

open NUnit.Framework
open FsUnit
open CurriculumParser
open Warnings

[<TestFixture>]
type MyTests() =

    [<SetUp>]
    member __.setup() =
        FSharpCustomMessageFormatter() |> ignore

[<Test>]
let competence_check () =
    let curriculum = DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.competences curriculum |> should equalSeq (seq {"ТК-1"}) 

[<Test>]
let hours_check () =
    let curriculum = DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")
    Warnings.hours curriculum |> should equalSeq (seq {(41,1); (33,2); (33,3); (33,4)})
