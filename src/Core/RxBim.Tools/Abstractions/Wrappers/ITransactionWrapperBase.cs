﻿namespace RxBim.Tools
{
    using System;

    /// <summary>
    /// Base transaction wrapper.
    /// </summary>
    public interface ITransactionWrapperBase : IDisposable, IWrapper
    {
        /// <summary>
        /// Starts.
        /// </summary>
        void Start();

        /// <summary>
        /// Rolls back all changes.
        /// </summary>
        void RollBack();

        /// <summary>
        /// Returns true if all changes have been rolled back. Otherwise, returns false.
        /// </summary>
        bool IsRolledBack();
    }
}