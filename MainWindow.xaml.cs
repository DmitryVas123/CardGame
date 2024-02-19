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

namespace FinalEkz
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumberOfPairs = 8; // Number of pairs of images
        private const int MaxAttempts = 20; // Maximum attempts allowed

        private List<Button> cards = new List<Button>();
        private Button openedCard1 = null;
        private Button openedCard2 = null;
        private int attempts = 0;
        private int matchedPairs = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Create pairs of images
            List<string> imagePaths = new List<string>
            {
                "Images/img1.jpg", "Images/img1.jpg",
                "Images/img2.jpg", "Images/img2.jpg",
                "Images/img3.jpg", "Images/img3.jpg",
                "Images/img4.jpg", "Images/img4.jpg",
                "Images/img5.jpg", "Images/img5.jpg",
                "Images/img6.jpg", "Images/img6.jpg",
                "Images/img7.jpg", "Images/img7.jpg",
                "Images/img8.jpg", "Images/img8.jpg"
            };

            // Shuffle image paths
            Random rng = new Random();
            imagePaths = imagePaths.OrderBy(x => rng.Next()).ToList();

            // Create cards
            foreach (string imagePath in imagePaths)
            {
                Button card = new Button();
                card.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/side.jpg", UriKind.Relative)),
                    Stretch = Stretch.Fill,
                };
                card.Tag = imagePath;
                card.Click += Card_Click;
                cards.Add(card);
                CardsGrid.Children.Add(card);
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            Button clickedCard = (Button)sender;
            Image image = (Image)clickedCard.Content;

            // Check if card is already opened or if all pairs are found
            if ((openedCard1 != null && clickedCard == openedCard1) || openedCard2 != null || matchedPairs == NumberOfPairs)
                return;

            // Open card
            Dispatcher.Invoke(() =>
            {
                image.Source = new BitmapImage(new Uri(clickedCard.Tag.ToString(), UriKind.Relative));
            });

            if (openedCard1 == null)
            {
                openedCard1 = clickedCard;
            }
            else
            {
                openedCard2 = clickedCard;
                CheckForPairMatch();
            }
        }

        private void CheckForPairMatch()
        {
            attempts++;

            if (openedCard1.Tag.ToString() == openedCard2.Tag.ToString())
            {
                matchedPairs++;
                openedCard1.Visibility = Visibility.Hidden;
                openedCard2.Visibility = Visibility.Hidden;
            }
            else
            {
                // No match, close cards after a short delay
                System.Threading.Thread.Sleep(1000);
                if (openedCard1 != null)
                {
                    openedCard1.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("Images/side.jpg", UriKind.Relative)),
                        Stretch = Stretch.Fill,
                    };
                }
                if (openedCard2 != null)
                {
                    openedCard2.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("Images/side.jpg", UriKind.Relative)),
                        Stretch = Stretch.Fill,
                    };
                }
            }

            openedCard1 = null;
            openedCard2 = null;
            UpdateAttemptsLabel();

            // Check if all pairs are matched
            if (matchedPairs == NumberOfPairs)
            {
                MessageBox.Show("Congratulations! You've won!");
            }
            // Check if maximum attempts reached
            else
            {
                // No match, close cards after a short delay
                System.Threading.Thread.Sleep(1000);
                if (openedCard1 != null)
                {
                    openedCard1.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("Images/side.jpg", UriKind.Relative)),
                        Stretch = Stretch.Fill,
                    };
                }
                if (openedCard2 != null)
                {
                    openedCard2.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("Images/side.jpg", UriKind.Relative)),
                        Stretch = Stretch.Fill,
                    };
                }
            }

            openedCard1 = null;
            openedCard2 = null;
            UpdateAttemptsLabel();

            // Check if all pairs are matched
            if (matchedPairs == NumberOfPairs)
            {
                MessageBox.Show("Congratulations! You've won!");
            }
            // Check if maximum attempts reached
            else if (attempts >= MaxAttempts)
            {
                MessageBox.Show("Sorry! You've reached the maximum number of attempts. Game Over!");
            }
        }

        private void UpdateAttemptsLabel()
        {
            AttemptsCounter.Content = $"Attempts: {attempts}/{MaxAttempts}";
        }
    }
}
