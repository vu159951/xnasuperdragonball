using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagement
{
    class UnitTestMenuScreen : MenuScreen
    {

        public UnitTestMenuScreen() : base("Unit Test Menu")
        {

            MenuEntry gregTests = new MenuEntry("Greg Tests");
            MenuEntry scottTests = new MenuEntry("Scott Tests");
            MenuEntry yinTests = new MenuEntry("Yin Tests");
            MenuEntry jesseTests = new MenuEntry("Jesse Tests");
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            // Hook up menu event handlers.
            gregTests.Selected += GotoGregTests;
            scottTests.Selected += GotoScottTests;
            yinTests.Selected += GotoYinTests;
            jesseTests.Selected += GotoJesseTests;

            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(gregTests);
            MenuEntries.Add(scottTests);
            MenuEntries.Add(yinTests);
            MenuEntries.Add(jesseTests);
            MenuEntries.Add(exitMenuEntry);
       }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void GotoGregTests(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GregTestMenu());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void GotoScottTests(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ScottTestMenu());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void GotoYinTests(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new YinTestMenu());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void GotoJesseTests(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new JesseTestMenu());
        }
        

       /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void exitMenuEntry(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

    }
}
