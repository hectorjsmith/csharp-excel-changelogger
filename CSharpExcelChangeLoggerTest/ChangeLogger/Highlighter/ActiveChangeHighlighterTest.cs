﻿using CSharpExcelChangeLogger.Api;
using CSharpExcelChangeLogger.ChangeLogger.Highlighter;
using CSharpExcelChangeLogger.ChangeLogger.Memory;
using CSharpExcelChangeLoggerTest.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpExcelChangeLoggerTest.ChangeLogger.Highlighter
{
    class ActiveChangeHighlighterTest
    {
        [Test]
        public void Given_ActiveChangeLogger_When_AfterChangeHookCalled_Then_RangeIsHighlighted()
        {
            int testColour = 111;
            IChangeLoggerApi api = ChangeLoggerApi.Instance;
            api.Configuration.CellHighlightColour = testColour;
            api.SetLogger(new TestLogger());

            SimpleMockSheet sheet = new SimpleMockSheet();
            SimpleMockRange range = new SimpleMockRange();

            IChangeHighlighter highlighter = new SimpleChangeHighlighter();
            highlighter.HighlightRange(new SimpleMockMemoryComparison(), sheet, range);

            Assert.AreEqual(testColour, range.FillColour, "Range should be filled with correct colour");
        }
    }
}
