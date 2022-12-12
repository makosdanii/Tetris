using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Tetris_Android
{
    public class ShapeField : ViewModelBase
    {
        private int top;
        private int left;
        private string color;
        private int prev;
        private Rectangle rect;

        public ShapeField()
        {
            Rect = new Rectangle(40.0, 300.0, 50.0, 50.0);
        }

        public Rectangle Rect 
        {
            get { return rect; }
            set { rect = value; OnPropertyChanged("Rect"); } 
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

        public int Prev
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