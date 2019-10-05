using Hyperledger.Fabric.SDK;

namespace HyperledgerTest
{
    public class SampleStoreEnrollement : IEnrollment
    {
        public SampleStoreEnrollement(string key, string certificate)
        {
            Key = key;
            Cert = certificate;
        }

        public SampleStoreEnrollement()
        {
        }

        public string Key { get; set; }
        public string Cert { get; set; }
    }

    //public class InterfaceConverter<TInterface, TConcrete> : CustomCreationConverter<TInterface> where TConcrete : TInterface, new()
    //{
    //    public override TInterface Create(Type objectType)
    //    {
    //        return new TConcrete();
    //    }
    //}
}
