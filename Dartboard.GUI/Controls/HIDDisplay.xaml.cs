using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Threading;
using DART.Dartboard.Models.HID;

namespace DART.Dartboard.GUI.Controls
{
    /// <summary>
    /// Interaction logic for HIDDisplay.xaml
    /// </summary>
    public partial class HIDDisplay : UserControl
    {
        private JoystickState _joystickState;

        public HIDDisplay()
        {
            InitializeComponent();
        }

        #region Gamepad State

        public static readonly DependencyProperty GamepadStateProperty = DependencyProperty.Register(
            "GamepadState", typeof(GamepadState), typeof(HIDDisplay),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnGamepadStatePropertyChanged));

        [Category("HID")]
        [Bindable(true)]
        public GamepadState GamepadState
        {
            get => (GamepadState) GetValue(GamepadStateProperty);
            set {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        SetValue(GamepadStateProperty, value);
                    });
                }
                catch (TaskCanceledException)
                {
                    // supress
                }
            }
        }

        public static void OnGamepadStatePropertyChanged(DependencyObject dep, DependencyPropertyChangedEventArgs args)
        {
            var control = (HIDDisplay) dep;
            control.GamepadState = (GamepadState) args.NewValue;
            control.InvalidateVisual();
        }

        #endregion


        #region Joystick State

        public static readonly DependencyProperty JoystickStateProperty = DependencyProperty.Register(
            "JoystickState", typeof(JoystickState), typeof(HIDDisplay),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnJoystickStatePropertyChanged));

        [Category("HID")]
        [Bindable(true)]
        public JoystickState JoystickState
        {
            get => (JoystickState)GetValue(JoystickStateProperty);
            set
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        SetValue(JoystickStateProperty, value);
                    });
                }
                catch (TaskCanceledException)
                {
                    // suppress
                }
            }
        }

        public static void OnJoystickStatePropertyChanged(DependencyObject dep, DependencyPropertyChangedEventArgs args)
        {
            var control = (HIDDisplay)dep;
            control.JoystickState = (JoystickState)args.NewValue;
            control.InvalidateVisual();
        }

        #endregion

        #region Guage Background Property

        

        public static readonly DependencyProperty GuageBackgroundProperty = DependencyProperty.Register(
            "GuageBackground", typeof(Brush), typeof(HIDDisplay),
            new FrameworkPropertyMetadata(
                Brushes.White,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnGuageBackgroundPropertyChanged));

        [Bindable(true)]
        [Category("Appearance")]
        public Brush GuageBackground
        {
            get => (Brush) GetValue(GuageBackgroundProperty);
            set => SetValue(GuageBackgroundProperty, value);
        }

        public static void OnGuageBackgroundPropertyChanged(DependencyObject dep,
            DependencyPropertyChangedEventArgs args)
        {
            var control = (HIDDisplay)dep;
            control.GuageBackground = (Brush)args.NewValue;
        }

        #endregion


        protected override void OnRender(DrawingContext drawingContext)
        {
            var center = new Point(ActualWidth / 2, ActualHeight / 2);

            #region Joystick

            const int joystickRadius = 75;
            var joystickCenter = new Point(center.X - 50, 100);

            // Circle with cross hairs
            DrawCrosshair(drawingContext, joystickCenter, joystickRadius);

            const int throttleWidth = 50;
            const int throttleHeight = 150;

            var throttleTopLeft = new Point(center.X + 75, 25);
            var throttleRect = CalculateRect(throttleTopLeft, throttleWidth, throttleHeight);
            DrawGuage(drawingContext, throttleRect);
            

            if (JoystickState != null)
            {
                var dot = CalculateCrosshairDot(joystickCenter, joystickRadius, JoystickState.X, JoystickState.Y);

                DrawCrosshairDot(drawingContext, dot, 7.5);
                DrawCrosshairDirection(drawingContext, dot, 25, 30.0 * JoystickState.RotationZ);

                Brush overrideBrush = null;
                string overrideString = null;
                double? heightOverride = null;

                if (JoystickState.Slider < 0.0001)
                {
                    if (DateTime.Now.Millisecond > 500)
                        overrideBrush = Brushes.Red;
                    else
                        overrideBrush = Brushes.Orange;

                    overrideString = "NO\nTHROT";

                    heightOverride = 1;
                }


                if (JoystickState.Buttons[2])
                {
                    overrideBrush = Brushes.Red;
                    overrideString = "Over\nDrive";
                }
                

                var activeRect = CalculateRect(throttleRect, heightOverride ?? JoystickState.Slider);
                FillGuage(
                    drawingContext,
                    activeRect,
                    heightOverride ?? JoystickState.Slider,
                    fillOverride: overrideBrush,
                    textOverride: overrideString);
            }

            #endregion

            #region Gamepad

            const int gamepadStickRadius = 50;

            var leftStickCenter = new Point(center.X - 100, center.Y + 50);
            DrawCrosshair(drawingContext, leftStickCenter, gamepadStickRadius);

            var rightStickCenter = new Point(center.X + 100, center.Y + 50);
            DrawCrosshair(drawingContext, rightStickCenter, gamepadStickRadius);

            const int triggerWidth = 35;
            const int triggerHeight = 100;

            var leftTrigger = CalculateRect(new Point(leftStickCenter.X + 60, center.Y), triggerWidth, triggerHeight);
            DrawGuage(drawingContext, leftTrigger);

            var rightTrigger = CalculateRect(new Point(rightStickCenter.X - 60 - triggerWidth, center.Y), triggerWidth, triggerHeight);
            DrawGuage(drawingContext, rightTrigger);

            var dpadCenter = new Point(center.X - 100, center.Y + 150);

            const int padWidth = 15;
            const double halfWidth = padWidth / 2.0;
            const int padLength = 35;

            var padButtons = new[]
            {
                new Rect(
                    new Point(dpadCenter.X + halfWidth, dpadCenter.Y + halfWidth),
                    new Point(dpadCenter.X - padLength, dpadCenter.Y - halfWidth)),
                new Rect(
                    new Point(dpadCenter.X + halfWidth, dpadCenter.Y + halfWidth),
                    new Point(dpadCenter.X - halfWidth, dpadCenter.Y - padLength)),
                new Rect(
                    new Point(dpadCenter.X - halfWidth, dpadCenter.Y + halfWidth),
                    new Point(dpadCenter.X + padLength, dpadCenter.Y - halfWidth)),
                new Rect(
                    new Point(dpadCenter.X + halfWidth, dpadCenter.Y - halfWidth),
                    new Point(dpadCenter.X - halfWidth, dpadCenter.Y + padLength)),
            };

            drawingContext.DrawRectangle(Brushes.Black, null, padButtons[0]);
            drawingContext.DrawRectangle(Brushes.Black, null, padButtons[1]);
            drawingContext.DrawRectangle(Brushes.Black, null, padButtons[2]);
            drawingContext.DrawRectangle(Brushes.Black, null, padButtons[3]);

            var btnCenter = new Point(center.X + 100, center.Y + 150);

            var btnPoints = new[]
            {
                new Point(btnCenter.X, btnCenter.Y + 25),
                new Point(btnCenter.X + 25, btnCenter.Y),
                new Point(btnCenter.X - 25, btnCenter.Y),
                new Point(btnCenter.X, btnCenter.Y - 25),
            };

            drawingContext.DrawRoundedRectangle(
                Brushes.Black, 
                null,
                new Rect(
                    new Point(btnPoints[3].X - 20, btnPoints[3].Y - 20),
                    new Point(btnPoints[0].X + 20, btnPoints[0].Y + 20)),
                20, 20);

            drawingContext.DrawRoundedRectangle(
                Brushes.Black,
                null,
                new Rect(
                    new Point(btnPoints[1].X + 20, btnPoints[1].Y - 20),
                    new Point(btnPoints[2].X - 20, btnPoints[2].Y + 20)),
                20, 20);

            DrawButton(drawingContext, btnPoints[0], Brushes.White, 'A');
            DrawButton(drawingContext, btnPoints[1], Brushes.White, 'B');
            DrawButton(drawingContext, btnPoints[2], Brushes.White, 'X');
            DrawButton(drawingContext, btnPoints[3], Brushes.White, 'Y');


            if (GamepadState != null)
            {

                DrawCrosshairDot(
                    drawingContext, 
                    CalculateCrosshairDot(leftStickCenter, gamepadStickRadius, GamepadState.LeftThumbX, GamepadState.LeftThumbY),
                    7.5);

                DrawCrosshairDot(
                    drawingContext,
                    CalculateCrosshairDot(rightStickCenter, gamepadStickRadius, GamepadState.RightThumbX, GamepadState.RightThumbY),
                    7.5);

                FillGuage(drawingContext, CalculateRect(leftTrigger, GamepadState.LeftTrigger), GamepadState.LeftTrigger, false);

                FillGuage(drawingContext, CalculateRect(rightTrigger, GamepadState.RightTrigger), GamepadState.RightTrigger, false);

                var dpad = new[]
                {
                    GamepadState.Buttons.HasFlag(GamepadButtonFlags.DPadLeft),
                    GamepadState.Buttons.HasFlag(GamepadButtonFlags.DPadUp),
                    GamepadState.Buttons.HasFlag(GamepadButtonFlags.DPadRight),
                    GamepadState.Buttons.HasFlag(GamepadButtonFlags.DPadDown),
                };

                for (int i = 0; i < 4; i++)
                {
                    if (dpad[i]) drawingContext.DrawRectangle(Foreground, null, padButtons[i]);
                }

                if (GamepadState.Buttons.HasFlag(GamepadButtonFlags.A))
                    DrawButton(drawingContext, btnPoints[0], Brushes.Green, 'A');

                if (GamepadState.Buttons.HasFlag(GamepadButtonFlags.B))
                    DrawButton(drawingContext, btnPoints[1], Brushes.Red, 'B');

                if (GamepadState.Buttons.HasFlag(GamepadButtonFlags.X))
                    DrawButton(drawingContext, btnPoints[2], Brushes.Blue, 'X');

                if (GamepadState.Buttons.HasFlag(GamepadButtonFlags.Y))
                    DrawButton(drawingContext, btnPoints[3], Brushes.Yellow, 'Y');

            }

            #endregion

            base.OnRender(drawingContext);
        }

        private double ToRad(double deg)
        {
            return deg * (Math.PI) / 180.0;
        }

        private Rect CalculateRect(Point topLeft, int width, int height, double perc = 1.0)
        {
            var throttleBottomRight = new Point(topLeft.X + width, topLeft.Y + height);

            return new Rect(new Point(topLeft.X, topLeft.Y), throttleBottomRight);
        }

        private Rect CalculateRect(Rect rect, double perc = 1.0)
        {
            return new Rect(new Point(rect.X, rect.Y + (1.0 - perc) * rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
        }

        private void DrawGuage(DrawingContext drawingContext, Rect rect)
        {
            drawingContext.DrawRectangle(GuageBackground, new Pen(BorderBrush, 2), rect);
        }

        private void FillGuage(DrawingContext drawingContext, Rect filledRect, double perc, bool drawText = true, Brush fillOverride = null, string textOverride = null)
        {
            drawingContext.DrawRectangle(fillOverride ?? Foreground, new Pen(BorderBrush, 2), filledRect);

            if (perc > 0.1 && drawText)
            {
                var ftext = new FormattedText(textOverride ?? perc.ToString("P"), CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                    FontSize,
                    BorderBrush);

                drawingContext.DrawText(ftext,
                    new Point(filledRect.X + (filledRect.Width / 2) - (ftext.Width / 2),
                        filledRect.Y + (filledRect.Height / 2) - (ftext.Height / 2)));
            }
        }

        private void DrawCrosshair(DrawingContext drawingContext, Point center, int radius)
        {
            drawingContext.DrawEllipse(GuageBackground, new Pen(BorderBrush, 2), center, radius, radius);

            drawingContext.DrawLine(
                new Pen(BorderBrush, 1),
                new Point(center.X, center.Y + radius),
                new Point(center.X, center.Y - radius));

            drawingContext.DrawLine(
                new Pen(BorderBrush, 1),
                new Point(center.X - radius, center.Y),
                new Point(center.X + radius, center.Y));
        }

        private Point CalculateCrosshairDot(Point center, double radius, double x, double y)
        {
            return new Point(center.X + (radius  * x), center.Y - (radius * y));
        }

        private void DrawCrosshairDot(DrawingContext drawingContext, Point dot, double radius)
        {
            drawingContext.DrawEllipse(Foreground, null, dot, radius, radius);
        }

        private void DrawCrosshairDirection(DrawingContext drawingContext, Point dot, int length, double angle)
        {
            var arrowPoint = new Point(
                dot.X - (length * Math.Cos(ToRad(90.0 - angle))),
                dot.Y - (length * Math.Sin(ToRad(90 - angle))));

            drawingContext.DrawLine(new Pen(Foreground, 3), dot, arrowPoint);
        }

        private void DrawButton(DrawingContext drawingContext, Point p, Brush color, char button)
        {
            var ftext = new FormattedText(
                button.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeights.Bold, FontStretch),
                FontSize,
                BorderBrush);

            drawingContext.DrawEllipse(color, new Pen(Brushes.Black, 1), p, 15, 15);
            drawingContext.DrawText(ftext, new Point(p.X - 4, p.Y - 7.5));
        }
    }


}
