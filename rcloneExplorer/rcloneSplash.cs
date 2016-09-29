using System;
using System.Windows.Forms;

namespace rcloneExplorer
{
  public partial class rcloneSplash : Form
  {
    private Timer splashTimer;
    public rcloneSplash()
    {
      InitializeComponent();
      this.CenterToScreen();
      this.Visible = true;
      initTimer();   
    }
    
    public void initTimer()
    {
      splashTimer = new Timer();
      splashTimer.Tick += new EventHandler(splashTimer_Tick);
      splashTimer.Interval = 1000;
      splashTimer.Start();
    }

    private void splashTimer_Tick(object sender, EventArgs e)
    {
      if (rcloneExplorer.loaded) { this.Close(); }
      if (lblSplashText.Text == " .  .  . ") { lblSplashText.Text = ""; }
      lblSplashText.Text = lblSplashText.Text + " . ";
    }
  }
}
