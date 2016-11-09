namespace Betfair.Tests

open System.Text
open Betfair.Http
open FsCheck
open FSharp.Data
open NUnit.Framework

[<TestFixture>]
module Http =

    [<Test>]
    let ``When convertHttpResponse is called with a 200 status code and a null text response, return HttpSuccess with empty string`` () =
        Assert.AreEqual(HttpSuccess "", convertHttpResponse {StatusCode = 200; Body = Text null; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty})

    [<Test>]
    let ``When convertHttpResponse is called with a 200 status code and a non-null text response, return HttpSuccess with text`` () =
        let convert200ResponseWithNonNullTextReturnsHttpSuccessWithText s = not (isNull s) ==> lazy (convertHttpResponse {StatusCode = 200; Body = Text s; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpSuccess s)
        Check.QuickThrowOnFailure convert200ResponseWithNonNullTextReturnsHttpSuccessWithText

    [<Test>]
    let ``When convertHttpResponse is called with a 200 status code and a null binary response, return HttpSuccess with empty string`` () =
        Assert.AreEqual(HttpSuccess "", convertHttpResponse {StatusCode = 200; Body = Binary null; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty})

    [<Test>]
    let ``When convertHttpResponse is called with a 200 status code and a non-null binary response, return HttpSuccess with the equivalent text`` () =
        let convert200ResponseWithNonNullBinaryReturnsHttpFailureWithText (s:string) = not (isNull s) ==> lazy (convertHttpResponse {StatusCode = 200; Body = Binary (Encoding.ASCII.GetBytes(s)); ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpSuccess s)
        Check.QuickThrowOnFailure convert200ResponseWithNonNullBinaryReturnsHttpFailureWithText

    [<Test>]
    let ``When convertHttpResponse is called with a non-200 status code and a null text response, return HttpFailure with status code and empty string`` () =
        let convertNon200ResponseWithNullTextReturnsHttpFailureWithEmptyString statusCode = statusCode <> 200 ==> lazy (convertHttpResponse {StatusCode = statusCode; Body = Text null; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpFailure (statusCode, ""))
        Check.QuickThrowOnFailure convertNon200ResponseWithNullTextReturnsHttpFailureWithEmptyString

    [<Test>]
    let ``When convertHttpResponse is called with a non-200 status code and a non-null text response, return HttpFailure with status code and text`` () =
        let convertNon200ResponseWithNonNullTextReturnsHttpFalureWithText statusCode s = (statusCode <> 200 && not (isNull s)) ==> lazy (convertHttpResponse {StatusCode = statusCode; Body = Text s; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpFailure (statusCode, s))
        Check.QuickThrowOnFailure convertNon200ResponseWithNonNullTextReturnsHttpFalureWithText

    [<Test>]
    let ``When convertHttpResponse is called with a non-200 status code and a null binary response, return HttpFailure with status code and empty string`` () =
        let convertNon200ResponseWithNullBinaryReturnsHttpFailureWithEmptyString statusCode = statusCode <> 200 ==> lazy (convertHttpResponse {StatusCode = statusCode; Body = Binary null; ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpFailure (statusCode, ""))
        Check.QuickThrowOnFailure convertNon200ResponseWithNullBinaryReturnsHttpFailureWithEmptyString

    [<Test>]
    let ``When convertHttpResponse is called with a non-200 status code and a non-null binary response, return HttpFailure with status code and text`` () =
        let convertNon200ResponseWithNonNullBinaryReturnsHttpSuccessWithText statusCode (s:string) = (statusCode <> 200 && not (isNull s)) ==> lazy (convertHttpResponse {StatusCode = statusCode; Body = Binary (Encoding.ASCII.GetBytes(s)); ResponseUrl = null; Headers = Map.empty; Cookies = Map.empty} = HttpFailure (statusCode, s))
        Check.QuickThrowOnFailure convertNon200ResponseWithNonNullBinaryReturnsHttpSuccessWithText