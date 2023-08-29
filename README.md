 # Tetris
The Popular Tetris game's implementation. Shapes are randomly generated and they fall down as the time elapses.
User can move and rotate them on the grid, which has a width that can be selected from three sizes. The user has to stack the shapes in order to completely fill bottom rows, not living any gap.
In that case the completed bottom row is blown away, making every block of the shapes placed on top of it to drop down one unit. The player loses if fails to constantly decrease the blocks and they overflow.
## Franeworks
Written in C Sharp and the front-end is made in three different subframeworks of **.NET**.
- a light-weight framework such as **WinForms** for the sake of simplicity
- **WPF** for the potential opportunity to improve the graphics later, since it enables the use of the computational power of GPU
- and **Xamarin** for application cross-platform compatibility
##Architecture
### Persistence
Differenciated layer in every architectural design.
It provides an interface for saving the current game, listing previous saves, and loading them.
Implemented by a class in each architecture which stores the data in text file.
In Xamarin, since the program has limited access to storage only through Environment, the implementation has to call that, which makes it slightly different to the others.
### MV & MVVM
The use of **View-Model** works quite trivially. Model classes defines the data structures (shapes by coordinates) with the businesss logic given in the methods so the data can be manipulated properly by user input or rule defined actions.
The view defines how the data is rendered. Note that code repetition might occour across the projects because the model was improved by the time but has not been made backward compatible, so that a abstract one could be used by each project.
Added the extra **ViewModel** layer after the regular practise for WPF applications to keep layers as decoupled as possible.
## Event driven implementation
The projects use the listener pattern based native C# Sharp event utilities.
