// =============================================
// Convert a number to Roman numerals
// =============================================

(*

Use the "tally" system:

* Start with N copies of "I".
  Eg. The number 8 becomes "IIIIIIII"
* Replace "IIIII" with "V"
* Replace "VV" with "X"
* Replace "XXXXX" with "L"
* Replace "LL" with "C"
* Replace "CCCCC"  with "D"
* Replace "DD" with "M"

*)

/// Helper to convert the built-in .NET library method
/// to a pipeable function
let replace (oldValue:string) (newValue:string) (inputStr:string) =
    inputStr.Replace(oldValue, newValue)

// Inline version
let toRomanNumerals number =
    String.replicate number "I"
    |> replace "IIIII" "V"  // partial application
    |> replace "VV" "X"
    |> replace "XXXXX" "L"
    |> replace "LL" "C"
    |> replace "CCCCC" "D"
    |> replace "DD" "M"

// test it
toRomanNumerals 12
toRomanNumerals 14
toRomanNumerals 1947







(* ======================================
Open-closed principle
"open for extension, closed for modification"

You can add new code, but don't change existing code


Pipeline-oriented programming works really well
with this guideline

====================================== *)












// ======================================
// New feature: What about the special forms IV,IX,XC etc?
// ======================================

module Version2 =

    let toRomanNumerals number =
        String.replicate number "I"
        |> replace "IIIII" "V"
        |> replace "VV" "X"
        |> replace "XXXXX" "L"
        |> replace "LL" "C"
        |> replace "CCCCC" "D"
        |> replace "DD" "M"
        // special forms are done highest to lowest
        |> replace "DCCCC" "CM"
        |> replace "CCCC" "CD"
        |> replace "LXXXX" "XC"
        |> replace "XXXX" "XL"
        |> replace "VIIII" "IX"
        |> replace "IIII" "IV"

// test it
open Version2
toRomanNumerals 4
toRomanNumerals 14
toRomanNumerals 19
toRomanNumerals 1947
toRomanNumerals 1999

// test it on all the numbers up to 30
[1..30] |> List.map toRomanNumerals |> String.concat ","



















// ======================================
// New feature: logging
// ======================================

module Version3 =

    let toRomanNumerals number =

        // helper function
        let logger msg numeral =
            printfn "%s : %s" msg numeral
            numeral  // return the numeral back to the pipeline

        String.replicate number "I"
        |> logger "at beginning"
        |> replace "IIIII" "V"
        |> replace "VV" "X"
        |> replace "XXXXX" "L"
        |> replace "LL" "C"
        |> replace "CCCCC" "D"
        |> replace "DD" "M"
        |> logger "before special forms"
        // special forms are done highest to lowest
        |> replace "DCCCC" "CM"
        |> replace "CCCC" "CD"
        |> replace "LXXXX" "XC"
        |> replace "XXXX" "XL"
        |> replace "VIIII" "IX"
        |> replace "IIII" "IV"
        |> logger "final"




// test it
open Version3
toRomanNumerals 4
toRomanNumerals 14
toRomanNumerals 19
