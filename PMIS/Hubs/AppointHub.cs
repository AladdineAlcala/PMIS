using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PMIS.ViewModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace PMIS.Hubs
{
    [Authorize]
    [HubName("appointmenthub")]
    public class AppointHub:Hub
    {
        private static readonly ConcurrentDictionary<string,UsersConnectionViewModel> UsersConnectionDict=new ConcurrentDictionary<string, UsersConnectionViewModel>(StringComparer.InvariantCultureIgnoreCase);


        public override Task OnConnected()
        {

            string userid = Context.User.Identity.GetUserId();
            string connectionid = Context.ConnectionId;

            var connectedUsers = UsersConnectionDict.GetOrAdd(userid, _ => new UsersConnectionViewModel()
            {
             
                UserId = userid,
                ConnectionId = new HashSet<string>()
            });

            lock (connectedUsers.ConnectionId)
            {
                connectedUsers.ConnectionId.Add(connectionid);

                if (connectedUsers.ConnectionId.Count == 1)
                {
                    Clients.Others.userconnected(connectedUsers);
                }
            }

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            Task userTask=Task.Run(() =>
            {
                UserDisconnected(Context.User.Identity.GetUserId(),Context.ConnectionId);
            });

            return base.OnDisconnected(stopCalled);
        }


        private async void UserDisconnected(string userid, string cid)
        {
            await Task.Delay(10000);

            string uid = userid;
            string connId = cid;


            UsersConnectionViewModel usersConnection;

            UsersConnectionDict.TryGetValue(uid, out usersConnection);

            if (usersConnection != null)

                lock (usersConnection.ConnectionId)
                {

                    usersConnection.ConnectionId.RemoveWhere(cId => cId.Equals(connId));

                    if (!usersConnection.ConnectionId.Any())
                    {
                        UsersConnectionViewModel removeuser;

                        UsersConnectionDict.TryRemove(uid, out removeuser);
                        Clients.Others.userDisconnected(uid);
                    }
                }


        }

        public void SendAppointment(string userid,List<AppointmentScheduleViewModel> appointmentlist)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AppointHub>();
            UsersConnectionViewModel userrecieverconnection;

            if (UsersConnectionDict.TryGetValue(userid, out userrecieverconnection))
            {
                var connectionid = userrecieverconnection.ConnectionId.FirstOrDefault();

                var obj = JsonConvert.SerializeObject(appointmentlist);

                context.Clients.Client(connectionid).displayAppointment(obj);

            }
        }


        public List<UsersConnectionViewModel> GetAllLoggedUsers()
        {
            return UsersConnectionDict.Values.ToList();
        }

    }
}