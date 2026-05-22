using MDD4All.UI.DataModels.Tree;
using System.Collections.ObjectModel;

namespace MDD4All.DME.AssemblyTree.ViewModels
{
    internal class RootNodeViewModel : AssemblyNodeViewModel
    {

        public RootNodeViewModel(string assemblyName, 
                                 ITree tree,
                                 AssemblyNodeViewModel parentNode) : base(tree, parentNode)
        {
            _title = assemblyName;
        }

        public override ObservableCollection<ITreeNode> Children { get; set; } = new ObservableCollection<ITreeNode>();

        private string _title = string.Empty;

        public override string Title
        {
            get {  return _title; }
        }

        public override string Icon { get; set; }
    }
}
