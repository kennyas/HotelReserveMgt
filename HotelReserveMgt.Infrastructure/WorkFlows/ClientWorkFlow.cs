using HotelReserveMgt.Core.Enums;
using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReserveMgt.Infrastructure.WorkFlows
{
    //public class ClientWorkFlow
    //{
    //}

    public class ClientWorkFlow
    {
       

        ClientState _state = ClientState.Registered;

        StateMachine<ClientState, ClientTrigger> _machine;
        StateMachine<ClientState, ClientTrigger>.TriggerWithParameters<int> _setVolumeTrigger;

        StateMachine<ClientState, ClientTrigger>.TriggerWithParameters<string> _setCalleeTrigger;

        string _client;

        string _room;

        public ClientWorkFlow(string client)
        {
            _client = client;
            _machine = new StateMachine<ClientState, ClientTrigger>(() => _state, s => _state = s);

            _setVolumeTrigger = _machine.SetTriggerParameters<int>(ClientTrigger.Register);
            _setCalleeTrigger = _machine.SetTriggerParameters<string>(ClientTrigger.BookRoom);

            _machine.Configure(ClientState.Registered)
                .Permit(ClientTrigger.BookRoom, ClientState.BookedRoom);

            _machine.Configure(ClientState.BookedRoom)
                .OnEntryFrom(_setCalleeTrigger, callee => OnRegistered(callee), "Room No to be booked")
                .Permit(ClientTrigger.ConfirmationMail, ClientState.Checkin);

            _machine.Configure(ClientState.Checkin)
                .Permit(ClientTrigger.CheckedOut, ClientState.Checkout);
            //.Permit(ClientTrigger.PlacedOnHold, State.OnHold);

            _machine.Configure(ClientState.Checkout)
                .SubstateOf(ClientState.Checkin)
                .Permit(ClientTrigger.Cleaned, ClientState.Registered);

            _machine.OnTransitioned(t => Console.WriteLine($"OnTransitioned: {t.Source} -> {t.Destination} via {t.Trigger}({string.Join(", ", t.Parameters)})"));
        }

        void OnRegistered(string roomId)
        {
            _room = roomId;
            Console.WriteLine("Order placed for room : [{0}]", _room);
        }

       
        public void Registered(string callee)
        {
            _machine.Fire(_setCalleeTrigger, callee);
        }

        public void Booked()
        {
            _machine.Fire(ClientTrigger.Register);
        }

        public void Occupied()
        {
            _machine.Fire(ClientTrigger.BookRoom);
        }

        public void Checkout()
        {
            _machine.Fire(ClientTrigger.ConfirmationMail);
        }

        public string ToDotGraph()
        {
            return UmlDotGraph.Format(_machine.GetInfo());
        }
    }
}
