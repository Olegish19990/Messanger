using MTP;
using MTP.PayloadBase;
using Server.Controllers;
using Server.Models;
using Server.ServerDataInfo;
using System.Reflection;

namespace Server.Routing;

internal class Route
{
    public string ActionString { get; set; }
    public Type controllerType;
    public string action;

    public Route(string actionString, Type controllerType, string action)
    {
        ActionString = actionString;
        this.controllerType = controllerType;
        this.action = action;
    }

    public void Execute<T>(ProtoMessage<T> pm, Client client, ActiveConnectionsManager activeConnectionsManager) where T : IPayload
    {
        object? controller = Activator.CreateInstance(controllerType);
        MethodInfo methodInfo = controllerType.GetMethod(action);

        // Если метод является обобщённым, подставляем нужный тип:
        MethodInfo genericMethod = methodInfo.MakeGenericMethod(typeof(T));
        genericMethod.Invoke(controller, new object[] { pm, client, activeConnectionsManager });

      
    
    }






}
