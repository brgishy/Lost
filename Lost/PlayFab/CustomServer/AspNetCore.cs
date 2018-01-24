//-----------------------------------------------------------------------
// <copyright file="AspNetCore.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY
namespace Microsoft.AspNetCore.Mvc
    using System;

    public class Controller
    {
    }

    public class RouteAttribute : System.Attribute

    public class HttpPostAttribute : System.Attribute

    public class HttpPutAttribute : System.Attribute

    public class HttpDeleteAttribute : System.Attribute

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FromBodyAttribute : Attribute
    {
    }

#endif