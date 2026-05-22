using MDD4All.UI.DataModels.Tree;
using System.Collections.ObjectModel;

namespace MDD4All.DME.AssemblyTree.ViewModels
{
    internal class NamespaceNodeViewModel : AssemblyNodeViewModel
    {

        private string _namespacePart = "";

        public NamespaceNodeViewModel(string namespacePart,
                                      ITree tree,
                                      AssemblyNodeViewModel parentNode) : base(tree, parentNode)
        {
            _namespacePart = namespacePart;
        }

        public override ObservableCollection<ITreeNode> Children { get; set; } = new ObservableCollection<ITreeNode>();

        public override string Title => _namespacePart;

        public override string Icon { get; set; }

        public string NamespacePart
        {
            get => _namespacePart;
        }
    }
}
