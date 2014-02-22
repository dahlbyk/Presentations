namespace AbstractToYield
{
    sealed class CannotBeExtended { }

    abstract class MustBeExtended
    {
        private readonly int value;
        protected readonly string notOverriddenMessage;

        protected MustBeExtended(int value)
        {
            this.value = value;
            notOverriddenMessage = "Not overridden: " + value;
        }

        public abstract override int GetHashCode();

        protected virtual void MayBeOverridden()
        {
            notOverriddenMessage.Dump();
        }
    }

    class CanBeExtended : MustBeExtended
    {
        public CanBeExtended()
            : this(5)
        {
        }

        public CanBeExtended(int value)
            : base(value)
        {
        }

        public override int GetHashCode()
        {
            return 42;
        }

        public void CanHazARef(ref string thing)
        {
            // Don't have to write to thing
        }

        public void CanHazAnOut(out string thing)
        {
            thing = "Compile error if I don't set thing";
        }

        void Test()
        {
            this.Dump();
            this.ToString().Dump();

            MayBeOverridden();

            var changeMeMaybe = "before";
            CanHazARef(ref changeMeMaybe);
            changeMeMaybe.Dump();
        }

        public new string ToString()
        {
            return "I am CanBeExtended, hear me rawr!";
        }
    }
}
