using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow _window;
        private ViewModel _viewModel;

        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;


        public App()
        {
            Startup += App_Startup;

        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _viewModel = new ViewModel();
            _window = new MainWindow
            {
                DataContext = _viewModel
            };

            _window.Show();

            _viewModel.LoadFile += ViewModel_LoadFile;
            _viewModel.SaveFile += ViewModel_SaveFile;
            _viewModel.GameOver += ViewModel_GameOver;
        }

        private void ViewModel_GameOver(object sender, EventArgs e)
        {
            if (MessageBox.Show("Try again?", "Game Over", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _viewModel.SetTableAsync();
            }
            else
                _window.Close();
        }

        private void ViewModel_SaveFile(object sender, EventArgs e)
        {
            if (_saveFileDialog == null) _saveFileDialog = new SaveFileDialog();

            _saveFileDialog.Filter = "Tetris|*.txt";
            try
            {
                if (_saveFileDialog.ShowDialog() == true) _viewModel.Save(_saveFileDialog.FileName);

            }catch(Exception)
            {
                MessageBox.Show("Try selecting other file", "Error", MessageBoxButton.OK);
            }
        }

        private void ViewModel_LoadFile(object sender, EventArgs e)
        {
            if (_openFileDialog == null) _openFileDialog = new OpenFileDialog();

            _openFileDialog.Filter = "Tetris|*.txt";

            try 
            { 
                if (_openFileDialog.ShowDialog() == true) _viewModel.LoadAsync(_openFileDialog.FileName);

            }catch(Exception)
            {
                MessageBox.Show("Try selecting other file", "Error", MessageBoxButton.OK);
            }
}

    }
}
