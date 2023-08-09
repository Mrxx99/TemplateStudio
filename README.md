[![VS marketplace](https://img.shields.io/visual-studio-marketplace/v/AvaloniaTeam.TemplateStudioForAvalonia.svg?label=VS-Marketplace)](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.TemplateStudioForAvalonia)
# Template Studio for Avalonia

Template Studio is a Visual Studio 2022 extension that accelerate the creation of new Avalonia apps using a wizard-based experience.

Projects created with this extension contains well-formed, readable code and incorporate the latest development features while implementing proven patterns and leading practices.

To get started, install the extension, then select the corresponding Template Studio project template when creating a new project in Visual Studio. Name your project, then click Create to launch the Template Studio wizard.

<figcaption>VS New Project Dialog (C#)</figcaption>

![VS New Project screenshot](https://github.com/AvaloniaUI/TemplateStudio/assets/53405089/6c5cf387-3df6-4821-9668-e544f40535a7)

<figcaption>VS New Project Dialog (F#)</figcaption>

![VS New F# Project screenshot](https://github.com/AvaloniaUI/TemplateStudio/assets/53405089/42ac695f-a78d-4ea8-a6ec-12522677dc76)



<figcaption>Template Studio for Avalonia (C#)</figcaption>

![Template Studio for Avalonia screenshot](https://github.com/AvaloniaUI/TemplateStudio/assets/53405089/1137f280-e418-4054-95b6-06bb84fbb136)

## Features

Template Studio approaches app creation using the following six attribute sets:

### **Platform**

First, on which platforms you are planning to run your app?

Options: **Desktop**, **Web**, **Andoid**, **IOS**.

### **App design pattern**

Next, what coding pattern do you want to use in your project.

Options: **MVVM Toolkit**, **ReactiveUI**.

![Template Studio C# design patterns](https://github.com/AvaloniaUI/TemplateStudio/assets/53405089/f1284435-b989-42d5-a476-a8719b2dc83d)

### **Features**

Specify which capabilities you want to use in your app, and we'll build out the framework for the features into your app.

Options: **Compiled Bindings**, **Embedded Support**, **Meadow Support**.

![Template Studio C# features](https://github.com/AvaloniaUI/TemplateStudio/assets/53405089/b8e01ce4-5fef-4794-bd2e-6c042126c0ad)

## Principles

1. Generated templates will be kept simple.
2. Generated templates are a starting point, not a completed application.
3. Generated templates must be able to compile and run once generated.
4. Templates should have comments to aid developers. This includes links to signup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
5. Code should follow [.NET Core coding style](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md).

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## Frameworks and libraries in generated code not created by our team

### Frameworks
- [MVVM Toolkit](https://aka.ms/mvvmtoolkit)
- [ReactiveUI](https://github.com/reactiveui/ReactiveUI)
