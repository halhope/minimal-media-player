using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;



namespace Beaver_Receiver
{
    public partial class App : Application
    {
        private static Mutex singleInstanceMutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "BeaverReceiverAppMutex";
            bool isNewInstance;

            singleInstanceMutex = new Mutex(true, mutexName, out isNewInstance);

            if (!isNewInstance)
            {
                MessageBox.Show("Another instance of Beaver Receiver is already running.", "Instance Already Running", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            singleInstanceMutex?.ReleaseMutex();
            singleInstanceMutex?.Dispose();
            base.OnExit(e);
        }
    }
    public partial class MainWindow : Window
    {
        private readonly MediaElement mediaElement;
        private bool isPlaying = false;
        private readonly string defaultStreamUrl = "https://ice4.somafm.com/groovesalad-128-aac";
        private bool isDragging = false; // Track dragging state
        private Point dragStartPoint;   // Store the initial click position
        private readonly Button playbackButton;
        private readonly DispatcherTimer pressTimer;  // Timer to track button press duration
        private bool isButtonHeld = false; // Flag to check if button is held
        private readonly Dictionary<string, string> allStreams = new Dictionary<string, string>
        {
            { "Beat Blender", "https://ice4.somafm.com/beatblender-128-aac" },
            { "Black Rock", "https://ice4.somafm.com/brfm-128-aac" },
            { "Boot Liquor", "https://ice4.somafm.com/bootliquor-128-aac" },
            { "Bossa Beyond", "https://ice4.somafm.com/bossa-128-aac" },
            { "Chillits", "https://ice4.somafm.com/chillits-128-aac" },
            { "Cliqhop", "https://ice4.somafm.com/cliqhop-128-aac" },
            { "Covers", "https://ice4.somafm.com/covers-128-aac" },
            { "Deep Space One", "https://ice4.somafm.com/deepspaceone-128-aac" },
            { "DEF CON Radio", "https://ice4.somafm.com/defcon-128-aac" },
            { "Digitalis", "https://ice4.somafm.com/digitalis-128-aac" },
            { "Doomed", "https://ice4.somafm.com/doomed-128-aac" },
            { "Drone Zone", "https://ice4.somafm.com/dronezone-128-aac" },
            { "Dub Step Beyond", "https://ice4.somafm.com/dubstep-128-aac" },
            { "Fluid", "https://ice4.somafm.com/fluid-128-aac" },
            { "Folk Forward", "https://ice4.somafm.com/folkfwd-128-aac" },
            { "Groove Salad", "https://ice4.somafm.com/groovesalad-128-aac" },
            { "Groove Salad Classic", "https://ice4.somafm.com/gsclassic-128-aac" },
            { "Heavyweight Reggae", "https://ice4.somafm.com/reggae-128-aac" },
            { "Illinois Street Lounge", "https://ice4.somafm.com/illstreet-128-aac" },
            { "Indie Pop Rocks!", "https://ice4.somafm.com/indiepop-128-aac" },
            { "Left Coast 70s", "https://ice4.somafm.com/seventies-128-aac" },
            { "Lush", "https://ice4.somafm.com/lush-128-aac" },
            { "Metal Detector", "https://ice4.somafm.com/metal-128-aac" },
            { "Mission Critical", "https://ice4.somafm.com/missioncontrol-128-aac" },
            { "n5MD", "https://ice4.somafm.com/n5md-128-aac" },
            { "PopTron", "https://ice4.somafm.com/poptron-128-aac" },
            { "Secret Agent", "https://ice4.somafm.com/secretagent-128-aac" },
            { "Seven Inch Soul", "https://ice4.somafm.com/7soul-128-aac" },
            { "SF 10-33", "https://ice4.somafm.com/sf1033-128-aac" },
            { "SF in SF", "https://ice4.somafm.com/sfinsf-128-aac" },
            { "SF Police Scanner", "https://ice4.somafm.com/scanner-128-aac" },
            { "SomaFM Live", "https://ice4.somafm.com/live-128-aac" },
            { "SomaFM Specials", "https://ice4.somafm.com/specials-128-aac" },
            { "Sonic Universe", "https://ice4.somafm.com/sonicuniverse-128-aac" },
            { "Space Station Soma", "https://ice4.somafm.com/spacestation-128-aac" },
            { "Suburbs of Goa", "https://ice4.somafm.com/suburbsofgoa-128-aac" },
            { "Synphaera Radio", "https://ice4.somafm.com/synphaera-128-aac" },
            { "The Dark Zone", "https://ice4.somafm.com/darkzone-128-aac" },
            { "The In-Sound", "https://ice4.somafm.com/insound-128-aac" },
            { "The Trip", "https://ice4.somafm.com/thetrip-128-aac" },
            { "ThistleRadio", "https://ice4.somafm.com/thistle-128-aac" },
            { "Tiki Time", "https://ice4.somafm.com/tikitime-128-aac" },
            { "Underground 80s", "https://ice4.somafm.com/u80s-128-aac" },
            { "Vaporwaves", "https://ice4.somafm.com/vaporwaves-128-aac" }
        };
        

