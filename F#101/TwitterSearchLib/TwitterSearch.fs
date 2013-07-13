#light

open System
open System.Net
open System.IO
open System.Text.RegularExpressions
open System.Xml.Linq
open Microsoft.FSharp.Control

let ns = XNamespace.Get("http://www.w3.org/2005/Atom")
let elem s (e : XElement) = e.Element(ns + s)
let elemv s e = match (elem s e) with
                | null  -> ""
                | el    -> el.Value

let getTweets query i = async {
    let url = sprintf "http://search.twitter.com/search.atom?rpp=100&page=%i&q=%s" i query
    
    let req = WebRequest.Create(url)
    let! resp = req.AsyncGetResponse()
    
    let reader =  new StreamReader(resp.GetResponseStream())

    let! xreader = XElement.LoadAsync(reader)
    return xreader.Elements() |> Seq.map (elemv "title")
    }

type WordCount (word, count) =
    member wc.Word = word
    member wc.Count = count
with override wc.ToString() =
        sprintf "%s (%i)" word count

type TwitterCloud (tweets : string seq, minThresh, maxThresh) =
    static member private alpha = new Regex("[^A-Z^a-z]+", RegexOptions.Compiled)
    static member GetCloud(query, pages, minThresh, maxThresh) =
        [1..pages] |> List.map (getTweets query)
                   |> Async.Parallel
                   |> Async.Run
                   |> Seq.concat
                   |> (fun t -> new TwitterCloud(t, minThresh, maxThresh))
    member c.Cloud =
        let tweetCount = tweets |> Seq.length |> float
        let thresh (_, c) = let frac = (float c)/tweetCount
                            frac > minThresh && frac < maxThresh

        tweets |> Seq.map (fun t -> t.ToLower())
               |> Seq.map TwitterCloud.alpha.Split
               |> Seq.concat
               |> Seq.group_by (fun w -> w)
               |> Seq.map (fun (w,s) -> w,(s |> Seq.length))
               |> Seq.filter thresh
               |> Seq.sort_by (fun (w,s) -> -s)
               |> Seq.map (fun (w,s) -> new WordCount(w,s))
               |> Seq.to_array

