using System.Collections.Generic;
using Game.Level.Area.AreaState;
using Injection;
using UnityEngine;

namespace Game.Level.Entity
{
    public sealed class EntityModule : Module<EntityModuleView>
    {
        [Inject] private Context _context;
        [Inject] private GameManager _gameManager;
        [Inject] private LevelView _levelView;

        private readonly List<CleanerController> _cleaners;
        private readonly List<UtilityController> _utilities;

        public EntityModule(EntityModuleView view) : base(view)
        {
            _cleaners = new List<CleanerController>();
            _utilities = new List<UtilityController>();
        }

        public override void Initialize()
        {
            SetAreas();
            SetRooms();
            SetCleaners();
            SetToilets();
            SetUtilities();
            SetElevators();
        }

        public override void Dispose()
        {
            foreach (var area in _gameManager.Areas)
            {
                area.Dispose();
            }
            _gameManager.Areas.Clear();

            foreach (var room in _gameManager.Rooms)
            {
                room.Dispose();
            }
            _gameManager.Rooms.Clear();

            foreach (var cleaner in _cleaners)
            {
                cleaner.Dispose();
            }
            _cleaners.Clear();

            foreach (var toilet in _gameManager.Toilets)
            {
                toilet.Dispose();
            }
            _gameManager.Toilets.Clear();

            foreach (var utility in _utilities)
            {
                utility.Dispose();
            }
            _utilities.Clear();

            _gameManager.Elevator.Dispose();

            _gameManager.Entities.Clear();
        }

        private void SetAreas()
        {
            foreach (var view in _view.AreaViews)
            {
                var area = new AreaController(view, _context);
                _gameManager.Areas.Add(area);
                _levelView.AddLvl(area.Number);
            }

            foreach (var area in _gameManager.Areas)
            {
                area.SwitchToState(new AreaInitializeState());
            }
        }

        private void SetRooms()
        {
            foreach (var view in _view.RoomViews)
            {
                var room = new RoomController(view, _context);
                _levelView.AddReward(room.Model.Area, room.GetTotalReward());
                _gameManager.Rooms.Add(room);
                _gameManager.Entities.Add(room);
            }
        }

        private void SetCleaners()
        {
            foreach (var view in _view.CleanerViews)
            {
                var cleaner = new CleanerController(view, view.UnitView, _context);
                _cleaners.Add(cleaner);
            }
        }

        private void SetToilets()
        {
            foreach (var view in _view.ToiletViews)
            {
                var toilet = new ToiletController(view, _context);
                _levelView.AddReward(toilet.Model.Area, toilet.GetTotalReward());
                _gameManager.Toilets.Add(toilet);
                _gameManager.Entities.Add(toilet);
            }
        }

        private void SetUtilities()
        {
            foreach (var view in _view.UtilityViews)
            {
                var utility = new UtilityController(view, _context);
                _utilities.Add(utility);
                _gameManager.Entities.Add(utility);
            }
        }

        private void SetElevators()
        {
            int index = 0;
            foreach (var view in _view.ElevatorViews)
            {
                if (index == 0) _gameManager.Elevator = new ElevatorController(view, _context);
                else GameObject.Destroy(view.gameObject);
                index++;
            }
        }
    }
}
