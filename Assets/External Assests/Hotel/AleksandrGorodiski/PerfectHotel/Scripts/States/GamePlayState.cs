using System;
using System.Collections.Generic;
using Game.Config;
using Game.Level;
using Game.Managers;
using Game.UI;
using Injection;
using UnityEngine;
using Game.Level.Player.PlayerState;
using Game.UI.Hud;
using Game.Level.Reception;
using Game.Modules.CashModule;
using Game.Level.Entity;
using Game.Level.Inventory;
using Game.Modules.UISpritesModule;
using Game.Modules.UINotificationModule;

namespace Game.States
{
    public sealed class GamePlayState : GameState
    {
        [Inject] private Injector _injector;
        [Inject] private Context _context;
        [Inject] private GameView _gameView;
        [Inject] private HudManager _hudManager;
        [Inject] private GameConfig _config;
        [Inject] private LevelView _levelView;

        private GameManager _gameManager;
        private PlayerView _playerView;

        private readonly List<Module> _levelModules;

        public GamePlayState()
        {
            _levelModules = new List<Module>();
        }

        public override void Initialize()
        {
            _gameManager = new GameManager(_config);
            _context.Install(_gameManager);

            _playerView = GameObject.Instantiate(_gameView.Player, Vector3.zero, Quaternion.identity).GetComponent<PlayerView>();
            _gameManager.Player = new PlayerController(_playerView, _context);
            _gameManager.Player.View.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            _gameView.CameraController.SetTarget(_gameManager.Player.Transform);
            _gameView.CameraController.enabled = true;

            InitLevelModules();

            _hudManager.ShowAdditional<GamePlayHudMediator>();

            _gameView.Joystick.enabled = true;

            _gameManager.Player.SwitchToState(new PlayerIdleState());
        }

        public override void Dispose()
        {
            _gameView.CameraController.enabled = false;

            DisposeLevelModules();

            _hudManager.HideAdditional<GamePlayHudMediator>();

            _gameView.Joystick.enabled = false;

            _gameManager.Player.Dispose();
            _gameManager.Dispose();

            _context.Uninstall(_gameManager);
        }

        private void InitLevelModules()
        {
            AddModule<ReceptionModule, ReceptionModuleView>(_levelView);
            AddModule<EntityModule, EntityModuleView>(_levelView);
            AddModule<CashModule, CashModuleView>(_gameView);
            AddModule<InventoryModule, InventoryModuleView>(_gameView);
            AddModule<UISpritesModule, UISpritesModuleView>(_gameView);
            AddModule<UINotificationModule, UINotificationModuleView>(_gameView);
        }

        private void AddModule<T, T1>(Component component) where T : Module
        {
            var view = component.GetComponent<T1>();
            var result = (T)Activator.CreateInstance(typeof(T), new object[] { view });
            _levelModules.Add(result);
            _injector.Inject(result);
            result.Initialize();
        }

        private void DisposeLevelModules()
        {
            foreach (var levelModule in _levelModules)
            {
                levelModule.Dispose();
            }
            _levelModules.Clear();
        }
    }
}