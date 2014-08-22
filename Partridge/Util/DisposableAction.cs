using System;

namespace Partridge.Util
{
    public class DisposableAction : IDisposable
    {
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public DisposableAction(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            this.action = action;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            action();
        }
    }
}
