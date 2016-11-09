namespace Betfair

open System.Diagnostics.CodeAnalysis
open System.Net
open Betfair.Http
open Betfair.LoginSerialisation
open FSharp.Data
open FSharp.Data.HttpRequestHeaders

module Login =
    let betfairCertData () =
        ("client-2048.p12", "g!250953") |> CertificateData

    let createLoginRequest () =
        HttpRequest (
            "https://identitysso.betfair.com/api/certlogin",
            WebRequestMethods.Http.Post,
            [ "X-Application", "4ztzpQ311aLreFka"; ContentType HttpContentTypes.FormValues ],
            FormValues [ "username", "joelfk"; "password", "g!250953" ],
            Some (addClientCertification (betfairCertData ())))

    let convertToLoginResponse loginResponseDeserializer httpResponse =
        match httpResponse with
        | HttpSuccess body -> loginResponseDeserializer body
        | HttpFailure _ ->  (OtherError, None) |> LoginResponse

    [<ExcludeFromCodeCoverage>]
    let login () =
        createLoginRequest ()
        |> executeHttpRequest
        |> convertToLoginResponse loginResponseDeserializer