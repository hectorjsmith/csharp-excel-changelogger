﻿using CSharpExcelChangeLogger.Base;
using CSharpExcelChangeLogger.ChangeLogger.Handler;
using CSharpExcelChangeLogger.ChangeLogger.Memory;
using CSharpExcelChangeLogger.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpExcelChangeLogger.ChangeLogger.Highlighter
{
    internal class SimpleChangeHighlighter : BaseClass, IChangeHandler
    {
        private readonly int _highlightColour;

        public SimpleChangeHighlighter(int highlightColour)
        {
            _highlightColour = highlightColour;
        }

        public void HandleChange(IMemoryComparison memoryComparison, IWorksheet sheet, IRange range)
        {
            if (!memoryComparison.IsColumnDelete && !memoryComparison.IsRowDelete)
            {
                Log.Debug(string.Format("Highlighting range '{0}' on sheet '{1}'", range.Address, sheet.Name));
                range.FillRange(_highlightColour);
            }
        }
    }
}
