using Clf.Blazor.Basic.Components.Controls.Models;
using Clf.Common.Drawing;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clf.Blazor.Basic.Components.Controls
{
    public class WidgetBase : ComponentBase
    {
        [Parameter]
        public string Class { get; set; } = "";

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public int Height { get; set; }

        [Obsolete]
        [Parameter]
        public string FontStyle { get; set; } = Models.FontStyle.DEFAULT_FONT_STYLE;

        [Parameter]
        public string TooltipText { get; set; } = string.Empty;
        [Parameter]
        public bool ShowTooltip { get; set; } = false;
    }
}
