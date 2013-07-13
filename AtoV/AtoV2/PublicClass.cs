using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtoV2
{
    public class PublicClass
    {
        public int PublicProperty { get; set; }

        protected int ProtectedProperty { get; set; }

        protected internal int ProtectedInternalProperty { get; set; }

        internal int InternalProperty { get; set; }

        public int PublicPropertyWithPrivateSetter
        {
            get;
            private set;
        }
    }
}