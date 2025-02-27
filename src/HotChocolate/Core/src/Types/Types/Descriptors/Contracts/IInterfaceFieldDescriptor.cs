using System;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Types;

public interface IInterfaceFieldDescriptor
    : IDescriptor<InterfaceFieldDefinition>
    , IFluent
{
    IInterfaceFieldDescriptor SyntaxNode(
        FieldDefinitionNode fieldDefinition);

    IInterfaceFieldDescriptor Name(string value);

    IInterfaceFieldDescriptor Description(string value);

    [Obsolete("Use `Deprecated`.")]
    IInterfaceFieldDescriptor DeprecationReason(
        string reason);

    IInterfaceFieldDescriptor Deprecated(string reason);

    IInterfaceFieldDescriptor Deprecated();

    IInterfaceFieldDescriptor Type<TOutputType>()
        where TOutputType : IOutputType;

    IInterfaceFieldDescriptor Type<TOutputType>(TOutputType type)
        where TOutputType : class, IOutputType;

    IInterfaceFieldDescriptor Type(ITypeNode type);

    IInterfaceFieldDescriptor Type(Type type);

    IInterfaceFieldDescriptor Ignore(bool ignore = true);

    IInterfaceFieldDescriptor Argument(
        string name,
        Action<IArgumentDescriptor> argument);

    IInterfaceFieldDescriptor Directive<T>(T directiveInstance)
        where T : class;

    IInterfaceFieldDescriptor Directive<T>()
        where T : class, new();

    IInterfaceFieldDescriptor Directive(
        string name,
        params ArgumentNode[] arguments);
}
