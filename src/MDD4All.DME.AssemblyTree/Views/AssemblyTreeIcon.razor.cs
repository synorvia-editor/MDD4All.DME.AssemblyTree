using MDD4All.DME.AssemblyTree.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MDD4All.DME.AssemblyTree.Views
{
    public partial class AssemblyTreeIcon
    {
        [Parameter]
        public AssemblyNodeViewModel DataContext { get; set; }

        public string UnicodeIcon
        {
            get
            {
                string result = "";

                if(DataContext is RootNodeViewModel)
                {
                    result = "🗎";
                }
                else if(DataContext is NamespaceNodeViewModel)
                {
                    result = "📁";
                }
                else if(DataContext is AssemblyElementNodeViewModel)
                {
                    result = "▤";
                }

                return result;
            }
        }

        public string CssStyle
        {
            get
            {
                string result = "";

                if (DataContext is RootNodeViewModel)
                {
                    result = "color: violet;";
                }
                else if (DataContext is NamespaceNodeViewModel)
                {
                    result = "color: darkred;";
                }
                else if (DataContext is AssemblyElementNodeViewModel)
                {
                    result = "color: forestgreen; font-weight: bold;";
                }

                return result;
            }
        }
    }
}