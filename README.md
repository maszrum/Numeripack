Numeripack
=========

Numeripack is a library containing several mathematical algorithms.

## Table of contents

- [CollectionPermutator](#collectionpermutator)
- [ExtremumIterator](#extremumiterator)
- [MinimalHamiltonianPathSeeker](#minimalhamiltonianpathseeker)
- [PermutationTree](#permutationtree)
- [GenericPermutator](#genericpermutator)
- [PositionalIncrementer](#positionalincrementer)
- [EnumStateMachine](#enumstatemachine)
  
## CollectionPermutator

From the set of input elements creates every possible combination of sets. The number of output sets can be any.
For example:

	Input elements type: int
	Input elements set: [1, 2, 3]
	Output sets identifier type: string
	Output sets identifiers: ["first", "second"]

It generates:

```
	1st iteration:
	"first": [1, 2, 3]
	"second": []
 
	2nd iteration:
	"first": [2,3]
	"second": [1]
 
	3rd iteration:
	"first": [1,3]
	"second": [2]

	4th iteration:
	"first": [3]
	"second": [1,2]
 
	5th iteration:
	"first": [1,2]
	"second": [3]
 
	6th iteration:
	"first": [2]
	"second": [1,3]
 
	7th iteration:
	"first": [1]
	"second": [2,3]

	8th iteration:
	"first": []
	"second": [1,2,3]
```

### Usage

Code below writes to console something similiar to example in previous section.

```csharp
var outputIdentifiers = new[] { "first", "second" };
var inputElements = new[] { 1, 2, 3 };
 
var permutator = new CollectionPermutator<int, string>(inputElements, outputIdentifiers);

while (permutator.Permute())
{
	var outputSets = permutator.Current;
 
	foreach (var outputSet in outputSets)
	{
		var joinedElements = string.Join(",", outputSet);
		Console.WriteLine($"{outputSet.Identifier}: [{joinedElements}]");
	}
}
```

## ExtremumIterator

Tries to find minimum, maximum or value closest to zero of specified function. The value is searched step by step with step function definied by user.

### Usage

Let's find minimum of quadratic function:

```csharp
internal class QuadraticFunction
{
    private readonly double _a;
    private readonly double _b;
    private readonly double _c;
 
    public double Argument { get; set; } = 0.0;
 
    public QuadraticFunction(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }
 
    public double GetValue()
    {
        return (_a * Argument * Argument) + (_b * Argument) + _c;
    }
 
    public double GetDerivative()
    {
        return (2 * _a * Argument) + _b;
    }
}
```
With argument property named **Argument** and value of that argument: ``GetValue()``.
We will find maximum of this function for ``[a, b, c] = [-4.2, 4.6, -12.3]``

```csharp
var qFunction = new QuadraticFunction(-4.2, 4.6, -12.3)
{
	Argument = -23.3 //x0 for our search
};

var iterator = new ExtremumIteratorBuilder()
	.Find(Extremum.Maximum) // may be Minimum or Zero
	.ForObject(qFunction)
	.SelectArgument(qf => qf.Argument)
	.WithStep(ctx =>
	{
		var qf = ctx.Object;
		var derivative = qe.GetDerivative();
		qe.Argument += derivative * 0.01;
		return qe.GetValue();
	})
	.WithMaximumSteps(1000)
	.Build();

var result = iterator.FindResult();
```

Result will be ``f(x) = -11.0404...`` with argument ``x = 0.5476...`` 
Or intead of calling ``FindResult()`` we can iterate step by step to final value:
```csharp
while (iterator.Next())
{
	var current = iterator.Current;
	Console.WriteLine($"{iterator.StepNumber}.  f({current.Argument}) = {current.Value}");
}
```
It will write to the console:

```
1.  f(-21,2968) = -2015,1907790080002
2.  f(-19,4618688) = -1692,6348126713365
3.  f(-17,7810718208) = -1421,992293780761
4.  f(-16,241461787852803) = -1194,9080644505104
5.  f(-14,831178997673167) = -1004,3716793255874
6.  f(-13,53935996186862) = -844,5009821682099
7.  f(-12,356053725071655) = -710,3605144941296
8.  f(-11,272145212165636) = -597,8091502493863
9.  f(-10,279285014343722) = -503,37225277164913
10.  f(-9,36982507313885) = -424,13440732156886
11.  f(-8,536759766995187) = -357,64941766960635
...
996.  f(0,5476190332922326) = -11,040476190476191
997.  f(0,5476190332922326) = -11,040476190476191
998.  f(0,5476190332922326) = -11,040476190476191
999.  f(0,5476190332922326) = -11,040476190476191
```

Additionally, you can use functions:
- to limit argument to specified domain:
``.WithArgumentDomain(arg => arg > -100 && arg < 100)``

- to specify when iterator should stop working:
``.EndsIf(ctx => ctx.CurrentExtremum.Value > 10000)``

## MinimalHamiltonianPathSeeker

Finds hamiltonian graph path that has minimum value. Hamiltonian graph is an undirected or directed graph that visits each vertex exactly once.

### Usage

Let's find minimal hamiltonian path for cost values:

|   | A | B | C | D | E | F | G |
|---|---|---|---|---|---|---|---|
| A | - | x | x | x | x | x | x |
| B | 2 | - | x | x | x | x | x |
| C | 6 | 3 | - | x | x | x | x |
| D | 4 | 7 | 3 | - | x | x | x |
| E | 5 | 1 | 6 | 8 | - | x | x |
| F | 8 | 3 | 1 | 7 | 4 | - | x |
| G | 9 | 3 | 5 | 6 | 1 | 4 | - |

It means that going from point A to B costs 2, B to C costs 3, ...

Our graph point is pair of its name and index.

```csharp
public class PathPoint
{
	public PathPoint(string name, int index)
	{
		Name = name;
		Index = index;
	}
 
	public string Name { get; set; }
	public int Index { get; set; }
}
```

Filling ``elements`` array with ``PathPoint`` objects and ``costValues`` with values from table.

```csharp
var elements = new[]
{
	new PathPoint("A", 0),
	new PathPoint("B", 1),
	new PathPoint("C", 2),
	new PathPoint("D", 3),
	new PathPoint("E", 4),
	new PathPoint("F", 5),
	new PathPoint("G", 6),
};
 
var costValues = new[,]
{
	{ 0, 2, 6, 4, 5, 8, 9 },
	{ 2, 0, 3, 7, 1, 3, 3 },
	{ 6, 3, 0, 3, 6, 1, 5 },
	{ 4, 7, 3, 0, 8, 7, 6 },
	{ 5, 1, 6, 8, 0, 4, 1 },
	{ 8, 3, 1, 7, 4, 0, 4 },
	{ 9, 3, 5, 6, 1, 4, 0 }
};
```

Finding minimum:

```csharp
var seeker = new MinimalHamiltonianCycleSeekerBuilder<PathPoint>()
	.ForElements(elements)
	.WithCostFunction((first, second, order) =>
	{
		return costValues[first.Index, second.Index];
	})
	.Build();
 
var result = seeker.FindResult();
```

Result will be: ``A -> B -> C -> F -> E -> G -> D`` with cost **17**
```
cost(A -> B) + cost (B -> C) + cost(C -> F) + cost(F -> E) + cost(E -> G) + cost(G -> D) = 
= 2 + 3 + 1 + 4 + 1 + 6 = 17
```

### Additional info

- Directed graph can be used. You should change the cost values over diagonal in the table. The effect will be that for example ``cost(B -> C) = 3 != 4 = cost(C -> B)``

- The cost function may depend on the order of the transition. For example, ``cost (A-> B)`` may be different if it is the first and fourth transition. Use the third parameter in the lamda function (``order``):
```csharp
.WithCostFunction((first, second, order)  => 
{
	...
})
```
## PermutationTree

Allows you to traverse any permutation of the Hamilton graph composed of elements given by the user. 
If there are three vertex defined in graph: ``A, B, C`` a result of traversal will be:

```
A
A -> B
A -> B -> C
A -> C
A -> C -> B
B
B -> C
B -> C -> A
B -> A
B -> A -> C
C
C -> A
C -> A -> B
C -> B
C -> B -> A
```

Algorithm allows to skip any part of the traversal. For example if you need to skip ``A`` and ``C -> A`` a result of the traversal will be:

```
B
B -> C
B -> C -> A
B -> A
B -> A -> C
C
C -> B
C -> B -> A
```

### Usage

```csharp
var elements = new[] { "A", "B", "C" };
var permutationTree = new TreePermutator<string>(elements);

while (permutationTree.MoveNext())
{
	Console.WriteLine(string.Join(" -> ", permutationTree.Current.Elements));
}
```

Full graph traversal will be writed to console. After traversal method ``GetResult()`` may be called to get every path that was found:

```csharp
var result = permutationTree.GetResult();
foreach (var path in result)
{
	Console.WriteLine(string.Join(" -> ", path));
}
```

It will write to the console:

```
A -> B -> C
A -> C -> B
B -> C -> A
B -> A -> C
C -> A -> B
C -> B -> A
```

### Skipping branches

Let's say we want to skip ``A`` and ``C -> A`` branches as in the first paragraph. The code will be:

```csharp
while (permutationTree.MoveNext())
{
	var currentElements = permutationTree.Current.Elements;
 
	if (currentElements.Count == 1 && currentElements[0] == "A")
	{
		permutationTree.Current.SkipBranch();
	}
	if (currentElements.Count == 2 && currentElements[0] == "C" && currentElements[1] == "A")
	{
		permutationTree.Current.SkipBranch();
	}
	else
	{
		Console.WriteLine(string.Join(" -> ", permutationTree.Current.Elements));
	}
}
```

Output will be:

```
A
B
B -> C
B -> C -> A
B -> A
B -> A -> C
C
C -> B
C -> B -> A
```

Print result to console:

```csharp
var result = _permutationTree.GetResult();
foreach (var path in result)
{
	Console.WriteLine(string.Join(" -> ", path));
}
```

```
B -> C -> A
B -> A -> C
C -> B -> A
```

## GenericPermutator

Generates all permutations of objects of a given type. Heap's algorithm is used.

### Usage

Let's generate permutation of three objects of type ``string``: ``["one", "two", "three"]``.

```csharp
var elements = new[] { "one", "two", "three" };
var permutator = new GenericPermutator<string>(elements);
 
foreach (var permutation in permutator)
{
	Console.WriteLine(string.Join(", ", permutation));
}
```

Console will print:

```
one, two, three
two, one, three
three, one, two
one, three, two
two, three, one
three, two, one
```

## PositionalIncrementer

Counter with base and number of items given by the user.

### Non-generic

Code for positional incrementator with: ``base = 3``, ``positions = 4``

```csharp
const int baseNumber = 3;
const int positions = 4;
var incrementer = new PositionalIncrementer(baseNumber, positions);
 
foreach (var value in incrementer)
{
	Console.WriteLine(string.Join(" ", value));
}
```

Output:

```
0 0 0 0
1 0 0 0
2 0 0 0
0 1 0 0
1 1 0 0
2 1 0 0
0 2 0 0
1 2 0 0
2 2 0 0
0 0 1 0
1 0 1 0
2 0 1 0
...
1 1 2 2
2 1 2 2
0 2 2 2
1 2 2 2
2 2 2 2
```

### Generic

Similarly for a generic incrementer with elements: ``["one", "two", "three", "four"]``

```csharp
var elements = new[] { "one", "two", "three", "four" };
const int positions = 4;
 
var incrementer = new PositionalIncrementer<string>(elements, positions);
 
foreach (var value in incrementer)
{
	Console.WriteLine(string.Join(" ", value));
}
```

Output:

```
one one one one
two one one one
three one one one
four one one one
one two one one
two two one one
...
four three four four
one four four four
two four four four
three four four four
four four four four
```

## EnumStateMachine

It allows you to build a state machine with specific transitions. Uses enum type to define a state. Transition throws exception ``InvalidOperationException`` if operation is not allowed.

### Usage

Let's build state machine for some process:

![Process State](/state-machine.png)

Define enum type (values must be powers of 2):
```csharp
enum ProcessState
{
    Standby = 1,
    Active = 2,
    Suspect = 4,
    Failed = 8
}
```

Build state machine, define allowed transitions and assign starting state:

```csharp
var builder = new EnumStateMachineBuilder<ProcessState>();
 
builder.For(ProcessState.Active)
	.AllowTransitionTo(ProcessState.Standby)
	.AllowTransitionTo(ProcessState.Suspect);
builder.For(ProcessState.Standby)
	.AllowTransitionTo(ProcessState.Active)
	.AllowTransitionTo(ProcessState.Suspect);
builder.For(ProcessState.Suspect)
	.AllowTransitionTo(ProcessState.Standby)
	.AllowTransitionTo(ProcessState.Failed);
builder.For(ProcessState.Failed)
	.AllowTransitionTo(ProcessState.Suspect);
 
var processState = builder.Build(ProcessState.Standby);
```

Object ``processState`` provides methods and properties:
 ```csharp
ProcessState CurrentState { get; }
IReadOnlyList<ProcessState> GetAllowedTransitions();
void GotoState(ProcessState state);
bool IsTransitionAllowed(ProcessState state);
```

You can track changes of state with ``StateChanged`` event.