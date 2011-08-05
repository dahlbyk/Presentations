#light

namespace System.Xml.Linq

open System.IO
open Microsoft.FSharp.Control

    [<AutoOpen>]
    module XElement =

        let UnblockViaNewThread f =
            async { //let ctxt = System.Threading.SynchronizationContext.Current
                    do! Async.SwitchToNewThread ()
                    let res = f()
                    do! Async.SwitchToThreadPool ()
                    //do! Async.SwitchTo ctxt
                    return res }

        let AsyncLoad(reader : TextReader) = UnblockViaNewThread (fun () -> XElement.Load(reader))
        let LoadAsync(reader : TextReader) = AsyncLoad(reader)
