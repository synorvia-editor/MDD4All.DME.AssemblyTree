using MDD4All.UI.DataModels.Tree;
using System.Collections.ObjectModel;

namespace MDD4All.DME.AssemblyTree.ViewModels
{
    public abstract class AssemblyNodeViewModel : ITreeNode
    {
        public AssemblyNodeViewModel(ITree tree, AssemblyNodeViewModel parentNode) 
        {
            Tree = tree;
            Parent = parentNode;
        }

        public ITree Tree { get; set; }

        public abstract ObservableCollection<ITreeNode> Children { get; set; }

        public ITreeNode Parent { get; set; }

        public int Index { get; set; }

        public bool HasChildNodes
        {
            get
            {
                return Children.Count > 0;
            }
        }

        public bool IsExpanded { get; set; }

        public bool IsSelected
        {
            get
            {
                bool result = false;
                if (Tree != null)
                {
                    if (Tree.SelectedNode == this)
                    {
                        result = true;
                    }
                }
                return result;
            }
            set { }
        }

        public bool IsLoading
        {
            get { return false; }
        }

        public bool IsDisabled { get; set; }

        public string DragDropOperationInformation { get; set; } = string.Empty;

        public event EventHandler TreeStateChanged;

        public abstract string Title { get; }

        public abstract string Icon { get; set; }
    }
}
