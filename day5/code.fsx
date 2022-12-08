open System.IO
open System.Text.RegularExpressions

let lines = File.ReadAllLines("day5\data.txt")

type Stack = { Number: int; Crates: char list }

type MoveOp = { Amount: int; From: int; To: int }

let getStack position =
    lines
    |> Array.take 8
    |> Array.map (fun l -> char (l.Substring((position * 4) + 1, 1)))
    |> Array.filter (fun c -> c <> ' ')
    |> (fun x ->
        { Number = position + 1
          Crates = x |> List.ofArray |> List.rev })


let crates = [ 0..8 ] |> List.map (fun s -> getStack s)

let parseMove (s: string) =
    let rx = Regex(@"move (?<count>[0-9]*) from (?<source>[0-9]*) to (?<target>[0-9]*)")
    let res = rx.Match(s)

    let a =
        res.Groups
        |> Seq.find (fun g -> g.Name = "count")
        |> (fun x -> int x.Value)

    let f =
        res.Groups
        |> Seq.find (fun g -> g.Name = "source")
        |> (fun x -> int x.Value)

    let t =
        res.Groups
        |> Seq.find (fun g -> g.Name = "target")
        |> (fun x -> int x.Value)

    { Amount = a; From = f; To = t }


let moves = lines |> Array.skip 10 |> Array.map parseMove


let updatedCrates =
    (crates, moves)
    ||> Array.fold (fun state move ->
        let target = state[move.To - 1]
        let source = state[move.From - 1]
        let size = (List.length source.Crates) - move.Amount
        // part 1
        //let pile = source.Crates[size..] |> List.rev

        // part 2
        let pile = source.Crates[size..]
        let remainder = source.Crates[0 .. (size - 1)]
        printfn "%A" source

        state
        |> List.map (fun s ->
            if s.Number = target.Number then
                { s with Crates = s.Crates @ pile }
            else if s.Number = source.Number then
                { s with Crates = remainder }
            else
                s

        ))

updatedCrates
|> List.filter (fun x -> (List.isEmpty x.Crates) |> not)
|> List.map (fun s -> s.Crates |> List.rev |> List.head)
|> List.map string
|> String.concat ""
