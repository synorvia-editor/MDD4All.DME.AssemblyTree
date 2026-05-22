using MDD4All.UI.DataModels.Tree;
using System.Collections.ObjectModel;

namespace MDD4All.DME.AssemblyTree.ViewModels
{
    public class AssemblyElementNodeViewModel : AssemblyNodeViewModel
    {
        private Type _type;
        
        public AssemblyElementNodeViewModel(Type type,
                                            ITree tree,
                                            string path,
                                            AssemblyNodeViewModel parentNode) : base(tree, parentNode)
        {
            _type = type;
            _path = path;
        }

        public override ObservableCollection<ITreeNode> Children { get; set; } = new ObservableCollection<ITreeNode>();

        public override string Title
        {
            get
            {
                return _type.Name;
            }
        }

        private string _path;

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public string TypeNameWithNamespace
        {
            get
            {
                return _type.Namespace + "." + _type.Name;
            }
        }


        public override string Icon { get; set; }
    }
}
