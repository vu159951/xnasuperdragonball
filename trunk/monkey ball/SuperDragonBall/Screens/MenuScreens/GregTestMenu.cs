using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperDragonBall
{
    class GregTestMenu : MenuScreen
    {

        public GregTestMenu()
            : base("Greg Unit Tests")
        {

            MenuEntry test1 = new MenuEntry("Test Flat Plane");
            MenuEntry test2 = new MenuEntry("Test 2");
      
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            test1.Selected += LaunchTest1;
            test2.Selected += LaunchTest2;
         
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(test1);
            MenuEntries.Add(test2);
       
            MenuEntries.Add(exitMenuEntry);
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void LaunchTest1(object sender, EventArgs e)
        {
           LoadingScreen.Load(ScreenManager, true, new GregTestFlatPlane());
           //LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void LaunchTest2(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GregTest2());
        }


    }
}
