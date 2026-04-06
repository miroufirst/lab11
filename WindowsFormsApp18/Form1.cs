using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp18
{
    public abstract class WagonHandler
    {
        protected WagonHandler Successor;
        public void SetSuccessor(WagonHandler successor) => this.Successor = successor;
        public abstract void HandleDraw(string type, Graphics g, int x, int y);
    }

    public class LocomotiveHandler : WagonHandler
    {
        public override void HandleDraw(string type, Graphics g, int x, int y)
        {
            if (type == "Locomotive")
            {
                g.FillRectangle(Brushes.Firebrick, x, y, 60, 40);
                g.FillRectangle(Brushes.Black, x + 45, y - 10, 10, 15);
                g.FillEllipse(Brushes.Black, x + 5, y + 35, 15, 15);
                g.FillEllipse(Brushes.Black, x + 40, y + 35, 15, 15);
            }
            else Successor?.HandleDraw(type, g, x, y);
        }
    }

    public class PassengerHandler : WagonHandler
    {
        public override void HandleDraw(string type, Graphics g, int x, int y)
        {
            if (type == "Passenger")
            {
                g.FillRectangle(Brushes.RoyalBlue, x, y, 60, 40);
                g.FillRectangle(Brushes.LightSkyBlue, x + 8, y + 8, 18, 15);
                g.FillRectangle(Brushes.LightSkyBlue, x + 34, y + 8, 18, 15);
                g.FillEllipse(Brushes.Black, x + 5, y + 35, 15, 15);
                g.FillEllipse(Brushes.Black, x + 40, y + 35, 15, 15);
            }
            else Successor?.HandleDraw(type, g, x, y);
        }
    }

    public class CargoHandler : WagonHandler
    {
        public override void HandleDraw(string type, Graphics g, int x, int y)
        {
            if (type == "Cargo")
            {
                g.FillRectangle(Brushes.SaddleBrown, x, y + 5, 60, 35);
                g.DrawLine(Pens.White, x, y + 5, x + 60, y + 40);
                g.FillEllipse(Brushes.Black, x + 5, y + 35, 15, 15);
                g.FillEllipse(Brushes.Black, x + 40, y + 35, 15, 15);
            }
            else Successor?.HandleDraw(type, g, x, y);
        }
    }


    public partial class Form1 : Form
    {
        private List<string> train = new List<string>();
        private WagonHandler chain;

        public Form1()
        {
            InitializeComponent();
            SetupChain();
        }
        private void SetupChain()
        {
            var loco = new LocomotiveHandler();
            var pass = new PassengerHandler();
            var cargo = new CargoHandler();

            loco.SetSuccessor(pass);
            pass.SetSuccessor(cargo);

            chain = loco;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbWagonType.SelectedItem != null)
            {
                train.Add(cbWagonType.SelectedItem.ToString());
                pnlCanvas.Invalidate();
            }
        }

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            int startX = 20;
            int startY = 80;
            int wagonWidth = 60;
            int gap = 15;

            for (int i = 0; i < train.Count; i++)
            {
                int currentX = startX + (i * (wagonWidth + gap));

                chain.HandleDraw(train[i], e.Graphics, currentX, startY);

                if (i < train.Count - 1)
                {
                    e.Graphics.DrawLine(Pens.Black, currentX + wagonWidth, startY + 30, currentX + wagonWidth + gap, startY + 30);
                }
            }
        }
    }
}
  
