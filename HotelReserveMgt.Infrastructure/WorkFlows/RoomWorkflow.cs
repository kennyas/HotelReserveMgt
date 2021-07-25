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
        public RegisterRequest _clientData { get; set; }

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

            //_assignTrigger = _machine.SetTriggerParameters<RegisterRequest>(RoomTrigger.Assigned);
            //_transferTrigger = _machine.SetTriggerParameters<RegisterRequest>(RoomTrigger.Transferred);


            _machine.Configure(RoomState.New)
                    .Permit(RoomTrigger.Booked, RoomState.Reserved)
                    .PermitIf(RoomTrigger.Clean,RoomState.Cleaned)
                    .PermitReentry(RoomTrigger.Released);
            //.OnEntry(() => OnEntry())
            //.OnActivate(() => OnActivate())
            //.Permit(RoomTrigger.Booked, RoomState.Unavailable)
            //.OnDeactivate(() => OnDeactivate())
            //.OnExit(() => OnExit());

            _machine.Configure(RoomState.Reserved)
                     .Permit(RoomTrigger.Assigned, RoomState.Occupied);
            // .Permit(RoomTrigger.Booked, RoomState.Unavailable)
            // .OnExit(() => OnExit())
            //.OnEntryFrom(RoomTrigger.Released, () => ProcessDecommission())
            // .OnDeactivate(() => OnDeactivate());


            _machine.Configure(RoomState.Occupied)
                    .Permit(RoomTrigger.RoomKeyObtained, RoomState.Unavailable);
                    //.OnEntry(() => OnEntry())
                    //.OnEntryFrom(_assignTrigger, owner => SetOwner(owner))
                    //.OnEntryFrom(_transferTrigger, owner => SetOwner(owner))
                    //.OnActivate(() => OnActivate())
                    //.OnExit(() => OnExit())
                    //.OnDeactivate(() => OnDeactivate())
                    //.PermitReentry(RoomTrigger.Transferred)
                    //.Permit(RoomTrigger.Released, RoomState.Free)
                    //.Permit(RoomTrigger.Booked, RoomState.Unavailable);



            //_machine.Configure(RoomState.Unavailable)
            //        .OnEntry(() => OnEntry())
            //        .OnActivate(() => OnActivate())
            //        .OnExit(() => OnExit())
            //        .OnDeactivate(() => OnDeactivate());
           

        }

        private void SetOwner(RegisterRequest owner)
        {
            _roomData.Client = owner;
        }

        private void ProcessDecommission()
        {
            Console.WriteLine("Clearing Client Data..");
            _roomData.Client = null;
            OnEntry();
        }


        public void OnEntry()
        {
            Console.WriteLine($"Entering {_state.ToString()} ...");
        }

        public void OnActivate()
        {
            Console.WriteLine($"Activating {_state.ToString()} ...");
        }

        public void OnDeactivate()
        {
            Console.WriteLine($"Deactivating {_state.ToString()} ...");
        }

        public void OnExit()
        {
            Console.WriteLine($"Exiting {_state.ToString()} ...");
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

        //public void FinishedTesting()
        //{
        //    Fire(RoomTrigger.Tested);
        //}

        public void Assign(RegisterRequest owner)
        {
            _isSuccesful = false;
            try
            {
                _machine.Fire(_assignTrigger, owner);
                _isSuccesful = true;
            }
            catch
            {
                Console.WriteLine("Error during state transition.");
                _isSuccesful = false;
            }
        }

        public void Release()
        {
            Fire(RoomTrigger.Released);
        }


        public void Transfer(RegisterRequest owner)
        {
            _isSuccesful = false;
            try
            {
                _machine.Fire(_transferTrigger, owner);
                _isSuccesful = true;
            }
            catch
            {
                Console.WriteLine("Error during state transition.");
                _isSuccesful = false;
            }
        }



        public bool IsSuccessful()
        {
            return _isSuccesful;
        }
    }
}