        public MainWindow()
        {
            InitializeComponent();

            // Retrieve saved settings for default stream URL and button position
            string savedStreamUrl = Properties.Settings.Default.LastStreamUrl;
            if (!string.IsNullOrEmpty(savedStreamUrl))
            {
                defaultStreamUrl = "https://ice4.somafm.com/groovesalad-128-aac";
            }

            // Load saved position
            double savedLeft = Properties.Settings.Default.LastWindowLeft;
            double savedTop = Properties.Settings.Default.LastWindowTop;

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
            this.Topmost = true;
            this.Width = 100;
            this.Height = 40;
            if (savedLeft >= 0 && savedTop >= 0)
            {
                this.Left = savedLeft;
                this.Top = savedTop;
            }
            else
            {
                // Apply default position if no saved value
                this.Left = SystemParameters.WorkArea.Width - this.Width - 10; // Align to top-right            
            }

            // Create a TextBlock for play icon
            TextBlock playTextBlock = new TextBlock
            {
                Text = "🪵", // Play icon (wood emoji)
                FontFamily = new FontFamily("Segoe UI Emoji"), // Use font that supports emojis
                FontSize = 16,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Create the playback button with transparent background
            playbackButton = new Button
            {
                Width = 80,
                Height = 40,
                Background = Brushes.Transparent, // Transparent background
                BorderBrush = Brushes.Transparent, // No border
                FontWeight = FontWeights.Bold,
                ToolTip = "Not playing" // Initial tooltip
            };

            // Create a grid to hold the ellipse and text
            Grid buttonGrid = new Grid();

            // Create an ellipse for the button content
            Ellipse ellipse = new Ellipse
            {
                Width = 35,
                Height = 35,
                Fill = Brushes.Red // Initial color (red for stopped)
            };

            // Add the ellipse to the grid
            buttonGrid.Children.Add(ellipse);

            // Create a text block to display the playback status
            TextBlock textBlock = new TextBlock
            {
                Text = "🪵", // Initial text (wood emoji)
                FontFamily = new FontFamily("Segoe UI Emoji"), // Use font that supports emojis
                HorizontalAlignment = HorizontalAlignment.Center, // Center horizontally
                VerticalAlignment = VerticalAlignment.Center // Center vertically
            };

            // Add the text block to the grid
            buttonGrid.Children.Add(textBlock);

            // Set the content of the button to be the grid
            playbackButton.Content = buttonGrid;
            // Mouse events for dragging
            playbackButton.PreviewMouseLeftButtonDown += StartDrag;
            playbackButton.PreviewMouseLeftButtonUp += EndDrag;
            playbackButton.PreviewMouseMove += DragButton;

            // Mouse event for click to toggle playback
            playbackButton.Click += TogglePlayback;

            // Mouse wheel event for volume control
            playbackButton.MouseWheel += OnMouseWheelScroll;

            // Attach the MouseEnter event for tooltip updates
            playbackButton.MouseEnter += UpdateToolTip;

            // Set up the press timer
            pressTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // 1 second threshold
            };
            pressTimer.Tick += OnPressTimerTick;

            // Create a grid to hold the button
            var grid = new Grid();
            grid.Children.Add(playbackButton);

            // Initialize MediaElement and add it to the grid (but keep it invisible)
            mediaElement = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop,
                Visibility = Visibility.Collapsed // Keep the MediaElement invisible
            };
            grid.Children.Add(mediaElement);

