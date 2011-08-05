#light

#r "System.Core.dll"
#r "System.Xml.dll"
#r "System.Xml.Linq.dll"
#r "FSharp.PowerPack.dll"

#load "AsyncOperations.fs"
open Microsoft.FSharp.Control

open System.Windows.Forms

let grid s =
    let form = new Form(Visible = true, TopMost = true)
    let grid = new DataGridView(Dock = DockStyle.Fill, Visible = true)
    form.Controls.Add(grid)
    grid.DataSource <- s |> Seq.to_array

