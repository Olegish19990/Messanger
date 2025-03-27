using MTP;
using MTP.PayloadBase;
using Server.Controllers;
using Server.Models;
using Server.ServerDataInfo;

namespace Server.Routing;

internal class Router
{
    private static Router? _instance;
    public List<Route> Routes { get; }

    public Router(List<Route> routes)
    {
        Routes = routes;
    }


    public void Handle<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager)
        where T : IPayload
    {
        Route route = Routes.First(r => r.ActionString == pm.Action);
        route.Execute<T>(pm, client, activeConnectionsManager);


    }

}
