open System.IO

let lines = File.ReadAllLines("day1\data.txt")

type WorkingSet = { Elfs: int list; Acc: int list }

let result =
    ({ Elfs = []; Acc = [] }, lines)
    ||> Array.fold (fun a n ->
        n
        |> function
            | "" ->
                { Elfs = a.Elfs @ [ (a.Acc |> List.sum) ]
                  Acc = [] }
            | _ -> { a with Acc = int n :: a.Acc })
    |> (fun s -> s.Elfs)

let max = List.max result

let top3 =
    result
    |> List.sortDescending
    |> List.take 3
    |> List.sum
