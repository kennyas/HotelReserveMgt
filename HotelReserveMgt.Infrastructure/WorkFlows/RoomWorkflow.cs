using HotelReserveMgt.Core.Domain.Entities;
using HotelReserveMgt.Core.DTOs.Account;
using HotelReserveMgt.Core.Enums;
using HotelReserveMgt.Core.Interfaces;
using Stateless;
using System;

namespace HotelReserveMgt.Infrastructure.WorkFlows
{
    public class RoomWorkflow : IRoomWorkflow
    {
        protected RoomState _state;
        protected StateMachine<RoomState, RoomTrigger> _machine;
        protected RoomState _previousState;
        protected bool _isSuccesful;

        public Room _roomData { get; set; }
       

        protected StateMachine<RoomState, RoomTrigger>.TriggerWithParameters<RegisterRequest> _assignTrigger;
        protected StateMachine<RoomState, RoomTrigger>.TriggerWithParameters<RegisterRequest> _transferTrigger;
        public RoomState CurrentState = RoomState.New;

        public RoomWorkflow(Room data)
        {
            InitializeStateMachine();
            _roomData = data;

        }

        public RoomState AssetState
        {
            get
            {
                return _state;
            }
            set
            {
                _previousState = _state;
                _state = value;
                Console.WriteLine("------------------------");
                Console.WriteLine($"Room No : {_roomData.Id.ToString()}");
                Console.WriteLine($"Previous Room state : {_previousState.ToString()}");
                Console.WriteLine($"New Room state : {_state.ToString()}");
            }
        }
        private void InitializeStateMachine()
        {
            _state = RoomState.New;


            _machine = new StateMachine<RoomState, RoomTrigger>(() => CurrentState, s => CurrentState = s);

            _machine.Configure(RoomState.New)
                    .Permit(RoomTrigger.Booked, RoomState.Reserved)
                    .PermitIf(RoomTrigger.Clean,RoomState.Cleaned)
                    .PermitReentry(RoomTrigger.Released);
            
            _machine.Configure(RoomState.Reserved)
                     .Permit(RoomTrigger.Assigned, RoomState.Occupied);
            

            _machine.Configure(RoomState.Occupied)
                    .Permit(RoomTrigger.RoomKeyObtained, RoomState.Unavailable);
                   

        }

        //private void SetOwner(RegisterRequest owner)
        //{
        //    _roomData.Client = owner;
        //}

        //private void ProcessDecommission()
        //{
        //    Console.WriteLine("Clearing Client Data..");
        //    _roomData.Client = null;
        //    OnEntry();
        //}


        public void RoomCleaned()
        {
            //Console.WriteLine($"Entering {_state.ToString()} ...");
            _machine.Fire(RoomTrigger.Clean);
        }

        public void RoomAdded()
        {
            _machine.Fire(RoomTrigger.AddRoom);
        }

        public void RoomReleased()
        {
            _machine.Fire(RoomTrigger.Released);
        }

        public void RoomAssigned()
        {
            _machine.Fire(RoomTrigger.Assigned);
        }
        public void Unavailable()
        {
            _machine.Fire(RoomTrigger.Unavailable);
        }
        public void Fire(RoomTrigger trigger)
        {
            _isSuccesful = false;
            try
            {
                _machine.Fire(trigger);
                _isSuccesful = true;
            }
            catch
            {
                Console.WriteLine("Error during state transition.");
                _isSuccesful = false;
            }
        }

        //public void Assign(RegisterRequest owner)
        //{
        //    _isSuccesful = false;
        //    try
        //    {
        //        _machine.Fire(_assignTrigger, owner);
        //        _isSuccesful = true;
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Error during state transition.");
        //        _isSuccesful = false;
        //    }
        //}

        public void RoomBooked()
        {
            Fire(RoomTrigger.Booked);
        }


    }
}
