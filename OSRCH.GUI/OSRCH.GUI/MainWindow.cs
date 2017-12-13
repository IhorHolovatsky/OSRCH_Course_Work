using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSRCH.BLL;
using OSRCH.BLL.Models;

namespace OSRCH.GUI
{
    public partial class MainWindow : Form
    {
        private readonly SynchronizationContext _synchronizationContext;

        #region Contants

        //left margin of whole crane 
        private const int BODY_MARGIN_LEFT = 30;
        private const int BODY_MARGIN_TOP = 20;

        //length of foot line
        private const int FOOT_LENGTH = 20;

        private const int SHIPMENT_WIDTH = 20;
        private const int SHIPMENT_HEIGHT = 30;

        private const int CRANE_BODY_RADIUS = 8;

        private const int PEN_WEIGHT = 2;
        #endregion

        #region Commands

        protected int MoveLeftLength { get; set; }
        protected int MoveRightLength { get; set; }
        protected int MoveTopLength { get; set; }
        protected int MoveBottomLength { get; set; }

        protected int RotateLeftDegrees { get; set; }
        protected int RotateRightDegrees { get; set; }
        #endregion

        #region Interrupts

        protected AutoResetEvent ContinuationEvent;
        protected bool IsInterrupted { get; set; }
        protected InterruptCode Interrupt { get; set; }

        protected enum InterruptCode
        {
            Barrier,
            PeopleInWorkingZone,
            BadWeather
        }
        #endregion


        public MainWindow()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
            ContinuationEvent = new AutoResetEvent(false);
            WorkingWidth = pbProjectionX.ClientRectangle.Width - (pbProjectionX.ClientRectangle.X + BODY_MARGIN_LEFT + FOOT_LENGTH / 2 + SHIPMENT_WIDTH / 2 + PEN_WEIGHT);
            WorkingHeight = pbProjectionX.ClientRectangle.Height - (pbProjectionX.ClientRectangle.Y + BODY_MARGIN_TOP);
            StartPositionX = WorkingWidth / 2;
            StartPositionY = WorkingHeight - SHIPMENT_HEIGHT;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Txt files|*.txt",
                Title = "Select an instruction"
            };

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            labelInstructionName.Text = openFileDialog1.SafeFileName;

            var sr = new
                System.IO.StreamReader(openFileDialog1.FileName);

            var instructions = new List<string>();

            while (!sr.EndOfStream)
            {
                instructions.Add(sr.ReadLine());
            }
            
            sr.Close();

            SimulatorBusinessLogic.CurrentInstructionsStrings = instructions;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.IsAccessible = false;

            //SimulatorBusinessLogic.RunSimulation(new CraneObject(),_synchronizationContext,logsTextBox,
            //    delegate(SimulationResponseModel model)
            //    {
            //        MessageBox.Show(this, model.Message, (model.IsSuccess) ? "Simulation end" : "Error happened",
            //            MessageBoxButtons.OK);
            //        startButton.IsAccessible = true;
            //    });

