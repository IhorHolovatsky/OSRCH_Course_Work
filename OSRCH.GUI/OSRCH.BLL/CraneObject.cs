using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRCH.BLL
{
    public class CraneObject:ICraneCommands 
    {
        public void Up(int value, SynchronizationContext context, RichTextBox logs)
        {
            Thread.Sleep(1000);
            context.Post(new SendOrPostCallback(o =>
            {
                logs.Text += $"{Environment.NewLine} Crane goes up for {value}";
                
            }), value);
        }

        public void Down(int value, SynchronizationContext context, RichTextBox logs)
        {
            Thread.Sleep(1000);
            context.Post(new SendOrPostCallback(o =>
            {
                logs.Text += $"{Environment.NewLine} Crane goes down for {value}";

            }), value);
        }

        public void Foward(int value, SynchronizationContext context, RichTextBox logs)
        {
            Thread.Sleep(1000);
            context.Post(new SendOrPostCallback(o =>
            {
                logs.Text += $"{Environment.NewLine} Crane goes forward for {value}";

            }), value);
        }

        public void Backward(int value, SynchronizationContext context, RichTextBox logs)
        {
            Thread.Sleep(1000);
            context.Post(new SendOrPostCallback(o =>
            {
                logs.Text += $"{Environment.NewLine} Crane goes backward for {value}";

            }), value);
        }

        public void Rotate(int value, SynchronizationContext context, RichTextBox logs)
        {
            Thread.Sleep(1000);
            context.Post(new SendOrPostCallback(o =>
            {
                logs.Text += $"{Environment.NewLine} Crane rotate for {value}";

            }), value);
        }

        
    }
}
