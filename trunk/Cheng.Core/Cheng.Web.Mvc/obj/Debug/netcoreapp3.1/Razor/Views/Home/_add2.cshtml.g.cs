#pragma checksum "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\Home\_add2.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dab87712bea57dd6ac1126b39327cca505b1ab86"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home__add2), @"mvc.1.0.view", @"/Views/Home/_add2.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\_ViewImports.cshtml"
using Cheng.Web.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\_ViewImports.cshtml"
using Cheng.Web.Mvc.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\_ViewImports.cshtml"
using Cheng.Web.Mvc.Models.ChuanZhi;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dab87712bea57dd6ac1126b39327cca505b1ab86", @"/Views/Home/_add2.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bbf08f4273783b959d38694dd0c7932f93d6a258", @"/Views/_ViewImports.cshtml")]
    public class Views_Home__add2 : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\Home\_add2.cshtml"
  
    var name = Model.GetType().GetProperty("pwd").GetValue(Model, null);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div>\r\n    ");
#nullable restore
#line 7 "E:\mycode\cheng.core\trunk\Cheng.Core\Cheng.Web.Mvc\Views\Home\_add2.cshtml"
Write(name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
