open System.IO
open System.Collections.Generic

let lines = File.ReadAllLines("day7\data.txt")

type Input =
    | Cd of dir: string
    | Ls
    | Dir of dir: string
    | FileInfo of filename: string * size: int

type File = { Filename: string; Size: int }

type Directory =
    { Name: string
      Files: File list
      Size: int }

type State =
    { Directories: Directory list
      History: Directory list }

let (|Prefix|_|) (p: string) (s: string) =
    if s.StartsWith(p) then
        Some(s.Substring(p.Length))
    else
        None

let parseLine (s: string) =
    match s with
    | Prefix "$ cd " rest -> Cd rest
    | Prefix "$ ls" _ -> Ls
    | Prefix "dir " rest -> Dir rest
    | _ ->
        (fun _ ->
            let split = s.Split(" ")
            FileInfo(split[1], int split[0]))
            ()

let commands = lines |> Array.map parseLine

let resultStacks =
    ({ Directories = [ { Name = "/"; Size = 0; Files = [] } ]
       History = [] },
     commands)
    ||> Array.fold (fun s c ->
        match c with
        | Ls -> s
        | Cd (dir) ->
            match dir with
            | ".." ->
                { s with
                    Directories = s.Directories[0 .. (s.Directories |> List.length) - 1]
                    History = s.History @ [ List.last s.Directories ] }
            | "/" ->
                { s with
                    Directories = [ List.head s.Directories ]
                    History = s.History @ List.tail s.Directories }
            | _ ->
                { s with
                    Directories =
                        s.Directories
                        @ [ { Name = dir; Files = []; Size = 0 } ] }
        | Dir (dir) -> s
        | FileInfo (filename, size) ->
            (fun _ ->
                let current = s.Directories |> List.last

                let updated =
                    { current with
                        Files =
                            current.Files
                            @ [ { Filename = filename; Size = size } ]
                        Size = current.Size + size }

                { s with
                    Directories =
                        s.Directories[1 .. (List.length s.Directories - 1)]
                        @ [ updated ] })
                ())


let finalDirectoryList s = s.Directories @ s.History

let atMostSize = 100000

let result = finalDirectoryList resultStacks

result
|> List.filter (fun d -> d.Size <= atMostSize)
|> List.sumBy (fun d -> d.Size)
