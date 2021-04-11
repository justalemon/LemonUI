using System;
#if FIVEM
using CitizenFX.Core.Native;
#elif RPH
using Rage.Native;
#elif (SHVDN2 || SHVDN3)
using GTA.Native;
#endif

namespace LemonUI.Scaleform
{
    /// <summary>
    /// Represents a generic Scaleform object.
    /// </summary>
    public abstract class BaseScaleform : IScaleform
    {
        #region Private Fields

        /// <summary>
        /// The ID of the scaleform.
        /// </summary>
#if FIVEM
        protected CitizenFX.Core.Scaleform scaleform = null;
#elif (SHVDN2 || SHVDN3)
        protected GTA.Scaleform scaleform = null;
#endif

        #endregion

        #region Public Properties

        /// <summary>
        /// If the Scaleform should be visible or not.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Scaleform class with the specified Scaleform object name.
        /// </summary>
        /// <param name="sc">The Scalform object.</param>
        public BaseScaleform(string sc)
        {
#if FIVEM
            scaleform = new CitizenFX.Core.Scaleform(sc);
#elif (SHVDN2 || SHVDN3)
            scaleform = new GTA.Scaleform(sc);
#endif
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Updates the parameters of the Scaleform.
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Draws the scaleform full screen.
        /// </summary>
        public void DrawFullScreen()
        {
            if (!Visible)
            {
                return;
            }
            Update();
#if FIVEM
            API.DrawScaleformMovieFullscreen(scaleform.Handle, 255, 255, 255, 255, 0);
#elif RPH
            NativeFunction.CallByName<int>("DRAW_SCALEFORM_MOVIE_FULLSCREEN", scaleform.Handle, 255, 255, 255, 255, 0);
#elif (SHVDN2 || SHVDN3)
            Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleform.Handle, 255, 255, 255, 255, 0);
#endif
        }
        /// <summary>
        /// Draws the scaleform full screen.
        /// </summary>
        public void Draw() => DrawFullScreen();
        /// <summary>
        /// Draws the scaleform full screen.
        /// </summary>
        public void Process() => DrawFullScreen();
        /// <summary>
        /// Marks the scaleform as no longer needed.
        /// </summary>
        public void Dispose()
        {
            int id = scaleform.Handle;
#if FIVEM
            API.SetScaleformMovieAsNoLongerNeeded(ref id);
#elif RPH
            NativeFunction.CallByName<int>("SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED", new NativeArgument(id));
#elif (SHVDN2 || SHVDN3)
            Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, new OutputArgument(id));
#endif
        }

        #endregion
    }
}
