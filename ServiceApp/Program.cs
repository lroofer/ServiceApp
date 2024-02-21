// See https://aka.ms/new-console-template for more information

using DetailsManager;

Console.WriteLine("Hello, World!");
var widget = new Widget("oklohoma", "Robot", 12, 13131.1313, true, "2020-12-17T12:30:33.13113", new List<Specification>());
var specification = new Specification("Natural", 13.31, true);
widget.AddSpecification(specification);
var widgets = new List<Widget> { widget };
var lst = new WidgetList(widgets);
Console.WriteLine(lst.ToJSON());