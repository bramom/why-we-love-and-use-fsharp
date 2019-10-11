// COMPOSITION SAMPLE

open System.IO
// Gets the information about a file
let fileInfo filename = new FileInfo(filename)

// Gets the file size from a FileInfo object
let fileSize (fileinfo : FileInfo) = fileinfo.Length

// Converts a byte count to MB
let bytesToMB (bytes : int64) = bytes / (1024L * 1024L)

// Gets all files under a given folder
// The following example demonstrates the use of yield! to combine individual sequences into a single final sequence.
let rec filesUnderFolder rootFolder =
    seq {
        for file in Directory.GetFiles(rootFolder) do
            yield file
        for dir in Directory.GetDirectories(rootFolder) do
            yield! filesUnderFolder dir
    }

// Doing things the imperative way
let getFolderSizeImperative folder =
    let filesInFolder = filesUnderFolder folder
    let fileInfos     = Seq.map fileInfo filesInFolder
    let fileSizes     = Seq.map fileSize fileInfos
    let totalSize     = Seq.sum fileSizes
    let fileSizeInMB  = bytesToMB totalSize
    fileSizeInMB

// We can improve upon this by using the pipe-forward operator (|>) which is defined as:
// let inline (|>) x f = f x
let getFolderSizePipeline folder =
    folder
    |> filesUnderFolder
    |> Seq.map fileInfo
    |> Seq.map fileSize
    |> Seq.sum
    |> bytesToMB

// Using the Function-Composition operator (>>)
// let inline (>>) f g x = g(f x)
let getFolderSize =
    filesUnderFolder
    >> Seq.map (fileInfo >> fileSize)
    >> Seq.sum
    >> bytesToMB

getFolderSize "."
|> printfn "%A"