            Task.Run(() =>
            {
                //ToDo вся робота з вантажем
                foreach (var currentInstructionsString in SimulatorBusinessLogic.CurrentInstructionsStrings)
                {
                    if (IsInterrupted)
                    {
                        int backupMoveLeftLength = MoveLeftLength;
                        int backupMoveRightLength = MoveRightLength;
                        int backupMoveTopLength = MoveTopLength;
                        int backupMoveBottomLength = MoveBottomLength;

                        int backupRotateLeftDegrees = RotateLeftDegrees;
                        int backupRotateRightDegrees = RotateRightDegrees;

                        switch (Interrupt)
                        {
                            case InterruptCode.Barrier:
                                // Wait for clearing
                                _synchronizationContext.Post(o =>
                                {
                                    Log("Interrupt : Barrier in working zone");
                                }, null);                               
                                break;
                            case InterruptCode.PeopleInWorkingZone:
                                // Wait for clearing
                                _synchronizationContext.Post(o =>
                                {
                                    Log("Interrupt : People in working zone");
                                }, null);          
                                break;
                            case InterruptCode.BadWeather:
                                _synchronizationContext.Post(o =>
                                {
                                    Log("Interrupt : Bad weather");
                                }, null);
                                // Put down and stop
                                MoveBottomLength = MoveTopLength = 0;
                                ReDrawCrane();
                                break;
                            default:
                                _synchronizationContext.Post(o =>
                                {
                                    Log("Interrupt : Unknown interrupt");
                                }, null);
                                break;
                        }
                        // Delay before start next command
                        ContinuationEvent.WaitOne();
                        // Clear interrupt variables
                        IsInterrupted = false;
                        // Restore position
                        MoveLeftLength = backupMoveLeftLength;
                        MoveRightLength = backupMoveRightLength;
                        MoveTopLength = backupMoveTopLength;
                        MoveBottomLength = backupMoveBottomLength;

                        RotateLeftDegrees = backupRotateLeftDegrees;
                        RotateRightDegrees = backupRotateRightDegrees;
                        ReDrawCrane();
                        Thread.Sleep(1000);
                    }

                    var value = SimulatorBusinessLogic.GetValueFromCommandString(currentInstructionsString);

                    String message;
                    if (currentInstructionsString.Contains("Up<"))
                    {
                        if (ValidateTop(value))
                        {
                            MoveTopLength += value;
                            message = $"Crane goes up for {value}";
                        }
                        else
                        {
                            message = $"Illegal argument for up command ({value})";
                        }
                    }
                    else if (currentInstructionsString.Contains("Down<"))
                    {
                        if (ValidateBottom(value))
                        {
                            MoveBottomLength += value;
                            message = $"Crane goes down for {value}";
                        }
                        else
                        {
                            message = $"Illegal argument for down command ({value})";
                        }
                    }
                    else if (currentInstructionsString.Contains("Forward<"))
                    {
                        if (ValidateRight(value))
                        {
                            MoveRightLength += value;
                            message = $"Crane goes forward for {value}";
                        }
                        else
                        {
                            message = $"Illegal argument for forward command ({value})";
                        }
                    }
                    else if (currentInstructionsString.Contains("Backward<"))
                    {
                        if (ValidateLeft(value))
                        {
                            MoveLeftLength += value;
                            message = $"Crane goes backward for {value}";
                        }
                        else
                        {
                            message = $"Illegal argument for backward command ({value})";
                        }
                    }
                    else if (currentInstructionsString.Contains("Rotate<"))
                    {
                        if (ValidateRotationAngle(value))
                        {
                            RotateLeftDegrees += value;
                            message = $"Crane rotates for {value}";
                        }
                        else
                        {
                            message = $"Illegal argument for rotate command ({value})";
                        }
                    }
                    else if (currentInstructionsString.Contains("TakeCargo"))
                    {
                        message = "Take cargo";
                    }
                    else if (currentInstructionsString.Contains("ReleaseCargo"))
                    {
                        message = "Release cargo";
                    }
                    else
                    {
                        message = "Unknown command!";
                    }
                    _synchronizationContext.Post(o =>
                    {
                        Log(message);
                    }, value);

                    ReDrawCrane();
                    Thread.Sleep(1000);
                }
            });
        }

        private void Log(string message)
        {
            string logString = $"{Environment.NewLine}Date: {DateTime.Now:d} | Time: {DateTime.Now:T} | Action : {message}";
            logsTextBox.Text += logString;
            using (var logFileStream = new FileStream(Path.Combine(Environment.CurrentDirectory, "log.txt"), FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (var logFileStreamWriter = new StreamWriter(logFileStream))
                {
                    logFileStreamWriter.Write(logString);
                }
            }
        }

        #region Validation

        private bool ValidateRotationAngle(int angle)
        {
            return Math.Abs(angle) <= 360;
        }

        private bool ValidateTop(int value)
        {
            return ValidateMoving(MoveTopLength + value, MoveBottomLength, MoveLeftLength, MoveRightLength);
        }

        private bool ValidateBottom(int value)
        {
            return ValidateMoving(MoveTopLength, MoveBottomLength + value, MoveLeftLength, MoveRightLength);
        }

        private bool ValidateLeft(int value)
        {
            return ValidateMoving(MoveTopLength, MoveBottomLength, MoveLeftLength + value, MoveRightLength);
        }

        private bool ValidateRight(int value)
        {
            return ValidateMoving(MoveTopLength, MoveBottomLength, MoveLeftLength, MoveRightLength + value);
        }

        private bool ValidateMoving(int moveTop, int moveBottom, int moveLeft, int moveRight)
        {
            int upperBorderX = WorkingWidth - SHIPMENT_WIDTH / 2;
            int lowBorderX = 0 + SHIPMENT_WIDTH / 2;
            int upperBorderY = WorkingHeight - SHIPMENT_HEIGHT;
            int lowBorderY = 0 + SHIPMENT_HEIGHT;

            int nowPositionY = StartPositionY + (moveBottom - moveTop);
            int nowPositionX = StartPositionX + (moveRight - moveLeft);
            return nowPositionY <= upperBorderY && nowPositionY >= lowBorderY && nowPositionX <= upperBorderX && nowPositionX >= lowBorderX;
        }

        #endregion

        private void ReDrawCrane()
        {
            pbProjectionX.Image = null;
            InitCraneX(pbProjectionX.CreateGraphics(), pbProjectionX.ClientRectangle);

            pbProjectionY.Image = null;
            InitCraneY(pbProjectionY.CreateGraphics(), pbProjectionY.ClientRectangle);
        }

        #region Projection X

        private void pbProjectionX_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var bounds = e.ClipRectangle;

            InitCraneX(graphics, bounds);
        }

        private void InitCraneX(Graphics graphics, Rectangle bounds)
        {
            DrawStaticElementsX(graphics, bounds);
            DrawDynamicElementsX(graphics, bounds);
        }

        private void DrawStaticElementsX(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Black, PEN_WEIGHT))
            {
                var relativeX = bounds.X + BODY_MARGIN_LEFT;
                var relativeY = bounds.Y + BODY_MARGIN_TOP;

                //draw foot
                //(bounds.Height - 1) - to not crop line width.
                graphics.DrawLine(pen, relativeX, bounds.Height - PEN_WEIGHT, relativeX + FOOT_LENGTH, bounds.Height - PEN_WEIGHT);

                //draw body
                graphics.DrawLine(pen, relativeX + FOOT_LENGTH / 2, bounds.Height, relativeX + FOOT_LENGTH / 2, relativeY);

                //draw arrow (crane's dick)
                graphics.DrawLine(pen, bounds.X, relativeY, bounds.Width, relativeY);

            }
        }

        protected int WorkingHeight;
        protected int WorkingWidth;

        protected int StartPositionX;
        protected int StartPositionY;

        private void DrawDynamicElementsX(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Black, PEN_WEIGHT))
            {
                var relativeX = bounds.X + BODY_MARGIN_LEFT + FOOT_LENGTH / 2 + SHIPMENT_WIDTH / 2 + PEN_WEIGHT;
                var relativeY = bounds.Y + BODY_MARGIN_TOP;

                //width where we can move dick to LEFT or RIGHT
                var workingWidth = bounds.Width - relativeX;
                //var halfOfWorkingWidth = workingWidth / 2;

                //height where we can move dick to UP or DOWN
                var workingHeight = bounds.Height - relativeY - SHIPMENT_HEIGHT / 2;
                //var halfOfWorkingHeight = workingHeight / 2;

                //TODO: add or subtract, to move shipment LEFT or RIGHT
                var positionX = StartPositionX/*halfOfWorkingWidth*/ + MoveRightLength - MoveLeftLength;

                //TODO: add or subtract, to move shipment TOP or DOWN
                var positionY = StartPositionY/*workingHeight-SHIPMENT_HEIGHT/2+halfOfWorkingHeight */+ MoveBottomLength - MoveTopLength;

                //TODO: add validation
                //con't move to left or right more than halfOfWorkingWidth
                //con't move to top or bottom more than halfOfWorkingHeight
                if (!ValidateSemanticX(workingWidth, workingHeight, positionX, positionY))
                {
                    //throw new IndexOutOfRangeException();
                }


                //draw shipment holder
                graphics.DrawLine(pen, relativeX + positionX, relativeY, relativeX + positionX, relativeY + positionY);


                //draw shipment
                graphics.DrawRectangle(pen, relativeX + positionX - SHIPMENT_WIDTH / 2, relativeY + positionY, SHIPMENT_WIDTH, SHIPMENT_HEIGHT);
            }
        }

        private bool ValidateSemanticX(int workingWidth, int workingHeight, int positionX, int positionY)
        {
            if (positionX <= 0 || positionX + SHIPMENT_WIDTH / 2 >= workingWidth)
            {
                return false;
            }

            if (positionY <= 0 || positionY + SHIPMENT_HEIGHT / 2 >= workingHeight)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Projection Y

        private void pbProjectionY_Paint(object sender, PaintEventArgs e)
        {
            InitCraneY(e.Graphics, e.ClipRectangle);
        }

        private void InitCraneY(Graphics graphics, Rectangle bounds)
        {
            DrawStaticElementsY(graphics, bounds);
            DrawDynamicElementsY(graphics, bounds);
        }
        private void DrawStaticElementsY(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Black, PEN_WEIGHT))
            {
                var relativeX = bounds.Width / 2;
                var relativeY = bounds.Height / 2;

                //draw crane body
                graphics.DrawEllipse(pen,
                                     new Rectangle()
                                     {
                                         Height = CRANE_BODY_RADIUS,
                                         Width = CRANE_BODY_RADIUS,
                                         Location = new Point(relativeX - CRANE_BODY_RADIUS / 2, relativeY - CRANE_BODY_RADIUS / 2)
                                     });
            }
        }

        private void DrawDynamicElementsY(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Black, PEN_WEIGHT))
            {
                var centerPoint = new Point(bounds.Width / 2, bounds.Height / 2);

                //TODO: To move left or right, modify RotateRightDegrees or RotateLeftDegrees
                var craneDickEndPoint = CalculateCircumferencePoint(RotateRightDegrees - RotateLeftDegrees, centerPoint, bounds.Height / 2);

                //draw crane dick
                graphics.DrawLine(pen, centerPoint, craneDickEndPoint);
            }
        }

        #endregion
        
        private Point CalculateCircumferencePoint(int angleInDegrees, Point center, int radius)
        {
            var angleInRadians = angleInDegrees * (Math.PI / 180);
            var res = new Point();

            var x = center.X + radius * Math.Cos(angleInRadians);
            var y = center.Y + radius * Math.Sin(angleInRadians);

            res.X = (int)x;
            res.Y = (int)y;

            return res;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MoveRightLength = 0;
            MoveBottomLength = 0;
            MoveLeftLength = 0;
            MoveTopLength = 0;
            RotateRightDegrees = 0;
            RotateLeftDegrees = 0;
            ReDrawCrane();
        }

        private void HandleInterrupt(InterruptCode code)
        {
            ContinueExecution.Visible = true;
            Interrupt = code;
            IsInterrupted = true;
        }

        private void BarrierInterruptButton_Click(object sender, EventArgs e)
        {
            HandleInterrupt(InterruptCode.Barrier);
        }

        private void PeopleInWorkingZoneInterruptButton_Click(object sender, EventArgs e)
        {
            HandleInterrupt(InterruptCode.PeopleInWorkingZone);
        }

        private void WeatherInterruptButton_Click(object sender, EventArgs e)
        {
            HandleInterrupt(InterruptCode.BadWeather);
        }

        private void ContinueExecution_Click(object sender, EventArgs e)
        {
            ContinuationEvent.Set();
            ContinueExecution.Visible = false;
        }
    }
}
