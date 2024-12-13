using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        private Storyboard leftStoryboard;
        private Storyboard rightStoryboard;
        private bool isLeftAnimationPlaying;
        private bool isRightAnimationPlaying;

        public Equipment()
        {
            InitializeComponent();
            InitializeStoryboards();
        }

        private void InitializeStoryboards()
        {
            try
            {
                leftStoryboard = (Storyboard)FindResource("Left1");
                rightStoryboard = (Storyboard)FindResource("Right1");

                isLeftAnimationPlaying = false;
                isRightAnimationPlaying = false;

                // Add completed event handler for the left storyboard
                leftStoryboard.Completed += (s, e) =>
                {
                    isLeftAnimationPlaying = false;
                    ButtonLeft.IsEnabled = true;  // Enable ButtonLeft after animation completes
                };

                // Add completed event handler for the right storyboard
                rightStoryboard.Completed += (s, e) =>
                {
                    isRightAnimationPlaying = false;
                    ButtonRight.IsEnabled = true;  // Enable ButtonRight after animation completes
                };
            }
            catch (ResourceReferenceKeyNotFoundException ex)
            {
                MessageBox.Show($"Resource not found: {ex.Message}");
            }
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (!isLeftAnimationPlaying)
            {
                // Start the left animation and disable ButtonLeft
                leftStoryboard.Begin(this, true);
                isLeftAnimationPlaying = true;
                ButtonLeft.IsEnabled = false;  // Disable ButtonLeft while the animation plays
            }
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (!isRightAnimationPlaying)
            {
                // Start the right animation and disable ButtonRight
                rightStoryboard.Begin(this, true);
                isRightAnimationPlaying = true;
                ButtonRight.IsEnabled = false;  // Disable ButtonRight while the animation plays
            }
        }
    }
}
