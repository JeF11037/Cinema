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

        public Cinema()
        {
            try
            {
                string a = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\MyDB.mdf";
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
        }

        private void ShowMenu()
        {
            menu.Width = 250;
            active.Width = 750;
        }

        private void DrawSeats()
        {
            Panel[,] seatLayout = new Panel[,] { };
            PictureBox[,] seats = new PictureBox[,] { };
            Label[,] seatNumber = new Label[,] { };
            switch (layout.Name)
            {
                case "small":
                    seatLayout = new Panel[(int)Math.Sqrt(16), (int)Math.Sqrt(16)];
                    seats = new PictureBox[(int)Math.Sqrt(16), (int)Math.Sqrt(16)];
                    seatNumber = new Label[(int)Math.Sqrt(16), (int)Math.Sqrt(16)];
                    break;
                case "medium":
                    seatLayout = new Panel[(int)Math.Sqrt(36), (int)Math.Sqrt(36)];
                    seats = new PictureBox[(int)Math.Sqrt(36), (int)Math.Sqrt(36)];
                    seatNumber = new Label[(int)Math.Sqrt(36), (int)Math.Sqrt(36)];
                    break;
                case "large":
                    seatLayout = new Panel[(int)Math.Sqrt(64), (int)Math.Sqrt(64)];
                    seats = new PictureBox[(int)Math.Sqrt(64), (int)Math.Sqrt(64)];
                    seatNumber = new Label[(int)Math.Sqrt(64), (int)Math.Sqrt(64)];
                    break;
            }
            int numberMultiplier = 150;
            int rowMultiplier = 100;
            int tick = 0;
            for (int row = 0; row < Math.Sqrt(seats.Length); row++)
            {
                for (int number = 0; number < Math.Sqrt(seats.Length); number++)
                {
                    tick++;
                    seatLayout[row, number] = new Panel
                    {
                        Width = rowMultiplier,
                        Height = numberMultiplier,
                        Location = new Point(10 + number * numberMultiplier, 10 + row * rowMultiplier)
                    };
                    seats[row, number] = new PictureBox
                    {
                        Name = row.ToString() + number.ToString(),
                        Dock = DockStyle.Top,
                        Image = Image.FromFile(dir + @"\img\green.png"),
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    seatNumber[row, number] = new Label
                    {
                        Dock = DockStyle.Top,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Text = tick.ToString()
                    };
                    seatLayout[row, number].Controls.Add(seatNumber[row, number]);
                    seatLayout[row, number].Controls.Add(seats[row, number]);
                    layout.Controls.Add(seatLayout[row, number]);
                }
            }
        }

        private void TableBoxValueChanged()
        {
            try
            {
                adminTable.DataSource = data.GetTable(tablesBox.SelectedValue.ToString());
            }
            catch (Exception)
            {
                data.CloseConnection();
                MessageBox.Show("No tables matched");
            }
        }

        private string previousIdentification;
        private void ShowElements(string identification)
        {
            ShowElements();
            switch (identification.ToLower())
            {
                case "large":
                    layout = new Panel
                    {
                        Name = "large"
                    };
                    active.Controls.Add(layout);
                    layout.Dock = DockStyle.Top;
                    layout.BackColor = Color.Wheat;
                    layout.Height = 650;
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
                    TableBoxValueChanged();
                    // Var
                    previousIdentification = "adminContainer";
                    break;
            }
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


        private Panel active;
        private Panel adminContainer;
        private Panel layout;
        private DataGridView adminTable;
        private ComboBox tablesBox;
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

            movies = new Button();
            body.Controls.Add(movies);
            movies.Dock = DockStyle.Top;
            movies.BackColor = Color.Wheat;
            movies.ForeColor = Color.Black;
            movies.FlatStyle = FlatStyle.Flat;
            movies.FlatAppearance.BorderSize = 0;
            movies.Font = new Font("Calibri", 22);
            movies.Text = "Movies";
            movies.Height = 50;
            movies.TextAlign = ContentAlignment.MiddleLeft;
            movies.Padding = new Padding(15, 0, 0, 0);
            movies.Click += Movies_Click;

            bookings = new Button();
            body.Controls.Add(bookings);
            bookings.Dock = DockStyle.Top;
            bookings.BackColor = Color.Wheat;
            bookings.ForeColor = Color.Black;
            bookings.FlatStyle = FlatStyle.Flat;
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

        private void TablesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            TableBoxValueChanged();
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
            DrawSeats();
        }

        private void MediumHall_Click(object sender, EventArgs e)
        {
            ShowElements("medium");
            DrawSeats();
        }

        private void SmallHall_Click(object sender, EventArgs e)
        {
            ShowElements("small");
            DrawSeats();
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
            ShowElements();
            LoadStartScene();
        }
    }
}
