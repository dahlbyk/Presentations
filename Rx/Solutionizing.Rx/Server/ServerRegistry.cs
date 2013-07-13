using System;
using MassTransit;
using MassTransit.StructureMapIntegration;
using StructureMap;

namespace Server
{
    public class ServerRegistry : MassTransitRegistryBase
    {
        public ServerRegistry()
        {
            RegisterControlBus("msmq://localhost/test_server_control", x =>
            {
            });

            RegisterServiceBus("msmq://localhost/test_server", x =>
            {
                x.SetConcurrentConsumerLimit(1);
                x.UseControlBus(ObjectFactory.GetInstance<IControlBus>());
            });
        }
    }
}
