#if FIVEM
using CitizenFX.Core.UI;
using Font = CitizenFX.Core.UI.Font;
#elif SHVDN2
using Font = GTA.Font;
#elif SHVDN3
using GTA.UI;
using Font = GTA.UI.Font;
#endif
using LemonUI.Elements;
using LemonUI.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace LemonUI.Menus
{
    /// <summary>
    /// Menu that looks like the ones used by Rockstar.
    /// </summary>
    public class NativeMenu : INativeMenu, IProcessable
    {
        #region Private Fields

        /// <summary>
        /// If the menu is visible or not.
        /// </summary>
        private bool visible = false;
        /// <summary>
        /// The index of the selected item in the menu.
        /// </summary>
        private int index = -1;
        /// <summary>
        /// The banner of the menu.
        /// </summary>
        private IDrawable bannerImage = null;
        /// <summary>
        /// The text of the menu.
        /// </summary>
        private ScaledText bannerText = null;
        /// <summary>
        /// The background of the drawable text.
        /// </summary>
        private IDrawable subtitleImage = null;
        /// <summary>
        /// The text of the subtitle.
        /// </summary>
        private ScaledText subtitleText = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// If the menu is visible on the screen.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (visible)
                {
                    Shown?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Close();
                }
            }
        }
        /// <summary>
        /// The title of the menu.
        /// </summary>
        public string Title
        {
            get
            {
                if (bannerText == null)
                {
                    throw new NullReferenceException("The Text parameter of this Menu is null.");
                }
                return bannerText.Text;
            }
            set
            {
                bannerText.Text = value;
            }
        }
        /// <summary>
        /// The banner shown at the top of the menu.
        /// </summary>
        public IDrawable Banner
        {
            get => bannerImage;
            set => bannerImage = value;
        }
        /// <summary>
        /// The text object on top of the banner.
        /// </summary>
        public ScaledText Text
        {
            get => bannerText;
            set => bannerText = value;
        }
        /// <summary>
        /// The current index of the menu.
        /// </summary>
        public int Index
        {
            get
            {
                if (Items == null || Items.Count == 0)
                {
                    return -1;
                }
                return index;
            }
            set
            {
                if (Items == null || Items.Count == 0)
                {
                    throw new InvalidOperationException("There are no items in this menu.");
                }
                else if (Items.Count >= value)
                {
                    throw new InvalidOperationException($"The index is over {Items.Count - 1}");
                }
                index = value;
            }
        }
        /// <summary>
        /// The subtitle of the menu.
        /// </summary>
        public string Subtitle
        {
            get => subtitleText.Text;
            set => subtitleText.Text = value;
        }
        /// <summary>
        /// The items that this menu contain.
        /// </summary>
        public List<IItem> Items { get; }
        /// <summary>
        /// Text shown when there are no items in the menu.
        /// </summary>
        public string NoItemsText { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event triggered when the menu is opened and shown to the user.
        /// </summary>
        public event EventHandler Shown;
        /// <summary>
        /// Event triggered when the menu starts closing.
        /// </summary>
        public event CancelEventHandler Closing;
        /// <summary>
        /// Event triggered when the menu finishes closing.
        /// </summary>
        public event EventHandler Closed;
        /// <summary>
        /// Event triggered when an item is selected.
        /// </summary>
        public event EventHandler Selected;
        /// <summary>
        /// Event triggered when the selected index has been changed.
        /// </summary>
        public event EventHandler ItemChanged;
        /// <summary>
        /// Event triggered when the index has been changed.
        /// </summary>
        public event EventHandler IndexChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new menu with the default banner texture.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subtitle">Subtitle of this menu.</param>
        public NativeMenu(string title, string subtitle) : this(title, new ScaledTexture(new PointF(0, 0), new SizeF(863, 215), "commonmenu", "interaction_bgd"), subtitle)
        {
        }

        /// <summary>
        /// Creates a new menu with the specified banner texture.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="banner">The drawable to use as the banner.</param>
        /// <param name="subtitle">Subtitle of this menu.</param>
        public NativeMenu(string title, IDrawable banner, string subtitle)
        {
            bannerImage = banner;
            bannerText = new ScaledText(new PointF(209, 22), title, 1.02f, Font.HouseScript)
            {
                Color = Color.FromArgb(255, 255, 255),
                Alignment = Alignment.Center
            };
            subtitleImage = new ScaledRectangle(new PointF(0, 126), new SizeF(863, 38))
            {
                Color = Color.FromArgb(0, 0, 0)
            };
            subtitleText = new ScaledText(new PointF(6, 111), subtitle, 0.35f, Font.ChaletLondon)
            {
                Color = Color.FromArgb(255, 255, 255)
            };
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Draws the menu and handles the controls.
        /// </summary>
        public void Process()
        {
            // If the menu is not visible, return
            if (!visible)
            {
                return;
            }

            // Otherwise, draw all other things
            bannerImage?.Process();
            bannerText?.Process();
            subtitleImage?.Process();
            subtitleText?.Process();
        }
        /// <summary>
        /// Calculates the positions and sizes of the elements.
        /// </summary>
        public void Recalculate()
        {
            bannerImage?.Recalculate();
            bannerText?.Recalculate();
            subtitleImage?.Recalculate();
            subtitleText?.Recalculate();
        }
        /// <summary>
        /// Closes the menu.
        /// </summary>
        public void Close()
        {
            // Create a new set of event arguments
            CancelEventArgs args = new CancelEventArgs();
            // And trigger the event
            Closed?.Invoke(this, args);

            // If we need to cancel the closure of the menu, return
            if (args.Cancel)
            {
                return;
            }
            // Otherwise, hide the menu
            visible = false;
        }

        #endregion
    }
}