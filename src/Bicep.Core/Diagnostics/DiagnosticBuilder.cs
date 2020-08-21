using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Extensions;
using Bicep.Core.Parser;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Diagnostics
{
    public static class DiagnosticBuilder
    {
        // ReSharper disable once InconsistentNaming
        public const string BCP061CyclicExpressionCode = "BCP061";

        public delegate ErrorDiagnostic ErrorBuilderDelegate(DiagnosticBuilderInternal builder);

        public class DiagnosticBuilderInternal
        {
            public DiagnosticBuilderInternal(TextSpan textSpan)
            {
                TextSpan = textSpan;
            }

            public TextSpan TextSpan { get; }

            public ErrorDiagnostic UnrecognizedToken(object token) => new ErrorDiagnostic(
                TextSpan,
                "BCP001",
                $"The following token is not recognized: '{token}'.");

            public ErrorDiagnostic UnterminatedMultilineComment() => new ErrorDiagnostic(
                TextSpan,
                "BCP002",
                "The multi-line comment at this location is not terminated. Terminate it with the */ character sequence.");

            public ErrorDiagnostic UnterminatedString() => new ErrorDiagnostic(
                TextSpan,
                "BCP003",
                "The string at this location is not terminated. Terminate the string with a single quote character.");

            public ErrorDiagnostic UnterminatedStringWithNewLine() => new ErrorDiagnostic(
                TextSpan,
                "BCP004",
                "The string at this location is not terminated due to an unexpected new line character.");

            public ErrorDiagnostic UnterminatedStringEscapeSequenceAtEof() => new ErrorDiagnostic(
                TextSpan,
                "BCP005",
                "The string at this location is not terminated. Complete the escape sequence and terminate the string with a single unescaped quote character.");

            public ErrorDiagnostic UnterminatedStringEscapeSequenceUnrecognized(object escapeChars) => new ErrorDiagnostic(
                TextSpan,
                "BCP006",
                $"The specified escape sequence is not recognized. Only the following characters can be escaped with a backslash: {escapeChars}.");

            public ErrorDiagnostic UnrecognizedDeclaration() => new ErrorDiagnostic(
                TextSpan,
                "BCP007",
                "This declaration type is not recognized. Specify a parameter, variable, resource, or output declaration.");

            public ErrorDiagnostic ExpectedParameterContinuation() => new ErrorDiagnostic(
                TextSpan,
                "BCP008",
                "Expected the '=' token, a parameter modifier, or a newline at this location.");

            public ErrorDiagnostic UnrecognizedExpression() => new ErrorDiagnostic(
                TextSpan,
                "BCP009",
                "Expected a literal value, an array, an object, a parenthesized expression, or a function call at this location.");

            public ErrorDiagnostic InvalidInteger() => new ErrorDiagnostic(
                TextSpan,
                "BCP010",
                "Expected a valid 32-bit signed integer.");

            public ErrorDiagnostic InvalidType() => new ErrorDiagnostic(
                TextSpan,
                "BCP011",
                "The type of the specified value is incorrect. Specify a string, boolean, or integer literal.");

            public ErrorDiagnostic ExpectedKeyword(object keyword) => new ErrorDiagnostic(
                TextSpan,
                "BCP012",
                $"Expected the '{keyword}' keyword at this location.");

            public ErrorDiagnostic ExpectedParameterIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP013",
                "Expected a parameter identifier at this location.");

            public ErrorDiagnostic ExpectedParameterType() => new ErrorDiagnostic(
                TextSpan,
                "BCP014",
                $"Expected a parameter type at this location. Please specify one of the following types: {LanguageConstants.PrimitiveTypesString}.");

            public ErrorDiagnostic ExpectedVariableIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP015",
                "Expected a variable identifier at this location.");

            public ErrorDiagnostic ExpectedOutputIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP016",
                "Expected an output identifier at this location.");

            public ErrorDiagnostic ExpectedResourceIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP017",
                "Expected a resource identifier at this location.");

            public ErrorDiagnostic ExpectedCharacter(object character) => new ErrorDiagnostic(
                TextSpan,
                "BCP018",
                $"Expected the '{character}' character at this location.");

            public ErrorDiagnostic ExpectedNewLine() => new ErrorDiagnostic(
                TextSpan,
                "BCP019",
                "Expected a new line character at this location.");

            public ErrorDiagnostic ExpectedPropertyIdentifier() => new ErrorDiagnostic(
                TextSpan,
                "BCP020",
                "Expected a property identifier at this location.");

            public ErrorDiagnostic ExpectedNumericLiteral() => new ErrorDiagnostic(
                TextSpan,
                "BCP021",
                "Expected a numeric literal at this location.");

            public ErrorDiagnostic ExpectedPropertyName() => new ErrorDiagnostic(
                TextSpan,
                "BCP022",
                "Expected a property name at this location.");

            public ErrorDiagnostic ExpectedFunctionName() => new ErrorDiagnostic(
                TextSpan,
                "BCP023",
                "Expected a function name at this location.");

            public ErrorDiagnostic IdentifierNameExceedsLimit() => new ErrorDiagnostic(
                TextSpan,
                "BCP024",
                $"The identifier exceeds the limit of {LanguageConstants.MaxIdentifierLength}. Reduce the length of the identifier.");

            public ErrorDiagnostic PropertyMultipleDeclarations(object property) => new ErrorDiagnostic(
                TextSpan,
                "BCP025",
                $"The property '{property}' is declared multiple times in this object. Remove or rename the duplicate properties.");

            public ErrorDiagnostic OutputTypeMismatch(object expectedType, object actualType) => new ErrorDiagnostic(
                TextSpan,
                "BCP026",
                $"The output expects a value of type '{expectedType}' but the provided value is of type '{actualType}'.");

            public ErrorDiagnostic ParameterTypeMismatch(object expectedType, object actualType) => new ErrorDiagnostic(
                TextSpan,
                "BCP027",
                $"The parameter expects a default value of type '{expectedType}' but provided value is of type '{actualType}'.");

            public ErrorDiagnostic IdentifierMultipleDeclarations(object identifier) => new ErrorDiagnostic(
                TextSpan,
                "BCP028",
                $"Identifier '{identifier}' is declared multiple times. Remove or rename the duplicates.");

            public ErrorDiagnostic InvalidResourceType() => new ErrorDiagnostic(
                TextSpan,
                "BCP029",
                "The resource type is not valid. Specify a valid resource type.");

            public ErrorDiagnostic InvalidOutputType() => new ErrorDiagnostic(
                TextSpan,
                "BCP030",
                $"The output type is not valid. Please specify one of the following types: {LanguageConstants.PrimitiveTypesString}.");

            public ErrorDiagnostic InvalidParameterType() => new ErrorDiagnostic(
                TextSpan,
                "BCP031",
                $"The parameter type is not valid. Please specify one of the following types: {LanguageConstants.PrimitiveTypesString}.");

            public ErrorDiagnostic CompileTimeConstantRequired() => new ErrorDiagnostic(
                TextSpan,
                "BCP032",
                "The value must be a compile-time constant.");

            public ErrorDiagnostic ExpectedValueTypeMismatch(object expectedType, object actualType) => new ErrorDiagnostic(
                TextSpan,
                "BCP033",
                $"Expected a value of type '{expectedType}' but the provided value is of type '{actualType}'.");

            public ErrorDiagnostic ArrayTypeMismatch(object expectedType, object actualType) => new ErrorDiagnostic(
                TextSpan,
                "BCP034",
                $"The enclosing array expected an item of type '{expectedType}', but the provided item was of type '{actualType}'.");

            public ErrorDiagnostic MissingRequiredProperties(object properties) => new ErrorDiagnostic(
                TextSpan,
                "BCP035",
                $"The specified object is missing the following required properties: {properties}.");

            public ErrorDiagnostic PropertyTypeMismatch(object property, object expectedType, object actualType) => new ErrorDiagnostic(
                TextSpan,
                "BCP036",
                $"The property '{property}' expected a value of type '{expectedType}' but the provided value is of type '{actualType}'.");

            public ErrorDiagnostic DisallowedProperty(object property, object type) => new ErrorDiagnostic(
                TextSpan,
                "BCP037",
                $"The property '{property}' is not allowed on objects of type '{type}'.");

            public ErrorDiagnostic InvalidExpression() => new ErrorDiagnostic(
                TextSpan,
                "BCP043",
                "This is not a valid expression.");

            public ErrorDiagnostic UnaryOperatorInvalidType(object operatorName, object type) => new ErrorDiagnostic(
                TextSpan,
                "BCP044",
                $"Cannot apply operator '{operatorName}' to operand of type '{type}'.");

            public ErrorDiagnostic BinaryOperatorInvalidType(object operatorName, object type1, object type2) => new ErrorDiagnostic(
                TextSpan,
                "BCP045",
                $"Cannot apply operator '{operatorName}' to operands of type '{type1}' and '{type2}'.");

            public ErrorDiagnostic ValueTypeMismatch(object type) => new ErrorDiagnostic(
                TextSpan,
                "BCP046",
                $"Expected a value of type '{type}'.");

            public ErrorDiagnostic ResourceTypeInterpolationUnsupported() => new ErrorDiagnostic(
                TextSpan,
                "BCP047",
                "String interpolation is unsupported for specifying the resource type.");

            public ErrorDiagnostic CannotResolveFunction(string functionName, IList<TypeSymbol> argumentTypes) => new ErrorDiagnostic(
                TextSpan,
                "BCP048",
                $"Cannot resolve function {functionName}({argumentTypes.Select(t => t.Name).ConcatString(", ")}).");

            public ErrorDiagnostic StringOrIntegerIndexerRequired(TypeSymbol wrongType) => new ErrorDiagnostic(
                TextSpan,
                "BCP049",
                $"The array index must be of type '{LanguageConstants.String}' or '{LanguageConstants.Int}' but the provided index was of type '{wrongType}'.");

            public ErrorDiagnostic ArrayRequiredForIntegerIndexer(TypeSymbol wrongType) => new ErrorDiagnostic(
                TextSpan,
                "BCP050",
                $"Cannot use an integer indexer on an expression of type '{wrongType}'. An '{LanguageConstants.Array}' type is required.");

            public ErrorDiagnostic ObjectRequiredForStringIndexer(TypeSymbol wrongType) => new ErrorDiagnostic(
                TextSpan,
                "BCP051",
                $"Cannot use a string indexer on an expression of type '{wrongType}'. An '{LanguageConstants.Object}' type is required.");

            public ErrorDiagnostic MalformedPropertyNameString() => new ErrorDiagnostic(
                TextSpan,
                "BCP052",
                "The property name in a string indexer is malformed.");

            public ErrorDiagnostic UnknownProperty(TypeSymbol type, string badProperty) => new ErrorDiagnostic(
                TextSpan,
                "BCP053",
                $"The type '{type}' does not contain property '{badProperty}'.");

            public ErrorDiagnostic NoPropertiesAllowed(TypeSymbol type) => new ErrorDiagnostic(
                TextSpan,
                "BCP054",
                $"The type '{type}' does not contain any properties.");

            public ErrorDiagnostic ObjectRequiredForPropertyAccess(TypeSymbol wrongType) => new ErrorDiagnostic(
                TextSpan,
                "BCP055",
                $"Cannot access properties of type '{wrongType}'. An '{LanguageConstants.Object}' type is required.");

            public ErrorDiagnostic AmbiguousSymbolReference(string name, IEnumerable<string> namespaces) => new ErrorDiagnostic(
                TextSpan,
                "BCP056",
                $"The reference to name '{name}' is ambiguous because it exists in namespaces '{namespaces.ConcatString(", ")}'. The reference must be fully-qualified.");

            public ErrorDiagnostic SymbolicNameDoesNotExist(string name) => new ErrorDiagnostic(
                TextSpan,
                "BCP057",
                $"The name '{name}' does not exist in the current context.");

            public ErrorDiagnostic OutputReferenceNotSupported(string name) => new ErrorDiagnostic(
                TextSpan,
                "BCP058",
                $"The name '{name}' is an output. Outputs cannot be referenced in expressions.");

            public ErrorDiagnostic SymbolicNameIsNotAFunction(string name) => new ErrorDiagnostic(
                TextSpan,
                "BCP059",
                $"The name '{name}' is not a function.");

            public ErrorDiagnostic CyclicExpression() => new ErrorDiagnostic(
                TextSpan,
                BCP061CyclicExpressionCode,
                "The expression is involved in a cycle.");

            public ErrorDiagnostic ReferencedSymbolHasErrors(string name) => new ErrorDiagnostic(
                TextSpan,
                "BCP062",
                $"The referenced declaration with name '{name}' is not valid.");

            public ErrorDiagnostic SymbolicNameIsNotAVariableOrParameter(string name) => new ErrorDiagnostic(
                TextSpan,
                "BCP063",
                $"The name '{name}' is not a parameter or variable.");

            public ErrorDiagnostic MalformedString() => new ErrorDiagnostic(
                TextSpan,
                "BCP064",
                "The string at this location is malformed.");

            public ErrorDiagnostic FunctionOnlyValidInParameterDefaults(string functionName) => new ErrorDiagnostic(
                TextSpan,
                "BCP065",
                $"Function '{functionName}' is not valid at this location. It can only be used in parameter default declarations.");

            public ErrorDiagnostic FunctionOnlyValidInResourceBody(string functionName) => new ErrorDiagnostic(
                TextSpan,
                "BCP066",
                $"Function '{functionName}' is not valid at this location. It can only be used in resource declarations.");
        }

        public static DiagnosticBuilderInternal ForPosition(TextSpan span)
            => new DiagnosticBuilderInternal(span);

        public static DiagnosticBuilderInternal ForPosition(IPositionable positionable)
            => new DiagnosticBuilderInternal(positionable.Span);
    }
}