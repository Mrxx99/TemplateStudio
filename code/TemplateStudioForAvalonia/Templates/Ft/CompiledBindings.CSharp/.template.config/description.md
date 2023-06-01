Bindings defined in the XAML are using reflection in order to find and access the requested property in your ViewModel. In Avalonia you can also use compiled bindings, which has some benefits:

If you use compiled bindings and the property you bind to is not found, you will get a compile-time error. Hence you get a much better debugging experience.

Reflection is known to be slow. Using compiled bindings can therefore improve the performance of your application.
