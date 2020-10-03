// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

#region Usings
using System.Diagnostics.CodeAnalysis;
#endregion

[assembly:
    SuppressMessage("Simplification",
                    "RCS1085:Use auto-implemented property.")]
[assembly: SuppressMessage("Sonar Code Smell", "S2292:Trivial properties should be auto-implemented")]
[assembly: SuppressMessage("Sonar Code Smell", "S1172:Unused method parameters should be removed")]
[assembly:
    SuppressMessage("Sonar Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization")]
[assembly:
    SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies")]