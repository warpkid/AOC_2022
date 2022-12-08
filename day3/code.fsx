open System.IO

let lines = File.ReadAllLines("day3\data.txt")

let getPriority (c: char) : int =
    [ 'a' .. 'z' ] @ [ 'A' .. 'Z' ]
    |> List.findIndex (fun i -> i = c)
    |> (+) 1

let setToPriority s =
    s |> Set.map getPriority |> Set.toList |> List.sum


let part1 =
    lines
    |> Array.map (fun x ->
        x
        |> seq
        |> Seq.splitInto 2
        |> Array.ofSeq
        |> (fun a -> a[0] |> Set.ofSeq, a[1] |> Set.ofSeq)
        |> (fun (s1, s2) -> Set.intersect s1 s2)
        |> setToPriority)
    |> Array.sum


let part2 =
    lines
    |> Array.chunkBySize 3
    |> Array.map (fun x ->
        x
        |> Array.map Set.ofSeq
        |> Set.intersectMany
        |> setToPriority)
    |> Array.sum
