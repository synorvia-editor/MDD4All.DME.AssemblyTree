using MDD4All.DME.AssemblyTree.ViewModels;
using Synorvia.UI.DataModels.Tree;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.AssemblyTree.Views
{
    public partial class AssemblyTreeView
    {
        [Parameter]
        public AssemblyTreeViewModel DataContext { get; set; }

        void OnSelectionChanged(ITreeNode node)
        {
            DataContext.SelectedNode = node;
        }
    }
}