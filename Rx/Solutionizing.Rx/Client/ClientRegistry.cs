using System;
using MassTransit;
using MassTransit.StructureMapIntegration;
using StructureMap;

namespace Client
{
    public class ClientRegistry : MassTransitRegistryBase
    {
        public ClientRegistry()
        {
            RegisterControlBus("msmq://localhost/test_client_control", x =>
            {
            });

            RegisterServiceBus("msmq://localhost/test_client", x =>
            {
                x.SetConcurrentConsumerLimit(1);
                x.UseControlBus(ObjectFactory.GetInstance<IControlBus>());
            });
        }
    }
}
