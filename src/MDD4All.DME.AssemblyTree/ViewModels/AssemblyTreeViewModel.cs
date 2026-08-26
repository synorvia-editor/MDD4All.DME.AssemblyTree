using MDD4All.AssemblyLoading.Contracts;
using MDD4All.UI.DataModels.Tree;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MDD4All.DME.AssemblyTree.ViewModels
{
    public class AssemblyTreeViewModel : ITree
    {
        public AssemblyTreeViewModel(Assembly assembly)
        {
            Initialize(assembly);
        }

        public AssemblyTreeViewModel(string fileName, IAssemblyProvider assemblyProvider)
        {
            Assembly assembly = assemblyProvider.GetAssemblyByPath(fileName);

            Initialize(assembly);
        }

        private void Initialize(Assembly assembly)
        {
            FileInfo assemblyFileInfo = new FileInfo(assembly.Location);

            RootNodeViewModel rootNodeViewModel = new RootNodeViewModel(assemblyFileInfo.Name, this, null);
            _treeRootNodes.Add(rootNodeViewModel);

            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch(ReflectionTypeLoadException reflectionTypeLoadException)
            {
                types = reflectionTypeLoadException.Types.Where(type => type != null).ToArray();
            }

            foreach (Type type in types)
            {
                if (type.Namespace == null)
                {
                    // Compiler-generated types (e.g. <PrivateImplementationDetails>, emitted
                    // for array/collection literals) have no namespace and are never valid
                    // selectable data models - skip them instead of crashing below.
                    continue;
                }

                if (IsGeneratedResourceClass(type))
                {
                    // Every resx compiles into a class of static properties. It is public
                    // because [Display] annotations point at it, but its constructor is
                    // internal - picking it as a data model can only fail.
                    continue;
                }

                NamespaceNodeViewModel namespaceNode = (NamespaceNodeViewModel)CreateOrGetNamespaceNode(type);

                AssemblyElementNodeViewModel assemblyElementNode = new AssemblyElementNodeViewModel(type, 
                                                                                                    this, 
                                                                                                    assemblyFileInfo.FullName, 
                                                                                                    namespaceNode);

                namespaceNode.Children.Add(assemblyElementNode);
            }
        }

        // Narrow on purpose: the builder writes its own name into the attribute, so this asks
        // for exactly one kind of generated type. Skipping everything marked as generated would
        // one day throw away a source-generated model that is perfectly editable.
        private bool IsGeneratedResourceClass(Type type)
        {
            bool result = false;

            object[] attributes = type.GetCustomAttributes(typeof(GeneratedCodeAttribute), false);

            foreach (object attribute in attributes)
            {
                if (attribute is GeneratedCodeAttribute generatedCode &&
                    generatedCode.Tool == "System.Resources.Tools.StronglyTypedResourceBuilder")
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private ITreeNode CreateOrGetNamespaceNode(Type type)
        {
            ITreeNode result;

            string? nameSpace = type.Namespace;

            if (nameSpace != null)
            {
                string[] namespaceParts = nameSpace.Split(".");

                AssemblyNodeViewModel currentNode = (AssemblyNodeViewModel)_treeRootNodes[0];

                foreach (string part in namespaceParts)
                {
                    // Per level. Kept across levels, the first match would make every deeper
                    // level count as found, and those namespaces would never be built - their
                    // types ended up one folder too high.
                    bool nodeFound = false;

                    // search for existing node
                    foreach (ITreeNode treeNode in currentNode.Children)
                    {
                        if (treeNode is NamespaceNodeViewModel)
                        {
                            NamespaceNodeViewModel namespaceNode = (NamespaceNodeViewModel)treeNode;

                            if (namespaceNode.NamespacePart == part)
                            {
                                result = namespaceNode;
                                nodeFound = true;
                                currentNode = namespaceNode;
                                break;
                            }
                        }
                    }

                    // if not found, create an new node
                    if (!nodeFound)
                    {
                        NamespaceNodeViewModel newRootNode = new NamespaceNodeViewModel(part, this, currentNode);
                        currentNode.Children.Add(newRootNode);
                        currentNode = newRootNode;
                    }
                }

                result = currentNode;
            }
            else
            {
                result = _treeRootNodes[0];
            }

            return result;
        }


        private ObservableCollection<ITreeNode> _treeRootNodes = new ObservableCollection<ITreeNode>();

        public ObservableCollection<ITreeNode> TreeRootNodes
        {
            get { return _treeRootNodes; }
        }

        public ITreeNode SelectedNode { get; set; }
    }
}
