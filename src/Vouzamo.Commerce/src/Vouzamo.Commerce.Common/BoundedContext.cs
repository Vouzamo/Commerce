using System;

namespace Vouzamo.Commerce.Common
{
    public abstract class BoundedContext
    {
        public Guid Id { get; protected set; }

        protected BoundedContext()
        {
            Id = Guid.NewGuid();
        }
    }
}
