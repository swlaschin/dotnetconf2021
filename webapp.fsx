// ==============================
// Web app in F# built using pipelines
// ==============================

#r "nuget:Suave"  // using https://suave.io/

open System
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open System.Threading
open System.Net

// =====================================
// A simple web app
// =====================================

let homePage = """
<h2>Hello from a simple F# web app</h2>
<form action="/hello" ><input type="submit" value="GET /hello"></form>
<form action="/goodbye/Scott" ><input type="submit" value="GET /goodbye/Scott"></form>
<hr/>
<form action="/hello" method="POST"><input type="submit" value="POST /hello"></form>
<form action="/goodbye" method="POST"><input type="submit" value="POST /goodbye"></form>
"""

let helloHandler :WebPart =
  fun ctx -> async {
      return! OK "Hello using a custom webpart" ctx
    }

let goodbyeHandler name :WebPart =
  fun ctx -> async {
      return!
        if String.IsNullOrWhiteSpace name then
          RequestErrors.BAD_REQUEST "name must not be blank" ctx
        else
          let msg = sprintf "Goodbye, %s" name
          OK msg ctx
    }

let logger :WebPart =
  fun ctx -> async {
      printfn "LOG method=%A path=%s" ctx.request.method ctx.request.path
      return (Some ctx)
    }

let webApp =
  choose
    [ GET >=> choose
        [ path "/" >=> OK homePage
          path "/hello" >=> OK "Hello using GET" >=> logger
          pathScan "/goodbye/%s" goodbyeHandler >=> logger
          ]
      POST >=> choose
        [ path "/hello" >=> helloHandler
          path "/goodbye" >=> OK "Goodbye using POST" ]
          ]

(*
open http://127.0.0.1:8080/

dotnet fsi webapp.fsx
*)






// for command line use, add a cancellation token
let cts = new CancellationTokenSource()
let config = {defaultConfig with cancellationToken = cts.Token }
let listening, server = startWebServerAsync config webApp

Async.Start(server, cts.Token)

printfn "WebApp started"
Console.ReadKey true |> ignore
cts.Cancel() // kill the app
printfn "WebApp stopped"



