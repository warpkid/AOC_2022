open System.IO

let lines = File.ReadAllLines("day6\data.txt")[0]

// part 1
// let windowSize = 4

// part 2
let windowSize = 14

let isUnique (c: char array) =
    c |> Seq.distinct |> Seq.length = windowSize


let result =
    lines
    |> seq
    |> Seq.windowed windowSize
    |> Seq.takeWhile (fun m -> isUnique m |> not)
    |> Seq.length
    |> (+) windowSize

printfn "%i" result
