open System.IO

type Hand =
    | Rock
    | Paper
    | Sciccors

type GameResult =
    | Win
    | Lose
    | Draw

let toGameResult (s: string) : GameResult =
    match s with
    | "X" -> Lose
    | "Y" -> Draw
    | "Z" -> Win

type Record = { TheirHand: Hand; MyHand: Hand }

type Round =
    { Hands: Record
      Result: GameResult
      Score: int }

let toHand h =
    match h with
    | "A"
    | "X" -> Rock
    | "B"
    | "Y" -> Paper
    | "C"
    | "Z" -> Sciccors
    | _ -> failwith "Unknown play"

let toHandWithResponse h r =
    let hand = toHand h
    let desire = toGameResult r

    match (hand, desire) with
    | Rock, Win -> Paper
    | Rock, Lose -> Sciccors
    | Rock, Draw -> Rock
    | Paper, Win -> Sciccors
    | Paper, Lose -> Rock
    | Paper, Draw -> Paper
    | Sciccors, Win -> Rock
    | Sciccors, Lose -> Paper
    | Sciccors, Draw -> Sciccors

let scoreMap = [ Rock, 1; Paper, 2; Sciccors, 3 ] |> Map.ofList


let getResult (t: Hand) (m: Hand) : GameResult =
    match (t, m) with
    | (Rock, Rock) -> Draw
    | (Rock, Paper) -> Win
    | (Rock, Sciccors) -> Lose
    | (Paper, Rock) -> Lose
    | (Paper, Paper) -> Draw
    | (Paper, Sciccors) -> Win
    | (Sciccors, Rock) -> Win
    | (Sciccors, Paper) -> Lose
    | (Sciccors, Sciccors) -> Draw


let getScore (m: Hand) (result: GameResult) : int =
    let score = scoreMap |> Map.find m

    match result with
    | Win -> score + 6
    | Lose -> score
    | Draw -> score + 3


let calculateGame (game: Record) : Round =
    let result = getResult game.TheirHand game.MyHand
    let score = getScore game.MyHand result
    printfn "Result is %A and score is %i" result score

    { Hands = game
      Result = result
      Score = score }

let lines = File.ReadAllLines("day2\data.txt")

// Part 1
let records =
    lines
    |> Array.map (fun l -> l.Split(' '))
    |> Array.map (fun l ->
        { TheirHand = (toHand l[0])
          MyHand = (toHand l[1]) })

(0, records)
||> Array.fold (fun state g -> state + (calculateGame g).Score)

// Part 2
let part2Records =
    lines
    |> Array.map (fun l -> l.Split(' '))
    |> Array.map (fun l ->
        { TheirHand = (toHand l[0])
          MyHand = toHandWithResponse l[0] l[1] })

(0, part2Records)
||> Array.fold (fun state g -> state + (calculateGame g).Score)
