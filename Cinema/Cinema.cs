﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cinema
{
    public partial class Cinema : Form
    {
        public Cinema()
        {
            //InitializeComponent();
            InitializeComponents();
        }

        private void LoadStartScene()
        {
            // Hide menu
            menu.Width = 0;
            active.Width = 1000;
        }

        private void ShowMenu()
        {
            // Show menu
            menu.Width = 250;
            active.Width = 750;
        }

        // Menu setup
        Panel menu;
        Panel header;
        Panel body;
        Button logout;
        Button halls;
        //
        Panel hallSubmenu;
        Button smallHall;
        Button mediumHall;
        Button largeHall;
        //
        Button movies;
        Button bookings;


        // Active screen setup
        Panel active;
        private void InitializeComponents()
        {
            // Default form settings
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 800);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.KeyPreview = true;

            // Form events
            this.KeyDown += Cinema_KeyDown;


            // Menu settings
            menu = new Panel();
            this.Controls.Add(menu);
            menu.Dock = DockStyle.Left;
            menu.BackColor = Color.Wheat;
            menu.Width = 250;

            body = new Panel();
            menu.Controls.Add(body);
            body.Dock = DockStyle.Top;
            body.BackColor = Color.Wheat;
            body.Height = 500;

            header = new Panel();
            menu.Controls.Add(header);
            header.Dock = DockStyle.Top;
            header.BackColor = Color.Wheat;
            header.Height = 200;

            logout = new Button();
            body.Controls.Add(logout);
            logout.Dock = DockStyle.Bottom;
            logout.BackColor = Color.Wheat;
            logout.ForeColor = Color.Snow;
            logout.FlatStyle = FlatStyle.Flat;
            logout.FlatAppearance.BorderSize = 0;
            logout.Font = new Font("Calibri", 22);
            logout.Text = "Logout";
            logout.Height = 50;
            logout.Click += Logout_Click;

            //
            hallSubmenu = new Panel();
            body.Controls.Add(hallSubmenu);
            hallSubmenu.Dock = DockStyle.Top;
            hallSubmenu.BackColor = Color.Wheat;
            hallSubmenu.Height = 0;

            largeHall = new Button();
            hallSubmenu.Controls.Add(largeHall);
            largeHall.Dock = DockStyle.Top;
            largeHall.BackColor = Color.Wheat;
            largeHall.ForeColor = Color.Snow;
            largeHall.FlatStyle = FlatStyle.Flat;
            largeHall.FlatAppearance.BorderSize = 0;
            largeHall.Font = new Font("Calibri", 22);
            largeHall.Text = "Large";
            largeHall.Height = 50;
            largeHall.TextAlign = ContentAlignment.MiddleLeft;
            largeHall.Padding = new Padding(30, 0, 0, 0);

            mediumHall = new Button();
            hallSubmenu.Controls.Add(mediumHall);
            mediumHall.Dock = DockStyle.Top;
            mediumHall.BackColor = Color.Wheat;
            mediumHall.ForeColor = Color.Snow;
            mediumHall.FlatStyle = FlatStyle.Flat;
            mediumHall.FlatAppearance.BorderSize = 0;
            mediumHall.Font = new Font("Calibri", 22);
            mediumHall.Text = "Medium";
            mediumHall.Height = 50;
            mediumHall.TextAlign = ContentAlignment.MiddleLeft;
            mediumHall.Padding = new Padding(30, 0, 0, 0);

            smallHall = new Button();
            hallSubmenu.Controls.Add(smallHall);
            smallHall.Dock = DockStyle.Top;
            smallHall.BackColor = Color.Wheat;
            smallHall.ForeColor = Color.Snow;
            smallHall.FlatStyle = FlatStyle.Flat;
            smallHall.FlatAppearance.BorderSize = 0;
            smallHall.Font = new Font("Calibri", 22);
            smallHall.Text = "Small";
            smallHall.Height = 50;
            smallHall.TextAlign = ContentAlignment.MiddleLeft;
            smallHall.Padding = new Padding(30, 0, 0, 0);
            //
            halls = new Button();
            body.Controls.Add(halls);
            halls.Dock = DockStyle.Top;
            halls.BackColor = Color.Wheat;
            halls.ForeColor = Color.Snow;
            halls.FlatStyle = FlatStyle.Flat;
            halls.FlatAppearance.BorderSize = 0;
            halls.Font = new Font("Calibri", 22);
            halls.Text = "Halls";
            halls.Height = 50;
            halls.TextAlign = ContentAlignment.MiddleLeft;
            halls.Padding = new Padding(15, 0, 0, 0);
            halls.Click += Halls_Click;

            movies = new Button();
            body.Controls.Add(movies);
            movies.Dock = DockStyle.Top;
            movies.BackColor = Color.Wheat;
            movies.ForeColor = Color.Snow;
            movies.FlatStyle = FlatStyle.Flat;
            movies.FlatAppearance.BorderSize = 0;
            movies.Font = new Font("Calibri", 22);
            movies.Text = "Movies";
            movies.Height = 50;
            movies.TextAlign = ContentAlignment.MiddleLeft;
            movies.Padding = new Padding(15, 0, 0, 0);

            bookings = new Button();
            body.Controls.Add(bookings);
            bookings.Dock = DockStyle.Top;
            bookings.BackColor = Color.Wheat;
            bookings.ForeColor = Color.Snow;
            bookings.FlatStyle = FlatStyle.Flat;
            bookings.FlatAppearance.BorderSize = 0;
            bookings.Font = new Font("Calibri", 22);
            bookings.Text = "Bookings";
            bookings.Height = 50;
            bookings.TextAlign = ContentAlignment.MiddleLeft;
            bookings.Padding = new Padding(15, 0, 0, 0);


            // Active form settings
            active = new Panel();
            this.Controls.Add(active);
            active.Dock = DockStyle.Right;
            active.BackColor = Color.BlanchedAlmond;
            active.Width = 750;
        }

        private void Halls_Click(object sender, EventArgs e)
        {
            if (hallSubmenu.Height == 0)
            {
                hallSubmenu.Height = 150;
            }
            else
            {
                hallSubmenu.Height = 0;
            }
        }

        private void Cinema_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 16)
            {
                ShowMenu();
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            LoadStartScene();
        }
    }
}
