# DependencyInjection.Interception
A library to perform interception based AOP using .Net Core DI

## Introduction
Extend .NET Core DI to create proxy wrapper around service class instances to intercept method execution
- execute custom code before and/or after method execution to inject dynamic behavior
- modularize cross cutting concerns (logging, caching, security, etc.) into their own classes
- flexibility to control which class/methods should be intercepted
 
## Getting Started
1. Install package from nuget `dotnet add package DependencyInjection.Interception`
2. Create one or more class that will perform interception by inheriting from `Interceptor` class and overriding `PreInterceptor` and `PostInterceptor` methods as needed
```
public class LogInterceptor : Interceptor
{
    public override void PreInterceptor(InterceptionContext context)
    {
        //Do pre-interception here...
    }

    public override void PostInterceptor(InterceptionContext context)
    {
        //Do post-interception here...
    }
}
```
3. Register interception class(es) to `ServiceCollection`
```
var services = new ServiceCollection();
services.AddSingleton<LogInterceptor>();
```
4. Decorate class to be intercepted with `Interceptable` attribute
```
[Interceptable]
public class Foo : IFoo
{
    ...
}
```
5. Decorate `Interceptor(Type)` attribute to class (or methods) by passing type of Interception class
```
[Interceptable]
[Interceptor(typeof(LogInterceptor))]
public class Foo : IFoo
{
    ...
}
```
6. Ensure methods are marked as `virtual` to ensure they can be intercepted
```
[Interceptable]
[Interceptor(typeof(LogInterceptor))]
public class Foo : IFoo
{
    public virtual void Bar()
    {
        ...
    }
}
```
7. Register service class(es) to `ServiceCollection`
```
var services = new ServiceCollection();
services.AddSingleton<LogInterceptor>();
services.AddTransient<IFoo, Foo>();
```
8. Build interceptable provider using `BuildServiceProviderWithInterception()` method
```
var provider = services.BuildServiceProviderWithInterception();
```
9. Get service class instance from `IServiceCollection`
```
var foo = provider.GetService<IFoo>();
```