﻿using CSharpExcelChangeHandler.Base;
using CSharpExcelChangeHandler.ChangeHandling.Handler;
using CSharpExcelChangeHandler.ChangeHandling.Memory;
using CSharpExcelChangeHandler.Excel;
using CSharpExcelChangeHandler.Excel.Cached;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpExcelChangeHandler.ChangeHandling.Processor
{
    class ActiveChangeProcessor<TWorksheetType, TRangeType> : IChangeProcessor<TWorksheetType, TRangeType>
        where TWorksheetType : IWorksheet where TRangeType : IRange
    {
        private readonly IChangeHandlerMemory _memory;
        private readonly ISet<IChangeHandler<TWorksheetType, TRangeType>> _handlerSet = new HashSet<IChangeHandler<TWorksheetType, TRangeType>>();

        public ActiveChangeProcessor(ILoggingManager loggingManager)
        {
            _memory = new ChangeHandlerMemory(loggingManager);
        }

        public void ClearAllHandlers()
        {
            _handlerSet.Clear();
        }

        public void AddHandler(IChangeHandler<TWorksheetType, TRangeType> handler)
        {
            _handlerSet.Add(handler);
        }

        public void BeforeChange(TWorksheetType sheet, TRangeType range)
        {
            if (_handlerSet.Count > 0)
            {
                _memory.SetMemory(new CachedWorksheetWrapper(sheet), new CachedRangeWrapper(range));
            }
        }

        public void AfterChange(TWorksheetType sheet, TRangeType range)
        {
            if (_handlerSet.Count > 0)
            {
                IMemoryComparison memoryComparison = _memory.Compare(new CachedWorksheetWrapper(sheet), new CachedRangeWrapper(range));
                if (!memoryComparison.LocationMatchesAndDataMatches)
                {
                    CallAllHandlers(memoryComparison, sheet, range);
                }
            }
        }

        private void CallAllHandlers(IMemoryComparison memoryComparison, TWorksheetType sheet, TRangeType range)
        {
            foreach (IChangeHandler<TWorksheetType, TRangeType> handler in _handlerSet)
            {
                handler.HandleChange(memoryComparison, sheet, range);
            }
        }
    }
}
