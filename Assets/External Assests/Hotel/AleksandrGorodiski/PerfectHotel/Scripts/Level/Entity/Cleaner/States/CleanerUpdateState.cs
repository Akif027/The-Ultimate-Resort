using Game.Level.Player.PlayerState;

namespace Game.Level.Cleaner.CleanerState
{
    public class CleanerUpdateState : CleanerState
    {
        public override void Initialize()
        {
            _cleaner.View.HudView.ReadyToUpdate();

            CheckIsUpdatable(_gameManager.Model.LoadProgress());

            _cleaner.ItemBuyUpdate.PLAYER_ON_ITEM += PlayerOnItem;
            _gameManager.PROGRESS_CHANGED += CheckIsUpdatable;
        }

        public override void Dispose()
        {
            _cleaner.ItemBuyUpdate.PLAYER_ON_ITEM -= PlayerOnItem;
            _gameManager.PROGRESS_CHANGED -= CheckIsUpdatable;
        }

        private void CheckIsUpdatable(int progress)
        {
            bool isUpdatable = _cleaner.IsUpdatable(_cleaner.Model.IsMaxed, progress, _cleaner.Model.TargetUpdateProgress);
            _cleaner.View.HudView.gameObject.SetActive(isUpdatable);

            if (isUpdatable)
                _gameManager.AddItem(_cleaner.ItemBuyUpdate);
            else
                _gameManager.RemoveItem(_cleaner.ItemBuyUpdate);
        }

        void PlayerOnItem(ItemController item)
        {
            if (_gameManager.Model.Cash <= 0 || !_cleaner.IsUpdatable(_cleaner.Model.IsMaxed, _gameManager.Model.LoadProgress(), _cleaner.Model.TargetUpdateProgress)) return;

            int amount = 1;

            _gameManager.Model.Cash -= amount;
            _gameManager.Model.Save();
            _gameManager.Model.SetChanged();

            _cleaner.Model.PriceNextLvl -= amount;

            _gameManager.FireFlyToRemoveCash(_cleaner.View.HudView.transform.position);

            if (_cleaner.Model.PriceNextLvl <= 0)
            {
                _cleaner.Model.Lvl++;
                _gameManager.Model.SavePlaceLvl(_cleaner.Model.ID, _cleaner.Model.Lvl);
                _cleaner.Model.UpdateModel();

                CheckIsUpdatable(_gameManager.Model.LoadProgress());

                _cleaner.UnitView.PlayUnitParticles();

                _gameView.CameraController.SetTarget(_cleaner.UnitView.transform);
                _gameView.CameraController.ZoomIn(true);

                _gameManager.Player.SwitchToState(new PlayerPauseState());
            }

            _cleaner.Model.SetChanged();
        }
    }
}

