using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OSRCH.BLL.Models;

namespace OSRCH.BLL
{
    public static class SimulatorBusinessLogic
    {

        public static List<string> CurrentInstructionsStrings { private get; set; }

        public static async void RunSimulation(ICraneCommands craneObject, SynchronizationContext context, RichTextBox logs, Action<SimulationResponseModel> setResponse)
        {
            var response = new SimulationResponseModel();

            if (CurrentInstructionsStrings.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = "Please, set instruction first";

            }

            setResponse(response);

            await Task.Run((() =>
            {
                

                //ToDo вся робота з вантажем
                foreach (var currentInstructionsString in CurrentInstructionsStrings)
                {
                    if (currentInstructionsString.Contains("Up<"))
                    {
                        craneObject.Up(GetValueFromCommandString(currentInstructionsString),context, logs);
                    }
                    if (currentInstructionsString.Contains("Down<"))
                    {
                        craneObject.Down(GetValueFromCommandString(currentInstructionsString), context, logs);
                    }
                    if (currentInstructionsString.Contains("Forward<"))
                    {
                        craneObject.Foward(GetValueFromCommandString(currentInstructionsString), context, logs);
                    }
                    if (currentInstructionsString.Contains("Backward<"))
                    {
                        craneObject.Backward(GetValueFromCommandString(currentInstructionsString), context, logs);
                    }
                    if (currentInstructionsString.Contains("Rotate<"))
                    {
                        craneObject.Rotate(GetValueFromCommandString(currentInstructionsString), context, logs);
                    }
                }

                
            }));
        }

        private static int GetValueFromCommandString(string command)
        {
            var subStrings = command.Split('<');
            return Convert.ToInt32(subStrings[1].Split('>')[0]);
        }

    }
}
