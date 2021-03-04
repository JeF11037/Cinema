using System;
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
        private readonly Random rnd = new Random();
        private readonly DataManager data;
        private readonly string dir = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        private readonly List<PictureBox> capturedChairs = new List<PictureBox>();

        public Cinema()
        {
            try
            {
                data = new DataManager(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+ dir + @"\MyDB.mdf;Integrated Security=True");
            }
            catch (Exception)
            {
                MessageBox.Show("Try to add DataBase or change link address", "DataBase error");
                this.Close();
            }
            //InitializeComponent();
            InitializeComponents();
        }

        private void InsertBasicRows(string table)
        {
            try
            {
                bar.Value = 0;
                switch (table)
                {
                    case "hall":
                        int hallsCount = 10;
                        string[] hallsTypes = new string[]
                        {
                        "small",
                        "medium",
                        "large"
                        };
                        for (int tick = 1; tick < hallsCount + 1; tick++)
                        {
                            data.InsertData(hallsTypes[rnd.Next(0, 3)], tick, false);
                            bar.Value += 1000 / hallsCount;
                        }
                        bar.Value = 1000;
                        break;
                    case "seat":
                        List<int> halls = data.GetIds("hall");
                        foreach (var el in halls)
                        {
                            int seats = 0;
                            switch (data.GetSpecificRow("hall", 1, el))
                            {
                                case "small":
                                    seats = 16;
                                    break;
                                case "medium":
                                    seats = 36;
                                    break;
                                case "large":
                                    seats = 64;
                                    break;
                            }
                            for (int row = 1; row < Math.Sqrt(seats) + 1; row++)
                            {
                                for (int number = 1; number < Math.Sqrt(seats) + 1; number++)
                                {
                                    data.InsertData(el, row, number, false);
                                }
                                bar.Value += 1;
                            }
                        }
                        bar.Value = 1000;
                        break;
                    case "movie":
                        int moviesCount = 10;
                        string[] categories = new string[]
                        {
                        "Historical",
                        "Comedy",
                        "Fantasy",
                        "Military",
                        "Action",
                        "Horror",
                        "Romance",
                        "Thriller",
                        "Drama",
                        "Mystery"
                        };
                        string[] firstNames = new string[]
                        {
                        "Medic",
                        "Recruits",
                        "Star",
                        "Doom",
                        "Joy",
                        "Caution",
                        "Afraid",
                        "Inception",
                        "Alerted",
                        "Humans",
                        "Aliens"
                        };
                        string[] languages = new string[]
                        {
                        "English",
                        "Russian",
                        "Estonian"
                        };
                        string[] secondNames = new string[]
                        {
                        "Of",
                        "Of",
                        "In",
                        "On",
                        "Without",
                        "Within",
                        "In",
                        "On",
                        };
                        string[] thirdNames = new string[]
                        {
                        "The ",
                        " "
                        };
                        string[] fourthNames = new string[]
                        {
                        "Time",
                        "Travellers",
                        "Stardust",
                        "Life",
                        "Technology",
                        "Galaxy",
                        "Spies",
                        "End",
                        "Truth"
                        };
                        for (int tick = 0; tick < moviesCount; tick++)
                        {
                            string movieName =
                                firstNames[rnd.Next(0, firstNames.Length)]
                                + " "
                                + secondNames[rnd.Next(0, secondNames.Length)]
                                + " "
                                + thirdNames[rnd.Next(0, thirdNames.Length)]
                                + fourthNames[rnd.Next(0, fourthNames.Length)];
                            data.InsertData(
                                movieName,
                                categories[rnd.Next(0, categories.Length)],
                                languages[rnd.Next(0, languages.Length)],
                                false,
                                rnd.Next(90, 300)
                                );
                            bar.Value += 1000 / moviesCount;
                        }
                        bar.Value = 1000;
                        break;
                    case "showtime":
                        int[] hours = new int[]
                        {
                            12,
                            13,
                            14,
                            15,
                            16,
                            17,
                            18,
                            19,
                            20,
                            21,
                            22,
                            23,
                            24
                        };
                        int[] minutes = new int[]
                        {
                            0,
                            15,
                            30,
                            45
                        };
                        int[] days = new int[]
                        {
                            1,
                            2,
                            3,
                            4,
                            5,
                            6,
                            7,
                            8,
                            9,
                            10
                        };
                        int showtimeCount = 30;
                        for (int tick = 0; tick < showtimeCount; tick++)
                        {
                            DateTime showtimeDate = DateTime.Now.AddDays(days[rnd.Next(0, days.Length)]);
                            showtimeDate.AddHours(hours[rnd.Next(0, hours.Length)]);
                            showtimeDate.AddMinutes(minutes[rnd.Next(0, minutes.Length)]);
                            data.InsertData(
                                data.GetIds("movie")[rnd.Next(0, data.GetIds("movie").Count)],
                                showtimeDate,
                                data.GetIds("hall")[rnd.Next(0, data.GetIds("hall").Count)]);
                            bar.Value += 10;
                        }
                        bar.Value = 1000;
                        break;
                }
                MessageBox.Show("Successfully inserted all basic rows to table : " + table);
                ShowElements("control");
                bar.Value = 0;
            }
            catch (Exception e)
            {
                bar.Value = 0;
                MessageBox.Show(e.Message, "Stopped because of... ");
                ShowElements("control");
                data.CloseConnection();
            }
        }

        private void RemoveRows(string table)
        {
            try
            {
                bar.Value = 0;
                data.ClearData(table);
                bar.Value += 1000;
                MessageBox.Show("Successfully deleted all basic rows of table : " + table);
                ShowElements("control");
                bar.Value = 0;
            }
            catch (Exception e)
            {
                bar.Value = 0;
                MessageBox.Show(e.Message, "Stopped because of... ");
                ShowElements("control");
                data.CloseConnection();
            }
        }

        /*
        private void RemoveRows(string table, int id)
        {
            try
            {
                bar.Value = 0;
                data.ClearData(table, id);
                bar.Value += 1000;
                MessageBox.Show("Successfully deleted all basic rows of table : " + table);
                ShowElements("control");
                bar.Value = 0;
            }
            catch (Exception e)
            {
                bar.Value = 0;
                MessageBox.Show(e.Message, "Stopped because of... ");
                ShowElements("control");
                data.CloseConnection();
            }
        }
        */
        private void CreateTables()
        {
            int tick = 1;
            try
            {
                bar.Value = 0;
                data.CreateTable("hall");
                tick++;
                bar.Value += 250;
                data.CreateTable("movie");
                tick++;
                bar.Value += 250;
                data.CreateTable("seat");
                tick++;
                bar.Value += 250;
                data.CreateTable("showtime");
                tick++;
                bar.Value += 250;
                data.CreateTable("ticket");
                MessageBox.Show("Successfully created all tables");
                ShowElements("control");
                bar.Value = 0;
            }
            catch (Exception e)
            {
                bar.Value = 0;
                MessageBox.Show(e.Message, "Stopped at " + tick);
                ShowElements("control");
                data.CloseConnection();
            }
        }

        private void DropTables()
        {
            int tick = 1;
            try
            {
                bar.Value = 0;
                data.DropTable("ticket");
                tick++;
                bar.Value += 250;
                data.DropTable("showtime");
                tick++;
                bar.Value += 250;
                data.DropTable("seat");
                tick++;
                bar.Value += 250;
                data.DropTable("movie");
                tick++;
                bar.Value += 250;
                data.DropTable("hall");
                MessageBox.Show("Successfully dropped all tables");
                ShowElements("control");
                bar.Value = 0;
            }
            catch (Exception e)
            {
                bar.Value = 0;
                MessageBox.Show(e.Message, "Stopped at " + tick);
                ShowElements("control");
                data.CloseConnection();
            }
        }

        private void LoadStartScene()
        {
            menu.Width = 0;
            active.Width = 1000;
            ShowElements("cinema");
        }

        private void ShowMenu()
        {
            menu.Width = 250;
            active.Width = 750;
            ShowElements();
        }

        private void DrawSeats(string n)
        {
            int numberMultiplier = 50;
            int rowMultiplier = 50;
            int offset = 30;
            List<int> ids = data.GetSeats(data.GetHallId(n));
            foreach (var el in ids)
            {
                string img;
                if (!Convert.ToBoolean(data.GetSpecificRow("seat", 4, el)))
                {
                    img = dir + @"\img\green.png";
                }
                else
                {
                    img = dir + @"\img\red.png";
                }
                PictureBox seat = new PictureBox
                {
                    Name =
                    data.GetSpecificRow("seat", 0, el) + "-" +
                    data.GetSpecificRow("seat", 2, el) + "-" +
                    data.GetSpecificRow("seat", 3, el) + "-" +
                    data.GetSpecificRow("seat", 4, el),
                    Image = Image.FromFile(img),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(50, 50),
                    Location = new Point
                    (
                        offset + Convert.ToInt32(data.GetSpecificRow("seat", 3, el)) * rowMultiplier,
                        offset + Convert.ToInt32(data.GetSpecificRow("seat", 2, el)) * numberMultiplier
                    )
                };
                seat.Click += Seat_Click;
                layout.Controls.Add(seat);
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            PictureBox seat = (PictureBox)sender;

            if (seat.Image != Image.FromFile(dir + @"\img\red.png"))
            {
                if (capturedChairs.Contains(seat))
                {
                    seat.Image = Image.FromFile(dir + @"\img\green.png");
                    capturedChairs.Add(seat);
                }
                else
                {
                    seat.Image = Image.FromFile(dir + @"\img\yellow.png");
                    capturedChairs.Add(seat);
                }
            }
            else
            {
                MessageBox.Show("Place is taken already");
            }
        }

        private void TableBoxValueChanged(string cause)
        {
            switch (cause)
            {
                case "admin":
                    try
                    {
                        adminTable.DataSource = data.GetTable(tablesBox.SelectedValue.ToString());
                    }
                    catch (Exception)
                    {
                        data.CloseConnection();
                        MessageBox.Show("No tables matched");
                    }
                    break;
                case "hall":
                    DrawSeats(hallBox.SelectedItem.ToString());
                    try
                    {
                        DrawSeats(hallBox.SelectedItem.ToString());
                    }
                    catch (Exception)
                    {
                        data.CloseConnection();
                        MessageBox.Show("No rows matched");
                    }
                    break;
            }
        }

        private double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private void ChangeFilm(int ix)
        {
            filmLabel.Text = 
                "Name : " +
                data.GetFilm(ix).Item1 + 
                "\nCategory : " +
                data.GetFilm(ix).Item2 +
                "\nLanguage : " +
                data.GetFilm(ix).Item3 +
                "\nSubtitles : " +
                data.GetFilm(ix).Item4.ToString() +
                "\nDuration : " +
                data.GetFilm(ix).Item5.ToString()
                ; 
        }

        private string previousIdentification;
        private int carouselIx = 0;
        private void ShowElements(string identification)
        {
            ShowElements();
            switch (identification.ToLower())
            {
                case "choose":
                    layout = new Panel
                    {
                        Name = "choose"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
                    Button cancel = new Button
                    {
                        Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2 - 125,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2 + 450),
                        BackColor = Color.Wheat,
                        ForeColor = Color.Black,
                        Height = 50,
                        Width = 120,
                        Font = new Font("Calibri", 22),
                        Text = "Cancel",
                    };
                    cancel.Click += Cancel_Click;
                    layout.Controls.Add(cancel);
                    Button confirm = new Button
                    {
                        Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2 + 450),
                        BackColor = Color.Wheat,
                        ForeColor = Color.Black,
                        Height = 50,
                        Width = 130,
                        Font = new Font("Calibri", 22),
                        Text = "Confirm",
                    };
                    confirm.Click += Confirm_Click;
                    layout.Controls.Add(confirm);
                    DrawSeats(data.GetHall(data.GetIds("showtime")[carouselIx]));
                    previousIdentification = "choose";
                    break;
                case "cinema":
                    layout = new Panel
                    {
                        Name = "cinema"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
                    Button previous = new Button
                    {
                        Dock = DockStyle.Left,
                        BackColor = Color.Wheat,
                        ForeColor = Color.Black,
                        FlatStyle = FlatStyle.Flat,
                        Height = 50,
                        Font = new Font("Calibri", 22),
                        Text = "<",
                    };
                    previous.Click += Previous_Click;
                    layout.Controls.Add(previous);
                    Button next = new Button
                    {
                        Dock = DockStyle.Right,
                        BackColor = Color.Wheat,
                        ForeColor = Color.Black,
                        FlatStyle = FlatStyle.Flat,
                        Height = 50,
                        Font = new Font("Calibri", 22),
                        Text = ">",
                    };
                    next.Click += Next_Click;
                    layout.Controls.Add(next);
                    film = new PictureBox
                    {
                        Image = Image.FromFile(dir + @"\img\q.png"),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2 - 125,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2 - 125),
                        Size = new Size(250, 250)
                    };
                    filmLabel = new Label
                    {
                        Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2 - 125,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2 + 125),
                        Height = 300,
                        Width = 400,
                        Text = "Disc",
                        Font = new Font("Calibri", 22)

                    };
                    layout.Controls.Add(film);
                    layout.Controls.Add(filmLabel);
                    Button order = new Button
                    {
                        Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2 - 125,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2 + 450),
                        BackColor = Color.Wheat,
                        ForeColor = Color.Black,
                        Height = 50,
                        Width = 100,
                        Font = new Font("Calibri", 22),
                        Text = "Order",
                    };
                    order.Click += Order_Click;
                    layout.Controls.Add(order);
                    ChangeFilm(data.GetMovieByShowtime(data.GetIds("showtime")[carouselIx]));
                    previousIdentification = "cinema";
                    break;
                case "large":
                    layout = new Panel
                    {
                        Name = "large"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
                    hallBox = new ComboBox();
                    layout.Controls.Add(hallBox);
                    hallBox.Dock = DockStyle.Top;
                    hallBox.Height = 20;
                    hallBox.Font = new Font("Calibri", 22);
                    hallBox.BackColor = Color.Wheat;
                    hallBox.ForeColor = Color.Black;
                    hallBox.DataSource = data.GetHalls("large");
                    hallBox.SelectedValueChanged += HallBox_SelectedValueChanged;
                    previousIdentification = "large";
                    break;
                case "medium":
                    layout = new Panel
                    {
                        Name = "medium"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
                    hallBox = new ComboBox();
                    layout.Controls.Add(hallBox);
                    hallBox.Dock = DockStyle.Top;
                    hallBox.Height = 20;
                    hallBox.Font = new Font("Calibri", 22);
                    hallBox.BackColor = Color.Wheat;
                    hallBox.ForeColor = Color.Black;
                    hallBox.DataSource = data.GetHalls("medium");
                    hallBox.SelectedValueChanged += HallBox_SelectedValueChanged;
                    previousIdentification = "medium";
                    break;
                case "small":
                    layout = new Panel
                    {
                        Name = "small"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
                    hallBox = new ComboBox();
                    layout.Controls.Add(hallBox);
                    hallBox.Dock = DockStyle.Top;
                    hallBox.Height = 20;
                    hallBox.Font = new Font("Calibri", 22);
                    hallBox.BackColor = Color.Wheat;
                    hallBox.ForeColor = Color.Black;
                    hallBox.DataSource = data.GetHalls("small");
                    hallBox.SelectedValueChanged += HallBox_SelectedValueChanged;
                    previousIdentification = "small";
                    break;
                case "control":
                    // Container
                    adminContainer = new Panel
                    {
                        Name = "adminContainer"
                    };
                    active.Controls.Add(adminContainer);
                    adminContainer.Dock = DockStyle.Top;
                    adminContainer.Height = 650;
                    // Buttons
                    create = new Button();
                    drop = new Button();
                    insert = new Button();
                    remove = new Button();
                    adminContainer.Controls.Add(insert);
                    adminContainer.Controls.Add(create);
                    adminContainer.Controls.Add(remove);
                    adminContainer.Controls.Add(drop);
                    create.Dock = DockStyle.Bottom;
                    create.BackColor = Color.Wheat;
                    create.ForeColor = Color.Black;
                    create.FlatStyle = FlatStyle.Flat;
                    create.FlatAppearance.BorderSize = 2;
                    create.FlatAppearance.BorderColor = Color.Black;
                    create.Height = 50;
                    create.Font = new Font("Calibri", 22);
                    create.Text = "Create tables";
                    create.Click += Create_Click;
                    drop.Dock = DockStyle.Bottom;
                    drop.BackColor = Color.Wheat;
                    drop.ForeColor = Color.Black;
                    drop.FlatStyle = FlatStyle.Flat;
                    drop.FlatAppearance.BorderSize = 2;
                    drop.FlatAppearance.BorderColor = Color.Black;
                    drop.Height = 50;
                    drop.Font = new Font("Calibri", 22);
                    drop.Text = "Drop tables";
                    drop.Click += Drop_Click;
                    insert.Dock = DockStyle.Bottom;
                    insert.BackColor = Color.Wheat;
                    insert.ForeColor = Color.Black;
                    insert.FlatStyle = FlatStyle.Flat;
                    insert.FlatAppearance.BorderSize = 2;
                    insert.FlatAppearance.BorderColor = Color.Black;
                    insert.Height = 50;
                    insert.Font = new Font("Calibri", 22);
                    insert.Text = "Insert basic rows";
                    insert.Click += Insert_Click;
                    remove.Dock = DockStyle.Bottom;
                    remove.BackColor = Color.Wheat;
                    remove.ForeColor = Color.Black;
                    remove.FlatStyle = FlatStyle.Flat;
                    remove.FlatAppearance.BorderSize = 2;
                    remove.FlatAppearance.BorderColor = Color.Black;
                    remove.Height = 50;
                    remove.Font = new Font("Calibri", 22);
                    remove.Text = "Remove all rows";
                    remove.Click += Remove_Click;
                    // Progress bar
                    bar = new ProgressBar();
                    adminContainer.Controls.Add(bar);
                    bar.Dock = DockStyle.Bottom;
                    bar.BackColor = Color.Wheat;
                    bar.Maximum = 1000;
                    bar.Minimum = 0;
                    bar.ForeColor = Color.Black;
                    // DataGridView
                    adminTable = new DataGridView();
                    adminContainer.Controls.Add(adminTable);
                    adminTable.Dock = DockStyle.Top;
                    adminTable.BackgroundColor = Color.Wheat;
                    adminTable.GridColor = Color.BlanchedAlmond;
                    adminTable.RowHeadersVisible = false;
                    adminTable.AllowUserToResizeColumns = false;
                    adminTable.AllowUserToResizeRows = false;
                    adminTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    adminTable.AllowUserToOrderColumns = true;
                    adminTable.RowTemplate.Height = 50;
                    adminTable.Height = 400;
                    adminTable.DefaultCellStyle = new DataGridViewCellStyle() 
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter,
                        BackColor = Color.BlanchedAlmond,
                        Font = new Font("Calibri", 22),
                        ForeColor = Color.Black,
                        SelectionBackColor = Color.Wheat,
                        SelectionForeColor = Color.Black
                    };
                    // Combobox
                    tablesBox = new ComboBox();
                    adminContainer.Controls.Add(tablesBox);
                    tablesBox.Dock = DockStyle.Top;
                    tablesBox.Height = 20;
                    tablesBox.Font = new Font("Calibri", 22);
                    tablesBox.BackColor = Color.Wheat;
                    tablesBox.ForeColor = Color.Black;
                    string[] tables = new string[]
                    {
                        "Hall",
                        "Movie",
                        "Seat",
                        "Showtime",
                        "Ticket"
                    };
                    tablesBox.DataSource = tables;
                    tablesBox.SelectedValueChanged += TablesBox_SelectedValueChanged;
                    // adminTable DataSource bound
                    TableBoxValueChanged("admin");
                    // Var
                    previousIdentification = "adminContainer";
                    break;
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            string places = "";
            foreach (var el in capturedChairs)
            {
                string[] name = el.Name.Split('-');
                data.InsertData(GetRandomNumber(2, 5), data.GetIds("showtime")[carouselIx], Convert.ToInt32(name[0]));
                el.Image = Image.FromFile(dir + @"\img\red.png");
                data.UpdateChair(Convert.ToInt32(name[0]));
                if (!string.IsNullOrEmpty(places))
                {
                    places += "| Row : " + name[1] + ", Number : " + name[2];
                }
                else
                {
                    places += "Row : " + name[1] + ", Number : " + name[2];
                }
            }
            MessageBox.Show("Your Ticket\nPlaces*\n"+places+"\nDate*\n"+data.GetDate(data.GetIds("showtime")[carouselIx])+"\nPrice*\n"+(Math.Round(GetRandomNumber(2, 5) * capturedChairs.Count, 2)).ToString());

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ShowElements("cinema");
        }

        private void ShowElements()
        {
            if (!string.IsNullOrEmpty(previousIdentification))
            {
                active.Controls.Remove(active.Controls.Find(previousIdentification, false)[0]);
                previousIdentification = "";
            }
        }

        private Panel menu;
        private Panel header;
        private Panel body;
        private Button logout;
        private Button control;
        private Button halls;
        private Panel hallSubmenu;

        private Button smallHall;
        private Button mediumHall;
        private Button largeHall;
        
        private Button movies;
        private Button bookings;

        private Label filmLabel;
        private PictureBox film;
        private Panel active;
        private Panel adminContainer;
        private Panel layout;
        private DataGridView adminTable;
        private ComboBox tablesBox;
        private ComboBox hallBox;
        private ProgressBar bar;
        private Button create;
        private Button drop;
        private Button insert;
        private Button remove;
        private void InitializeComponents()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 800);
            this.Icon = new Icon("../../img/logo.ico");
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.KeyPreview = true;

            this.KeyDown += Cinema_KeyDown;

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
            logout.ForeColor = Color.Black;
            logout.FlatStyle = FlatStyle.Flat;
            logout.FlatAppearance.BorderSize = 0;
            logout.Font = new Font("Calibri", 22);
            logout.Text = "Logout";
            logout.Height = 50;
            logout.Click += Logout_Click;

            control = new Button();
            body.Controls.Add(control);
            control.Dock = DockStyle.Top;
            control.BackColor = Color.Wheat;
            control.ForeColor = Color.Black;
            control.FlatStyle = FlatStyle.Flat;
            control.FlatAppearance.BorderSize = 0;
            control.Font = new Font("Calibri", 22);
            control.Text = "Cotrols";
            control.Height = 50;
            control.TextAlign = ContentAlignment.MiddleLeft;
            control.Padding = new Padding(15, 0, 0, 0);
            control.Click += Control_Click;

            hallSubmenu = new Panel();
            body.Controls.Add(hallSubmenu);
            hallSubmenu.Dock = DockStyle.Top;
            hallSubmenu.BackColor = Color.Wheat;
            hallSubmenu.Height = 0;

            largeHall = new Button();
            hallSubmenu.Controls.Add(largeHall);
            largeHall.Dock = DockStyle.Top;
            largeHall.BackColor = Color.Wheat;
            largeHall.ForeColor = Color.Black;
            largeHall.FlatStyle = FlatStyle.Flat;
            largeHall.FlatAppearance.BorderSize = 0;
            largeHall.Font = new Font("Calibri", 22);
            largeHall.Text = "Large";
            largeHall.Height = 50;
            largeHall.TextAlign = ContentAlignment.MiddleLeft;
            largeHall.Padding = new Padding(30, 0, 0, 0);
            largeHall.Click += LargeHall_Click;

            mediumHall = new Button();
            hallSubmenu.Controls.Add(mediumHall);
            mediumHall.Dock = DockStyle.Top;
            mediumHall.BackColor = Color.Wheat;
            mediumHall.ForeColor = Color.Black;
            mediumHall.FlatStyle = FlatStyle.Flat;
            mediumHall.FlatAppearance.BorderSize = 0;
            mediumHall.Font = new Font("Calibri", 22);
            mediumHall.Text = "Medium";
            mediumHall.Height = 50;
            mediumHall.TextAlign = ContentAlignment.MiddleLeft;
            mediumHall.Padding = new Padding(30, 0, 0, 0);
            mediumHall.Click += MediumHall_Click;

            smallHall = new Button();
            hallSubmenu.Controls.Add(smallHall);
            smallHall.Dock = DockStyle.Top;
            smallHall.BackColor = Color.Wheat;
            smallHall.ForeColor = Color.Black;
            smallHall.FlatStyle = FlatStyle.Flat;
            smallHall.FlatAppearance.BorderSize = 0;
            smallHall.Font = new Font("Calibri", 22);
            smallHall.Text = "Small";
            smallHall.Height = 50;
            smallHall.TextAlign = ContentAlignment.MiddleLeft;
            smallHall.Padding = new Padding(30, 0, 0, 0);
            smallHall.Click += SmallHall_Click;
            
            halls = new Button();
            body.Controls.Add(halls);
            halls.Dock = DockStyle.Top;
            halls.BackColor = Color.Wheat;
            halls.ForeColor = Color.Black;
            halls.FlatStyle = FlatStyle.Flat;
            halls.FlatAppearance.BorderSize = 0;
            halls.Font = new Font("Calibri", 22);
            halls.Text = "Halls";
            halls.Height = 50;
            halls.TextAlign = ContentAlignment.MiddleLeft;
            halls.Padding = new Padding(15, 0, 0, 0);
            halls.Click += Halls_Click;

            movies = new Button
            {
                //body.Controls.Add(movies);
                Dock = DockStyle.Top,
                BackColor = Color.Wheat,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            movies.FlatAppearance.BorderSize = 0;
            movies.Font = new Font("Calibri", 22);
            movies.Text = "Movies";
            movies.Height = 50;
            movies.TextAlign = ContentAlignment.MiddleLeft;
            movies.Padding = new Padding(15, 0, 0, 0);
            movies.Click += Movies_Click;

            bookings = new Button
            {
                //body.Controls.Add(bookings);
                Dock = DockStyle.Top,
                BackColor = Color.Wheat,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            bookings.FlatAppearance.BorderSize = 0;
            bookings.Font = new Font("Calibri", 22);
            bookings.Text = "Bookings";
            bookings.Height = 50;
            bookings.TextAlign = ContentAlignment.MiddleLeft;
            bookings.Padding = new Padding(15, 0, 0, 0);
            bookings.Click += Bookings_Click;


            active = new Panel();
            this.Controls.Add(active);
            active.Dock = DockStyle.Right;
            active.BackColor = Color.BlanchedAlmond;
            active.Width = 750;
            active.Padding = new Padding(50);
        }

        private void Order_Click(object sender, EventArgs e)
        {
            ShowElements("choose");
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (carouselIx < data.GetIds("showtime").Count-1)
            {
                carouselIx++;
            }
            else
            {
                carouselIx = 0;
            }
            ChangeFilm(data.GetMovieByShowtime(data.GetIds("showtime")[carouselIx]));
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            if (carouselIx > 0)
            {
                carouselIx--;
            }
            else
            {
                carouselIx = data.GetIds("showtime").Count-1;
            }
            ChangeFilm(data.GetMovieByShowtime(data.GetIds("showtime")[carouselIx]));
        }

        private void TablesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            TableBoxValueChanged("admin");
        }

        private void HallBox_SelectedValueChanged(object sender, EventArgs e)
        {
            TableBoxValueChanged("hall");
        }

        private void Insert_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (!data.IsTableEmpty("movie"))
            {
                InsertBasicRows("movie");
            }
            else
            {
                MessageBox.Show("Values inserted in Movie already");
            }
            if (!data.IsTableEmpty("hall"))
            {
                InsertBasicRows("hall");
            }
            else
            {
                MessageBox.Show("Values inserted in Hall already");
            }
            if (!data.IsTableEmpty("seat"))
            {
                InsertBasicRows("seat");
            }
            else
            {
                MessageBox.Show("Values inserted in Seat already");
            }
            if (!data.IsTableEmpty("showtime"))
            {
                InsertBasicRows("showtime");
            }
            else
            {
                MessageBox.Show("Values inserted in Showtime already");
            }
            this.Enabled = true;
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            RemoveRows("ticket");
            RemoveRows("showtime");
            RemoveRows("seat");
            RemoveRows("movie");
            RemoveRows("hall");
            this.Enabled = true;
        }

        private void Drop_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            DropTables();
            this.Enabled = true;
        }

        private void Create_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            CreateTables();
            this.Enabled = true;
        }

        private void Control_Click(object sender, EventArgs e)
        {
            ShowElements("control");
        }

        private void Movies_Click(object sender, EventArgs e)
        {
            ShowElements();
        }

        private void Bookings_Click(object sender, EventArgs e)
        {
            ShowElements();
        }

        private void LargeHall_Click(object sender, EventArgs e)
        {
            ShowElements("large");
        }

        private void MediumHall_Click(object sender, EventArgs e)
        {
            ShowElements("medium");
        }

        private void SmallHall_Click(object sender, EventArgs e)
        {
            ShowElements("small");
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