            // Attach right-click context menu to the grid
            AttachContextMenu(grid);

            // Set the grid as the window content
            this.Content = grid;
        }



        private void PlayOrToggleStream()
        {
            // Set the stream and start playback using the default stream
            SetStream(Properties.Settings.Default.LastStreamUrl);
        }

        private void SetDefaultStream(string url)
        {
            // Store the selected stream URL in settings
            Properties.Settings.Default.LastStreamUrl = url;
            Properties.Settings.Default.Save();
        }

        private void SetStream(string url)
        {
            SetDefaultStream(url); // Automatically save as default

            if (isPlaying)
            {
                mediaElement.Stop();
            }

            mediaElement.Source = new Uri(url);
            mediaElement.MediaOpened += (sender, args) =>
            {
                mediaElement.Play();
                isPlaying = true;
            };
        }

        private void OnMouseWheelScroll(object sender, MouseWheelEventArgs e)
        {
            double volumeChange = e.Delta > 0 ? 0.1 : -0.1; // Scroll up to increase, down to decrease
            mediaElement.Volume = Math.Clamp(mediaElement.Volume + volumeChange, 0.0, 1.0); // Clamp volume between 0.0 and 1.0
        }

        private void UpdateToolTip(object sender, MouseEventArgs e)
        {
            string streamName = GetStreamName(Properties.Settings.Default.LastStreamUrl);
            playbackButton.ToolTip = isPlaying
                ? streamName != null
                    ? $"Playing: {streamName}"
                    : $"Playing: {Properties.Settings.Default.LastStreamUrl}"
                : "Not playing";
        }

        private string GetStreamName(string url)
        {
            // Iterate through the dictionary to find the matching URL
            foreach (var stream in allStreams)
            {
                if (stream.Value == url) // If the URL matches
                {
                    return stream.Key; // Return the name of the stream
                }
            }
            return null; // Return null if no match is found
        }

        private void AttachContextMenu(FrameworkElement element)
        {
            element.MouseRightButtonUp += (sender, e) =>
            {
                if (!isDragging)
                {
                    var contextMenu = new ContextMenu();

                    // Add Select Stream menu item with sub-menu for all streams
                    var selectStreamMenuItem = new MenuItem { Header = "Select Stream" };
                    foreach (var stream in allStreams)
                    {
                        var streamItem = new MenuItem { Header = stream.Key };
                        streamItem.Click += (s, args) => SetStream(stream.Value);
                        selectStreamMenuItem.Items.Add(streamItem);
                    }
                    contextMenu.Items.Add(selectStreamMenuItem);

                    // Add custom URL entry option
                    var customUrlItem = new MenuItem { Header = "Enter Custom URL" };
                    customUrlItem.Click += (s, args) => ShowCustomUrlDialog();
                    contextMenu.Items.Add(customUrlItem);

                    // Open the dynamically created context menu
                    contextMenu.IsOpen = true;
                }
            };
        }

