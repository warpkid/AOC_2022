open System.IO

let lines = File.ReadAllLines("day4\data.txt")

let pairToRange (i: string) =
    i.Split('-')
    |> Array.map int
    |> (fun x -> [| x[0] .. x[1] |])
    |> Set.ofArray

let fullyCovered s1 s2 =
    s1 |> Set.isSubset s2 || s2 |> Set.isSubset s1

let partlyCovered s1 s2 =
    s1 |> Set.intersect s2 |> (not << Set.isEmpty)
    || s2 |> Set.intersect s1 |> (not << Set.isEmpty)

lines
|> Array.map (fun s -> s.Split(','))
|> Array.map (fun s -> pairToRange s[0], pairToRange s[1])
|> Array.map (fun (s1, s2) -> fullyCovered s1 s2)
|> Array.filter id
|> Array.length


lines
|> Array.map (fun s -> s.Split(','))
|> Array.map (fun s -> pairToRange s[0], pairToRange s[1])
|> Array.map (fun (s1, s2) -> partlyCovered s1 s2)
|> Array.filter id
|> Array.length
