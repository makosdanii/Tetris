using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris_WPF
{
    class ShapeField : ViewModelBase
    {
        private int top;
        private int left;
        private string color;
        private double prev;

        public ShapeField()
        {

        }

        public int Top
        {
            get { return top; }
            set
            {
                top = value;
                OnPropertyChanged();
            }
        }
        public int Left
        {
            get { return left; }
            set
            {
                left = value;
                OnPropertyChanged();
            }
        }

        public string Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }

        public double Prev
        {
            get { return prev; }
            set
            {
                prev = value;
                OnPropertyChanged();
            }
        }
    }
}