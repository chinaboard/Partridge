using System;

namespace Partridge.Http
{
    public interface IKayakServer
    {
        IDisposable Start();
        Action<Action<ISocket>> GetConnection();
    }
}
