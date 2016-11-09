namespace Betfair

open System.Diagnostics.CodeAnalysis
open System.Net
open System.Security.Cryptography.X509Certificates
open System.Text
open FSharp.Data

module Http =
    type Url = string
    type HttpHeader = string * string
    type HttpMethod = string
    type CustomizeHtmlRequestMethod = HttpWebRequest -> HttpWebRequest
    type HttpRequest = HttpRequest of Url * HttpMethod * HttpHeader seq * HttpRequestBody * CustomizeHtmlRequestMethod option
    type CertificateData = CertificateData of string * string

    type HttpResponse =
        | HttpSuccess of string
        | HttpFailure of int * string
    
    [<ExcludeFromCodeCoverage>]
    let addClientCertification (certData: CertificateData) (request: HttpWebRequest) =
        let (CertificateData (filename, password)) = certData
        request.ClientCertificates.Add(new X509Certificate2(filename, password)) |> ignore
        request

    let getResponseString body =
        match body with
        | Binary bytes ->
            match bytes with
            | null -> ""
            | _ -> Encoding.ASCII.GetString(bytes)
        | Text s ->
            match s with
            | null -> ""
            | _ -> s

    let convertHttpResponse response =
        match response.StatusCode with
        | 200 ->
            getResponseString response.Body |> HttpSuccess
        | _ ->
            (response.StatusCode, getResponseString response.Body) |> HttpFailure

    [<ExcludeFromCodeCoverage>]
    let executeHttpRequest request =
        let (HttpRequest (url, httpMethod, httpHeaders, body, customizeHtmlRequestMethod)) = request
        let response =
            match customizeHtmlRequestMethod with
            | Some customizeHtmlRequestMethodValue -> 
                Http.Request(url = url, httpMethod = httpMethod, headers = httpHeaders, body = body, customizeHttpRequest = customizeHtmlRequestMethodValue)
            | None ->
                Http.Request(url = url, httpMethod = httpMethod, headers = httpHeaders, body = body)
        convertHttpResponse response