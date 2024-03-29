﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment.Application;

namespace AutoUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version myVersion;
                myVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                version.Text = string.Concat("Testing:", myVersion);
            }
            else
            {
                version.Text = "No version in debug mode";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch(DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application can't be downloaded at this time.\n\n Please check your network connection or try again later. Error:" + dde.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch(InvalidDeploymentException ide)
                {
                    MessageBox.Show("Can't check for a new version of the application. The ClickOnce deployment is carrupt. Please redeploy the application and try again. Error:" + ide.Message,"Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                catch(InvalidOperationException ioe)
                {
                    MessageBox.Show("This application can't be updated. It's likely not a ClicOnce application. Error: " + ioe.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (info.UpdateAvailable)
                {
                    if (MessageBox.Show("A newer version is available. Would you like to update it now?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            ad.Update();
                            Application.Restart();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are running the latest version.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
