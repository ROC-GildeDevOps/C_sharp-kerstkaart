using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace C_sharp_kerstkaart
{
    public partial class Form1 : Form
    {
        // Zeg maar of we quality willen of niet. Wordt nog aangevraagd bij de gebruiker.
        private bool quality = false;

        // Aanroepen van de WindowsMediaPlayer class
        private WindowsMediaPlayer mediaPlayer; // Class om muziek af te spelen

        private Timer snowTimer; // Class om de sneeuwvlokken te updaten op tijd
        private List<Snowflake> snowflakes; // Lijst om sneeuwvlokken in op te slaan
        private Random rand;  // Class om random nummers te genereren

        public Form1()
        {
            // Vraag nog ff aan gebruiken of ie quality wil of niet.
            DialogResult dialogResult = MessageBox.Show("Wilt u qualiteit?", "Optioneel : Kwaliteitsnormen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                quality = true;
            }

            // ikke graag ff terug horen van functie wa de naam is.
            string userName = PromptForName();

            InitializeComponent();

            // Kijken of de string nie leeg is.
            if (!string.IsNullOrWhiteSpace(userName))
            {
                // Naampie in textie zetten.
                lbl_header.Text += "\n" + userName + "!";
            }

            // DoubleBuffered is een property van de form class, die we op true zetten om flickering te voorkomen.
            this.DoubleBuffered = quality;

            // Ja checken of the mp3 wel wordt gevonden.
            try
            {
                // Jaja muziek aan forceren!
                mediaPlayer = new WindowsMediaPlayer();
                mediaPlayer.URL = quality + ".mp3";
                mediaPlayer.settings.setMode("loop", true);
                mediaPlayer.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Er is een probleem opgetreden bij het laden van de muziek: {ex.Message}",
                    "Fout bij Muziek Laden",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            // Snowflake animatie
            rand = new Random(); // Random number generator
            snowflakes = new List<Snowflake>(); // List to store snowflakes

            // Timer to update snowflake positions
            snowTimer = new Timer();
            snowTimer.Interval = 30; 
            snowTimer.Tick += SnowTimer_Tick;
            snowTimer.Start();
        }

        private string PromptForName()
        {
            // Ja zeg maar variablen
            string name = null;
            bool isValid = false;

            // Herhalen tot dat de naam geldig is.
            while (!isValid)
            {
                // Zieke extensie om een input pop-up te maken.
                name = Microsoft.VisualBasic.Interaction.InputBox(
                    quality
                    ? "Welkom bij de kerstkaart generator! Voer de naam van ontvanger in om verder te gaan."
                    : "Naar wie gaat deze `prachtige` kaart? :",
                    "Kerstkaart opmaak",
                    ""
                );

                // Als result leeg is, dan exit.
                if (string.IsNullOrWhiteSpace(name))
                {
                    // Weet zeker maatje?
                    DialogResult dialogResult = MessageBox.Show(
                        quality
                        ? "Weet je zeker dat je deze kaart wilt versturen zonder naam?"
                        : "Wil echt niemand naam op kaart zetten???",
                        "No Name Provided",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );


                    // Alles behalve ja is afsluiten.
                    if (dialogResult != DialogResult.Yes)
                    {
                        // Geen application.exit hier, omdat ie anders nog een error pop-up geeft.
                        Environment.Exit(0);
                    }
                }

                // Is hij niet leeg en gwn letters dan boem jaja.
                if (name.All(char.IsLetter))
                {
                    isValid = true;
                }
                else
                {
                    MessageBox.Show(
                        quality
                        ? "Oeps, er lijkt iets mis te zijn met de naam die je hebt ingevoerd. Probeer het opnieuw!"
                        : "Jow bami! De meeste mensen hebben zeg maar alleen letters in hun naam. Misschien even het alfabet leren!",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }

            return name;
        }

        // Timer afgelopen dan deze functie weer starten.
        private void SnowTimer_Tick(object sender, EventArgs e)
        {
            // snowflakes is een lijst van sneeuwvlokken, die we elke keer willen updaten.
            // rand.Next is hetzelfde als math.random in JS`, omdat het gedefineerd is in de Random class.
            // this === form

            // 20% op where we dropping.
            if (rand.Next(0, 10) < 2)
            {
                snowflakes.Add(new Snowflake(rand.Next(0, this.Width), 0, rand.Next(2, 5), rand.Next(5, 10)));
            }

            // Beweeg alle sneeuwvlokken naar beneden
            foreach (var snowflake in snowflakes)
            {
                snowflake.Y += snowflake.Speed;
                if (snowflake.Y > this.Height)
                {
                    snowflake.Y = 0;
                    snowflake.X = rand.Next(0, this.Width);
                }
            }

            // Maak het scherm leeg.
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var snowflake in snowflakes)
            {
                // Teken een mooie witte vlek op de positie van de sneeuwvlok.
                e.Graphics.FillEllipse(Brushes.White, snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size);
            }
        }

        // Deze class representeert een individuele sneeuwvlok. Ja kan dit nie echt dommer verwoorden ofz.
        private class Snowflake
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Speed { get; set; }
            public int Size { get; set; }

            public Snowflake(int x, int y, int speed, int size)
            {
                X = x;
                Y = y;
                Speed = speed;
                Size = size;
            }
        }


    }
}
