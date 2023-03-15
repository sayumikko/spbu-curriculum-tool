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
let competence_test () =
    let curriculum =
        DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.competences curriculum |> should equalSeq (seq { "ТК-1" })

[<Test>]
let correct_competence_test () =
    let curriculum =
        DocxCurriculum(
            System.AppDomain.CurrentDomain.BaseDirectory
            + "/../../../correct_curricula.docx"
        )

    Warnings.competences curriculum |> should be Empty

[<Test>]
[<Ignore("Не работает корректно до исправления парсинга иностранных языков")>]
let hours_test () =
    let curriculum =
        DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.hours curriculum |> should equalSeq (seq { (41, 1) })

[<Test>]
let correct_hours_test () =
    let curriculum =
        DocxCurriculum(
            System.AppDomain.CurrentDomain.BaseDirectory
            + "/../../../correct_curricula.docx"
        )

    Warnings.hours curriculum |> should be Empty

[<Test>]
let correct_semester_edu_level_test () =
    let curriculum =
        DocxCurriculum(
            System.AppDomain.CurrentDomain.BaseDirectory
            + "/../../../correct_curricula.docx"
        )

    Warnings.level_of_education_semesters curriculum |> should be Empty

[<Test>]
let semester_edu_level_test () =
    let curriculum =
        DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.level_of_education_semesters curriculum |> should equalSeq (seq { 4 })

[<Test>]
let correct_code_test () =
    let curriculum =
        DocxCurriculum(
            System.AppDomain.CurrentDomain.BaseDirectory
            + "/../../../correct_curricula.docx"
        )

    Warnings.codes curriculum |> should be Empty

[<Test>]
let code_test () =
    let curriculum =
        DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.codes curriculum
    |> should
        equalSeq
        (seq {
            "0035672"
            "0035743438"
            "00390"
        })

[<Test>]
[<Ignore("Тест написан для warning'a, который будет добавлена в будущем")>]
let academic_hours_test () =
    let curriculum =
        DocxCurriculum(System.AppDomain.CurrentDomain.BaseDirectory + "/../../../test_curricula.docx")

    Warnings.academic_hours curriculum
    |> should
        equalSeq
        (seq {
            (43, 0)
            (62, 56)
        })

[<Test>]
[<Ignore("Тест написан для warning'a, который будет добавлена в будущем")>]
let correct_academic_hours_test () =
    let curriculum =
        DocxCurriculum(
            System.AppDomain.CurrentDomain.BaseDirectory
            + "/../../../correct_curricula.docx"
        )

    Warnings.academic_hours curriculum |> should be Empty
