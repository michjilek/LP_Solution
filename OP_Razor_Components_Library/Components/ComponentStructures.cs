
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace OP_Razor_Components_Library.Components;
public class ComponentItem
{

    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public string ImageSource { get; set; }
    public double ImgWidth { get; set; }
    public double ImgHeight { get; set; }
    public int MyProperty { get; set; }
    public string Href { get; set; }
    public NavLinkMatch Match { get; set; }
    public bool IsSelected { get; set; }
}