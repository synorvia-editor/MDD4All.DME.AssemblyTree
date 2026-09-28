Shows the types of a .NET assembly as a tree: the assembly, its namespaces, the types
in each. Built on the tree of Synorvia.UI.BlazorComponents, and picks a type the way a
file dialog picks a file.

The assembly comes through an IAssemblyProvider from MDD4All.AssemblyLoading.Contracts,
so it can live in a load context of its own. Types without a namespace and generated
resource classes are left out.
