using System;
using RabbitMQ.Client;

namespace UltimatR
{
    public interface IConnectionPool : IDisposable
    {
        IConnection Get(string connectionName = null);
    }
}