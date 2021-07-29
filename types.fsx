open System

// ===================================
// F# does support OO-style classes but
// we generally use the "composable" type system
// for most situations
// ===================================


// ===================================
// record types aka "AND" types
// ===================================

// immutable record definition
type Thing = { Id:int; Description:string }

// record construction
let aThing = { Id=1; Description="a thing" }












// copy a record using "with"
let anotherThing = {aThing with Id=2}













// anonymous record construction -- no separate type definition needed
let aContact = {| Name="Scott"; Email="scott@example.com" |}












// ===================================
// discriminated unions aka "choice" types aka "OR" types
// ===================================

type PrimaryColor = Red | Yellow | Blue














// each choice can have data associated with it
type RGB = {R:int; G:int; B:int}

type Color =
    | Primary of PrimaryColor
    | RGB of RGB
    | Named of string


















// ===================================
// Composing with types (like lego)
// ===================================

(*
Some requirements:

We accept three payment methods:
Cash, PayPal, or Card.

For Cash we don't need any extra information
For PayPal we need an email address
For Cards we need a card type and card number

How would you implement this?
*)







module PaymentDomain =


    type EmailAddress = string
    type CardType = Visa | Mastercard
    type CardNumber = string

    type CreditCardInfo = {
        CardType : CardType
        CardNumber : CardNumber
    }
    type PaymentMethod =
      | Cash
      | PayPal of EmailAddress
      | Card of CreditCardInfo













// Another thing to model:
//   "A payment is an amount and payment method and currency"

    type PaymentAmount = decimal
    type Currency = EUR | USD

    type Payment = {
      Amount : PaymentAmount
      Currency : Currency
      Method : PaymentMethod
      }




// ===================================
// Domain modeling with types
// ===================================

// nouns
type Suit = Club | Diamond | Spade | Heart
type Rank = Two | Three | Four | Five | Six | Seven | Eight
            | Nine | Ten | Jack | Queen | King
type Card = {Suit:Suit; Rank:Rank}

type Hand = Card list
type Deck = Card list

type Player = {Name:string; Hand:Hand}
type Game = {Deck:Deck; Players: Player list}

// actions
type Deal = Deck -> (Deck * Card)        // X*Y means a pair, a tuple
type PickupCard = (Hand * Card) -> Hand







// ===================================
// Making illegal states unrepresentable
// ===================================

(*
Business rules:

* Email addresses are verified by sending a link to click
* Rule 1: If the email is changed, the verified flag must be reset to false.
* Rule 2: The verified flag can only be set by a special verification service
* Rule 3: Password resets can only be sent to verified emails

*)

let isValidEmail (emailAddress:string) =
    not (String.IsNullOrWhiteSpace emailAddress)
    && emailAddress.Contains("@")

module BadDesign =

    // this design breaks Rule 1,2
    type EmailContactInfo = {EmailAddress:string; IsVerified:bool}

    // this design could break Rule 3
    let sendPasswordReset (contactInfo:EmailContactInfo) =
        // if isNull contactInfo then
        //     nullArg "contactInfo"
        if isNull contactInfo.EmailAddress then
            nullArg "contactInfo.EmailAddress"
        if not (isValidEmail(contactInfo.EmailAddress)) then  // must contains @ sign
            invalidArg "contactInfo.EmailAddress" "must be valid email"
        if not contactInfo.IsVerified then
            failwith "email is not verified"
        else
            printfn "sending email to %A" contactInfo.EmailAddress















module BetterDesign =

    // Wrapper around a string. By having a private constructor
    // and a "factory method" that does validation
    // you can ensure that the email is always a valid format
    type EmailAddress = EmailAddress of string


    let createEmailAddress str =
        if isValidEmail str then
            Some (EmailAddress str)
        else
            None









    // A verified email is DIFFERENT from a normal email.
    // So use a wrapper again!
    type VerifiedEmail = VerifiedEmail of EmailAddress











    // The service that verifies an email
    type VerificationHash = string
    type VerificationService =
        (EmailAddress * VerificationHash) -> VerifiedEmail option
    //   ^give me a email
    //                  ^and a hash
    //                                       ^I might give you a verified email













    // the final type is a choice between verified and unverified
    type EmailContactInfo =
      | Unverified of EmailAddress
      | Verified of VerifiedEmail


(*
* Rule 1: If the email is changed, the verified flag must be reset to false.
* Rule 2: The verified flag can only be set by a special verification service
*)





(*
* Rule 3: Password resets can only be sent to verified emails
*)
    let sendPasswordReset (verifiedEmail:VerifiedEmail) =

        // none of these can ever happen

        // if contactInfo == null ...
        // if contactInfo.EmailAddress == null ...
        // if not isValidEmail(contactInfo.EmailAddress)
        // if not contactInfo.IsVerified then


        printfn "sending email to %A" verifiedEmail


