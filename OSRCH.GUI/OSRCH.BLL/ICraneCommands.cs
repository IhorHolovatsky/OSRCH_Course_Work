using System.Threading;
using System.Windows.Forms;

namespace OSRCH.BLL
{
    public interface ICraneCommands
    {
        void Up(int value, SynchronizationContext context, RichTextBox logs);
        void Down(int value, SynchronizationContext context, RichTextBox logs);
        void Foward(int value, SynchronizationContext context, RichTextBox logs);
        void Backward(int value, SynchronizationContext context, RichTextBox logs);
        void Rotate(int value, SynchronizationContext context, RichTextBox logs);
    }
}