        private void ShowCustomUrlDialog()
        {
            // Create the dialog window
            Window customUrlDialog = new Window
            {
                Title = "Enter Custom URL",
                Width = 400,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            // Create a stack panel for layout
            StackPanel stackPanel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            // Add instructions
            TextBlock instructions = new TextBlock
            {
                Text = "Enter a custom stream URL",
                Margin = new Thickness(0, 0, 0, 10),
                TextWrapping = TextWrapping.Wrap
            };
            stackPanel.Children.Add(instructions);

            // Add a TextBox for URL input
            TextBox urlTextBox = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 10),
                Width = 360
            };
            stackPanel.Children.Add(urlTextBox);

            // Add OK and Cancel buttons
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button okButton = new Button
            {
                Content = "OK",
                Width = 75,
                Margin = new Thickness(5, 0, 0, 0)
            };
            okButton.Click += (sender, e) =>
            {
                string enteredUrl = urlTextBox.Text.Trim();
                if (Uri.TryCreate(enteredUrl, UriKind.Absolute, out Uri uriResult) &&
                    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    SetDefaultStream(enteredUrl); // Save custom URL as default

                    customUrlDialog.Close();
                    if (isPlaying)
                    {
                        mediaElement.Stop();
                        mediaElement.Source = new Uri(defaultStreamUrl);
                        mediaElement.Play();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid URL. Please enter a valid HTTP or HTTPS URL.", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };
            buttonPanel.Children.Add(okButton);

            Button cancelButton = new Button
            {
                Content = "Cancel",
                Width = 75
            };
            cancelButton.Click += (sender, e) =>
            {
                customUrlDialog.Close();
            };
            buttonPanel.Children.Add(cancelButton);

            stackPanel.Children.Add(buttonPanel);

            // Set the dialog content and show it
            customUrlDialog.Content = stackPanel;
            customUrlDialog.ShowDialog();
        }

        private void TogglePlayback(object sender, RoutedEventArgs e)
        {
            if (!isButtonHeld)
            {
                var button = sender as Button;
                var grid = button.Content as Grid;
                var textBlock = grid.Children.OfType<TextBlock>().FirstOrDefault();

                if (isPlaying)
                {
                    mediaElement.Stop();
                    textBlock.Text = "🪵"; // Change text to wood emoji when stopped
                    ((Ellipse)grid.Children[0]).Fill = Brushes.BurlyWood; // Change color to red when stopped
                    isPlaying = false;
                }
                else
                {
                    mediaElement.Source = new Uri(defaultStreamUrl);
                    mediaElement.Play();
                    textBlock.Text = "💧"; // Change text to water emoji when playing
                    ((Ellipse)grid.Children[0]).Fill = Brushes.CadetBlue; // Change color to green when playing
                    isPlaying = true;
                }
            }
        }

        private void StartDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                dragStartPoint = e.GetPosition(this);
                this.Cursor = Cursors.Hand;

                pressTimer.Start();
                isButtonHeld = false;

                Debug.WriteLine("Drag started. pressTimer started.");
            }
        }


        private void EndDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                pressTimer.Stop();
                this.Cursor = Cursors.Arrow;

                // Always save window position when dragging ends, regardless of button hold or not
                Properties.Settings.Default.LastWindowLeft = this.Left;
                Properties.Settings.Default.LastWindowTop = this.Top;
                Properties.Settings.Default.Save();
                Debug.WriteLine($"Saved Window Position: Left={this.Left}, Top={this.Top}");

                isDragging = false; // Reset dragging flag
            }
        }


        private void DragButton(object sender, MouseEventArgs e)
        {
            if (isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                var currentPoint = e.GetPosition(this);
                var offsetX = currentPoint.X - dragStartPoint.X;
                var offsetY = currentPoint.Y - dragStartPoint.Y;

                this.Left += offsetX;
                this.Top += offsetY;

                if (Math.Abs(offsetX) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(offsetY) > SystemParameters.MinimumVerticalDragDistance)
                {
                    isDragging = true;
                }
            }
        }

        private void OnPressTimerTick(object sender, EventArgs e)
        {
            isButtonHeld = true;
            pressTimer.Stop();
        }

    }
}