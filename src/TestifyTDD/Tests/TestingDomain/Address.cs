namespace Tests.TestingDomain
{
    public class Address : UnitTestDomainBase
    {
        // These are here to show that members with nonpublic getters are ignored
        private string _asecret;
        private string PrivateProperty { get; set; }
        protected string ProtectedProperty { get; set; }

        // Public getters
        public string StreetAddress_1 { get; set; }
        public string StreetAddress_2 { get; set; }
        public int StreetNumber { get; set; }
        public long LotSquareFootage { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsInUsa { get; set; }
        public int LotNumber { get; private set; }
        public string DivisionName { get { return "I have no setter"; } }

        // This is here for testing lambdas using public methods
        public Address GetNeighboringAddress() { return this; /* Recursive Neighbor, FTW! */ }
    }
}