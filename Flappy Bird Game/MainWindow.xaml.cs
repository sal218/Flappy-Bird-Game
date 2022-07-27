using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Threading namespace for the timer
using System.Windows.Threading;

namespace Flappy_Bird_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        double score;
        int gravity = 8; //natural motion is downwards
        bool gameover; //either true or false
        Rect flappyBirdHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {

            txtScore.Content = "Score: " + score;

            //Updates the position of flappy bird, allowing us to know where they are
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width, flappyBird.Height);

            //This controls the direction flappy bird is headed. If the value is minus, it pushes flappy bird up
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);


            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 455)
            {
                EndGame();
            }


            foreach (var x in MyCanvas.Children.OfType<Image>()) 
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                {
                    //pipe speed
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if(score >= 5)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - 6);
                    }
                    
                    //If pipe goes beyond canvases left most position, then set it to return on the right side
                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);
                        
                        //Reason for .5 is since there are two pipes that need to be passed to equate to 1 incrementation of score
                        score += .5;
                    }

                    //Determines the exact pipe in question as it goes through the loop
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x),x.Width,x.Height);

                    //If flappy bird intersects with the pipe, end the game
                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }

                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);

                    }

                }

            }

        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            //If space bar is pressed
            if (e.Key == Key.Space)
            {
                //Rotate flappy bird 20 degrees up from its center
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                
                //Moves flappy bird up 
                gravity = -8;
            }

            //If R key press and given that the game is indeed over, restart the game
            if (e.Key == Key.R && gameover == true)
            {
                StartGame();

            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            //Rotate flappy bird 5 degrees downwards from its center
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);

            //When keys are released, gravity is set back to default 
            gravity = 8;


        }

        //Considered initial conditions
        private void StartGame()
        {
            //Keeps canvas element in focus when game starts
            MyCanvas.Focus();

            int temp = 300;

            score = 0;

            gameover = false;
            Canvas.SetTop(flappyBird, 190);

            //Any element that is an image, we are going to loop through them
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                //Checks if any of the elements (x) match the tag specified in quotes
                if ((string)x.Tag == "obs1")
                {
                    Canvas.SetLeft(x, 500);
                }

                if ((string)x.Tag == "obs2")
                {
                    Canvas.SetLeft(x, 800);
                }

                if ((string)x.Tag == "obs3")
                {
                    Canvas.SetLeft(x, 1100);
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    //when it finds the second cloud, it will move it further down
                    temp = 800;
                }
            }

            //Starts the game timer
            gameTimer.Start();

        }

        private void EndGame()
        {
            gameTimer.Stop();
            gameover = true;

            txtScore.Content += " Game Over !! Press R to try again";
        }
    }
}
