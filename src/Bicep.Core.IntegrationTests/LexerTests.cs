﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bicep.Core.Parser;
using Bicep.Core.Samples;
using Bicep.Core.UnitTests.Json;
using Bicep.Core.UnitTests.Serialization;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bicep.Core.IntegrationTests
{
    [TestClass]
    public class LexerTests
    {
        public TestContext? TestContext { get; set; }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method, DynamicDataDisplayNameDeclaringType = typeof(DataSet), DynamicDataDisplayName = nameof(DataSet.GetDisplayName))]
        public void LexerShouldRoundtrip(DataSet dataSet)
        {
            var lexer = new Lexer(new SlidingTextWindow(dataSet.Bicep));
            lexer.Lex();

            var serialized = new StringBuilder();
            new TokenWriter(serialized).WriteTokens(lexer.GetTokens());

            serialized.ToString().Should().Be(dataSet.Bicep, "because the lexer should not lose information");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method, DynamicDataDisplayNameDeclaringType = typeof(DataSet), DynamicDataDisplayName = nameof(DataSet.GetDisplayName))]
        public void LexerShouldProduceValidTokenLocations(DataSet dataSet)
        {
            var lexer = new Lexer(new SlidingTextWindow(dataSet.Bicep));
            lexer.Lex();

            foreach (Token token in lexer.GetTokens())
            {
                // lookup the text of the token in original contents by token's span
                string tokenText = dataSet.Bicep[new Range(token.Span.Position, token.Span.Position + token.Span.Length)];

                tokenText.Should().Be(token.Text, "because token text at location should match original contents at the same location");
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method, DynamicDataDisplayNameDeclaringType = typeof(DataSet), DynamicDataDisplayName = nameof(DataSet.GetDisplayName))]
        public void LexerShouldProduceExpectedTokens(DataSet dataSet)
        {
            var lexer = new Lexer(new SlidingTextWindow(dataSet.Bicep));
            lexer.Lex();

            var actual = JToken.FromObject(lexer.GetTokens().Select(token => new TokenItem(token)), DataSetSerialization.CreateSerializer());
            FileHelper.SaveResultFile(this.TestContext!, $"{dataSet.Name}_Tokens_Actual.json", actual.ToString(Formatting.Indented));

            var expected = JToken.Parse(dataSet.Tokens);
            FileHelper.SaveResultFile(this.TestContext!, $"{dataSet.Name}_Tokens_Expected.json", expected.ToString(Formatting.Indented));

            JsonAssert.AreEqual(expected, actual, this.TestContext!, $"{dataSet.Name}_Tokens_Delta.json");

            lexer.GetTokens().Count(token => token.Type == TokenType.EndOfFile).Should().Be(1, "because there should only be 1 EOF token");
            lexer.GetTokens().Last().Type.Should().Be(TokenType.EndOfFile, "because the last token should always be EOF.");
        }

        [DataTestMethod]
        [DynamicData(nameof(GetValidData), DynamicDataSourceType.Method, DynamicDataDisplayNameDeclaringType = typeof(DataSet), DynamicDataDisplayName = nameof(DataSet.GetDisplayName))]
        public void LexerShouldProduceValidStringLiteralTokensOnValidFiles(DataSet dataSet)
        {
            var lexer = new Lexer(new SlidingTextWindow(dataSet.Bicep));
            lexer.Lex();

            foreach (Token stringToken in lexer.GetTokens().Where(token => token.Type == TokenType.String))
            {
                Action getStringValue = () => Lexer.GetStringValue(stringToken);
                getStringValue.Should().NotThrow($"because string token at span {stringToken.Span} should have a string value. Token Text = {stringToken.Text}");
            }
        }

        private static IEnumerable<object[]> GetData() => DataSets.AllDataSets.ToDynamicTestData();

        private static IEnumerable<object[]> GetValidData() => DataSets.AllDataSets.Where(ds => ds.IsValid).ToDynamicTestData();
    }
}
