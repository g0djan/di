using System;
using System.Drawing;
using System.Windows.Forms;
using FractalPainting.Infrastructure.UiActions;

namespace FractalPainting.App
{
	public class MainForm : Form
	{
        public MainForm(IUiAction[] actions, PictureBoxImageHolder pictureBox, SettingsManager settingsManager, MenuStrip mainMenu)
        {
			var imageSettings = settingsManager.Load().ImageSettings;
			ClientSize = new Size(imageSettings.Width, imageSettings.Height);
            
			mainMenu.Items.AddRange(actions.ToMenuItems());
			Controls.Add(mainMenu);
            
			pictureBox.RecreateImage(imageSettings);
			pictureBox.Dock = DockStyle.Fill;
			Controls.Add(pictureBox);
		}

	    protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Text = "Fractal Painter";
		}
	}
}