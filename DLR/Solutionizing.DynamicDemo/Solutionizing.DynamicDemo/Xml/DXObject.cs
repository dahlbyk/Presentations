using System;
using System.Dynamic;
using System.Xml.Linq;

namespace Solutionizing.DynamicDemo.Xml
{
    public abstract class DXObject<T> : DynamicObject, IEquatable<DXObject<T>>, IEquatable<T> where T : XObject
    {
        protected T inner;

        protected DXObject(T inner)
        {
            this.inner = inner;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.Type.IsAssignableFrom(typeof(T)))
            {
                result = inner;
                return true;
            }

            var converter = GetConverterForType(binder.Type);

            if (converter == null)
                return base.TryConvert(binder, out result);

            result = converter(inner);
            return true;
        }

        protected abstract Func<T, object> GetConverterForType(Type type);

        public bool Equals(DXObject<T> other)
        {
            return other == null ? inner == null : Equals(other.inner);
        }

        public bool Equals(T other)
        {
            return other == null ? inner == null : other.Equals(inner);
        }

        public override bool Equals(object obj)
        {
            if (obj is T)
                return Equals((T)obj);
            if (obj is DXObject<T>)
                return Equals((DXObject<T>)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return inner == null ? 0 : inner.GetHashCode();
        }

        public override string ToString()
        {
            return inner == null ? "" : inner.ToString();
        }
    }
}
