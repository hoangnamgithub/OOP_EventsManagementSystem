using OOP_EventsManagementSystem.Styles;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OOP_EventsManagementSystem.ViewModel;
using static OOP_EventsManagementSystem.ViewModel.LocationVM;
using System.Windows.Media.Imaging;
using OOP_EventsManagementSystem.Model;


namespace OOP_EventsManagementSystem.View
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : UserControl
    {
        private bool _isDragging = false;
        private double _lastX;

        public Location()
        {
            InitializeComponent();
            this.DataContext = new LocationVM();
        }     

        private Border _draggedBorder = null;
        private Point _lastMousePosition;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Bắt đầu kéo
            _draggedBorder = sender as Border;
            if (_draggedBorder != null)
            {
                _lastMousePosition = e.GetPosition(MyScrollViewer);
                _draggedBorder.CaptureMouse();
            }
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                AnimateBorderResize(border);
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            if (border != null)
            {
                ResetBorderSize(border);
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedBorder != null && e.LeftButton == MouseButtonState.Pressed)
            {
                // Tính toán sự thay đổi vị trí chuột
                Point currentPosition = e.GetPosition(MyScrollViewer);
                double deltaX = currentPosition.X - _lastMousePosition.X;

                // Di chuyển ScrollViewer theo chiều ngang
                MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - deltaX);
                _lastMousePosition = currentPosition;

                // Lấy vị trí của Border trong không gian của TargetGrid
                GeneralTransform transform = _draggedBorder.TransformToVisual(TargetGrid);
                Rect borderRect = transform.TransformBounds(new Rect(new Point(0, 0), _draggedBorder.RenderSize));

                // Lấy vị trí của TargetGrid
                Rect gridRect = new Rect(0, 0, TargetGrid.ActualWidth, TargetGrid.ActualHeight);

                // Kiểm tra nếu Border giao với TargetGrid
                if (gridRect.IntersectsWith(borderRect))
                {
                    AnimateBorderResize(_draggedBorder);
                }
                else
                {
                    ResetBorderSize(_draggedBorder);
                }
            }
        }




        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Kết thúc kéo
            if (_draggedBorder != null)
            {
                _draggedBorder.ReleaseMouseCapture();
                _draggedBorder = null;
            }
        }

        private void AnimateBorderResize(Border border)
        {
            if (border.Width != TargetGrid.ActualWidth || border.Height != TargetGrid.ActualHeight)
            {
                // Tạo DoubleAnimation để thay đổi chiều rộng của Border
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = border.Width,
                    To = TargetGrid.ActualWidth, // Resize to the width of the Grid
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                // Tạo DoubleAnimation cho chiều cao
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = border.Height,
                    To = TargetGrid.ActualHeight, // Resize to the height of the Grid
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                // Áp dụng animation cho Border
                border.BeginAnimation(Border.WidthProperty, widthAnimation);
                border.BeginAnimation(Border.HeightProperty, heightAnimation);
            }
        }

        private void ResetBorderSize(Border border)
        {
            if (border.Width != 205 || border.Height != 170)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = border.Width,
                    To = 205, // Kích thước ban đầu
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = border.Height,
                    To = 170, // Kích thước ban đầu
                    Duration = TimeSpan.FromSeconds(0.3),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                border.BeginAnimation(Border.WidthProperty, widthAnimation);
                border.BeginAnimation(Border.HeightProperty, heightAnimation);
            }
        }


        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (var item in ItemsControl.Items)
            {
                var borderItem = item as BorderItem;
                if (borderItem != null)
                {
                    var border = FindBorderFromItem(borderItem);
                    if (border != null)
                    {
                        // Lấy vị trí của Border trong không gian của TargetGrid
                        var borderTransform = border.TransformToAncestor(TargetGrid);
                        var borderRect = borderTransform.TransformBounds(new Rect(0, 0, border.Width, border.Height));

                        // Lấy vị trí của TargetGrid
                        var gridRect = new Rect(0, 0, TargetGrid.ActualWidth, TargetGrid.ActualHeight);

                        // Kiểm tra nếu Border giao với TargetGrid
                        if (gridRect.IntersectsWith(borderRect))
                        {
                            AnimateBorderResize(border);
                        }
                    }
                }
            }

            if (_isDragging)
            {
                // Nếu đang kéo thì di chuyển nội dung
                double delta = e.GetPosition(MyScrollViewer).X - _lastX;  // Tính sự thay đổi vị trí chuột
                MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset - delta);  // Cuộn ngang
                _lastX = e.GetPosition(MyScrollViewer).X;  // Cập nhật vị trí chuột
            }
        }


        private Border FindBorderFromItem(BorderItem borderItem)
        {
            var container = ItemsControl.ItemContainerGenerator.ContainerFromItem(borderItem) as ContentPresenter;
            if (container != null)
            {
                // Tìm Border trong cây visual
                return FindVisualChild<Border>(container);
            }
            return null;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T correctlyTyped)
                {
                    return correctlyTyped;
                }
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private Rect GetBorderRect(BorderItem borderItem)
        {
            // Đây là một phương thức để lấy tọa độ của Border từ mỗi item trong ItemsControl
            return new Rect(0, 0, borderItem.Width, borderItem.Height);
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một VenueViewModel mới để thêm venue
            var newVenue = new VenueViewModel
            {
                VenueId = 0, // ID mặc định
                VenueName = string.Empty,
                Address = string.Empty,
                Cost = 0,
                Capacity = 0
            };

            // Lấy DataContext là LocationVM
            var locationVM = DataContext as LocationVM;

            // Đảm bảo rằng LoadData được truyền vào là một delegate hợp lệ
            Action reloadAction = locationVM != null ? new Action(locationVM.LoadData) : (() => { });

            // Tạo LocationDescription với VenueViewModel và hành động reload
            var locationDescription = new LocationDescription(newVenue, reloadAction);

            locationDescription.ShowDialog();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            // Xác định VenueViewModel được liên kết với nút này
            var button = sender as System.Windows.Controls.Button;
            if (button?.DataContext is VenueViewModel venueToDelete)
            {
                // Hiện thông báo xác nhận
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa địa điểm \"{venueToDelete.VenueName}\" không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    // Nếu người dùng xác nhận, xóa venue
                    DeleteVenue(venueToDelete.VenueId);
                }
            }
        }
        private void DeleteVenue(int venueId)
        {
            using (var context = new EventManagementDbContext())
            {
                // Tìm venue theo VenueId
                var venue = context.Venues.FirstOrDefault(v => v.VenueId == venueId);

                if (venue != null)
                {
                    // Xóa venue
                    context.Venues.Remove(venue);
                    context.SaveChanges();

                    // Hiển thị thông báo
                    MessageBox.Show("Địa điểm đã được xóa thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reload lại dữ liệu trong danh sách
                    var locationVM = this.DataContext as LocationVM;
                    locationVM?.LoadData();
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu không tìm thấy venue
                    MessageBox.Show("Không tìm thấy địa điểm trong cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }

    // Class BorderItem để bind dữ liệu
    public class BorderItem
    {
        public string HeaderText { get; set; }
        public string ImageSource { get; set; }
        public double Width { get; set; } = 205;
        public double Height { get; set; } = 170;
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int Cost { get; set; }
    }
}
