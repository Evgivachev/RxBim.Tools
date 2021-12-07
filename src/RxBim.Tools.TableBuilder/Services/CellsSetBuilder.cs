﻿namespace RxBim.Tools.TableBuilder.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Models.Contents;
    using Models.Styles;

    /// <summary>
    /// The builder of a <see cref="CellsSet"/> of a <see cref="Table"/>.
    /// </summary>
    /// <typeparam name="TSet">Type of <see cref="CellsSet"/> implementation.</typeparam>
    /// <typeparam name="TBuilder">Type of <see cref="CellsSetBuilder{TSet,TBuilder}"/> implementation.</typeparam>
    public abstract class CellsSetBuilder<TSet, TBuilder> : TableItemBuilderBase<TSet, TBuilder>
        where TSet : CellsSet
        where TBuilder : CellsSetBuilder<TSet, TBuilder>
    {
        /// <inheritdoc />
        protected CellsSetBuilder(TSet set)
            : base(set)
        {
        }

        /// <summary>
        /// Returns collection of <see cref="CellBuilder"/> for cells.
        /// </summary>
        public IEnumerable<CellBuilder> ToCells()
        {
            return ObjectForBuild.Cells.Select(x => (CellBuilder)x);
        }

        /// <summary>
        /// Fills cells with text values from list items.
        /// </summary>
        /// <param name="source">List of items.</param>
        /// <param name="cellsAction">Delegate. Applies to all filled cells.</param>
        /// <typeparam name="TSource">The type of the list item.</typeparam>
        public TBuilder FromList<TSource>(List<TSource> source, Action<CellBuilder>? cellsAction = null)
        {
            if (!source.Any())
                return (TBuilder)this;

            if (source.Count > ObjectForBuild.Cells.Count())
            {
                throw new ArgumentException(
                    "The number of items in the list should not be more than the number of cells in this set!");
            }

            for (var i = 0; i < source.Count; i++)
            {
                CellBuilder cell = ObjectForBuild.Cells[i];
                cell.SetContent(new TextCellContent(source[i]?.ToString() ?? string.Empty));
                cellsAction?.Invoke(cell);
            }

            return (TBuilder)this;
        }

        /// <summary>
        /// Sets format for the cells set.
        /// </summary>
        /// <param name="format">Format value.</param>
        /// <param name="setForInnerCells">Sets this format for all inner cells.</param>
        public TBuilder SetFormat(CellFormatStyle format, bool setForInnerCells = false)
        {
            new CellFormatStyleBuilder(ObjectForBuild.Format).SetFromFormat(format);
            if (setForInnerCells)
            {
                foreach (var cellBuilder in ToCells())
                    cellBuilder.SetFormat(format);
            }

            return (TBuilder)this;
        }
    }
}