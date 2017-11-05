﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRCH.GUI
{
    public partial class MainWindow : Form
    {
        #region Contants

        //left margin of whole crane 
        private const int BODY_MARGIN_LEFT = 30;
        private const int BODY_MARGIN_TOP = 20;

        //length of foot line
        private const int FOOT_LENGTH = 20;
        
        private const int SHIPMENT_WIDTH = 20;
        private const int SHIPMENT_HEIGHT = 30;

        private const int PEN_WEIGHT = 2;
        #endregion

        #region Properties

        protected int MoveLeftLength { get; set; }
        protected int MoveRightLength { get; set; }
        protected int MoveTopLength { get; set; }
        protected int MoveBottomLength { get; set; }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void pbProjectionX_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            var bounds = e.ClipRectangle;

            InitCrane(graphics, bounds);
        }


        private void ReDrawCrane()
        {
            pbProjectionX.Image = null;
            InitCrane(pbProjectionX.CreateGraphics(), pbProjectionX.ClientRectangle);
        }

        private void InitCrane(Graphics graphics, Rectangle bounds)
        {
            DrawStaticElements(graphics, bounds);
            DrawDynamicElements(graphics, bounds);
        }

        private void DrawStaticElements(Graphics graphics, Rectangle bounds)
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

        private void DrawDynamicElements(Graphics graphics, Rectangle bounds)
        {
            using (var pen = new Pen(Color.Black, PEN_WEIGHT))
            {
                var relativeX = bounds.X + BODY_MARGIN_LEFT + FOOT_LENGTH / 2 + SHIPMENT_WIDTH / 2 + PEN_WEIGHT;
                var relativeY = bounds.Y + BODY_MARGIN_TOP;

                //width where we can move dick to LEFT or RIGHT
                var workingWidth = bounds.Width - relativeX ;
                var halfOfWorkingWidth = workingWidth / 2;

                //height where we can move dick to UP or DOWN
                var workingHeight = bounds.Height - relativeY - SHIPMENT_HEIGHT / 2;
                var halfOfWorkingHeight = workingHeight / 2;

                //TODO: add or subtract, to move shipment LEFT or RIGHT
                var positionX = halfOfWorkingWidth + MoveRightLength - MoveLeftLength ;

                //TODO: add or subtract, to move shipment TOP or DOWN
                var positionY = halfOfWorkingHeight + MoveBottomLength - MoveTopLength;

                //TODO: add validation
                //con't move to left or right more than halfOfWorkingWidth
                //con't move to top or bottom more than halfOfWorkingHeight
                if (!ValidateSemantic(workingWidth, workingHeight, positionX, positionY))
                {
                    throw new IndexOutOfRangeException();
                }


                //draw shipment holder
                graphics.DrawLine(pen, relativeX + positionX, relativeY, relativeX + positionX, relativeY + positionY);


                //draw shipment
                graphics.DrawRectangle(pen, relativeX + positionX - SHIPMENT_WIDTH/2, relativeY + positionY, SHIPMENT_WIDTH, SHIPMENT_HEIGHT);
            }
        }

        private bool ValidateSemantic(int workingWidth, int workingHeight, int positionX, int positionY)
        {
            if (positionX <= 0 || positionX + SHIPMENT_WIDTH/2 >= workingWidth)
            {
                return false;
            }

            if (positionY <= 0 || positionY + SHIPMENT_HEIGHT / 2 >= workingHeight)
            {
                return false;
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MoveBottomLength += 5;

            try
            {
                ReDrawCrane();
            }
            catch (IndexOutOfRangeException ex)
            {
                MoveBottomLength -= 5;
                ReDrawCrane();
                MessageBox.Show("Impossible to move crane, because it's not in his working area!");
            }
        }
    }
}
