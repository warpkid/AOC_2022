open System.IO

let lines = File.ReadAllLines("day6\data.txt")[0]

let windowSizes = [ 4; 14 ]

windowSizes
|> List.iter (fun windowSize ->
    let isUnique (c: char array) =
        c |> Seq.distinct |> Seq.length = windowSize


    let result =
        lines
        |> seq
        |> Seq.windowed windowSize
        |> Seq.takeWhile (fun m -> isUnique m |> not)
        |> Seq.length
        |> (+) windowSize

    printfn "%i" result)
