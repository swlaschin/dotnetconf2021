(*
Definition of FizzBuzz:
    Given a number
    If it is divisible by 3, return "Fizz"
    If it is divisible by 5, return "Buzz"
    If it is divisible by 3 and 5, return "FizzBuzz"
    Otherwise return the number as a string
    Do NOT print anything
*)

// helper function
let isDivisibleBy divisor n =
    (n % divisor) = 0

// a straightforward implementation
let simpleFizzBuzz n =
    if n |> isDivisibleBy 15 then
        "FizzBuzz"    // NOTE no return keyword needed
    else if n |> isDivisibleBy 3 then
        "Fizz"
    else if n |> isDivisibleBy 5 then
        "Buzz"
    else
        string n


// test it
simpleFizzBuzz 3
simpleFizzBuzz 4

// test it on all the numbers up to 30
[1..30]
|> List.map simpleFizzBuzz
|> String.concat ","


// =============================================
// Implement FizzBuzz as a pipeline
// =============================================

(*
Rewrite this using a piping model.

let fizzBuzz n =
    n
    |> handle15case
    |> handle3case
    |> handle5case
    |> finalStep
*)



// Define a record structure to pass between the tests for 3,5,7 etc
type FizzBuzzData = {ResultString:string; Number:int}

/// Test whether a data.number is divisible by 15
/// If true, return the "FizzBuzz" in data.ResultString.
/// BUT only do this if not already handled (data.ResultString is empty)
let handle15case fizzBuzzData =

    // is it already handled?
    if fizzBuzzData.ResultString <> "" then
        fizzBuzzData // leave alone

    // is it divisible?
    else if not (fizzBuzzData.Number |> isDivisibleBy 15) then
        fizzBuzzData // leave alone

    // ok, handle this case
    else
        // create a new value which *is* handled
        {ResultString="FizzBuzz"; Number=fizzBuzzData.Number}




/// A much more generic version of handle15case
/// --------------------------------------------
/// Test whether data.number is divisible by divisor
/// If true, return the label in data.handled.
/// BUT only do this if not already handled (data.handled is empty)
let handle divisor label fizzBuzzData =

    // is it already handled?
    if fizzBuzzData.ResultString <> "" then
        fizzBuzzData // leave alone

    // is it divisible?
    else if not (fizzBuzzData.Number |> isDivisibleBy divisor) then
        fizzBuzzData // leave alone

    // ok, handle this case
    else
        // create a new value which is handled
        {ResultString=label; Number=fizzBuzzData.Number}


// If still unhandled at the end,
// convert data.Number into a string,
// else return data.ResultString
let finalStep fizzBuzzData =
    if fizzBuzzData.ResultString = "" then
        string fizzBuzzData.Number
    else
        fizzBuzzData.ResultString

// Finally, the main fizzBuzz function!
let fizzBuzz (n:int) :string =
    let initialData = {ResultString=""; Number=n}
    initialData
    |> handle 15 "FizzBuzz"
    |> handle 3 "Fizz"
    |> handle 5 "Buzz"
    |> finalStep


// test it interactively
[1..30]
|> List.map fizzBuzz





