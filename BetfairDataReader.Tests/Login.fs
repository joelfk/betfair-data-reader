namespace Betfair.Tests

open System.Net
open Betfair.Http
open Betfair.Login
open Betfair.LoginSerialisation
open FsCheck
open FSharp.Data
open FSharp.Data.HttpRequestHeaders
open NUnit.Framework

[<TestFixture>]
module Login =

    [<Test>]
    let ``When betfairCertData is called, it returns the correct filename and password`` () =
        let (CertificateData (filename, password)) = betfairCertData ()
        Assert.AreEqual("client-2048.p12", filename)
        Assert.AreEqual("g!250953", password)

    [<Test>]
    let ``When createLoginRequest is called, it returns the correct data`` () =
        let (HttpRequest (url, httpMethod, httpHeaders, body, _)) = createLoginRequest ()
        Assert.AreEqual("https://identitysso.betfair.com/api/certlogin", url)
        Assert.AreEqual(WebRequestMethods.Http.Post, httpMethod)
        Assert.AreEqual([ "X-Application", "4ztzpQ311aLreFka"; ContentType HttpContentTypes.FormValues ], httpHeaders)
        Assert.AreEqual(FormValues [ "username", "joelfk"; "password", "g!250953" ], body)

    [<Test>]
    let ``When convertToLoginResponse given HttpSuccess, return the deserialized body`` () =
        let mockLoginResponseDeserializer s = LoginResponse (Success, Some (SessionToken s))
        let runConvertToLoginResponse s = lazy (convertToLoginResponse mockLoginResponseDeserializer (HttpSuccess s) = LoginResponse (Success, Some (SessionToken s)))
        Check.QuickThrowOnFailure runConvertToLoginResponse

    [<Test>]
    let ``When convertToLoginResponse given HttpFailure, return an error login response`` () =
        let mockLoginResponseDeserializer s = LoginResponse (Success, Some (SessionToken s))
        let runConvertToLoginResponse code s = lazy (convertToLoginResponse mockLoginResponseDeserializer (HttpFailure (code, s)) = LoginResponse (OtherError, None))
        Check.QuickThrowOnFailure runConvertToLoginResponse