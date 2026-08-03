using MDD4All.AssemblyLoading.Contracts;
using MDD4All.UI.DataModels.Tree;
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

                NamespaceNodeViewModel namespaceNode = (NamespaceNodeViewModel)CreateOrGetNamespaceNode(type);

                AssemblyElementNodeViewModel assemblyElementNode = new AssemblyElementNodeViewModel(type, 
                                                                                                    this, 
                                                                                                    assemblyFileInfo.FullName, 
                                                                                                    namespaceNode);

                namespaceNode.Children.Add(assemblyElementNode);
            }
        }

        private ITreeNode CreateOrGetNamespaceNode(Type type)
        {
            ITreeNode result;

            string? nameSpace = type.Namespace;

            if (nameSpace != null)
            {
                string[] namespaceParts = nameSpace.Split(".");

                AssemblyNodeViewModel currentNode = (AssemblyNodeViewModel)_treeRootNodes[0];

                bool nodeFound = false;

                foreach (string part in namespaceParts)
                {
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
