
// F# Tutorial File
//
// This file contains sample code to guide you through the
// primitives of the F# language.  
//
// Learn more about F# at http://fsharp.net
// 
// For a larger collection of F# samples, see:
//     http://go.microsoft.com/fwlink/?LinkID=124614
//
// Contents:
//   - Simple computations
//   - Functions on integers  
//   - Tuples 
//   - Booleans
//   - Strings
//   - Lists
//   - Arrays
//   - More Collections
//   - Functions
//   - Types: unions
//   - Types: records
//   - Types: classes
//   - Types: interfaces
//   - Types: classes with interface implementations
//   - Printing 

// open some standard namespaces
open System

// Simple computations
// ---------------------------------------------------------------
// Here are some simple computations.  Note how code can be documented
// with '///' comments.  Hover over any reference to a variable to 
// see its documen
/// A very simple constant integer
let int1 = 1

/// A second very simple constant integer
let int2 = 2

/// Add two integers
let int3 = int1 + int2


// Functions on integers  
// ---------------------------------------------------------------

/// A function on integers
let f x = 2*x*x - 5*x + 3
let g x y = x*x + y*y
let h = (fun x y z -> z - y - x)

/// The result of a simple computation 
let result = f (int3 + 4)

let resultg = g 7

let i = printfn "Next value: %i %s"


/// Another function on integers
let increment x = x + 1

/// Compute the factorial of an integer
let rec factorial n = if n=0 then 1 else n * factorial (n-1)

/// Compute the highest-common-factor of two integers
let rec hcf a b =                       // notice: 2 parameters separated by spaces
    if a=0 then b
    elif a<b then hcf a (b-a)           // notice: 2 arguments separated by spaces
    else hcf (a-b) b
    // note: function arguments are usually space separated
    // note: 'let rec' defines a recursive function

      
// Tuples
// ---------------------------------------------------------------

// A simple tuple of integers
let pointA = (1, 2, 3)

// A simple tuple of an integer, a string and a double-precision floating point number
let dataB = (1, "fred", 3.1415)

let anInt, aString, aFloat = dataB

/// A function that swaps the order of two values in a tuple
let Swap (a, b) = (b, a)

// Booleans
// ---------------------------------------------------------------

/// A simple boolean value
let boolean1 = true

/// A second simple boolean value
let boolean2 = false

/// Compute a new boolean using ands, ors, and nots
let boolean3 = not boolean1 && (boolean2 || false)

// Strings
// ---------------------------------------------------------------

/// A simple string
let stringA  = "Hello"

/// A second simple string
let stringB  = "world"

/// "Hello world" computed using string concatentation
let stringC  = stringA + " " + stringB

/// "Hello world" computed using a .NET library function
let stringD = String.Join(" ",[| stringA; stringB |])
  // Try re-typing the above line to see intellisense in action
  // Note, ctrl-J on (partial) identifiers re-activates it

// Functional Lists
// ---------------------------------------------------------------

/// The empty list
let listA = [ ]           

/// A list with 3 integers
let listB = [ 1; 2; 3 ]     

/// A list with 3 integers, note :: is the 'cons' operation
let listC = 1 :: [2; 3]    

/// Compute the sum of a list of integers using a recursive function
let rec SumList xs =
    match xs with
    | []    -> 0
    | y::ys -> y + SumList ys

/// Sum of a list
let listD = SumList [1; 2; 3]  

/// The list of integers between 1 and 10 inclusive 
let oneToTen = [1..10]

/// The squares of the first 10 integers
let squaresOfOneToTen = [ for x in 0..10 -> x*x ]


// Mutable Arrays
// ---------------------------------------------------------------

/// Create an array 
let arr = Array.create 4 "hello"
arr.[1] <- "world"
arr.[3] <- "don"

/// Compute the length of the array by using an instance method on the array object
let arrLength = arr.Length        

// Extract a sub-array using slicing notation
let front = arr.[0..2]


// More Collections
// ---------------------------------------------------------------

/// A dictionary with integer keys and string values
let lookupTable = dict [ (1, "One"); (2, "Two") ]

let oneString = lookupTable.[1]

// For some other common data structures, see:
//   System.Collections.Generic
//   Microsoft.FSharp.Collections
//   Microsoft.FSharp.Collections.Seq
//   Microsoft.FSharp.Collections.Set
//   Microsoft.FSharp.Collections.Map

// Functions
// ---------------------------------------------------------------

/// A function that squares its input
let Square x = x*x              

// Map a function across a list of values
let squares1 = List.map Square [1; 2; 3; 4]
let squares2 = List.map (fun x -> x*x) [1; 2; 3; 4]

// Pipelines
let squares3 = [1; 2; 3; 4] |> List.map (fun x -> x*x) 
let SumOfSquaresUpTo n = 
  [1..n] 
  |> List.map Square 
  |> List.sum

// Types: unions
// ---------------------------------------------------------------

type Expr = 
  | Num of int
  | Add of Expr * Expr
  | Mul of Expr * Expr
  | Var of string
  
let rec Evaluate (env:Map<string,int>) exp = 
    match exp with
    | Num n when n > 5 -> -n
    | Num n -> n
    | Add (x,y) -> Evaluate env x + Evaluate env y
    | Mul (x,y) -> Evaluate env x * Evaluate env y
    | Var id    -> env.[id]
  
let envA = Map.ofList [ "a",1 ;
                        "b",2 ;
                        "c",3 ]
             
let expT1 = Add(Var "a",Mul(Num 6,Var "b"))
let resT1 = Evaluate envA expT1


// Types: records
// ---------------------------------------------------------------

type Card = { Name  : string;
              Phone : string;
              Ok    : bool }
             member x.NameUpper() = 
                x.Name.ToUpper()
              
let cardA = { Name = "Alf" ; Phone = "(206) 555-0157" ; Ok = false }
let cardB = { cardA with Phone = "(206) 555-0112"; Ok = true }
let ShowCard (c:Card) = 
  c.NameUpper() + " Phone: " + c.Phone + (if not c.Ok then " (unchecked)" else "")

ShowCard cardB

// Types: classes
// ---------------------------------------------------------------

/// A 2-dimensional vector
type Vector2D(dx:float, dy:float) = 
    // The pre-computed length of the vector
    let length = sqrt(dx*dx + dy*dy)
    /// The displacement along the X-axis
    member v.DX = dx
    /// The displacement along the Y-axis
    member v.DY = dy
    /// The length of the vector
    member v.Length = length
    // Re-scale the vector by a constant
    member v.Scale(k) = Vector2D(k*dx, k*dy)
    

// Types: interfaces
// ---------------------------------------------------------------

type IPeekPoke = 
    abstract Peek: unit -> int
    abstract Poke: int -> unit

              
// Types: classes with interface implementations
// ---------------------------------------------------------------

/// A widget which counts the number of times it is poked
type Widget(initialState:int) = 
    /// The internal state of the Widget
    let mutable state = initialState

    // Implement the IPeekPoke interface
    interface IPeekPoke with 
        member x.Poke(n) = state <- state + n
        member x.Peek() = state 
        
    /// Has the Widget been poked?
    member x.HasBeenPoked = (state <> 0)


let widget = Widget(12) :> IPeekPoke

widget.Poke(4)
let peekResult = widget.Peek()

              
// Printing
// ---------------------------------------------------------------

// Print an integer
printfn "peekResult = %d" peekResult 

// Print a result using %A for generic printing
printfn "listC = %A" listC
