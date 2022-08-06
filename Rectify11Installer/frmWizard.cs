﻿using Microsoft.Win32;
using Rectify11Installer.Core;
using Rectify11Installer.Pages;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Rectify11Installer
{
    public partial class frmWizard : Form
    {
        WelcomePage WelcomePage = new WelcomePage();
        EulaPage EulaPage = new EulaPage();
        InstallOptnsPage InstallOptnsPage = new InstallOptnsPage();
        ThemeChoicePage ThemeChoicePage = new ThemeChoicePage();
        public frmWizard()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            if (Theme.IsUsingDarkMode)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
            wlcmPage.Controls.Add(WelcomePage);
            eulPage.Controls.Add(EulaPage);
            installPage.Controls.Add(InstallOptnsPage);
            themePage.Controls.Add(ThemeChoicePage);
            WelcomePage.InstallButton.Click += InstallButton_Click;
            WelcomePage.UninstallButton.Click += UninstallButton_Click;
            nextButton.Click += NextButton_Click;
            navBackButton.Click += BackButton_Click;
            cancelButton.Click += CancelButton_Click;
            versionLabel.Text = versionLabel.Text + ProductVersion;
            Navigate(WelcomePage);
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
            /*
            MessageBox.Show(Properties.Settings.Default.IsInstalled.ToString());
            Properties.Settings.Default.IsInstalled = true;
            Properties.Settings.Default.Save();
            */
            /*
            Patches list = PatchesParser.GetAll();
            PatchesPatch[] ok = list.Items;
            foreach (PatchesPatch patch in ok)
            {
                MessageBox.Show(patch.Package, patch.HardlinkTarget);
            }
            */
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            switch (e.Category)
            {
                case UserPreferenceCategory.General:
                    {
                        Theme.InitTheme();
                        if (Theme.IsUsingDarkMode)
                        {
                            BackColor = Color.Black;
                            ForeColor = Color.White;
                        }
                        else
                        {
                            BackColor = Color.White;
                            ForeColor = Color.Black;
                        }
                    }
                    break;
            }
        }
        #region Navigation
        private void Navigate(WizardPage page)
        {
            headerText.Text = page.WizardHeader;
            sideImage.BackgroundImage = page.SideImage;
            if (page == WelcomePage)
            {
                navPane.SelectedTab = wlcmPage;
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel2.Visible = false;
                sideImage.BackgroundImage = page.SideImage;
            }
            else if (page == EulaPage)
            {
                headerText.Text = page.WizardHeader;
                sideImage.BackgroundImage = page.SideImage;
                tableLayoutPanel1.Visible = true;
                tableLayoutPanel2.Visible = true;
                nextButton.ButtonText = Strings.Rectify11.buttonAgree;
                navPane.SelectedTab = eulPage;
            }
            else if (page == InstallOptnsPage)
            {
                headerText.Text = page.WizardHeader;
                sideImage.BackgroundImage = page.SideImage;
                nextButton.ButtonText = Strings.Rectify11.buttonNext;
                navPane.SelectedTab = installPage;
            }
            else if (page == ThemeChoicePage)
            {
                navPane.SelectedTab = themePage;
            }
        }
        #endregion
        #region Private Methods
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (navPane.SelectedTab == eulPage)
                Navigate(InstallOptnsPage);
            else if (navPane.SelectedTab == installPage)
                Navigate(ThemeChoicePage);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if (navPane.SelectedTab == eulPage)
                Navigate(WelcomePage);
            else if (navPane.SelectedTab == installPage)
                Navigate(EulaPage);
            else if (navPane.SelectedTab == themePage)
                Navigate(InstallOptnsPage);
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (Helper.CheckIfUpdatesPending())
            {
                Navigate(EulaPage);
            }

        }

        private void UninstallButton_Click(object sender, EventArgs e)
        {
            if (Helper.CheckIfUpdatesPending())
            {
                //Navigate(UninstallConfirmPage);
            }
        }
        #endregion
    }
}
