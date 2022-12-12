using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tetris_Android
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IDataAccess _dataAccess;
        private GameModel _gameModel;
        private ViewModel _viewModel;
        private GamePage _gamePage;
        private NavigationPage _mainPage;

        private SettingsPage _settingsPage;

        private IStore _store;
        private BrowserModel _browserModel;
        private BrowserViewModel _browserViewModel;
        private LoadGamePage _loadGamePage;
        private SaveGamePage _saveGamePage;

        private bool isStarted;
        private bool addShape;

        public App()
        {
            _dataAccess = DependencyService.Get<IDataAccess>();
            _gameModel = new GameModel(_dataAccess);
            _gameModel.GameLost += GameModel_GameLost;
            _gameModel.Stuck += GameModel_Stuck;

            _viewModel = new ViewModel(_gameModel);
            _gameModel.Changed += _viewModel.Model_Drawn;

            _viewModel.SetGame += ViewModel_SetGame;
            _viewModel.ToggleGame += new EventHandler(ViewModel_ToggleGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _gamePage = new GamePage();
            _gamePage.BindingContext = _viewModel;

            _settingsPage = new SettingsPage();
            _settingsPage.BindingContext = _viewModel;

            _store = DependencyService.Get<IStore>();
            _browserModel = new BrowserModel(_store);
            _browserViewModel = new BrowserViewModel(_browserModel);
            _browserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(BrowserViewModel_GameLoading);
            _browserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(BrowserViewModel_GameSaving);

            _loadGamePage = new LoadGamePage();
            _loadGamePage.BindingContext = _browserViewModel;

            _saveGamePage = new SaveGamePage();
            _saveGamePage.BindingContext = _browserViewModel;

            _mainPage = new NavigationPage(_gamePage);
            MainPage = _mainPage;

            isStarted = false;

            RefreshViewModel();

        }


        protected override void OnStart()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (isStarted)
                {
                    if (addShape)
                    {
                        addShape = false;
                        _gameModel.AddShape();
                    }
                    else Navigate('S');
                }

                return true;
            });
        }

        protected override void OnSleep()
        {
            // elmentjük a jelenleg folyó játékot //free mem?
            try
            {
                Task.Run(async () => await _gameModel.SaveAsync("SuspendedGame"));
            }
            catch { }
        }

        protected override void OnResume()
        {
            // betöltjük a felfüggesztett játékot, amennyiben van
            try
            {
                Task.Run(async () =>
                {
                    await _gameModel.LoadAsync("SuspendedGame");
                    RefreshViewModel();
                });
            }
            catch { }

        }

        #region ViewModel event handlers

        private async void ViewModel_SetGame(object sender, SingleIntEventArg e)
        {
            if (await MainPage.DisplayAlert("Tetris", "Table will be cleared", "OK", "Cancel"))
            {
                await _gameModel.LoadAsync(String.Empty, e.MyInt);
                RefreshViewModel();
            }

            _viewModel.TableSize = _gameModel.Size.X; 
        }
        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void ViewModel_ToggleGame(object sender, EventArgs e)
        {
            isStarted = !isStarted;
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            await _browserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_loadGamePage); // átnavigálunk a lapra
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            await _browserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_saveGamePage); // átnavigálunk a lapra
        }

        private async void ViewModel_ExitGame(object sender, EventArgs e)
        {
            await _mainPage.PushAsync(_settingsPage); // átnavigálunk a beállítások lapra
        }


        /// <summary>
        /// Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void BrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _gameModel.LoadAsync(e.Name);
                RefreshViewModel();
            }
            catch
            {
                await MainPage.DisplayAlert("Tetris", "Cannot be loaded", "OK");
            }
        }

        /// <summary>
        /// Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void BrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            try
            {
                // elmentjük a játékot
                await _gameModel.SaveAsync(e.Name);
            }
            catch 
            {
                await MainPage.DisplayAlert("Tetris", "Error occurred", "OK");
            }

            await MainPage.DisplayAlert("Tetris", "Saved successfully", "OK");
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private async void GameModel_GameLost(object sender, EventArgs e)
        {
            isStarted = false;
            _viewModel.Status = String.Empty;

            if (await MainPage.DisplayAlert("Tetris", "Game lost", "Retry", "Cancel"))
            {
                await _gameModel.LoadAsync(String.Empty);
                RefreshViewModel();
            }
        }
        private void GameModel_Stuck(object sender, EventArgs e)
        {
            addShape = true;
        }

        #endregion

        #region Private methods
        private void Navigate(Char key)
        {
            switch (key)
            {
                case 'W': _gameModel.Shapes[^1].Rotate(); break;
                case 'S': _gameModel.Shapes[^1].Move(Direction.DOWN); break;
                case 'A': _gameModel.Shapes[^1].Move(Direction.LEFT); break;
                case 'D': _gameModel.Shapes[^1].Move(Direction.RIGHT); break;
                default:
                    break;
            }
        }

        private void RefreshViewModel()
        {
            _viewModel.RefreshCollection();
            addShape = true;
        }
        #endregion

    }
}
