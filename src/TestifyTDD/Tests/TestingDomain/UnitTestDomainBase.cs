namespace Tests.TestingDomain
{
    public class UnitTestDomainBase
    {
        public virtual long ID
        {
            get;
            protected set;
        }

        public virtual bool IsNew
        {
            get;
            protected set;
        }

        public UnitTestDomainBase()
        {
            IsNew = true;
        }
    }
}